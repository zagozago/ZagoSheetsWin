# ZagoSheetsWin

<details>
<summary>🌐 Language wey you wan read · Choose language</summary>

- [English](../../../README.md)
- [简体中文](../zh/README.md)
- [हिन्दी](../hi/README.md)
- [Español](../es/README.md)
- [العربية](../ar/README.md)
- [Français](../fr/README.md)
- [বাংলা](../bn/README.md)
- [Português (Brasil)](../pt/README.md)
- [Bahasa Indonesia](../id/README.md)
- [اردو](../ur/README.md)
- [Русский](../ru/README.md)
- [Deutsch](../de/README.md)
- [日本語](../ja/README.md)
- [Tiếng Việt](../vi/README.md)
- [Türkçe](../tr/README.md)
- [한국어](../ko/README.md)
- [Italiano](../it/README.md)
- [ไทย](../th/README.md)
- [Filipino](../fil/README.md)
- [Bahasa Melayu](../ms/README.md)
- [Kiswahili](../sw/README.md)
- **Nigerian Pidgin** — Na dis page you dey
- [मराठी](../mr/README.md)
- [తెలుగు](../te/README.md)
- [Hausa](../ha/README.md)
- [ਪੰਜਾਬੀ](../pa/README.md)
- [தமிழ்](../ta/README.md)
- [粵語](../yue/README.md)
- [فارسی](../fa/README.md)
- [አማርኛ](../am/README.md)
- [Basa Jawa](../jv/README.md)
- [ગુજરાતી](../gu/README.md)
- [ಕನ್ನಡ](../kn/README.md)
- [Yorùbá](../yo/README.md)
- [भोजपुरी](../bho/README.md)
- [پښتو](../ps/README.md)
- [ଓଡ଼ିଆ](../or/README.md)
- [မြန်မာ](../my/README.md)
- [മലയാളം](../ml/README.md)
- [Polski](../pl/README.md)
- [Basa Sunda](../su/README.md)
- [मैथिली](../mai/README.md)

</details>

**Open spreadsheet files wey dey your Windows computer straight for Google Sheets.**

ZagoSheetsWin na small Windows app wey dey make am easy to open spreadsheet wey dey your computer:

**double-click file → upload am and convert am → open am for Google Sheets**

When import don work well, ZagoSheetsWin fit replace the original file for your folder with Internet shortcut (`.url`) wey go carry you go the Google Sheets document. E go still keep backup of the original file for your computer, so you fit recover am.

The aim na to make Google Sheets work like normal Windows app wey fit open your local spreadsheet files.

## Wetin e dey do

ZagoSheetsWin dey connect spreadsheet files for Windows with Google Sheets. When you open file wey e support, the app fit:

- check the spreadsheet file and make sure say e valid;
- make backup wey you fit recover;
- upload the file straight to Google Drive with Google official APIs;
- convert am to real Google Sheets document;
- open the spreadsheet for your default browser;
- create local `.url` shortcut to the Google document;
- avoid another upload when you open the same file next time.

You no need to upload manually for Drive, search browser for the file or dey convert am again and again.

## File formats wey e support

Formats wey dem dey work on now: `.xlsx`, `.xls`, `.ods`, `.csv` and `.tsv`.

Some formats fit get extra compatibility limits. If file get features wey conversion no fit preserve safely, the app go handle am with care so your data no go just disappear without warning.

## Dem build am for Windows

Dem design ZagoSheetsWin for Windows. E dey work with **Open with**, file type registration, double-click opening, optional File Explorer integration, and normal Windows installer.

The app no go change your Windows default apps behind your back. Na you dey control which app go open each file type.

## Safety from the beginning

ZagoSheetsWin dey treat replacement of local file as work wey must get way to recover am. Before e remove original file from the folder, e go check say:

1. backup wey person fit recover dey;
2. Google Sheets document don create well;
3. local file connection don save properly;
4. Internet shortcut don write and pass verification.

If something fail, the original file go remain.

> No ever destroy person data without make dem know.

## Backups

The app fit keep original files for private backup space for your computer before e replace dem with shortcuts. You fit set storage limit, how long backups go stay, and cleanup options.

Backup dey protect the original file wey you import. **E no be two-way synchronization**: anything wey you change later for Google Sheets no go write back to the original spreadsheet file.

## Privacy and Google access

ZagoSheetsWin dey talk directly from your computer to Google APIs.

- Your spreadsheet content no dey go Zagotools server.
- OAuth tokens dey stay for your computer, and Windows security dey protect dem.
- The app dey use Google Drive `drive.file` permission scope, so access dey limited to files wey you create or open through the app.
- Spreadsheet import no need analytics tracking.

Privacy Policy and Terms of Use: https://zagotools.top/legal.html

## How the project dey now

Dem still dey develop ZagoSheetsWin, so for now na **alpha software**. The main Windows → Google Sheets process dey work, but dem still dey improve installation, recovery, file compatibility, languages and user experience. Things fit change before the first stable release.

## How e take relate to Open in Google

ZagoSheetsWin come from [Open in Google](https://github.com/SwatiK425/open-in-google) wey [SwatiK425](https://github.com/SwatiK425) build. That project give am the first foundation and idea.

ZagoSheetsWin don grow into independent Windows app with im own architecture, installer, interface, backup and recovery, file association, format handling, and way to open local files for Google Sheets. The original project still dey independent. General improvements fit go back to the original project when e make sense.

## Open source

ZagoSheetsWin na free and open-source software. E dey keep the original Open in Google copyright credit and license requirements, and e dey show the later work from ZagoSheetsWin / Zagotools separate.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## License

MIT License. Check [LICENSE](../../../LICENSE) for details.

---

**ZagoSheetsWin — Zagotools project**

Small software for real problems.
