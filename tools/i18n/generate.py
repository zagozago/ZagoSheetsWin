#!/usr/bin/env python3
"""Validate the canonical catalog and generate native technology resources.

No network, translations, locale selection or user-state mutation occurs here.
Use --check in CI; --init-pack CODE emits the exact matrix with null targets.
"""
import argparse, collections, hashlib, json, pathlib, re, sys, unicodedata

ROOT = pathlib.Path(__file__).resolve().parents[2]
SOURCE = ROOT / 'i18n/source.json'
NAMED = re.compile(r'\{([A-Za-z][A-Za-z0-9_]*)(?::([^{}]+))?\}')
INNO = re.compile(r'%[1-9]')
# Exact localized unit spellings, not blanket exceptions for invariants.
LOCALIZED_UNITS = {'fr': {'MB': 'Mo', 'GB': 'Go'}, 'ru': {'MB': 'МБ', 'GB': 'ГБ'},
                   'ar': {'MB': 'ميغابايت'}}
TECHNICAL_LABELS = {
    'dialog.oauthFileFilter': {'OAuth JSON|*.json'},
    'installer.standard.Messages.ButtonOK': {'OK'},
    'installer.standard.Messages.UninstallDisplayNameMark32Bit': {'32-bit'},
    'installer.standard.Messages.UninstallDisplayNameMark64Bit': {'64-bit'},
}
SCRIPT_RANGES = {
    **{c:[(0x0600,0x06ff),(0x0750,0x077f),(0x08a0,0x08ff)] for c in ['ar','ur','fa','ps','sd']},
    **{c:[(0x0900,0x097f)] for c in ['hi','mr','bho','mai','ne']},
    **{c:[(0x4e00,0x9fff)] for c in ['zh','yue']},
    **{c:[(0x0400,0x052f)] for c in ['ru','uk']},
    'bn':[(0x0980,0x09ff)],'pa':[(0x0a00,0x0a7f)],'gu':[(0x0a80,0x0aff)],
    'or':[(0x0b00,0x0b7f)],'ta':[(0x0b80,0x0bff)],'te':[(0x0c00,0x0c7f)],
    'kn':[(0x0c80,0x0cff)],'ml':[(0x0d00,0x0d7f)],'th':[(0x0e00,0x0e7f)],
    'my':[(0x1000,0x109f)],'am':[(0x1200,0x137f)],
    'ja':[(0x3040,0x30ff),(0x4e00,0x9fff)],'ko':[(0x1100,0x11ff),(0xac00,0xd7af)]
}

def slots(text, scope):
    return collections.Counter(INNO.findall(text) if scope.startswith('installer.') else
                               [(m.group(1), m.group(2)) for m in NAMED.finditer(text)])

