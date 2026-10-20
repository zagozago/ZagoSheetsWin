# ZagoSheetsWin

> ⚠️ **Alpha 0.9.43 — current safety notice in English; full editorial translation pending.**
>
> Spreadsheet don open for Google Sheets. Shortcut don replace di original file. To recover am, open Backups.
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.43:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [Local core](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) and [protected distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) passed on commit `9fbec2a`; real Google acceptance is still pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 Language wey you wan read · Choose language</summary>

- [EN - English](../../../README.md)
- [ZH - 中文 / Chinese (Simplified) (zh)](../zh/README.md)
- [HI - हिन्दी / Hindi (hi)](../hi/README.md)
- [ES - Español / Spanish (es)](../es/README.md)
- [AR - العربية / Arabic (ar)](../ar/README.md)
- [FR - Français / French (fr)](../fr/README.md)
- [BN - বাংলা / Bengali (bn)](../bn/README.md)
- [PT - Português / Portuguese (pt)](../pt/README.md)
- [ID - Bahasa Indonesia / Indonesian (id)](../id/README.md)
- [UR - اردو / Urdu (ur)](../ur/README.md)
- [RU - Русский / Russian (ru)](../ru/README.md)
- [DE - Deutsch / German (de)](../de/README.md)
- [JA - 日本語 / Japanese (ja)](../ja/README.md)
- [VI - Tiếng Việt / Vietnamese (vi)](../vi/README.md)
- [TR - Türkçe / Turkish (tr)](../tr/README.md)
- [KO - 한국어 / Korean (ko)](../ko/README.md)
- [IT - Italiano / Italian (it)](../it/README.md)
- [TH - ไทย / Thai (th)](../th/README.md)
- [TL - Filipino](../fil/README.md)
- [MS - Bahasa Melayu / Malay (ms)](../ms/README.md)
- [SW - Kiswahili / Swahili (sw)](../sw/README.md)
- **PCM - Nigerian Pidgin** — Na dis page you dey
- [MR - मराठी / Marathi (mr)](../mr/README.md)
- [TE - తెలుగు / Telugu (te)](../te/README.md)
- [HA - Hausa](../ha/README.md)
- [PA - ਪੰਜਾਬੀ / Punjabi (pa)](../pa/README.md)
- [TA - தமிழ் / Tamil (ta)](../ta/README.md)
- [YUE - 粵語 / Cantonese (yue)](../yue/README.md)
- [FA - فارسی / Persian (fa)](../fa/README.md)
- [AM - አማርኛ / Amharic (am)](../am/README.md)
- [JV - Basa Jawa / Javanese (jv)](../jv/README.md)
- [GU - ગુજરાતી / Gujarati (gu)](../gu/README.md)
- [KN - ಕನ್ನಡ / Kannada (kn)](../kn/README.md)
- [YO - Yorùbá / Yoruba (yo)](../yo/README.md)
- [BHO - भोजपुरी / Bhojpuri (bho)](../bho/README.md)
- [PS - پښتو / Pashto (ps)](../ps/README.md)
- [OR - ଓଡ଼ିଆ / Odia (or)](../or/README.md)
- [MY - မြန်မာဘာသာ / Burmese (my)](../my/README.md)
- [ML - മലയാളം / Malayalam (ml)](../ml/README.md)
- [PL - Polski / Polish (pl)](../pl/README.md)
- [SU - Basa Sunda / Sundanese (su)](../su/README.md)
- [MAI - मैथिली / Maithili (mai)](../mai/README.md)
- [UK - Українська / Ukrainian (uk)](../uk/README.md)
- [OM - Afaan Oromoo / Oromo (om)](../om/README.md)
- [UZ - Oʻzbekcha / Uzbek (uz)](../uz/README.md)
- [SD - سنڌي / Sindhi (sd)](../sd/README.md)
- [NE - नेपाली / Nepali (ne)](../ne/README.md)
- [IG - Igbo](../ig/README.md)
- [AZ - Azərbaycanca / Azerbaijani (az)](../az/README.md)
- [NL - Nederlands / Dutch (nl)](../nl/README.md)
- [GN - Avañe’ẽ / Guarani (gn)](../gn/README.md)

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
