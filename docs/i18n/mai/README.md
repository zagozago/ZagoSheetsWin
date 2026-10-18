# ZagoSheetsWin

> ⚠️ **0.9.37 — Important safety update (English notice; full translation review pending).**
>
> स्प्रेडशीट Google Sheets मे खुजि गेल अछि। मूल फाइलक स्थान पर शॉर्टकट बनाओल गेल अछि। मूल फाइल वापस पाबय लेल बैकअप खोलू।
>
> **Conversion fidelity:** After a confirmed Google import, the original local file **may be replaced with a `.url` shortcut even when formulas, values or formatting differ**. The original bytes remain in Backups. Choose **Import as copy BEFORE opening** if you need the original to stay in the source folder. Windows **read-only** files may block replacement; in that case, the original is preserved. **New in 0.9.36:** CSV/TSV detects comma, semicolon, TAB and pipe; a per-import chooser resolves ambiguity. The bounded Windows-1252 fallback and higher text limits (60,000 logical rows / 600,000 cells) apply to new interpretations. **Backups → More actions → Reimport into a new document** repairs from the verified original backup without overwriting the previous document/shortcut. **New in 0.9.37:** the app, installer, file associations and shortcuts share one spreadsheet icon. Some text below describes an older alpha policy; use the [current English README](../../../README.md) or [Portuguese user guide](../../USER_GUIDE.md) for complete 0.9.37 behavior.


<details>
<summary>🌐 दस्तावेजक भाषा · भाषा चुनू</summary>

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
- **MAI - मैथिली / Maithili (mai)** — वर्तमान पन्ना
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

**Windows सँ अपन कंप्यूटरक स्थानीय स्प्रेडशीट फाइल सीधे Google Sheets मे खोलू।**

ZagoSheetsWin Windows लेल हलुक ऐप अछि, जे स्थानीय स्प्रेडशीट खोलबाक प्रक्रिया सरल बनबैत अछि:

**फाइल पर दू बेर क्लिक → अपलोड आ रूपांतरण → Google Sheets मे खोलू**

सफल आयातक बाद ZagoSheetsWin मूल स्थानीय फाइलक स्थान पर Google Sheets दस्तावेज धरि लऽ जाएवला इंटरनेट शॉर्टकट (`.url`) राखि सकैत अछि। मूल फाइलक फेर सँ प्राप्त कयल जा सकैवला स्थानीय बैकअप सेहो सुरक्षित राखल जाइत अछि।

उद्देश्य सरल अछि: स्थानीय स्प्रेडशीट खोलैत समय Google Sheets सामान्य Windows ऐप जकाँ काज करय।

## ई की करैत अछि

ZagoSheetsWin Windows केर स्प्रेडशीट फाइल सभकेँ Google Sheets सँ जोड़ैत अछि। समर्थित स्थानीय फाइल खोलला पर ऐप:

- स्प्रेडशीटक पहचान आ जाँच कऽ सकैत अछि;
- फेर सँ प्राप्त करबाक योग्य बैकअप बना सकैत अछि;
- Google केर आधिकारिक API सँ सीधे Google Drive पर अपलोड कऽ सकैत अछि;
- ओकरा मूल Google Sheets दस्तावेज मे बदलि सकैत अछि;
- परिणाम डिफॉल्ट ब्राउजर मे खोलि सकैत अछि;
- Google दस्तावेज लेल स्थानीय `.url` शॉर्टकट बना सकैत अछि;
- अगिला बेर ओही फाइल खोलला पर फेर अपलोड होयब सँ बचा सकैत अछि।

Drive पर हाथे अपलोड करब, ब्राउजर मे फाइल ताकब वा बेर-बेर रूपांतरण करब जरूरी नहि।

## समर्थित प्रारूप

वर्तमान विकास मे लक्षित प्रारूप अछि: `.xlsx`, `.xls`, `.ods`, `.csv`, `.tsv`।

किछु प्रारूप मे अतिरिक्त अनुकूलता सीमा रहि सकैत अछि। जाहि फाइलक विशेषता सुरक्षित रूप सँ नहि बचाओल जा सकै, तकरा सावधानी सँ सँभारल जाइत अछि ताकि बिना जानकारीक डेटा नहि हराय।

## Windows लेल बनाओल गेल

ZagoSheetsWin विशेष रूप सँ Windows लेल विकसित अछि। एकरा मे **Open with (एकरा सँ खोलू)**, फाइल प्रकारक पंजीकरण, दू बेर क्लिक सँ खोलब, वैकल्पिक File Explorer एकीकरण आ मूल Windows इंस्टॉलर शामिल अछि।

