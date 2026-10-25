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

**Open local spreadsheet files directly in Google Sheets from Windows.**

ZagoSheetsWin is a lightweight Windows application that turns opening a local spreadsheet into a simple workflow:

**double-click the file → upload and convert → open in Google Sheets**

After a successful import, ZagoSheetsWin can replace the original local file with an Internet shortcut (`.url`) pointing to the Google Sheets document, while keeping a recoverable local backup of the original file.

The goal is simple: make Google Sheets feel like a native Windows application for opening spreadsheet files.

## What it does

ZagoSheetsWin integrates spreadsheet files with Windows and Google Sheets.

When you open a supported local file, the application can:

- detect and validate the spreadsheet;
- create a recoverable backup;
- upload it directly to Google Drive using Google's official APIs;
- convert it to a native Google Sheets document;
- open the resulting spreadsheet in your default browser;
- create a local `.url` shortcut to the Google document;
- avoid uploading the same file again on subsequent opens.

No manual Drive upload. No browser navigation. No repeated conversion workflow.

## Supported formats

Supported formats:

- `.xlsx`
- `.xls`
- `.ods`
- `.csv`
- `.tsv`

Some formats may have additional compatibility restrictions. Files containing features that cannot be safely preserved are handled conservatively to avoid silent data loss.

## Languages and localization

ZagoSheetsWin includes **51 languages** across the Windows application and installer. You can choose a language during installation or in the app. Automatic language detection follows your Windows language preferences; a language you choose explicitly takes precedence and is preserved across updates.

The languages follow the project's **T51 order**, organized primarily by estimated numbers of speakers rather than alphabetically. Names are displayed in their native scripts, with English names alongside them where appropriate. Localization also supports right-to-left scripts.

**The language selector at the top of this README changes the documentation language, not the application's settings.** The application and installer have their own language selection controls.

The application and installer include the validated 51-language catalog. Translated README pages are preserved from earlier development; this page and the release notes describe the current distribution status.

## Designed for Windows

ZagoSheetsWin is built specifically for Windows and integrates with the operating system through:

- **Open with**
- file type registration
- double-click handling
- optional Explorer integration
- a native Windows installer

The application does not silently override your Windows default applications. File associations remain under the user's control.

## Safe by design

ZagoSheetsWin treats the local file replacement as a recoverable operation.

Before removing an original file from its folder, the application verifies that:

1. a recoverable backup exists;
2. the Google Sheets document was successfully created;
3. the local association was persisted;
4. the Internet shortcut was successfully written and validated.

If the process fails, the original file is preserved.

The application is intentionally designed around a simple rule:

> Never destroy user data silently.

## Backups

Original files can be stored in a private local backup area before being replaced by shortcuts.

Backup management includes configurable storage limits, retention policies and cleanup controls.

Backups protect the original imported file. They are **not** a bidirectional synchronization system: changes made later in Google Sheets are not written back to the original spreadsheet file.

## Privacy and Google access

ZagoSheetsWin communicates directly from your computer to Google's APIs.

- No Zagotools server receives your spreadsheet contents.
- OAuth tokens are stored locally and protected using Windows security mechanisms.
- The application uses the Google Drive `drive.file` scope, limiting access to files created or opened through the application.
- No analytics are required for the spreadsheet import workflow.

Privacy Policy and Terms of Use:

https://zagotools.top/legal.html

## Project status

Version **1.0.0 is being prepared for public release**, based on the manually accepted 0.9.48 application. No new import, authentication or recovery behavior is introduced in this release preparation.

Official installers will be published in [GitHub Releases](https://github.com/zagozago/ZagoSheetsWin/releases). Until the 1.0.0 release and its assets are published, this repository does not provide a final 1.0 download.

### Installation and authenticity

- Windows 11, x64; per-user installation, without administrator privileges.
- Download `ZagoSheetsWin-Setup-win-x64.exe` from the official Release, together with `SHA256SUMS.txt` and `build-info.json`.
- The EXE is **not Authenticode-signed** (`NotSigned`). Windows may display an unknown-publisher or reputation warning. A hash identifies the downloaded bytes against the published reference; it does not certify the publisher or absence of malware.
- Check the version, source commit and SHA-256 before installation. Do not disable Windows security features. An explicit malware detection needs investigation separately from an unknown-publisher warning.
- Updates use the new official installer. Installation and uninstallation preserve local settings, backups and user-created shortcuts; uninstalling does not delete Google documents.

The protected build remains in the **Protected alpha distribution** workflow on `feature/zagosheetswin`, using the existing `zagosheets-distribution` Environment approval. Ordinary CI builds use simulated Google responses and are not the official OAuth-configured distribution.

See [1.0 release preparation and checks](docs/RELEASE_1_0.md), [change history](CHANGELOG.md), [security review](docs/SECURITY_REVIEW_0930.md) and [project plan](docs/ALPHA_NEXT.md).

## Relationship with Open in Google

ZagoSheetsWin is based on and derived from
[Open in Google](https://github.com/SwatiK425/open-in-google) by
[SwatiK425](https://github.com/SwatiK425).

Open in Google provided the original foundation and inspiration for the project.

ZagoSheetsWin has since evolved into an independent Windows application with its own architecture, installer, user interface, backup and recovery system, file association workflow, format handling and local-to-Google-Sheets experience.

The upstream project remains an independent project. Generic improvements may be contributed upstream when appropriate, while ZagoSheetsWin continues to evolve independently.

## Open source

ZagoSheetsWin is free and open-source software.

The project preserves the attribution and licensing requirements of the original Open in Google code while clearly identifying the subsequent ZagoSheetsWin / Zagotools development.

See:

- [LICENSE](LICENSE)
- [ATTRIBUTION.md](ATTRIBUTION.md)
- [NOTICE.md](NOTICE.md)
- [third-party/NOTICE.md](third-party/NOTICE.md)

## License

MIT License.

See [LICENSE](LICENSE) for details.

---

**ZagoSheetsWin — a Zagotools project**

Small software for real problems.

## Technical documentation

- [Architecture](docs/ARCHITECTURE.md)
- [Development and fork](docs/DEVELOPMENT.md)
- [Original upstream README](docs/upstream/README.original.md)
- [Recovery and update implementation](docs/STAGE_8.md)

The runtime is the native C# application; inherited PowerShell scripts are historical references. Tokens use Windows DPAPI. Imports request only `https://www.googleapis.com/auth/drive.file` and send spreadsheet contents directly to Google. Backups preserve the imported original, not subsequent online edits. Conversion can change formulas or unsupported features; recover the original through Backups when needed.
