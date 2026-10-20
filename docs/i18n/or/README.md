# ZagoSheetsWin

> ⚠️ **Alpha 0.9.43 — current safety notice in English; full editorial translation pending.**
>
> ସ୍ପ୍ରେଡ୍‌ଶିଟ୍ Google Sheets ରେ ଖୋଲାଯାଇଛି। ମୂଳ ଫାଇଲ ସ୍ଥାନରେ ଏକ ସର୍ଟକଟ୍ ତିଆରି ହୋଇଛି। ମୂଳ ଫାଇଲ ଫେରି ପାଇବା ପାଇଁ ବ୍ୟାକଅପ୍ ଖୋଲନ୍ତୁ।
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.43:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [Local core](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) and [protected distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) passed on commit `9fbec2a`; real Google acceptance is still pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 ଦଲିଲର ଭାଷା · ଭାଷା ବାଛନ୍ତୁ</summary>

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
- **OR - ଓଡ଼ିଆ / Odia (or)** — ବର୍ତ୍ତମାନର ପୃଷ୍ଠା
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

**Windows ରୁ ସ୍ଥାନୀୟ ସ୍ପ୍ରେଡ୍‌ସିଟ୍ ଫାଇଲ୍ ସିଧା Google Sheets ରେ ଖୋଲନ୍ତୁ।**

ZagoSheetsWin ହେଉଛି Windows ପାଇଁ ଏକ ହାଲୁକା ଆପ୍ଲିକେସନ୍, ଯାହା ସ୍ଥାନୀୟ ସ୍ପ୍ରେଡ୍‌ସିଟ୍ ଖୋଲିବାକୁ ସହଜ କରେ:

**ଫାଇଲ୍‌କୁ ଦୁଇଥର କ୍ଲିକ୍ → ଅପ୍‌ଲୋଡ୍ ଓ ରୂପାନ୍ତର → Google Sheets ରେ ଖୋଲନ୍ତୁ**

ସଫଳ ଆମଦାନି ପରେ ZagoSheetsWin ମୂଳ ସ୍ଥାନୀୟ ଫାଇଲ୍ ବଦଳରେ Google Sheets ଦଲିଲକୁ ନେଇଯାଉଥିବା ଇଣ୍ଟରନେଟ୍ ସର୍ଟକଟ୍ (`.url`) ରଖିପାରେ। ସେହି ସମୟରେ ମୂଳ ଫାଇଲ୍‌ର ପୁନରୁଦ୍ଧାରଯୋଗ୍ୟ ସ୍ଥାନୀୟ ବ୍ୟାକଅପ୍ ସଂରକ୍ଷଣ କରେ।

ଲକ୍ଷ୍ୟ ସରଳ: ସ୍ଥାନୀୟ ସ୍ପ୍ରେଡ୍‌ସିଟ୍ ଖୋଲିବାବେଳେ Google Sheets ସାଧାରଣ Windows ଆପ୍ ପରି କାମ କରୁ।

## ଏହା କ'ଣ କରେ

ZagoSheetsWin Windows ର ସ୍ପ୍ରେଡ୍‌ସିଟ୍ ଫାଇଲ୍‌କୁ Google Sheets ସହ ସଂଯୋଗ କରେ। ସମର୍ଥିତ ସ୍ଥାନୀୟ ଫାଇଲ୍ ଖୋଲିବାବେଳେ ଆପ୍:

- ସ୍ପ୍ରେଡ୍‌ସିଟ୍ ଚିହ୍ନଟ ଓ ଯାଞ୍ଚ କରିପାରେ;
- ପୁନରୁଦ୍ଧାରଯୋଗ୍ୟ ବ୍ୟାକଅପ୍ ତିଆରି କରିପାରେ;
- Google ର ଅଧିକୃତ API ମାଧ୍ୟମରେ ସିଧା Google Drive କୁ ଅପ୍‌ଲୋଡ୍ କରିପାରେ;
- ଏହାକୁ ମୂଳ Google Sheets ଦଲିଲରେ ପରିବର୍ତ୍ତନ କରିପାରେ;
- ଫଳାଫଳକୁ ଡିଫଲ୍ଟ ବ୍ରାଉଜରରେ ଖୋଲିପାରେ;
- Google ଦଲିଲ ପାଇଁ ସ୍ଥାନୀୟ `.url` ସର୍ଟକଟ୍ ତିଆରି କରିପାରେ;
- ପରେ ସେହି ଫାଇଲ୍ ଖୋଲିବାବେଳେ ପୁଣି ଅପ୍‌ଲୋଡ୍ କରିବା ଏଡ଼ାଇପାରେ।

Drive ରେ ନିଜେ ଅପ୍‌ଲୋଡ୍, ବ୍ରାଉଜରରେ ଖୋଜା କିମ୍ବା ବାରମ୍ବାର ରୂପାନ୍ତର ଆବଶ୍ୟକ ନାହିଁ।

