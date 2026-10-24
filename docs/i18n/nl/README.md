# ZagoSheetsWin

> ⚠️ **Alpha 0.9.48 — current safety notice in English; full editorial translation pending.**
>
> Het werkblad is geopend in Google Sheets. Het oorspronkelijke bestand is vervangen door een snelkoppeling. Open Back-ups om het te herstellen.
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.48:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Recovery 0.9.44:** a later **independent** opening may start a fresh import only if earlier pending Google document absence is verified; the old backup/history remain protected. Network errors, uncertain responses and concurrent retries do not authorize duplicates. **UI 0.9.45–0.9.48:** refined settings/first use, local 51-language texts, three collapsible backup panels, checkboxes reflecting selection, bulk actions in 32-item batches, visual/installer updates and RTL action insets on Home/Settings. These UI changes do not change the manual backup policy.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [native UI CI](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40121298442) and [protected alpha distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40120263404) passed on earlier 0.9.48 commits; the final RTL hotfix `08dbced` has a [separate CI run](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40126683024), so consult the [current status](../../RELEASE_STATUS.md). Real Google acceptance remains pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 Documentatietaal · Kies je taal</summary>

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
- [PCM - Nigerian Pidgin](../pcm/README.md)
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
- **NL - Nederlands / Dutch (nl)** — huidige pagina
- [GN - Avañe’ẽ / Guarani (gn)](../gn/README.md)

</details>

**Open lokale spreadsheetbestanden rechtstreeks in Google Spreadsheets vanuit Windows.**

ZagoSheetsWin is een lichtgewicht Windows-app die het openen van lokale spreadsheets eenvoudig maakt:

**dubbelklik op het bestand → uploaden en converteren → openen in Google Spreadsheets**

Na een geslaagde import kan ZagoSheetsWin het oorspronkelijke lokale bestand vervangen door een internetsnelkoppeling (`.url`) naar het Google Spreadsheets-document, terwijl een herstelbare lokale back-up van het origineel bewaard blijft.

Het doel is eenvoudig: Google Spreadsheets laten aanvoelen als een gewone Windows-app voor het openen van lokale spreadsheetbestanden.

## Wat doet de app?

ZagoSheetsWin verbindt spreadsheetbestanden in Windows met Google Spreadsheets. Wanneer je een ondersteund lokaal bestand opent, kan de app:

- de spreadsheet herkennen en controleren;
- een herstelbare back-up maken;
- het bestand rechtstreeks uploaden naar Google Drive via de officiële Google-API's;
- het converteren naar een eigen Google Spreadsheets-document;
- het resultaat openen in je standaardbrowser;
- een lokale `.url`-snelkoppeling naar het Google-document maken;
- voorkomen dat hetzelfde bestand bij een volgende opening opnieuw wordt geüpload.

Geen handmatige upload via Drive, niet zoeken in je browser en geen herhaalde conversie.

## Ondersteunde bestandsformaten

De huidige ontwikkeling richt zich op `.xlsx`, `.xls`, `.ods`, `.csv` en `.tsv`.

Voor sommige formaten gelden extra compatibiliteitsbeperkingen. Bestanden met functies die niet veilig behouden kunnen blijven, worden voorzichtig verwerkt om ongemerkt gegevensverlies te voorkomen.

## Ontworpen voor Windows

ZagoSheetsWin is speciaal voor Windows gebouwd. De integratie omvat **Openen met**, registratie van bestandstypen, openen via dubbelklikken, optionele integratie met Verkenner en een native Windows-installatieprogramma.

De app wijzigt niet ongemerkt je standaardprogramma's. Bestandskoppelingen blijven onder jouw controle.

## Veiligheid als uitgangspunt

Het vervangen van een lokaal bestand wordt behandeld als een herstelbare bewerking. Voordat het oorspronkelijke bestand uit de map wordt verwijderd, controleert de app of:

1. er een herstelbare back-up aanwezig is;
2. het Google Spreadsheets-document succesvol is aangemaakt;
3. de lokale koppeling duurzaam is opgeslagen;
4. de internetsnelkoppeling correct is geschreven en gecontroleerd.

Als het proces mislukt, blijft het oorspronkelijke bestand behouden.

> Vernietig gebruikersgegevens nooit ongemerkt.

## Back-ups

Oorspronkelijke bestanden kunnen in een privéopslag voor lokale back-ups worden bewaard voordat ze door snelkoppelingen worden vervangen. Opslaglimieten, bewaartermijnen en opruimopties zijn instelbaar.

Een back-up beschermt het oorspronkelijke bestand van het importmoment. Dit is **geen tweerichtingssynchronisatie**: wijzigingen die je later in Google Spreadsheets aanbrengt, worden niet teruggeschreven naar het originele spreadsheetbestand.

## Privacy en toegang tot Google

ZagoSheetsWin communiceert rechtstreeks vanaf je computer met de Google-API's.

- De inhoud van je spreadsheets wordt niet naar Zagotools-servers verzonden.
- OAuth-tokens worden lokaal opgeslagen en beschermd met de beveiligingsmechanismen van Windows.
- De app gebruikt het Google Drive-bereik `drive.file`, waarmee toegang beperkt wordt tot bestanden die via de app zijn gemaakt of geopend.
- Voor het importproces is geen analysetracking vereist.

Privacybeleid en gebruiksvoorwaarden: https://zagotools.top/legal.html

## Projectstatus

ZagoSheetsWin wordt actief ontwikkeld en moet momenteel worden beschouwd als **alfasoftware**. De belangrijkste werkwijze Windows → Google Spreadsheets functioneert al. Installatie, herstel, formaatcompatibiliteit, internationalisering en gebruiksgemak worden verder verbeterd. Tot aan de eerste stabiele versie zijn wijzigingen mogelijk.

## Relatie met Open in Google

ZagoSheetsWin is gebaseerd op en afgeleid van [Open in Google](https://github.com/SwatiK425/open-in-google) van [SwatiK425](https://github.com/SwatiK425). Dat project leverde de oorspronkelijke technische basis en inspiratie.

ZagoSheetsWin is inmiddels een zelfstandige Windows-app met een eigen architectuur, installatieprogramma, interface, systeem voor back-ups en herstel, bestandskoppelingen, formaatverwerking en ervaring voor het openen van lokale bestanden in Google Spreadsheets. Het oorspronkelijke project blijft onafhankelijk; algemene verbeteringen kunnen waar zinvol aan upstream worden aangeboden.

## Open source

ZagoSheetsWin is gratis opensourcesoftware. Het project bewaart de vereiste auteursvermeldingen en licentievoorwaarden van de oorspronkelijke Open in Google-code en maakt de latere ontwikkeling van ZagoSheetsWin / Zagotools duidelijk herkenbaar.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Licentie

MIT-licentie. Zie [LICENSE](../../../LICENSE) voor meer informatie.

---

**ZagoSheetsWin — een project van Zagotools**

Kleine software voor echte problemen.