एप्प उपयोगकर्ताकेँ नहि जनौने Windows केर डिफॉल्ट ऐप नहि बदलैत अछि। फाइल संबंधक नियंत्रण उपयोगकर्ताक हाथ मे रहैत अछि।

## प्रारंभ सँ सुरक्षा

स्थानीय फाइलक प्रतिस्थापन एहन प्रक्रिया होयबाक चाही जकरा फेर सँ ठीक कयल जा सकय। मूल फाइल फोल्डर सँ हटेबा सँ पहिने ऐप सुनिश्चित करैत अछि जे:

1. पुनर्प्राप्ति योग्य बैकअप उपलब्ध अछि;
2. Google Sheets दस्तावेज सफलतापूर्वक बनि गेल अछि;
3. स्थानीय फाइलक संबंध स्थायी रूप सँ सुरक्षित अछि;
4. इंटरनेट शॉर्टकट लिखि आ जाँचि लेल गेल अछि।

प्रक्रिया असफल भेलापर मूल फाइल सुरक्षित रहैत अछि।

> उपयोगकर्ताकेँ नहि जनौने ओकर डेटा कहियो नष्ट नहि करू।

## बैकअप

मूल फाइल शॉर्टकट सँ बदलला सँ पहिने निजी स्थानीय बैकअप स्थान मे राखल जा सकैत अछि। भंडारण सीमा, संरक्षण अवधि आ सफाइक विकल्प निर्धारित कयल जा सकैत अछि।

बैकअप आयातक समयक मूल फाइलक सुरक्षा करैत अछि। ई **दू दिशा मे सिंक नहि अछि**: Google Sheets मे बाद मे कयल परिवर्तन मूल स्प्रेडशीट फाइल मे फेर नहि लिखल जाइत अछि।

## गोपनीयता आ Google पहुँच

ZagoSheetsWin अहाँक कंप्यूटर सँ सीधे Google API सँ संपर्क करैत अछि।

- स्प्रेडशीटक सामग्री Zagotools सर्वर पर नहि पठाओल जाइत अछि।
- OAuth टोकन स्थानीय रूप सँ सुरक्षित अछि आ Windows सुरक्षा प्रणाली ओकर रक्षा करैत अछि।
- `drive.file` अनुमति ऐप सँ बनाओल वा खोलल फाइल सभ धरि पहुँच सीमित करैत अछि।
- स्प्रेडशीट आयात लेल एनालिटिक्स ट्रैकिंग जरूरी नहि अछि।

गोपनीयता नीति आ उपयोगक शर्त: https://zagotools.top/legal.html

## परियोजनाक स्थिति

ZagoSheetsWin केर सक्रिय विकास चलि रहल अछि आ एखन ई **अल्फा सॉफ्टवेयर** अछि। Windows → Google Sheets केर मुख्य प्रक्रिया काज करैत अछि। स्थापना, पुनर्प्राप्ति, प्रारूप अनुकूलता, अंतरराष्ट्रीयकरण आ उपयोगकर्ता अनुभव मे सुधार जारी अछि। पहिल स्थिर संस्करण सँ पहिने बदलाव भऽ सकैत अछि।

## Open in Google सँ संबंध

ZagoSheetsWin, [SwatiK425](https://github.com/SwatiK425) केर [Open in Google](https://github.com/SwatiK425/open-in-google) परियोजना पर आधारित विकसित अछि। ओ मूल परियोजना शुरुआती आधार आ प्रेरणा देने छल।

एखन ZagoSheetsWin अपन संरचना, इंस्टॉलर, इंटरफेस, बैकअप आ पुनर्प्राप्ति व्यवस्था, फाइल संबंध, प्रारूप प्रबंधन आ स्थानीय फाइल Google Sheets मे खोलबाक अलग तरीका सहित स्वतंत्र Windows ऐप अछि। मूल परियोजना स्वतंत्र अछि; उपयुक्त सामान्य सुधार upstream केर समक्ष प्रस्तुत कयल जा सकैत अछि।

## मुक्त स्रोत

ZagoSheetsWin मुफ्त आ मुक्त-स्रोत सॉफ्टवेयर अछि। मूल Open in Google कोडक श्रेय आ लाइसेंसक शर्त सुरक्षित राखल जाइत अछि, आ ZagoSheetsWin / Zagotools केर बादक विकास अलग सँ दर्शाओल जाइत अछि।

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## लाइसेंस

MIT लाइसेंस। विवरण लेल [LICENSE](../../../LICENSE) देखू।

---

**ZagoSheetsWin — Zagotools परियोजना**

वास्तविक समस्या लेल छोट सॉफ्टवेयर।
