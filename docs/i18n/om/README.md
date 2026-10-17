# ZagoSheetsWin

> ⚠️ **0.9.35 — Important safety update (English notice; full translation review pending).**
>
> Gabateen Google Sheets keessatti banameera. Faayiliin jalqabaa karaa gabaabaadhaan bakka buufameera. Deebisuuf Kuusdeebii bani.
>
> **Conversion fidelity:** After a confirmed Google import, the original local file **may be replaced with a `.url` shortcut even when formulas, values or formatting differ**. The original bytes remain in Backups. Choose **Import as copy BEFORE opening** if you need the original to stay in the source folder. Windows **read-only** files may block replacement; in that case, the original is preserved. Some text below describes an older alpha policy; use the [current English README](../../../README.md) or [Portuguese user guide](../../USER_GUIDE.md) for complete 0.9.35 behavior.


<details>
<summary>🌐 Afaan galmee · Afaan filadhu</summary>

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
- **OM - Afaan Oromoo / Oromo (om)** — fuula ammaa
- [UZ - Oʻzbekcha / Uzbek (uz)](../uz/README.md)
- [SD - سنڌي / Sindhi (sd)](../sd/README.md)
- [NE - नेपाली / Nepali (ne)](../ne/README.md)
- [IG - Igbo](../ig/README.md)
- [AZ - Azərbaycanca / Azerbaijani (az)](../az/README.md)
- [NL - Nederlands / Dutch (nl)](../nl/README.md)
- [GN - Avañe’ẽ / Guarani (gn)](../gn/README.md)

</details>

**Faayiloota spreadsheet kompiitara kee keessa jiran Windows irraa kallattiin Google Sheets keessatti bani.**

ZagoSheetsWin appii Windows salphaa ta’ee, faayiloota spreadsheet kompiitara keessa jiran banuu siif laaffisuudha:

**faayilii lama cuqaasi → olkaa’i fi jijjiiri → Google Sheets keessatti bani**

Erga galchiinsi milkaa’ee booda, ZagoSheetsWin faayilii jalqabaa bakka isaa gabaabduu Intarneetii (`.url`) galmee Google Sheetsitti si geessuun bakka buusuu danda’a. Garuu waraabbii vargaabii (backup) faayilii jalqabaa, kan deebisanii argachuun danda’amu, kompiitara irratti ni eega.

Kaayyoon isaa salphaadha: Google Sheets akka appii Windows idilee ta’ee faayiloota spreadsheet kompiitara kee keessaa banuu danda’u gochuudha.

## Maal hojjeta

ZagoSheetsWin faayiloota spreadsheet Windows fi Google Sheets walitti hidha. Yeroo faayilii deeggaramu banu, appiin kun:

- spreadsheet adda baasuu fi sirrii ta’uu isaa mirkaneessuu;
- waraabbii vargaabii deebisanii argachuun danda’amu uumuu;
- APIwwan Google kanneen ofiisaa fayyadamuun kallattiin gara Google Drive olkaa’uu;
- gara galmee Google Sheets isa dhugaatti jijjiiruu;
- spreadsheet argame biraawzarii durtii keessatti banuu;
- gabaabduu `.url` kan galmee Google agarsiisu uumuu;
- yeroo itti aanu faayilii sanuma banu irra deebi’ee olkaa’uu hambisuu danda’a.

Harkaan gara Drive olkaa’uun, biraawzarii keessa barbaaduun ykn irra deddeebi’anii jijjiiruun hin barbaachisu.

## Akaakuu faayilii deeggaraman

Akaakuwwan yeroo ammaa irratti hojjetamaa jiran: `.xlsx`, `.xls`, `.ods`, `.csv` fi `.tsv`.

Akaakuwwan tokko tokko daangaa wal-simannaa dabalataa qabaachuu danda’u. Faayiloonni amaloota nageenyaan eegamuu hin dandeenye qaban of eeggannoodhaan qabamu, akka odeeffannoon utuu hin beeksisin hin badneef.

## Windows irratti hojjechuuf kan qophaa’e

ZagoSheetsWin addatti Windows'f hojjetame. Innis **Open with (Kanaan bani)**, galmeessuu akaakuu faayilii, lama cuqaasuun banuu, filannoo walitti hidhamiinsa File Explorer fi sagantaa ittiin fe’amu (installer) Windows qaba.

