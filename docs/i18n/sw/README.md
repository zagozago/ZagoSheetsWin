# ZagoSheetsWin

> ⚠️ **Alpha 0.9.43 — current safety notice in English; full editorial translation pending.**
>
> Lahajedwali limefunguliwa katika Google Sheets. Faili asili limebadilishwa na njia ya mkato. Ili kulirejesha, fungua Nakala rudufu.
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.43:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [Local core](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) and [protected distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) passed on commit `9fbec2a`; real Google acceptance is still pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 Lugha ya nyaraka · Chagua lugha</summary>

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
- **SW - Kiswahili / Swahili (sw)** — ukurasa wa sasa
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
- [NL - Nederlands / Dutch (nl)](../nl/README.md)
- [GN - Avañe’ẽ / Guarani (gn)](../gn/README.md)

</details>

**Fungua faili za lahajedwali zilizo kwenye kompyuta moja kwa moja katika Google Sheets kupitia Windows.**

ZagoSheetsWin ni programu nyepesi ya Windows inayorahisisha kufungua lahajedwali za ndani:

**bofya faili mara mbili → pakia na badilisha muundo → fungua katika Google Sheets**

Baada ya uingizaji uliofanikiwa, ZagoSheetsWin inaweza kubadilisha faili asili ya ndani na njia ya mkato ya Intaneti (`.url`) inayoelekeza kwenye hati ya Google Sheets, huku ikihifadhi nakala ya ndani inayoweza kurejeshwa.

Lengo ni kufanya Google Sheets ihisi kama programu ya kawaida ya Windows unapofungua lahajedwali za ndani.

## Inafanya nini

ZagoSheetsWin inaunganisha faili za lahajedwali za Windows na Google Sheets. Unapofungua faili inayotumika, programu inaweza:

- kutambua na kuthibitisha lahajedwali;
- kuunda nakala rudufu inayoweza kurejeshwa;
- kupakia moja kwa moja kwenye Google Drive kwa kutumia API rasmi za Google;
- kuibadilisha kuwa hati asilia ya Google Sheets;
- kufungua matokeo katika kivinjari chaguomsingi;
- kuunda njia ya mkato ya `.url` inayoelekeza kwenye hati ya Google;
- kuzuia kupakia tena faili ileile wakati wa kuifungua baadaye.

Hakuna kupakia faili kwa mkono kwenye Drive, kuitafuta kwenye kivinjari au kurudia ubadilishaji.

## Miundo inayotumika

Miundo inayolengwa katika maendeleo ya sasa ni `.xlsx`, `.xls`, `.ods`, `.csv` na `.tsv`.

Baadhi ya miundo inaweza kuwa na vikwazo vya ziada vya uoanifu. Faili zenye vipengele visivyoweza kuhifadhiwa salama hushughulikiwa kwa uangalifu ili kuepuka upotevu wa data bila taarifa.

## Imeundwa kwa Windows

ZagoSheetsWin imeundwa mahsusi kwa Windows, ikiwa na **Open with (Fungua kwa)**, usajili wa aina za faili, kufungua kwa kubofya mara mbili, muunganisho wa hiari na File Explorer, na kisakinishi asilia cha Windows.

Programu haibadilishi programu chaguomsingi za Windows kwa siri. Mtumiaji anaendelea kudhibiti programu zinazohusishwa na faili.

## Usalama tangu mwanzo

Kubadilisha faili ya ndani ni operesheni inayopaswa kuweza kurejeshwa. Kabla ya kuondoa faili asili kwenye folda yake, programu huthibitisha kuwa:

1. nakala rudufu inayoweza kurejeshwa ipo;
2. hati ya Google Sheets imeundwa kwa mafanikio;
3. uhusiano wa faili ya ndani umehifadhiwa kwa kudumu;
4. njia ya mkato ya Intaneti imeandikwa na kuthibitishwa.

Ikiwa mchakato utashindwa, faili asili huhifadhiwa.

> Usiharibu data ya mtumiaji kimyakimya kamwe.

## Nakala rudufu

Faili asili zinaweza kuhifadhiwa katika eneo binafsi la nakala rudufu kwenye kompyuta kabla ya kubadilishwa na njia za mkato. Vikomo vya nafasi, muda wa kuhifadhi na usafishaji vinaweza kusanidiwa.

Nakala rudufu hulinda faili asili wakati wa kuingizwa. **Si usawazishaji wa pande mbili**: mabadiliko ya baadaye kwenye Google Sheets hayaandikwi tena kwenye faili asili ya lahajedwali.

## Faragha na ufikiaji wa Google

ZagoSheetsWin huwasiliana moja kwa moja kutoka kwenye kompyuta yako na API za Google.

- Maudhui ya lahajedwali hayatumiwi kwenye seva za Zagotools.
- Tokeni za OAuth huhifadhiwa kwenye kompyuta na kulindwa na mifumo ya usalama ya Windows.
- Wigo wa ruhusa wa `drive.file` huweka ufikiaji kwenye faili zilizoundwa au kufunguliwa kupitia programu.
- Uingizaji wa lahajedwali hauhitaji ufuatiliaji wa uchanganuzi.

Sera ya Faragha na Masharti ya Matumizi: https://zagotools.top/legal.html

## Hali ya mradi

ZagoSheetsWin inaendelea kutengenezwa na kwa sasa ni **programu ya hatua ya alfa**. Mtiririko mkuu wa Windows → Google Sheets unafanya kazi; usakinishaji, urejeshaji, uoanifu wa miundo, ujanibishaji wa lugha na uzoefu wa mtumiaji vinaendelea kuboreshwa. Mabadiliko yanaweza kutokea kabla ya toleo thabiti la kwanza.

## Uhusiano na Open in Google

ZagoSheetsWin inatokana na [Open in Google](https://github.com/SwatiK425/open-in-google) ya [SwatiK425](https://github.com/SwatiK425), ambayo ilitoa msingi na msukumo wa awali.

Tangu hapo imekuwa programu huru ya Windows yenye usanifu, kisakinishi, kiolesura, mfumo wa nakala rudufu na urejeshaji, uhusishaji wa faili, ushughulikiaji wa miundo na uzoefu wa kufungua faili za ndani kwenye Google Sheets. Mradi asili unaendelea kuwa huru, na maboresho ya jumla yanaweza kupendekezwa kwake yanapofaa.

## Chanzo huria

ZagoSheetsWin ni programu ya bure na ya chanzo huria. Inahifadhi sifa na masharti ya leseni ya msimbo wa awali wa Open in Google na kutaja wazi maendeleo ya baadaye ya ZagoSheetsWin / Zagotools.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Leseni

Leseni ya MIT. Tazama [LICENSE](../../../LICENSE) kwa maelezo.

---

**ZagoSheetsWin — mradi wa Zagotools**

Programu ndogo kwa matatizo halisi.
