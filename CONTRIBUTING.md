# Contributing to ZagoSheetsWin

Thank you for your interest in ZagoSheetsWin. The project is free and open source under the MIT license. Please preserve attribution to [Open in Google](https://github.com/SwatiK425/open-in-google) and all applicable third-party notices.

## Development status

ZagoSheetsWin is in active alpha development. The current C#/.NET and Windows Forms application is maintained on `feature/zagosheetswin` while release preparation continues. The default `main` branch may not yet represent the current application and installer. Check the target branch before submitting a change.

The application is intended for Windows 11. Other operating systems may be used for some compilation and tests, but native Windows installation, Explorer integration and user-interface checks require Windows.

## Prerequisites

- The .NET SDK version selected by [`global.json`](global.json) (currently **10.0.401**).
- Python 3 for the localization generator and catalogue checks.
- **Inno Setup 6** on Windows only if you want to build the installer.
- A Windows test environment for native UI, installer, and file-association acceptance tests.

No real Google account or OAuth credentials are required for the core offline build and test commands below.

## Build and test

From the repository root, on the current application branch:

```powershell
dotnet restore SheetsWindows.slnx --locked-mode
dotnet build SheetsWindows.slnx --configuration Release --no-restore
dotnet test SheetsWindows.slnx --configuration Release --no-build --no-restore
```

Check the source catalogs, ordering and generated localization files:

```powershell
python tools/i18n/generate.py --check
python tools/i18n/test_catalog.py
python tools/i18n/generate.py --validate-pack i18n/packs/en.json --release
```

To publish the Windows launcher for local packaging:

```powershell
dotnet publish src/SheetsWindows.Windows --configuration Release --runtime win-x64 --self-contained true --output artifacts/SheetsWindows-win-x64
```

To compile the installer on Windows, install Inno Setup 6 and compile `installer/SheetsWindows.iss` with `ISCC.exe` after publishing the launcher. The existing [CI workflows](.github/workflows) are the reference for automated checks and installer verification.

**Important:** Passing offline tests does not certify a real Google Drive import or safe replacement of a user's files. Use test data and a disposable Windows profile for installer and destructive-operation tests.

## Localization

The 51-language ordering and language metadata come from [`i18n/locales.json`](i18n/locales.json). The text catalogs live in `i18n/source.json` and `i18n/packs/`. Generated resources are validated by `tools/i18n/generate.py`; edit the source catalogs rather than manually changing generated files.

Translation quality, native-language editorial review, rendering, and runtime verification are separate concerns. A successful catalog validation is not a claim of completed native-language review.

## Data safety and privacy

Changes touching import, backup, remote conversion, shortcut replacement, authentication or local state require particularly careful tests. Preserve recoverable originals when an operation fails, avoid unintended reuploads and remote overwrites, and do not commit real user documents.

Never commit OAuth credentials, refresh or access tokens, private license keys, user logs, backups, or local configuration containing secrets. The official alpha distribution uses protected GitHub Actions secrets that are not necessary for ordinary source review and offline unit tests.

## Pull requests

- Open small, focused changes against the branch that contains the code being modified.
- Describe the user impact, the failure or behavior addressed, and how you tested it.
- Include regression tests for behavioral changes, especially recovery and data-loss scenarios.
- Keep copyright, MIT license, upstream attribution and third-party notices intact.
- Do not change the 51-language order or labels independently of their canonical metadata.

Engineering notes in [`docs/`](docs/) may remain in Portuguese. Translation of historical planning documents is not a contribution requirement.