## ସମର୍ଥିତ ଫର୍ମାଟ୍

ବର୍ତ୍ତମାନ ବିକାଶର ଲକ୍ଷ୍ୟ ଫର୍ମାଟ୍: `.xlsx`, `.xls`, `.ods`, `.csv` ଏବଂ `.tsv`।

କିଛି ଫର୍ମାଟ୍‌ରେ ଅତିରିକ୍ତ ସୁସଙ୍ଗତତା ସୀମା ଥାଇପାରେ। ସୁରକ୍ଷିତ ଭାବେ ରଖାଯାଇପାରୁ ନଥିବା ବିଶେଷତା ଥିବା ଫାଇଲ୍‌କୁ ସାବଧାନରେ ପରିଚାଳନା କରାଯାଏ, ଯାହାଦ୍ୱାରା ଅଜାଣତରେ ତଥ୍ୟ ହରାଇବା ନାହିଁ।

## Windows ପାଇଁ ତିଆରି

ZagoSheetsWin ବିଶେଷ ଭାବେ Windows ପାଇଁ ତିଆରି। ଏଥିରେ **Open with (ଏଥିରେ ଖୋଲନ୍ତୁ)**, ଫାଇଲ୍ ପ୍ରକାର ପଞ୍ଜୀକରଣ, ଦୁଇଥର କ୍ଲିକ୍‌ରେ ଖୋଲିବା, ଇଚ୍ଛାଧୀନ File Explorer ସମନ୍ୱୟ ଓ Windows ଇନ୍‌ଷ୍ଟଲର୍ ରହିଛି।

ଆପ୍ ଆପଣଙ୍କୁ ନ ଜଣାଇ Windows ର ଡିଫଲ୍ଟ ଆପ୍ ବଦଳାଏ ନାହିଁ। ଫାଇଲ୍ ସଂଯୋଗର ନିୟନ୍ତ୍ରଣ ବ୍ୟବହାରକାରୀଙ୍କ ପାଖରେ ରହେ।

## ଆରମ୍ଭରୁ ସୁରକ୍ଷା

ସ୍ଥାନୀୟ ଫାଇଲ୍ ବଦଳାଇବା ଏକ ପୁନରୁଦ୍ଧାରଯୋଗ୍ୟ କାର୍ଯ୍ୟ। ମୂଳ ଫାଇଲ୍‌କୁ ତାହାର ଫୋଲ୍ଡରରୁ ହଟାଇବା ପୂର୍ବରୁ ଆପ୍ ଯାଞ୍ଚ କରେ:

1. ପୁନରୁଦ୍ଧାରଯୋଗ୍ୟ ବ୍ୟାକଅପ୍ ଅଛି;
2. Google Sheets ଦଲିଲ ସଫଳତାର ସହିତ ତିଆରି ହୋଇଛି;
3. ସ୍ଥାନୀୟ ଫାଇଲ୍ ସଂଯୋଗ ସ୍ଥାୟୀ ଭାବେ ସଂରକ୍ଷିତ ହୋଇଛି;
4. ଇଣ୍ଟରନେଟ୍ ସର୍ଟକଟ୍ ଲେଖାଯାଇ ଯାଞ୍ଚ ହୋଇଛି।

ପ୍ରକ୍ରିୟା ବିଫଳ ହେଲେ ମୂଳ ଫାଇଲ୍ ରଖାଯାଏ।

> ବ୍ୟବହାରକାରୀଙ୍କୁ ନ ଜଣାଇ କେବେ ତାଙ୍କର ତଥ୍ୟ ନଷ୍ଟ କରନ୍ତୁ ନାହିଁ।

## ବ୍ୟାକଅପ୍

ସର୍ଟକଟ୍‌ରେ ବଦଳାଇବା ପୂର୍ବରୁ ମୂଳ ଫାଇଲ୍‌ଗୁଡ଼ିକୁ ବ୍ୟକ୍ତିଗତ ସ୍ଥାନୀୟ ବ୍ୟାକଅପ୍ ଅଞ୍ଚଳରେ ରଖାଯାଇପାରେ। ସଂରକ୍ଷଣ ସୀମା, ରଖିବାର ଅବଧି ଓ ସଫା କରିବାର ବିକଳ୍ପ ସେଟ୍ କରାଯାଇପାରେ।

ବ୍ୟାକଅପ୍ ଆମଦାନି ସମୟର ମୂଳ ଫାଇଲ୍‌କୁ ରକ୍ଷା କରେ। ଏହା **ଦୁଇମୁଖୀ ସିଙ୍କ୍ ନୁହେଁ**: Google Sheets ରେ ପରବର୍ତ୍ତୀ ପରିବର୍ତ୍ତନ ମୂଳ ସ୍ପ୍ରେଡ୍‌ସିଟ୍ ଫାଇଲ୍‌କୁ ଫେରି ଲେଖାଯାଏ ନାହିଁ।

## ଗୋପନୀୟତା ଏବଂ Google ପ୍ରବେଶ