Appiin kun osoo ati hin beekin appii durtii Windows hin jijjiiru. Walitti hidhamiinsa faayilii fayyadamaan to’ata.

## Nageenya jalqabaa irraa

Faayilii naannoo bakka buusuun hojii deebisanii sirreessuun danda’amu ta’uu qaba. Faayilii jalqabaa galmee isaa keessaa osoo hin kaasin appiin ni mirkaneessa:

1. waraabbiin vargaabii deebisanii argachuun danda’amu jiraachuu;
2. galmeen Google Sheets milkaa’inaan uumamuu;
3. walitti hidhamiinsi faayilii naannoo dhaabbataan kuufamuu;
4. gabaabduun Intarneetii barreeffamee mirkanaa’uu.

Yoo hojiin kufe faayiliin jalqabaa ni hafu.

> Odeeffannoo fayyadamaa osoo hin beeksisin gonkumaa hin balleessin.

## Waraabbii vargaabii

Faayiloonni jalqabaa gabaabduudhaan bakka buufamuu isaanii dura iddoo dhuunfaa kompiitara keessaatti kuufamuu danda’u. Daangaan bakka kuusaa, turtiin kuusaa fi filannoon qulqulleessuu ni qindaa’u.

Waraabbiin faayilii yeroo galchiinsaa ture eega. Kun **walsimsiisa kallattii lamaa miti**: jijjiiramni yeroo boodaa Google Sheets irratti godhamu gara faayilii spreadsheet jalqabaatti hin barreeffamu.

## Dhuunfummaa fi hayyama Google

ZagoSheetsWin kompiitara kee irraa kallattiin API Google waliin quunnama.

- Qabiyyeen spreadsheet kee gara sarvaroota Zagotools hin ergamu.
- Mallattooleen OAuth kompiitara irratti kuufamanii sirna nageenya Windows tiin eegamu.
- Hayyamni `drive.file` faayiloota appiidhaan uumaman ykn banaman qofaaf qaqqabummaa daangeessa.
- Galchiinsi spreadsheet hordoffii xiinxalaa hin barbaadu.

Imaammata Dhuunfummaa fi Haalota Fayyadamaa: https://zagotools.top/legal.html

## Haala pirojektii

ZagoSheetsWin amma ijaaramaa jira, kanaaf **softiweerii sadarkaa alfa** jedhamee ilaalamuu qaba. Adeemsi ijoo Windows → Google Sheets ni hojjeta. Fe’iinsa, deebisanii argachuu, wal-simannaa faayilii, afaanota adda addaa fi muuxannoo fayyadamaa irratti fooyya’iinsi itti fufa. Gad-lakkisa tasgabbaa’aa jalqabaa dura jijjiiramni jiraachuu danda’a.

## Hariiroo Open in Google waliin

ZagoSheetsWin [Open in Google](https://github.com/SwatiK425/open-in-google) kan [SwatiK425](https://github.com/SwatiK425) irratti hundaa’ee kan irraa guddate dha. Pirojektiin jalqabaa hundee teeknikaa fi kaka’umsa kenne.

Amma ZagoSheetsWin appii Windows of danda’e ta’ee, caasaa, installer, interface, sirna waraabbii fi deebisanii argachuu, walitti hidhamiinsa faayilii, bulchiinsa akaakuu faayilii fi muuxannoo Google Sheets keessatti banuu mataa isaa qaba. Pirojektiin jalqabaa of danda’ee jira; fooyya’iinsi waliigalaa yeroo mijatutti upstream'f dhiyaachuu danda’a.

## Madda banaa

ZagoSheetsWin softiweerii bilisaa fi madda banaa ti. Maqaa abbaa hojii fi ulaagaalee hayyamaa koodii jalqabaa Open in Google ni eega; hojii ZagoSheetsWin / Zagotools itti aansuun hojjatame adda baasee ibsa.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Hayyama

Hayyama MIT. Bal’inaaf [LICENSE](../../../LICENSE) ilaali.

---

**ZagoSheetsWin — pirojektii Zagotools**

Softiweerii xiqqaa rakkoolee dhugaa furuuf.
