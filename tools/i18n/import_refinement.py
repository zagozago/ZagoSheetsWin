"""Import the supplied CSV verbatim; structural UI decorations remain code/catalog data."""
import csv,json,re,collections,pathlib,hashlib
ROOT=pathlib.Path(__file__).resolve().parents[2]
def read(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def write(p,v):p.write_text(json.dumps(v,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
source=read(ROOT/'i18n/source.json');languages=read(ROOT/'i18n/locales.json')['languages']; locales={x['locale']:x['code'] for x in languages}
rows=list(csv.DictReader((ROOT/'i18n/refinement-20261024.csv').open(encoding='utf-8-sig',newline='')))
assert set(rows[0])==set('translation_id screen_ids change_type source_pt_br locale translation existing_key'.split())
groups=collections.defaultdict(list)
for r in rows:groups[r['translation_id']].append(r)
custom={'navigation.first_use':'header.firstUse','backup.space.used':'recovery.usedHeading','backup.count':'recovery.count','about.heading':'about.heading','google.connection.heading':'setup.connectionHeading','google.switch_account_short':'setup.switchAccountShort'}
na={'google.status.confirmed':'Fragment only; retain the existing complete authorization-state message, without concatenating translations.','advanced.xls.save_preference':'Catalog retained; autosave has no Save button. No button added.'}
mapping=[];imports={}
for tid,rs in groups.items():
 assert len(rs)==51 and collections.Counter(r['locale'] for r in rs)==collections.Counter(locales.keys()),tid
 assert len({r['source_pt_br'] for r in rs})==1 and len({r['existing_key'] for r in rs})==1,tid
 src=rs[0]['source_pt_br'];ph=collections.Counter(re.findall(r'\{\w+(?::[^}]+)?\}',src))
 for r in rs:
  assert r['translation'].strip() and collections.Counter(re.findall(r'\{\w+(?::[^}]+)?\}',r['translation']))==ph,(tid,r['locale'])
 if tid in na:
  mapping.append(dict(translation_id=tid,key=rs[0]['existing_key'],mode='not_applicable',reason=na[tid]));continue
 key=custom.get(tid) or rs[0]['existing_key'] or tid
 existing=source['entries'].get(key);values={locales[r['locale']]:r['translation'] for r in rs}
 # Preserve native menu indicators, ellipses and existing mnemonic characters.
 if existing:
  for code in values:
   old=existing['text'] if code=='pt' else read(ROOT/f'i18n/packs/{code}.json')['strings'][key]
   new=values[code]
   if '&' in old and '&' not in new:
    char=old[old.index('&')+1:old.index('&')+2];idx=new.casefold().find(char.casefold())
    if idx>=0:new=new[:idx]+'&'+new[idx:]
    else:raise ValueError((tid,code,'Existing mnemonic not present in CSV'))
   for suffix in ['…',' ▾']:
    if old.endswith(suffix) and not new.endswith(suffix):new=new.rstrip('.')+suffix
   if old.startswith('○ ') and not new.startswith('○ '):new='○ '+new
   values[code]=new
 text=values['pt'];entry=dict(existing or dict(context='AS IS → TO BE 2026-10-24 supplied CSV: '+tid,scope='app',markup=[],notes=[]))
 entry.update(text=text,placeholders=re.findall(r'\{(\w+)(?::[^}]+)?\}',text),invariants=[x for x in (existing or {}).get('invariants',[]) if x in text])
 source['entries'][key]=entry;imports[key]=values
 mapping.append(dict(translation_id=tid,key=key,mode='reuse' if rs[0]['existing_key']==key else 'create',reason='Existing aggregate/title has different semantics; contextual key.' if key in custom and key!=rs[0]['existing_key'] else 'CSV translation; native UI decorations preserved.'))
source['catalogRevision']=17;source['productVersion']='0.9.47';write(ROOT/'i18n/source.json',source)
hash_=hashlib.sha256(json.dumps(source['entries'],ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest()
for lang in languages:
 code=lang['code']
 if code=='pt':continue
 path=ROOT/f'i18n/packs/{code}.json';pack=read(path)
 for k,v in imports.items():pack['strings'][k]=v[code]
 pack['catalogRevision']=source['catalogRevision'];pack['sourceHash']=hash_
 if code!='pt':pack['sourceMatchesReviewed']=sorted(set(pack.get('sourceMatchesReviewed',[]))|{k for k,v in imports.items() if v[code]==v['pt']})
 write(path,pack)
write(ROOT/'docs/implementation/refinement-20261024-mapping.json',dict(csv='i18n/refinement-20261024.csv',rows=len(rows),locales=51,mappings=mapping))
print(len(groups),'messages validated; mapping written; no translations generated.')