ZagoSheetsWin ଆପଣଙ୍କ କମ୍ପ୍ୟୁଟରରୁ ସିଧା Google API ସହ ଯୋଗାଯୋଗ କରେ।

- ସ୍ପ୍ରେଡ୍‌ସିଟ୍‌ର ବିଷୟବସ୍ତୁ Zagotools ସର୍ଭରକୁ ପଠାଯାଏ ନାହିଁ।
- OAuth ଟୋକନ୍‌ଗୁଡ଼ିକ ସ୍ଥାନୀୟ ଭାବେ ରହେ ଓ Windows ସୁରକ୍ଷା ବ୍ୟବସ୍ଥାରେ ସୁରକ୍ଷିତ ହୁଏ।
- `drive.file` ଅନୁମତି ଆପ୍ ଦ୍ୱାରା ତିଆରି କିମ୍ବା ଖୋଲା ଫାଇଲ୍‌ରେ ପ୍ରବେଶକୁ ସୀମିତ କରେ।
- ଆମଦାନି ପ୍ରକ୍ରିୟାକୁ ବ୍ୟବହାର ବିଶ୍ଳେଷଣ ଟ୍ରାକିଂ ଦରକାର ନାହିଁ।

ଗୋପନୀୟତା ନୀତି ଓ ବ୍ୟବହାର ସର୍ତ୍ତ: https://zagotools.top/legal.html

## ପ୍ରକଳ୍ପର ସ୍ଥିତି

ZagoSheetsWin ଏବେ ସକ୍ରିୟ ବିକାଶରେ ଅଛି ଓ **ଆଲ୍‌ଫା ସଫ୍ଟୱେର୍** ଅଟେ। Windows → Google Sheets ର ମୁଖ୍ୟ ପ୍ରକ୍ରିୟା କାମ କରୁଛି। ଇନ୍‌ଷ୍ଟଲେସନ୍, ପୁନରୁଦ୍ଧାର, ଫର୍ମାଟ୍ ସୁସଙ୍ଗତତା, ଭାଷା ସମର୍ଥନ ଓ ବ୍ୟବହାରକାରୀ ଅନୁଭବ ସୁଧାରାଯାଉଛି। ପ୍ରଥମ ସ୍ଥିର ସଂସ୍କରଣ ପୂର୍ବରୁ ପରିବର୍ତ୍ତନ ହୋଇପାରେ।

## Open in Google ସହ ସମ୍ପର୍କ

ZagoSheetsWin, [SwatiK425](https://github.com/SwatiK425)ଙ୍କ [Open in Google](https://github.com/SwatiK425/open-in-google) ପ୍ରକଳ୍ପ ଉପରେ ଆଧାରିତ। ସେହି ପ୍ରକଳ୍ପ ପ୍ରଥମ ଆଧାର ଓ ପ୍ରେରଣା ଦେଇଥିଲା।

ଏବେ ZagoSheetsWin ନିଜସ୍ୱ ସ୍ଥାପତ୍ୟ, ଇନ୍‌ଷ୍ଟଲର୍, ଇଣ୍ଟରଫେସ୍, ବ୍ୟାକଅପ୍ ଓ ପୁନରୁଦ୍ଧାର, ଫାଇଲ୍ ସଂଯୋଗ, ଫର୍ମାଟ୍ ପରିଚାଳନା ଏବଂ ସ୍ଥାନୀୟ ଫାଇଲ୍ Google Sheets ରେ ଖୋଲିବାର ସୁବିଧା ଥିବା ସ୍ୱାଧୀନ Windows ଆପ୍। ମୂଳ ପ୍ରକଳ୍ପ ସ୍ୱାଧୀନ ରହେ; ଉପଯୁକ୍ତ ସାଧାରଣ ସୁଧାରକୁ upstream କୁ ପ୍ରସ୍ତାବ କରାଯାଇପାରେ।

## ମୁକ୍ତ ଉତ୍ସ

ZagoSheetsWin ମାଗଣା ଓ ମୁକ୍ତ ଉତ୍ସ ସଫ୍ଟୱେର୍। ମୂଳ Open in Google କୋଡ୍‌ର ଶ୍ରେୟ ଓ ଲାଇସେନ୍ସ ସର୍ତ୍ତ ରଖାଯାଇଛି ଏବଂ ZagoSheetsWin / Zagotools ର ପରବର୍ତ୍ତୀ ବିକାଶକୁ ସ୍ପଷ୍ଟ କରାଯାଇଛି।

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## ଲାଇସେନ୍ସ

MIT ଲାଇସେନ୍ସ। ବିବରଣୀ ପାଇଁ [LICENSE](../../../LICENSE) ଦେଖନ୍ତୁ।

---

**ZagoSheetsWin — Zagotools ପ୍ରକଳ୍ପ**

ବାସ୍ତବ ସମସ୍ୟା ପାଇଁ ଛୋଟ ସଫ୍ଟୱେର୍।
