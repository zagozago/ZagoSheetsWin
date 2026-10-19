# ZagoSheetsWin

<details>
<summary>🌐 Documentation languages · Choose your language</summary>

- **EN - English** — current page
- [ZH - 中文 / Chinese (Simplified) (zh)](docs/i18n/zh/README.md)
- [HI - हिन्दी / Hindi (hi)](docs/i18n/hi/README.md)
- [ES - Español / Spanish (es)](docs/i18n/es/README.md)
- [AR - العربية / Arabic (ar)](docs/i18n/ar/README.md)
- [FR - Français / French (fr)](docs/i18n/fr/README.md)
- [BN - বাংলা / Bengali (bn)](docs/i18n/bn/README.md)
- [PT - Português / Portuguese (pt)](docs/i18n/pt/README.md)
- [ID - Bahasa Indonesia / Indonesian (id)](docs/i18n/id/README.md)
- [UR - اردو / Urdu (ur)](docs/i18n/ur/README.md)
- [RU - Русский / Russian (ru)](docs/i18n/ru/README.md)
- [DE - Deutsch / German (de)](docs/i18n/de/README.md)
- [JA - 日本語 / Japanese (ja)](docs/i18n/ja/README.md)
- [VI - Tiếng Việt / Vietnamese (vi)](docs/i18n/vi/README.md)
- [TR - Türkçe / Turkish (tr)](docs/i18n/tr/README.md)
- [KO - 한국어 / Korean (ko)](docs/i18n/ko/README.md)
- [IT - Italiano / Italian (it)](docs/i18n/it/README.md)
- [TH - ไทย / Thai (th)](docs/i18n/th/README.md)
- [TL - Filipino](docs/i18n/fil/README.md)
- [MS - Bahasa Melayu / Malay (ms)](docs/i18n/ms/README.md)
- [SW - Kiswahili / Swahili (sw)](docs/i18n/sw/README.md)
- [PCM - Nigerian Pidgin](docs/i18n/pcm/README.md)
- [MR - मराठी / Marathi (mr)](docs/i18n/mr/README.md)
- [TE - తెలుగు / Telugu (te)](docs/i18n/te/README.md)
- [HA - Hausa](docs/i18n/ha/README.md)
- [PA - ਪੰਜਾਬੀ / Punjabi (pa)](docs/i18n/pa/README.md)
- [TA - தமிழ் / Tamil (ta)](docs/i18n/ta/README.md)
- [YUE - 粵語 / Cantonese (yue)](docs/i18n/yue/README.md)
- [FA - فارسی / Persian (fa)](docs/i18n/fa/README.md)
- [AM - አማርኛ / Amharic (am)](docs/i18n/am/README.md)
- [JV - Basa Jawa / Javanese (jv)](docs/i18n/jv/README.md)
- [GU - ગુજરાતી / Gujarati (gu)](docs/i18n/gu/README.md)
- [KN - ಕನ್ನಡ / Kannada (kn)](docs/i18n/kn/README.md)
- [YO - Yorùbá / Yoruba (yo)](docs/i18n/yo/README.md)
- [BHO - भोजपुरी / Bhojpuri (bho)](docs/i18n/bho/README.md)
- [PS - پښتو / Pashto (ps)](docs/i18n/ps/README.md)
- [OR - ଓଡ଼ିଆ / Odia (or)](docs/i18n/or/README.md)
- [MY - မြန်မာဘာသာ / Burmese (my)](docs/i18n/my/README.md)
- [ML - മലയാളം / Malayalam (ml)](docs/i18n/ml/README.md)
- [PL - Polski / Polish (pl)](docs/i18n/pl/README.md)
- [SU - Basa Sunda / Sundanese (su)](docs/i18n/su/README.md)
- [MAI - मैथिली / Maithili (mai)](docs/i18n/mai/README.md)
- [UK - Українська / Ukrainian (uk)](docs/i18n/uk/README.md)
- [OM - Afaan Oromoo / Oromo (om)](docs/i18n/om/README.md)
- [UZ - Oʻzbekcha / Uzbek (uz)](docs/i18n/uz/README.md)
- [SD - سنڌي / Sindhi (sd)](docs/i18n/sd/README.md)
- [NE - नेपाली / Nepali (ne)](docs/i18n/ne/README.md)
- [IG - Igbo](docs/i18n/ig/README.md)
- [AZ - Azərbaycanca / Azerbaijani (az)](docs/i18n/az/README.md)
- [NL - Nederlands / Dutch (nl)](docs/i18n/nl/README.md)
- [GN - Avañe’ẽ / Guarani (gn)](docs/i18n/gn/README.md)

