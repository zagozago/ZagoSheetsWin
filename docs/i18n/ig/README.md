# ZagoSheetsWin

> ⚠️ **Alpha 0.9.43 — current safety notice in English; full editorial translation pending.**
>
> E meghere akwụkwọ mgbakọ na Google Sheets. E jiri ụzọ mkpirisi dochie faịlụ mbụ. Iji weghachite ya, mepee Nnomi ndabere.
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.43:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [Local core](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) and [protected distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) passed on commit `9fbec2a`; real Google acceptance is still pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 Asụsụ akwụkwọ nkọwa · Họrọ asụsụ</summary>

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
- **IG - Igbo** — ibe dị ugbu a
- [AZ - Azərbaycanca / Azerbaijani (az)](../az/README.md)
- [NL - Nederlands / Dutch (nl)](../nl/README.md)
- [GN - Avañe’ẽ / Guarani (gn)](../gn/README.md)

</details>

**Mepee faịlụ spreadsheet ndị dị na kọmputa gị ozugbo na Google Sheets site na Windows.**

ZagoSheetsWin bụ ngwa Windows dị mfe nke na-eme ka imepe spreadsheet dị na kọmputa gị dị mfe:

**pịa faịlụ ugboro abụọ → bulite ma gbanwee → mepee na Google Sheets**

Mgbe mbubata gara nke ọma, ZagoSheetsWin nwere ike iji ụzọ mkpirisi Ịntanetị (`.url`) nke na-eduga na akwụkwọ Google Sheets dochie faịlụ mbụ. Ọ na-echekwa ndabere faịlụ mbụ na kọmputa nke a pụrụ iweghachi.

Ebumnuche ya dị mfe: ime ka Google Sheets rụọ ọrụ dịka ngwa Windows nkịtị mgbe a na-emepe spreadsheet ndị dị na kọmputa.

## Ihe ọ na-eme

ZagoSheetsWin na-ejikọta faịlụ spreadsheet Windows na Google Sheets. Mgbe ị meghere faịlụ a na-akwado, ngwa ahụ nwere ike:

- mata ma nyochaa spreadsheet;
- mepụta ndabere a pụrụ iweghachi;
- bulite faịlụ ozugbo na Google Drive site na API gọọmentị Google;
- gbanwee ya ka ọ bụrụ akwụkwọ Google Sheets nke mbụ;
- mepee spreadsheet ahụ na ihe nchọgharị ndabara;
- mepụta ụzọ mkpirisi `.url` dị na kọmputa gaa na akwụkwọ Google;
- zere ibulite otu faịlụ ahụ ọzọ mgbe e mesịrị.

Ọ dịghị mkpa ibulite faịlụ n'aka na Drive, ịchọ ya na ihe nchọgharị, ma ọ bụ ịgbanwe ya ugboro ugboro.

## Ụdị faịlụ a na-akwado

Ụdị ndị a na-elekwasị anya na ha ugbu a: `.xlsx`, `.xls`, `.ods`, `.csv` na `.tsv`.

Ụfọdụ ụdị nwere ike ịnwe oke ndakọrịta ọzọ. A na-eji nlezianya ejikwa faịlụ nwere atụmatụ ndị a na-apụghị ichekwa n'enweghị nsogbu ka data ghara ịla n'iyi n'amaghị ama.

## Emere ya maka Windows

A rụrụ ZagoSheetsWin kpọmkwem maka Windows. Ọ na-enye **Open with (Jiri mepee)**, ndebanye ụdị faịlụ, imepe site n'ịpị ugboro abụọ, njikọ File Explorer nhọrọ, na ihe nrụnye Windows.

Ngwa ahụ anaghị agbanwe ngwa ndabara Windows n'amaghị onye ọrụ. Onye ọrụ ka na-achị njikọ faịlụ.

## Nchekwa site na nhazi

A na-ewere dochie faịlụ dị na kọmputa dịka ọrụ a ga-enwe ike iweghachi. Tupu ewepụ faịlụ mbụ n'obere nchekwa ya, ngwa ahụ na-enyocha na:

1. ndabere a pụrụ iweghachi dị;
2. e mepụtara akwụkwọ Google Sheets nke ọma;
3. echekwara njikọ faịlụ dị na kọmputa n'ụzọ na-adịgide;
4. e dere ma nyochaa ụzọ mkpirisi Ịntanetị nke ọma.

Ọ bụrụ na usoro ahụ ada, a na-echekwa faịlụ mbụ.

> Ebibila data onye ọrụ n'amaghị ya.

## Ndabere

Enwere ike idobe faịlụ mbụ n'ebe nchekwa ndabere nzuzo dị na kọmputa tupu ejiri ụzọ mkpirisi dochie ha. Enwere ike ịhazi oke nchekwa, oge idobe na nhọrọ nhicha.

Ndabere na-echekwa faịlụ mbụ n'oge mbubata. Nke a **abụghị mmekọrịta ụzọ abụọ**: mgbanwe ndị emere emechaa na Google Sheets anaghị alaghachi na faịlụ spreadsheet mbụ.

## Nzuzo na ịnweta Google

ZagoSheetsWin na-ekwurịta okwu ozugbo na API Google site na kọmputa gị.

- A naghị eziga ọdịnaya spreadsheet na sava Zagotools.
- A na-echekwa token OAuth na kọmputa, usoro nchebe Windows na-echekwa ha.
- Ikike `drive.file` na-egbochi ịnweta naanị faịlụ emepụtara ma ọ bụ mepere site na ngwa ahụ.
- Mbubata spreadsheet anaghị achọ nlekota nyocha ojiji.

Iwu Nzuzo na Usoro Ojiji: https://zagotools.top/legal.html

## Ọnọdụ ọrụ a

A ka na-arụ ọrụ na ZagoSheetsWin, ya mere ugbu a ọ bụ **sọftụwia ọkwa alpha**. Usoro bụ isi Windows → Google Sheets na-arụ ọrụ. A ka na-emeziwanye nrụnye, iweghachi, ndakọrịta faịlụ, nkwado asụsụ na ahụmịhe onye ọrụ. Mgbanwe nwere ike ime tupu ụdị kwụsiri ike mbụ.

## Mmekọrịta ya na Open in Google

ZagoSheetsWin sitere na [Open in Google](https://github.com/SwatiK425/open-in-google) nke [SwatiK425](https://github.com/SwatiK425). Ọrụ mbụ ahụ nyere ntọala teknụzụ na echiche mbụ.

Ugbu a ZagoSheetsWin aghọwo ngwa Windows nọọrọ onwe ya nke nwere usoro nhazi, ihe nrụnye, ihu ngwa, usoro ndabere na iweghachi, njikọ faịlụ, njikwa ụdị faịlụ, na ụzọ imepe faịlụ dị na kọmputa na Google Sheets nke ya. Ọrụ mbụ ka dị iche; enwere ike izigara upstream ndozi izugbe mgbe ọ dabara.

## Isi mmalite mepere emepe

ZagoSheetsWin bụ sọftụwia efu nke isi mmalite ya mepere emepe. Ọ na-edobe nkwanye ugwu nye ndị kere koodu Open in Google mbụ na ọnọdụ ikike ya, ma kọwaa mmepe ndị ZagoSheetsWin / Zagotools mere emechaa.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Ikikere

Ikikere MIT. Lee [LICENSE](../../../LICENSE) maka nkọwa.

---

**ZagoSheetsWin — ọrụ Zagotools**

Obere sọftụwia maka nsogbu ndị dị adị.
