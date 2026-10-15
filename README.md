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

**Status: alpha — current development candidate 0.9.30. Version 1.0.0 has not been released.** The active application code is on `feature/zagosheetswin`; `main` has historically hosted multilingual documentation and is not the authoritative code branch for alpha builds.

> Documentation translations are retained as snapshots. For up-to-date behavior, limits and safety restrictions, use this English README and the [current documentation index](docs/README.md). Availability of 51 interfaces does not imply that every translation has completed native editorial review.

## How it works

With a Google account authorized and a supported file type associated in Windows, open a spreadsheet through the app or double-click it:

1. ZagoSheetsWin validates the local file and creates a recoverable backup.
2. The app imports and converts the file through the official Google Drive API into a native Google Sheets document.
3. When replacement is allowed and verified, the app creates a `.url` Internet shortcut **in the original folder** and retires the original file there. The recoverable backup remains in the app's private local storage.
4. The browser opens the Google Sheets document. Later opening that same shortcut goes straight to the remote document without another upload.

Replacement is **not unconditional**. Import-as-copy keeps the source file and can create a shortcut in the app's private shortcut folder. Protected or synchronized locations, unverifiable workbook features and conversion failures can force copy-only behavior or stop the operation.

There is **no bidirectional synchronization**: subsequent online edits are not copied back into the original Excel/ODS/CSV/TSV file or its original backup. ZagoSheetsWin does not silently overwrite a remote Google Sheets document after the local file changes.

## Supported spreadsheet formats

| Input | Current alpha behavior | Important limitations |
|---|---|---|
| `.xlsx` | Native Sheets import; eligible local files may be replaced by a shortcut | VBA/macros are not supported; native import does not guarantee visual/functional equivalence |
| `.xls` | Binary Excel is converted locally to XLSX; some HTML exports named `.xls` are read as tables | Formulas, VBA, drawings and other advanced features can require copy-only import; original backup is retained |
| `.ods` | Real OpenDocument packages are parsed and imported using the ODS media type | Complex formulas, dates, styles, merges or unsupported structures may require copy-only import; malformed/renamed HTML `.ods` is rejected; real-world coverage remains under validation |
| `.csv` | Parsed and normalized to XLSX with literal text cells | Encoding and delimiter must be unambiguous; text is not automatically converted to dates/numbers/formulas |
| `.tsv` | Tab-separated input normalized with literal text cells | Strict parsing and encoding checks apply |

The file input limit is **20 MiB**. Table processing limits include **500,000 cells**, **50,000 rows**, **1,000 columns** and **32,767 characters per text field**; additional format, export and package limits apply. Import success does **not** certify full visual fidelity. See [formats and limitations](docs/FORMATS_AND_LIMITS.md).

## Installation and first use

1. Use the [Protected alpha distribution workflow](https://github.com/zagozago/ZagoSheetsWin/actions/workflows/distribution.yml) for a successful, reviewed build of the intended commit. Extract the installer artifact and verify its published checksum/build metadata when supplied. Actions artifacts can expire. There is currently **no stable GitHub Release**.
2. Run `ZagoSheetsWin-Setup-win-x64.exe` on Windows x64. Installation is per-user; it does not require manually editing Windows defaults.
3. Complete the three-page first-use assistant, including Google authorization through your browser. You do not need to create a Google Cloud project for ordinary distribution builds with the bundled desktop OAuth configuration.
4. Use **Windows Settings → Apps → Default apps** to select ZagoSheetsWin for the supported extensions you want to open by double-click. The app registers as an option but does not silently change your defaults.
5. Start with a disposable test spreadsheet and verify the resulting Sheets file, local shortcut, backup and restoration.

The application also has a native four-page **Help/tutorial** dialog. It is distinct from the three-page first-use assistant. The default appearance is **light theme**; dark theme can be selected.

For current procedures use the [user guide](docs/USER_GUIDE.md) and [troubleshooting](docs/TROUBLESHOOTING.md). **Do not follow `SETUP.md` for installation of this application:** that file documents the legacy upstream PowerShell tool.

## Backups and recovery

A snapshot of the original bytes is kept before replacement. The default policy is **30 days of retention** with a **200 MB storage quota**, configurable up to **1,000 MB (1 GB)**. **Automatic cleanup is opt-in** and is performed during application operations, not by a resident background service. Deleting a completed backup prevents restoring those original bytes; completed backups can be selected and deleted only with confirmation. Pending or uncertain operations have additional safeguards.

From **Backups/Recovery**, users can restore to a new local destination, continue an existing operation as a copy, conclude an eligible replacement, and export privacy-filtered diagnostics. Do not delete `%LOCALAPPDATA%\SheetsWindows` as a troubleshooting step: it contains recovery state. The uninstaller intentionally preserves local state and Google documents.

## Google, security and privacy

The app uses a desktop OAuth flow with PKCE and the Google Drive scope `https://www.googleapis.com/auth/drive.file`. User tokens are protected locally with Windows DPAPI. Spreadsheet contents go directly to Google APIs rather than through a Zagotools server. Diagnostic files do not include cell contents, OAuth tokens or Google account identifiers. The suggested diagnostic **filename** may include a sanitized local base name: review filenames before sharing.

The source is available under the **MIT license**, preserving copyright and attribution for Swati K / [Open in Google](https://github.com/SwatiK425/open-in-google). Third-party libraries may have separate license terms; review [LICENSE](LICENSE), [ATTRIBUTION.md](ATTRIBUTION.md), [NOTICE.md](NOTICE.md) and [third-party notices](third-party/NOTICE.md).

Privacy and terms: [Privacy](https://zagotools.top/legal.html#privacidade), [Terms](https://zagotools.top/legal.html#termos), [Licenses](https://zagotools.top/legal.html#licencas). A future web help center is **planned, not currently declared live**; for now use the versioned [documentation index](docs/README.md).

## Languages and accessibility

The Windows application and installer provide **51 language selections**, following the canonical T51 ordering in `i18n/locales.json`. Selected languages are preserved; automatic selection follows Windows preferences. The UI supports right-to-left layouts for relevant languages, Windows font fallback, native UI controls and light/dark themes. Localization packaging and automated checks do not certify every translation's editorial quality.

## Project documentation and development

- [Documentation index: current vs. historical](docs/README.md)
- [User guide](docs/USER_GUIDE.md)
- [Current architecture](docs/ARCHITECTURE_CURRENT.md)
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
