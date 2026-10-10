# ZagoSheetsWin

<details>
<summary>🌐 Documentation languages · Choose your language</summary>

- **English** — current page
- [简体中文](docs/i18n/zh/README.md)
- [हिन्दी](docs/i18n/hi/README.md)
- [Español](docs/i18n/es/README.md)
- [العربية](docs/i18n/ar/README.md)
- [Français](docs/i18n/fr/README.md)
- [বাংলা](docs/i18n/bn/README.md)
- [Português (Brasil)](docs/i18n/pt/README.md)
- [Bahasa Indonesia](docs/i18n/id/README.md)
- [اردو](docs/i18n/ur/README.md)
- [Русский](docs/i18n/ru/README.md)
- [Deutsch](docs/i18n/de/README.md)
- [日本語](docs/i18n/ja/README.md)
- [Tiếng Việt](docs/i18n/vi/README.md)
- [Türkçe](docs/i18n/tr/README.md)
- [한국어](docs/i18n/ko/README.md)
- [Italiano](docs/i18n/it/README.md)
- [ไทย](docs/i18n/th/README.md)
- [Filipino](docs/i18n/fil/README.md)
- [Bahasa Melayu](docs/i18n/ms/README.md)
- [Kiswahili](docs/i18n/sw/README.md)
- [Nigerian Pidgin](docs/i18n/pcm/README.md)
- [मराठी](docs/i18n/mr/README.md)
- [తెలుగు](docs/i18n/te/README.md)
- [Hausa](docs/i18n/ha/README.md)
- [ਪੰਜਾਬੀ](docs/i18n/pa/README.md)
- [தமிழ்](docs/i18n/ta/README.md)
- [粵語](docs/i18n/yue/README.md)
- [فارسی](docs/i18n/fa/README.md)
- [አማርኛ](docs/i18n/am/README.md)
- [Basa Jawa](docs/i18n/jv/README.md)
- [ગુજરાતી](docs/i18n/gu/README.md)
- [ಕನ್ನಡ](docs/i18n/kn/README.md)
- [Yorùbá](docs/i18n/yo/README.md)
- [भोजपुरी](docs/i18n/bho/README.md)
- [پښتو](docs/i18n/ps/README.md)
- [ଓଡ଼ିଆ](docs/i18n/or/README.md)
- [မြန်မာ](docs/i18n/my/README.md)
- [മലയാളം](docs/i18n/ml/README.md)
- [Polski](docs/i18n/pl/README.md)
- [Basa Sunda](docs/i18n/su/README.md)
- [मैथिली](docs/i18n/mai/README.md)
- [Українська](docs/i18n/uk/README.md)
- [Afaan Oromoo](docs/i18n/om/README.md)
- [Oʻzbekcha](docs/i18n/uz/README.md)
- [سنڌي](docs/i18n/sd/README.md)
- [नेपाली](docs/i18n/ne/README.md)
- [Asụsụ Igbo](docs/i18n/ig/README.md)
- [Azərbaycan dili](docs/i18n/az/README.md)
- [Nederlands](docs/i18n/nl/README.md)
- [Avañe’ẽ](docs/i18n/gn/README.md)

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

Current development targets:

- `.xlsx`
- `.xls`
- `.ods`
- `.csv`
- `.tsv`

Some formats may have additional compatibility restrictions. Files containing features that cannot be safely preserved are handled conservatively to avoid silent data loss.

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

ZagoSheetsWin is currently under active development and should be considered **alpha software**.

The core Windows → Google Sheets workflow is functional and is being expanded with improvements to installation, recovery, format compatibility, internationalization and user experience.

Expect changes while the project approaches its first stable release.

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