def source_hash(source):
    return hashlib.sha256(json.dumps(source['entries'],ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()

def validate(source, locales):
    entries = source['entries']
    assert source['sourceLanguage'] == 'pt' and source['sourceLocale'] == 'pt-BR'
    assert source['fallbackLanguage'] == 'en'
    assert len(locales['canonicalOrder']) == 51 and len(set(locales['canonicalOrder'])) == 51
    assert locales['canonicalOrder'] == [v['code'] for v in locales['languages']]
    assert {v['code'] for v in locales['languages'] if v['direction'] == 'rtl'} == {'ar','ur','fa','ps','sd'}
    for key, entry in entries.items():
        assert re.fullmatch(r'[A-Za-z][A-Za-z0-9]*(?:\.[A-Za-z][A-Za-z0-9_]*)+', key), key
        text = entry['text']
        text.encode('utf-8', 'strict')
        assert entry['context'] and entry['scope']
        expected = collections.Counter(entry['placeholders'])
        actual = collections.Counter(INNO.findall(text) if entry['scope'].startswith('installer.') else
                                     [m.group(1) for m in NAMED.finditer(text)])
        assert expected == actual, (key, expected, actual)
        assert '\r' not in text
    assert set(source['emphasisKeys']) <= entries.keys()
    assert set(source['standardInstallerMessages']) <= entries.keys()

def validate_pack(pack, source, locales, release=False):
    assert pack['status'] in ['not_started','translated','complete','source'], 'Unknown pack stage.'
    assert pack['language'] in locales['canonicalOrder']
    assert pack['catalogRevision'] == source['catalogRevision']
    assert pack['sourceHash'] == source_hash(source), 'Pack belongs to a different source revision.'
    assert set(pack['strings']) == set(source['entries']), 'Pack must have the exact source key matrix.'
    language = next(v for v in locales['languages'] if v['code'] == pack['language'])
    assert pack['locale'] == language['locale'] and pack['direction'] == language['direction']
    for key, text in pack['strings'].items():
        entry = source['entries'][key]
        if text is None:
            assert not release and pack['status'] == 'not_started', key
            continue
        assert isinstance(text, str), key
        text.encode('utf-8', 'strict')
        assert text.strip() or not entry['text'].strip(), key
        assert slots(text, entry['scope']) == slots(entry['text'], entry['scope']), key
        for token in entry['invariants']:
            unit = LOCALIZED_UNITS.get(pack['language'], {}).get(token)
            unit_count = (text.count(unit) if pack['language']=='ar' else len(re.findall(r'(?<!\w)'+re.escape(unit)+r'(?!\w)',text))) if unit else 0
            count = text.count(token) + unit_count
            assert count == entry['text'].count(token), (key, token)
        assert re.findall(r'\*\.[a-z0-9]+',text)==re.findall(r'\*\.[a-z0-9]+',entry['text']), (key,'file masks changed')
        if '|' in entry['text']: assert text.count('|') == entry['text'].count('|'), key
        def visible(value):
            value=INNO.sub('',value) if entry['scope'].startswith('installer.') else NAMED.sub('',value)
            for token in sorted(entry['invariants'],key=len,reverse=True):
                value=value.replace(token,'')
                unit=LOCALIZED_UNITS.get(pack['language'],{}).get(token)
                if unit: value=value.replace(unit,'') if pack['language']=='ar' else re.sub(r'(?<!\w)'+re.escape(unit)+r'(?!\w)','',value)
            return value
        original=visible(entry['text']); translated=visible(text)
        def digits(value):
            return [unicodedata.digit(c) for c in value if c.isdecimal()]
        if key in pack.get('expandedNumericNotation',[]):
            assert key=='setup.formatLimits' and digits(original)==[2,0,5,0,0,5,0], (key,'unrecognized expanded numeric notation')
            values=[int(re.sub(r'[, .\u202f]','',value)) for value in re.findall(r'\d+(?:[, .\u202f]\d{3})*',translated)]
            assert values==[20,500000,50000,1000], (key,'functional capacity values changed')
        else:
            assert digits(original)==digits(translated), (key,'functional numbers changed')
        technical = text in TECHNICAL_LABELS.get(key,set()) or (key=='tutorial.pageCount' and not any(c.isalpha() for c in translated))
        if pack['language'] in SCRIPT_RANGES and any(c.isalpha() for c in original) and key!='installer.standard.Messages.ComponentSize1' and not technical:
            assert any(low<=ord(c)<=high for c in translated for low,high in SCRIPT_RANGES[pack['language']]), (key,'expected script absent')
    assert set(pack.get('expandedNumericNotation',[])) <= {'setup.formatLimits'}, 'Unrecognized numeric notation exception.'
    if 'installer.standard.Messages.ComponentSize1' in pack['strings'] and pack['strings']['installer.standard.Messages.ComponentSize1'] is not None:
        assert pack['strings']['installer.standard.Messages.ComponentSize1']==source['entries']['installer.standard.Messages.ComponentSize1']['text'], 'Installer KB size format must remain invariant.'
    if pack['status']=='translated':
        assert all(isinstance(value,str) for value in pack['strings'].values()), 'Translated pack has missing content.'
        assert pack['editorialQa']=='passed', 'Translated pack needs editorial review.'
    if release:
        assert pack['status'] in ['source','complete']
        assert pack['editorialQa'] == 'passed' and pack['layoutQa'] == 'passed'
        identical={k for k,v in pack['strings'].items() if v==source['entries'][k]['text'] and source['entries'][k]['scope']=='app'}
        if pack['language']!='pt': assert identical <= set(pack.get('sourceMatchesReviewed',[])), 'Unreviewed source-language matches.'
        # Native review is reported separately; it is never inferred from technical PASS.

def files(source):
    runtime = {'productVersion':source['productVersion'], 'sourceLocale':source['sourceLocale'],
               'sourceHash':source_hash(source),
               'emphasisKeys':source['emphasisKeys'],
               'strings':{k:v['text'] for k,v in source['entries'].items() if v['scope'] in ['app','emphasis']}}
    custom = ['; Generated from i18n/source.json. Edit the source catalog, not this file.', '[CustomMessages]']
    for key, entry in source['entries'].items():
        if entry['scope'] == 'installer.custom':
            custom.append('Zago_'+key.removeprefix('installer.')+'='+entry['text'].replace('\n','%n'))
    result = {
        ROOT/'i18n/generated/pt.json': (json.dumps(runtime, ensure_ascii=False, indent=2)+'\n').encode('utf-8'),
        ROOT/'installer/i18n/pt.isl': ('\n'.join(custom)+'\n').encode('utf-8-sig')
    }
    locales=json.loads((ROOT/'i18n/locales.json').read_text(encoding='utf-8'))
    for path in sorted((ROOT/'i18n/packs').glob('*.json')):
        pack=json.loads(path.read_text(encoding='utf-8'));validate_pack(pack,source,locales)
        if pack['status']!='complete' and not (source.get('multilingualReady') and pack['status']=='translated'): continue
        code=pack['language']; values=pack['strings']
        target=dict(runtime,strings={k:values[k] for k in runtime['strings']})
        result[ROOT/f'i18n/generated/{code}.json']=(json.dumps(target,ensure_ascii=False,indent=2)+'\n').encode('utf-8')
        lines=['; Generated from the canonical language pack.', '[CustomMessages]']
        lines += ['Zago_'+k.removeprefix('installer.')+'='+values[k].replace('\n','%n') for k,e in source['entries'].items() if e['scope']=='installer.custom']
        result[ROOT/f'installer/i18n/{code}.isl']=('\n'.join(lines)+'\n').encode('utf-8-sig')
    # Override cataloged standard strings, including the corrected PT %1 slot.
    for code in locales['canonicalOrder'] if source.get('multilingualReady') else ['pt','en']:
        values={k:e['text'] for k,e in source['entries'].items()} if code=='pt' else json.loads((ROOT/f'i18n/packs/{code}.json').read_text(encoding='utf-8'))['strings']
        lines=['; Generated standard message overrides.'];section=None
        for k,meta in source['standardInstallerMessages'].items():
            if meta['section']!=section: section=meta['section'];lines+=['['+section+']']
            lines += [meta['name']+'='+values[k]]
        result[ROOT/f'installer/i18n/{code}-standard.isl']=('\n'.join(lines)+'\n').encode('utf-8-sig')
    if source.get('multilingualReady'):
        for language in locales['languages']:
            code=language['code']
            options=['; Generated Unicode language options.', '[LangOptions]',
                     'LanguageName='+language['name'], 'LanguageID=$'+format(language['installerLanguageId'],'04x'),
                     'LanguageCodePage=0', 'DialogFontName='+language['fontFamily'], 'DialogFontSize=9',
                     'WelcomeFontName='+language['fontFamily'], 'RightToLeft='+('yes' if language['direction']=='rtl' else 'no')]
            result[ROOT/f'installer/i18n/{code}-options.isl']=('\n'.join(options)+'\n').encode('utf-8-sig')
            result[ROOT/f'installer/i18n/{code}.isl'] += ('Zago_languageCode='+code+'\n').encode('utf-8')
    return result

def main():
    parser=argparse.ArgumentParser();parser.add_argument('--check',action='store_true')
    parser.add_argument('--init-pack');parser.add_argument('--validate-pack',type=pathlib.Path)
    parser.add_argument('--release',action='store_true');args=parser.parse_args()
    source=json.loads(SOURCE.read_text(encoding="utf-8"));locales=json.loads((ROOT/'i18n/locales.json').read_text(encoding="utf-8"))
    validate(source,locales)
    if args.init_pack:
        language=next(v for v in locales['languages'] if v['code']==args.init_pack)
        pack={'language':language['code'],'locale':language['locale'],'direction':language['direction'],
              'catalogRevision':source['catalogRevision'],'status':'source' if language['code']=='pt' else 'not_started',
              'sourceHash':source_hash(source),
              'editorialQa':'not_reviewed','layoutQa':'not_reviewed','nativeReview':'not_reviewed',
              'strings':{k:v['text'] if language['code']=='pt' else None for k,v in source['entries'].items()}}
        validate_pack(pack,source,locales)
        print(json.dumps(pack,ensure_ascii=False,indent=2));return
    if args.validate_pack:
        validate_pack(json.loads(args.validate_pack.read_text(encoding="utf-8")),source,locales,args.release)
        print('Pack validation passed.');return
    if args.release: raise ValueError('--release requires --validate-pack.')
    for path, content in files(source).items():
        if args.check:
            assert path.exists() and path.read_bytes()==content, str(path)+' is stale; run tools/i18n/generate.py.'
        else: path.parent.mkdir(parents=True,exist_ok=True);path.write_bytes(content)
    digest=hashlib.sha256('\n'.join(source['entries']).encode()).hexdigest()
    print(f'Catalog validated: {len(source["entries"])} keys, 51 locales, matrix SHA-256 {digest}.')

if __name__=='__main__':
    # Windows ANSI code pages cannot represent the canonical 51-language matrix.
    if hasattr(sys.stdout, 'reconfigure'): sys.stdout.reconfigure(encoding='utf-8')
    if hasattr(sys.stderr, 'reconfigure'): sys.stderr.reconfigure(encoding='utf-8')
    try: main()
    except (AssertionError, ValueError, KeyError, StopIteration) as error:
        print('Localization validation failed:',str(error),file=sys.stderr);sys.exit(1)
