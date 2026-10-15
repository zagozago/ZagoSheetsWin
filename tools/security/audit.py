#!/usr/bin/env python3
"""Fail closed on release-version drift, obvious tracked secrets and unsafe workflow triggers."""
import argparse, hashlib, json, pathlib, re, subprocess, sys, xml.etree.ElementTree as ET
ROOT = pathlib.Path(__file__).resolve().parents[2]
def audit():
    version = ET.parse(ROOT/'src/SheetsWindows.Windows/SheetsWindows.Windows.csproj').findtext('./PropertyGroup/Version')
    if not version or not re.fullmatch(r'\d+\.\d+\.\d+', version): raise ValueError('Invalid launcher version')
    source = json.loads((ROOT/'i18n/source.json').read_text(encoding='utf-8-sig'))
    if source['productVersion'] != version: raise ValueError('Catalog and launcher versions differ')
    installer = (ROOT/'installer/SheetsWindows.iss').read_text()
    if f'#define PilotVersion "{version}"' not in installer: raise ValueError('Installer fallback version differs')
    if f'ZagoSheetsWin {version}' not in (ROOT/'third-party/NOTICE.md').read_text(): raise ValueError('Notice version differs')
    try:
        listed = subprocess.check_output(['git','ls-files','-z'], cwd=ROOT, stderr=subprocess.DEVNULL).decode().split('\0')
        paths = [ROOT/p for p in listed if p]
    except (subprocess.CalledProcessError, FileNotFoundError):
        paths = [p for p in ROOT.rglob('*') if p.is_file() and not set(p.parts)&{'bin','obj','artifacts','__pycache__'}]
    patterns = [re.compile(r'-----BEGIN (?:RSA |EC |OPENSSH )?PRIVATE KEY-----'),
                re.compile(r'GOCSPX-[A-Za-z0-9_-]{20,}'), re.compile(r'AIza[A-Za-z0-9_-]{35}')]
    scanned = 0
    for p in paths:
        if p.suffix.lower() not in {'.cs','.json','.yml','.yaml','.ps1','.py','.md','.props','.csproj','.iss'} or not p.exists(): continue
        text = p.read_text(encoding='utf-8-sig'); scanned += 1
        if any(pattern.search(text) for pattern in patterns): raise ValueError('Potential tracked credential: '+str(p.relative_to(ROOT)))
    for p in (ROOT/'.github/workflows').glob('*.yml'):
        text = p.read_text()
        if 'pull_request_target' in text: raise ValueError('Privileged PR trigger requires separate security review')
        for action in re.findall(r'^\s*-?\s*uses:\s*([^\s]+)', text, re.M):
            if not action.startswith('./') and not re.fullmatch(r'[^@]+@[a-f0-9]{40}', action): raise ValueError('Unpinned workflow action: '+action)
        if p.name != 'distribution.yml' and 'ZAGOSHEETS_OAUTH_DESKTOP_JSON' in text: raise ValueError('OAuth config leaked into ordinary CI')
        if p.name == 'distribution.yml' and 'environment: zagosheets-distribution' not in text: raise ValueError('Distribution gate absent')
    return {'version':version,'scannedTextFiles':scanned,'trackedCredentialPatterns':'none detected','scope':'current tree; not Git history or runtime penetration testing'}
def dependencies(path):
    data=json.loads(path.read_text(encoding='utf-8-sig')); found=[]
    for project in data.get('projects',[]):
        for framework in project.get('frameworks',[]):
            for group in ['topLevelPackages','transitivePackages']:
                for package in framework.get(group,[]):
                    for item in package.get('vulnerabilities',[]):
                        found.append({'package':package['id'],'version':package.get('resolvedVersion'),'severity':item.get('severity'),'advisory':item.get('advisoryurl')})
    if found: raise ValueError('Vulnerable dependencies: '+json.dumps(found))
    if not data.get('projects'): raise ValueError('Dependency audit returned no projects')
    if data.get('logs') and any(x.get('level') in {'error','warning'} for x in data['logs']): raise ValueError('Dependency audit feed failed')
    return {'knownVulnerabilities':0,'projects':len(data.get('projects',[]))}
if __name__=='__main__':
    parser=argparse.ArgumentParser();parser.add_argument('--dependencies',type=pathlib.Path);args=parser.parse_args()
    try: print(json.dumps(dependencies(args.dependencies) if args.dependencies else audit(),ensure_ascii=False))
    except (ValueError,KeyError,ET.ParseError) as error: print('Security check failed:',error,file=sys.stderr);sys.exit(1)
