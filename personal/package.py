"""Package an existing Windows runtime snapshot; never reads saves or logs."""
import argparse
import hashlib
import json
import zipfile
from datetime import datetime
from pathlib import Path
from zoneinfo import ZoneInfo


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    for name in ('runtime', 'dotnet', 'nuget', 'output', 'revision'):
        parser.add_argument('--' + name, required=True)
    parser.add_argument('--dotnet-version', default='10.0.13')
    args = parser.parse_args()
    runtime, dotnet, nuget = (Path(getattr(args, k)).resolve() for k in ('runtime', 'dotnet', 'nuget'))
    repo = Path(__file__).resolve().parents[1]
    output = Path(args.output).resolve()
    output.parent.mkdir(parents=True, exist_ok=True)
    if output.exists():
        raise FileExistsError(f'Refusing to overwrite {output}')
    files = {}

    def add(source, target):
        source = Path(source)
        if not source.is_file():
            raise FileNotFoundError(source)
        if target in files:
            raise ValueError(f'Duplicate archive name: {target}')
        files[target] = source.read_bytes()

    def tree(source, target):
        source = Path(source)
        if not source.is_dir():
            raise FileNotFoundError(source)
        for path in sorted(source.rglob('*')):
            if path.is_file():
                add(path, target + '/' + path.relative_to(source).as_posix())

    for name in ('tModLoader.dll', 'tModLoader.pdb', 'tModLoader.xml', 'tModLoader.deps.json',
                 'tModLoader.runtimeconfig.json', 'tModLoader.runtimeconfig.dev.json', 'tMLMod.targets'):
        add(runtime / name, name)
    tree(runtime / 'Libraries', 'Libraries')
    tree(runtime / 'BuildHost-netcore', 'BuildHost-netcore')
    add(dotnet / 'dotnet.exe', 'dotnet/dotnet.exe')
    add(dotnet / 'LICENSE.txt', 'dotnet/LICENSE.txt')
    add(dotnet / 'ThirdPartyNotices.txt', 'dotnet/ThirdPartyNotices.txt')
    for part in ('host/fxr', 'shared/Microsoft.NETCore.App'):
        tree(dotnet / part / args.dotnet_version, 'dotnet/' + part + '/' + args.dotnet_version)
    add(repo / 'LICENSE', 'LICENSE')
    add(repo / 'README.md', 'README.md')
    for name in ('README.md', 'UPSTREAM_README.md', 'native/mojoshader-sampler-span.patch'):
        add(repo / 'personal' / name, 'personal/' + name)
    for name in ('Launch.ps1', 'Start-Personal.cmd'):
        # Windows cmd.exe needs CRLF; normalize PowerShell too.
        content = (repo / 'personal' / name).read_text(encoding='utf-8')
        files[name] = content.replace('\r\n', '\n').replace('\n', '\r\n').encode('utf-8')
    files['UserData/Mods/enabled.json'] = b'[]\n'

    # Keep license texts and package metadata alongside bundled dependencies.
    fna = repo / 'FNA'
    for path in sorted(fna.rglob('*')):
        if path.is_file() and any(s in path.name.lower() for s in ('license', 'copying', 'notice')):
            add(path, 'licenses/FNA/' + path.relative_to(fna).as_posix())
    deps = json.loads((runtime / 'tModLoader.deps.json').read_text(encoding='utf-8'))
    for name, entry in deps['libraries'].items():
        if entry.get('type') != 'package':
            continue
        package = nuget / name.lower()
        if not package.is_dir():
            raise FileNotFoundError(f'License metadata package missing: {package}')
        for path in sorted(package.rglob('*')):
            if path.is_file() and (path.suffix == '.nuspec' or any(s in path.name.lower() for s in ('license', 'copying', 'notice'))):
                add(path, 'licenses/nuget/' + name + '/' + path.relative_to(package).as_posix())
    manifest = {
        'edition': '1.4.5.8 chu123122个人版',
        'created': datetime.now(ZoneInfo('Asia/Shanghai')).isoformat(),
        'sourceRevision': args.revision,
        'platform': 'Windows x64',
        'dotnetRuntime': args.dotnet_version,
        'buildNote': 'Existing local runtime snapshot, Debug IL with post-build JIT flags adjusted; source revision records publication snapshot, not reproducible-build attestation.',
        'excluded': ['Terraria Content', 'personal saves', 'logs', 'crash dumps', 'mod packages', '.NET SDK'],
        'files': [{'path': n, 'bytes': len(b), 'sha256': hashlib.sha256(b).hexdigest()} for n, b in sorted(files.items())],
    }
    files['PACKAGE-MANIFEST.json'] = (json.dumps(manifest, ensure_ascii=False, indent=2) + '\n').encode('utf-8')
    for name in files:
        if name.lower().endswith(('.tmod', '.wld', '.twld', '.plr', '.tplr', '.dmp', '.log')) or 'tmodloader-logs' in name.lower():
            raise ValueError(f'Unexpected private artifact: {name}')
    with zipfile.ZipFile(output, 'x', zipfile.ZIP_DEFLATED, compresslevel=6) as archive:
        for name, data in sorted(files.items()):
            archive.writestr(name, data)
    sha256 = hashlib.sha256(output.read_bytes()).hexdigest()
    output.with_name('SHA256SUMS.txt').write_text(f'{sha256}  {output.name}\n', encoding='ascii')
    print(json.dumps({'package': str(output), 'bytes': output.stat().st_size, 'sha256': sha256, 'files': len(files)}, indent=2))


if __name__ == '__main__':
    main()
