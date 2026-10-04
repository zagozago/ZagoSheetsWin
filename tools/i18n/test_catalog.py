import copy, importlib.util, json, unittest
from pathlib import Path

spec=importlib.util.spec_from_file_location('catalog_generator',Path(__file__).with_name('generate.py'))
generator=importlib.util.module_from_spec(spec);spec.loader.exec_module(generator)

class CatalogContracts(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.source=json.loads(generator.SOURCE.read_text())
        cls.locales=json.loads((generator.ROOT/'i18n/locales.json').read_text())

    def pack(self, code='pt'):
        language=next(v for v in self.locales['languages'] if v['code']==code)
        return {'language':code,'locale':language['locale'],'direction':language['direction'],
                'sourceHash':generator.source_hash(self.source),
                'catalogRevision':self.source['catalogRevision'],'status':'source' if code=='pt' else 'not_started',
                'strings':{k:v['text'] if code=='pt' else None for k,v in self.source['entries'].items()}}

    def test_all_51_pack_templates_use_exact_matrix_and_script_metadata(self):
        generator.validate(self.source,self.locales)
        for code in self.locales['canonicalOrder']:
            generator.validate_pack(self.pack(code),self.source,self.locales)
        languages={v['code']:v for v in self.locales['languages']}
        self.assertEqual('fil-PH',languages['fil']['locale']);self.assertEqual('TL',languages['fil']['displayCode'])
        self.assertEqual('yue-Hant-HK',languages['yue']['locale'])
        self.assertEqual('pa-Guru-IN',languages['pa']['locale'])

    def test_missing_keys_or_wrong_script_direction_are_rejected(self):
        for mutate in [lambda p:p['strings'].pop('home.help'), lambda p:p.update(direction='rtl'),lambda p:p.update(locale='pt-PT'),lambda p:p.update(sourceHash='old-source')]:
            pack=self.pack();mutate(pack)
            with self.assertRaises(AssertionError):generator.validate_pack(pack,self.source,self.locales)

    def test_placeholder_and_brand_corruption_are_rejected(self):
        for key,value in [('recovery.usage','{usedMb}'),('home.description','Texto sem as marcas')]:
            pack=self.pack();pack['strings'][key]=value
            with self.assertRaises(AssertionError):generator.validate_pack(pack,self.source,self.locales)

    def test_incomplete_pack_cannot_pass_release_gate(self):
        with self.assertRaises(AssertionError):generator.validate_pack(self.pack('ar'),self.source,self.locales,True)

    def test_non_latin_pack_rejects_portuguese_and_wrong_functional_numbers(self):
        pack=self.pack('ar');pack['strings']['home.help']='&Ajuda'
        with self.assertRaises(AssertionError):generator.validate_pack(pack,self.source,self.locales)
        pack=self.pack();pack['strings']['recovery.limits']=pack['strings']['recovery.limits'].replace('200','300')
        with self.assertRaises(AssertionError):generator.validate_pack(pack,self.source,self.locales)

    def test_source_requires_editorial_and_layout_qa_for_release(self):
        with self.assertRaises(KeyError):generator.validate_pack(self.pack(),self.source,self.locales,True)

    def test_generated_resources_are_exact_and_only_portuguese_is_embedded(self):
        for path, content in generator.files(self.source).items():self.assertEqual(path.read_bytes(),content)
        runtime=json.loads((generator.ROOT/'i18n/generated/pt.json').read_text())
        self.assertEqual('pt-BR',runtime['sourceLocale'])
        self.assertNotIn('installer.runSetup',runtime['strings'])

if __name__=='__main__':unittest.main()