</details>

**Open local spreadsheet files in Google Sheets from Windows.** ZagoSheetsWin is a free, open-source Windows application developed by **Zagotools** and derived from [Open in Google](https://github.com/SwatiK425/open-in-google).

**Status: alpha — current development candidate 0.9.42. Version 1.0.0 has not been released.** The active application code is on `feature/zagosheetswin`; `main` has historically hosted multilingual documentation and is not the authoritative code branch for alpha builds.

> Documentation translations are retained as snapshots. For up-to-date behavior, limits and safety restrictions, use this English README and the [current documentation index](docs/README.md). Availability of 51 interfaces does not imply that every translation has completed native editorial review.

## How it works

With a Google account authorized and a supported file type associated in Windows, open a spreadsheet through the app or double-click it:

1. ZagoSheetsWin validates the local file and creates a recoverable backup.
2. The app imports and converts the file through the official Google Drive API into a native Google Sheets document.
3. When replacement is allowed and verified, the app creates a `.url` Internet shortcut **in the original folder** and retires the original file there. The recoverable backup remains in the app's private local storage.
4. The browser opens the Google Sheets document. Later opening that same shortcut goes straight to the remote document without another upload.

Replacement depends on **user preferences and safety checks**, but **full spreadsheet fidelity is not required for a verified Google import**. Since alpha 0.9.33, even when formulas, values or complex spreadsheet features cannot be proven equivalent, an otherwise successful import may replace the original file with a shortcut. The **verified local backup** is how you recover the original bytes. **Check the result in Google Sheets**, especially when using XLS or ODS. Choose **Import as copy** before opening the file if you want to keep the original in place. Synchronization/network protections or fatal validation failures may still preserve the original or block the operation. A Windows file marked **read-only** may block removal: the app reports an error and preserves the source and existing backup.

There is **no bidirectional synchronization**: subsequent online edits are not copied back into the original Excel/ODS/CSV/TSV file or its original backup. ZagoSheetsWin does not silently overwrite a remote Google Sheets document after the local file changes.

## Supported spreadsheet formats

| Input | Current alpha behavior | Important limitations |
|---|---|---|
| `.xlsx` | Native Sheets import; eligible local files may be replaced by a shortcut | VBA/macros are not supported; native import does not guarantee visual/functional equivalence |
| `.xls` | Binary Excel is converted locally to XLSX; some HTML exports named `.xls` are read as tables | Some formulas, values and advanced features can change: successful imports may replace the original; use the backup to recover the original bytes; VBA/macros are not preserved |
| `.ods` | Real OpenDocument packages are checked and imported using the original ODS bytes; sparse empty trailing cells are handled without expanding them in memory | Complex formulas, dates, styles and merges may change despite successful replacement; invalid/renamed HTML `.ods` is rejected; end-to-end Google validation remains pending |
| `.csv` | Strict parser v4 with comma/semicolon/TAB/pipe, `sep=` first-line hint, single-column/headerless text and literal cells | Real ambiguity prompts contextual choice and preview; unsafe quoting/irregular width is rejected; remotely exported values must match for replacement |
| `.tsv` | Text import with quoted/multiline fields and versioned interpretation | One-column TSV can be valid; confirmed mismatched values block replacement; legacy remote sessions keep their parser/payload |

The file input limit remains **20 MiB** (and the normalized upload must also fit). For **new CSV/TSV v4 imports**, limits are **100,000 logical rows, 2,000,000 cells and 1,000 columns**; historical parser versions retain their original limits. Other formats retain **50,000 rows / 500,000 cells** as applicable. The text field limit is **32,767 characters**. For text-only verification, XML/package ceilings are higher (128 MiB per worksheet XML; 160 MiB expanded package), not for XLS/ODS/XLSX generally. Import success is **not** complete visual/functional fidelity. See [formats and limitations](docs/FORMATS_AND_LIMITS.md).

## Improvements in alpha 0.9.36 and 0.9.37

**Text import (0.9.36–0.9.41):** CSV/TSV supports comma, semicolon, TAB, pipe, precise quote/multiline handling, single-column/headerless input, an optional first-line `sep=` hint, contextual choice with a three-row preview for genuinely ambiguous delimiters and bounded Windows-1252 fallback. Interpretation and upload payload are versioned per operation; **existing remote imports are never silently reinterpreted**. New CSV/TSV imports may contain up to **100,000 logical rows and 2,000,000 cells**, subject to the input/payload size and other limits. **For CSV/TSV, mismatched Google-exported cell values block replacement**; complex XLS/ODS fidelity differences remain warnings under the separate 0.9.33 policy.

**Explicit repair (0.9.36):** From **Backups → More actions → Reimport into a new document**, select the interpretation of a previously misread CSV/TSV. The application reads the verified original backup and creates a **separate** Google Sheets document. The old document, its shortcut, and original operation remain untouched. Repair requires the original backup to be available.

**Failure handling (0.9.36–0.9.39):** Error messages remain visible after minimizing/restoring, late progress does not overwrite terminal failure, and diagnostics distinguish the attempt from full history. A permanently invalid source is not retried automatically. Genuine delimiter ambiguity presents a highlighted **Choose separator** action and preview. Uncertain remote upload completion is reconciled against the **existing** operation marker, never blindly recreated; a missing shortcut can be safely repaired from journal state without another upload.

**Backup policy (0.9.39–0.9.42):** Cleanup of expired **completed** backups and oldest eligible completed backups to respect the configured quota is **automatic**, no cleanup toggle. Pending/uncertain backups remain protected. The 0.9.40 global import queue was superseded in 0.9.42: different files can upload in parallel; only **actual backup-capacity shortage** triggers a bounded, cancellable wait. An individual file larger than the backup quota is rejected immediately without deletion.

**Consistent icon (0.9.37):** The app, installer, native windows, file associations and `.url` shortcuts share the supplied spreadsheet icon. Existing Zagotools logos and upstream MIT credits remain unchanged.

## Installation and first use

1. Use the [Protected alpha distribution workflow](https://github.com/zagozago/ZagoSheetsWin/actions/workflows/distribution.yml) for a successful, reviewed build of the intended commit. Extract the installer artifact and verify its published checksum/build metadata when supplied. Actions artifacts can expire. There is currently **no stable GitHub Release**.
2. Run `ZagoSheetsWin-Setup-win-x64.exe` on Windows x64. Installation is per-user; it does not require manually editing Windows defaults.
3. Complete the three-page first-use assistant, including Google authorization through your browser. You do not need to create a Google Cloud project for ordinary distribution builds with the bundled desktop OAuth configuration.
4. Use **Windows Settings → Apps → Default apps** to select ZagoSheetsWin for the supported extensions you want to open by double-click. The app registers as an option but does not silently change your defaults.
5. Start with a disposable test spreadsheet and verify the resulting Sheets file, local shortcut, backup and restoration.

The application has a native four-page tutorial separate from the three-page first-use assistant. The **Help** button on the home screen opens a language-specific URL on Zagotools; **availability of that web page has not been verified**. For dependable instructions, use the versioned GitHub guides. The default appearance is **light theme**; dark theme can be selected.

For current procedures use the [user guide](docs/USER_GUIDE.md) and [troubleshooting](docs/TROUBLESHOOTING.md). **Do not follow `SETUP.md` for installation of this application:** that file documents the legacy upstream PowerShell tool.

## Backups and recovery

A snapshot of the original bytes is kept before replacement, including after conversions with uncertain fidelity. The default policy is **30 days of retention** with a **200 MB storage quota**, configurable up to **1,000 MB (1 GB)**. **Automatic cleanup is always enabled for eligible completed backups** (both time-based retention and quota rotation), during application operations rather than via a resident background service. Backup retention and quota settings **save automatically**. The cleanup toggle was removed. The list has **File, Date, Size, Status** headers that sort ascending/descending when clicked; multiple rows are selected directly, without checkboxes. Deleting selected backups requires confirmation, with stronger safeguards for pending operations. After a successful replacement, the app shows a local confirmation with a **Backups** button and a persistent **Don't show again** option (only one confirmation is shown at a time).

From **Backups/Recovery**, users can restore to a new local destination, continue an existing operation as a copy, conclude an eligible replacement, and export privacy-filtered diagnostics. Do not delete `%LOCALAPPDATA%\SheetsWindows` as a troubleshooting step: it contains recovery state. The uninstaller intentionally preserves local state and Google documents.

## Google, security and privacy

The app uses a desktop OAuth flow with PKCE and the Google Drive scope `https://www.googleapis.com/auth/drive.file`. User tokens are protected locally with Windows DPAPI. Spreadsheet contents go directly to Google APIs rather than through a Zagotools server. Diagnostic files do not include cell contents, OAuth tokens or Google account identifiers. The suggested diagnostic **filename** may include a sanitized local base name: review filenames before sharing.

The source is available under the **MIT license**, preserving copyright and attribution for Swati K / [Open in Google](https://github.com/SwatiK425/open-in-google). Third-party libraries may have separate license terms; review [LICENSE](LICENSE), [ATTRIBUTION.md](ATTRIBUTION.md), [NOTICE.md](NOTICE.md) and [third-party notices](third-party/NOTICE.md).

Privacy and terms: [Privacy](https://zagotools.top/legal.html#privacidade), [Terms](https://zagotools.top/legal.html#termos), [Licenses](https://zagotools.top/legal.html#licencas). A future web help center is **planned, not currently declared live**; for now use the versioned [documentation index](docs/README.md).

## Current interface preferences

Advanced Options contains Google client setup, folder/synchronization and replacement consent, plus the specific XLS replacement preference. **The global Formats and Text settings section was removed in 0.9.32**; all five formats remain supported. Existing text preferences remain available to legacy operations, while **the new 0.9.36 per-import text chooser** is shown when input ambiguity requires a decision or during an explicit repair. It does **not** restore the removed global settings section.

## Languages and accessibility

The Windows application and installer provide **51 language selections**, following the canonical T51 ordering in `i18n/locales.json`. Selected languages are preserved; automatic selection follows Windows preferences. The UI supports right-to-left layouts for relevant languages, Windows font fallback, native UI controls and light/dark themes. Localization packaging and automated checks do not certify every translation's editorial quality.

## Project documentation and development

- [Documentation index: current vs. historical](docs/README.md)
- [User guide](docs/USER_GUIDE.md)
- [Current architecture](docs/ARCHITECTURE_CURRENT.md)
- [Current alpha baseline and CI evidence](docs/RELEASE_STATUS.md)
- [Alpha 0.9.36 import review and verified test scope](docs/IMPORT_REVIEW_0936.md)
- [Regression reports 0.9.38–0.9.42](docs/REGRESSION_0942.md) and [stable-release readiness](docs/PRE_RELEASE_CHECKLIST.md)
- [Formats and limits](docs/FORMATS_AND_LIMITS.md)
- [Troubleshooting](docs/TROUBLESHOOTING.md)
- [Stable-release readiness checklist](docs/PRE_RELEASE_CHECKLIST.md)
- [Changelog (development milestones, not GitHub Releases)](CHANGELOG.md)
- [Contributing](CONTRIBUTING.md)
- [Historical development records](docs/HISTORY.md)

## Relationship with Open in Google

Based on / derived from **[Open in Google](https://github.com/SwatiK425/open-in-google)** by **Swati K (SwatiK425)**. The audited upstream reference is `124419b9ffce1f42c696ab9068fa9db6a4a9c199`. ZagoSheetsWin is an independent evolution built with a native C#/.NET Windows app, recovery journal, installer and integration with Google Sheets. The upstream project is independent and is not presented as endorsing this fork.

---

**ZagoSheetsWin — Zagotools.** Small software for real problems.
