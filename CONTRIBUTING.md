# Contributing to ZagoSheetsWin

ZagoSheetsWin is free and open-source software under the MIT license. It is developed by Zagotools and derived from [Open in Google](https://github.com/SwatiK425/open-in-google) by Swati K. **Preserve** the original copyright, `LICENSE`, `ATTRIBUTION.md`, `NOTICE.md` and all applicable `third-party/` notices. Contributions are not an endorsement from the upstream author.

## Which branch?

As of 15 October 2026, **`feature/zagosheetswin`** contains the current C#/.NET application; the default `main` historically carries the international README and 50 translations, not this application. **Choose your PR base deliberately.** Do not assume `main` contains a stable release. Version 1.0.0 has not been published.

For software changes, branch from the current application branch; for documentation changes, prefer the current documentation consolidation branch while it is under review. Avoid mixing source refactors and documentation cleanup in the same commit.

## Prerequisites

- .NET **10** SDK, respecting `global.json` (minimum selected SDK `10.0.401` with `latestPatch` roll-forward). The protected distribution workflow explicitly installs `10.0.402`.
- Python 3 to run the localization generator, catalogue checks and security scripts.
- Windows x64 for WinForms behavior, Windows registry/file-association checks and installer tests.
- Inno Setup 6 to compile the Windows installer.
- No production Google credentials are needed for ordinary offline build and tests.

## Build and check

```powershell
dotnet restore SheetsWindows.slnx --locked-mode
dotnet build SheetsWindows.slnx --configuration Release --no-restore
dotnet test SheetsWindows.slnx --configuration Release --no-build --no-restore

python tools/i18n/generate.py --check
python tools/i18n/test_catalog.py
python tools/i18n/generate.py --validate-pack i18n/packs/en.json --release
```

For a Windows publish without embedded production OAuth configuration:

```powershell
dotnet publish src/SheetsWindows.Windows --configuration Release --runtime win-x64 --self-contained true --output artifacts/SheetsWindows-win-x64
```

The protected distribution uses additional audited dependencies, client configuration and installer checks. **Do not run `installer/Test-Lifecycle.ps1` against a personal Windows profile**: it is intended for a disposable CI test environment. Follow `.github/workflows/local-core.yml` and `.github/workflows/distribution.yml` for current automated commands; do not copy historical SDK/version flags from old alpha stage files.

Passing offline tests is not proof that import, restoration or file removal work on a real Windows 11 computer and Google account. Use test documents, test accounts and a disposable profile for destructive tests.

## Data safety expectations

For any change touching import, schema, conversion, remote association, snapshot, backup policy, recovery or deletion:

1. Preserve original bytes when a required step is unverified.
2. Do not repeat a remote **create POST** blindly after uncertain network completion.
3. Keep account, source identity, hash and operation marker consistent.
4. Test concurrent opens, failed uploads, suspended sessions, interrupted retirement and recovery.
5. Respect the copy-only rule for synchronized/network locations and formats lacking verified fidelity.
6. Do not automatically overwrite remote Sheets after local changes.
7. Do not break recovery compatibility or discard local state without a migration plan.

Files under `%LOCALAPPDATA%\SheetsWindows` contain user state; never ask users to erase them as a routine update step.

## Localization: 51 languages

Canonical language ordering and identifiers: `i18n/locales.json`. Authoring sources: `i18n/source.json` and `i18n/packs/*.json`. Generated artifacts should be regenerated and verified, not edited manually. English fallback, right-to-left handling, visual layout and native/editorial review are distinct checks.

The root README contains a **separate collection of 50 translated documentation pages**, preserved under `docs/i18n/`. Update these through a reviewed translation process; avoid accidentally replacing them with an old English README. Do not claim that all 51 languages are editorially certified.

## Security and distribution

Never commit real spreadsheet files, personal logs, private backup content, tokens, OAuth refresh/access tokens, actual JSON credentials, secrets or private license material. Review any generated examples and diagnostic filenames for leaked names. Production desktop client configuration is supplied only through the approved protected workflow. A desktop `client_secret` embedded into shipped binaries cannot be treated like an unextractable server secret.

A passing dependency audit, a SHA-256 checksum, and a signed binary are different controls. Do not call a checksum a cryptographic publisher signature. If a dependency has commercial use conditions, review license eligibility before distribution.

## Pull-request checklist

- State base branch, change scope, potential data-loss risks and applicable issue.
- Include behavior/regression tests and their platform; distinguish mocked Drive tests from real Google testing.
- Identify any migration, backward-compatibility or translation impact.
- Include screenshots if WinForms UI changes, with high-DPI/RTL considerations.
- Maintain upstream credits and license notices.
- Update the [CHANGELOG](CHANGELOG.md) under **Unreleased** for meaningful implemented changes; do not fabricate a GitHub Release.
- Update [current documentation](docs/README.md) and [stable readiness checklist](docs/PRE_RELEASE_CHECKLIST.md) when applicable.

Historical engineering notes may remain in Portuguese; translating them is not a prerequisite for external contributors. Report security vulnerabilities responsibly rather than publishing secrets in a PR or issue.
