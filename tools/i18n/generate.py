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
            assert text.count(token) == entry['text'].count(token), (key, token)
        if '|' in entry['text']: assert text.count('|') == entry['text'].count('|'), key
        def visible(value):
            value=INNO.sub('',value) if entry['scope'].startswith('installer.') else NAMED.sub('',value)
            for token in sorted(entry['invariants'],key=len,reverse=True): value=value.replace(token,'')
            return value
        original=visible(entry['text']); translated=visible(text)
        def digits(value):
            return [unicodedata.digit(c) for c in value if c.isdecimal()]
        assert digits(original)==digits(translated), (key,'functional numbers changed')
        if pack['language'] in SCRIPT_RANGES and any(c.isalpha() for c in original):
            assert any(low<=ord(c)<=high for c in translated for low,high in SCRIPT_RANGES[pack['language']]), (key,'expected script absent')
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
    return {
        ROOT/'i18n/generated/pt.json': (json.dumps(runtime, ensure_ascii=False, indent=2)+'\n').encode('utf-8'),
        ROOT/'installer/i18n/pt.isl': ('\n'.join(custom)+'\n').encode('utf-8-sig')
    }

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
