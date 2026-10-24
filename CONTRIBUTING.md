# Contributing to ZagoSheetsWin

ZagoSheetsWin is free and open-source software under the MIT license. It is developed by Zagotools and derived from [Open in Google](https://github.com/SwatiK425/open-in-google) by Swati K. **Preserve** the original copyright, `LICENSE`, `ATTRIBUTION.md`, `NOTICE.md` and all applicable `third-party/` notices. Contributions are not an endorsement from the upstream author.

## Which branch?

As of 24 October 2026, **`feature/zagosheetswin`** contains the current C#/.NET application (alpha 0.9.48; RTL hotfix 08dbced has a dedicated CI run, while preceding UI distribution passed on another SHA); the default `main` historically carries the international README and 50 translations, not this application. **Choose your PR base deliberately.** Do not assume `main` contains a stable release. Version 1.0.0 has not been published.

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
5. Respect copy-only rules for synchronized/network locations and user preferences. **Separate text and non-text verification:** CSV/TSV differences in exported cell values **block source retirement and shortcut publication**, while complex XLS/ODS mismatches may be warnings under the 0.9.33 policy. Preserve verified backup and explain conversion risks.
6. Do not automatically overwrite remote Sheets after local changes. Preserve the read-only file attribute on failed retirement and allow safe resume after the user changes it.
7. Do not break recovery compatibility or discard local state without a migration plan.
8. Keep CSV/TSV **interpretation version and options immutable per operation** once remote intent is created. Use explicit **repair into a new document** for incorrectly imported old data; never rewrite the prior Google document or shortcut.
9. Include 60,000-row/600,000-cell **text-only v2** boundary cases in parser and export verification tests; maintain old limits for legacy formats and operations. Keep permanent failures non-retryable until source changes; preserve terminal failure UI and attempt-scoped diagnostics.

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
- Check native UI recovery/backups column sorting, auto-save policy, conversion confirmation, read-only error rendering, per-import text chooser, explicit backup repair, restored-window errors, and localized messages in the 51-language checks when changing related code.
- Update the [CHANGELOG](CHANGELOG.md) under **Unreleased** for meaningful implemented changes; do not fabricate a GitHub Release.
- Update [current documentation](docs/README.md) and [stable readiness checklist](docs/PRE_RELEASE_CHECKLIST.md) when applicable.

Historical engineering notes may remain in Portuguese; translating them is not a prerequisite for external contributors. Report security vulnerabilities responsibly rather than publishing secrets in a PR or issue.


## Current 0.9.48 change-control expectations

- **CSV/TSV v4:** 100,000 rows / 2,000,000 cells, with 1,000 columns and 20 MiB source/payload; v1–v3 unchanged. Test exported XML budget 128 MiB/160 MiB text-only, empty cells and literal values.
- **Backup management:** cleanup is **manual only**. Do not reintroduce automatic retention cleanup, quota rotation, capacity polling or maintenance after import without an explicit product decision. Retain atomic snapshot quota admission; quota failures preserve source and all earlier backup data.
- **Concurrency:** different source identities progress concurrently; **there is no backup-capacity wait** and no global import queue. Keep locks on source identity and a short atomic quota/snapshot section.
- **Google retry:** rate-limit 403 only for recognized API reasons on reads, not arbitrary 403 or remote POST.
- **Release gates:** [Local core 39346495162](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) and [protected distribution 39346495298](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) passed on the exact 0.9.43 SHA `9fbec2a`; still validate real Google imports, signed artifact status and 1.0 release commit separately.

- **Low-memory UI:** simulate `OutOfMemoryException` and verify minimal **Close** path without attempts to serialize logs or construct more controls; test home window fixed-size/minimize/restore and 500×192 client dimensions.
- **Remote uncertainty:** marker reconciliation may perform two bounded read-only rechecks, never an extra POST/upload session. Avoid sensitive details in HTTP acknowledgment logs.

- **Pending operations (0.9.44):** new independent openings may start a fresh operation only after safe reconciliation confirms earlier document missing; keep old backups/associations/history; never equate failed network/ambiguous responses or same-opening retry with absence.
- **UI (0.9.45–0.9.48):** verify approved 85×51 localized strings, both themes and DPI 100/125/150/200, including RTL. Batch operations use `SelectedItems` and keep `CleanupState` eligibility.
- **RTL hotfix:** `InsetFlowLayoutPanel` must maintain **physical left/right insets** only on Home/Settings as specified. Do not inadvertently alter Core/Infrastructure or backup policy while changing RTL.
- **Release gate:** pass Local core and protected distribution on the **exact selected SHA**; CI for the hotfix and Google acceptance are distinct checks.
