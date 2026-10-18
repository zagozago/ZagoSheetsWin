# ZagoSheetsWin

> ⚠️ **0.9.37 — Important safety update (English notice; full translation review pending).**
>
> اسپریڈشیٹ Google Sheets میں کھول دی گئی ہے۔ اصل فائل کی جگہ شارٹ کٹ بنایا گیا ہے۔ اصل فائل واپس حاصل کرنے کے لیے بیک اپ کھولیں۔
>
> **Conversion fidelity:** After a confirmed Google import, the original local file **may be replaced with a `.url` shortcut even when formulas, values or formatting differ**. The original bytes remain in Backups. Choose **Import as copy BEFORE opening** if you need the original to stay in the source folder. Windows **read-only** files may block replacement; in that case, the original is preserved. **New in 0.9.36:** CSV/TSV detects comma, semicolon, TAB and pipe; a per-import chooser resolves ambiguity. The bounded Windows-1252 fallback and higher text limits (60,000 logical rows / 600,000 cells) apply to new interpretations. **Backups → More actions → Reimport into a new document** repairs from the verified original backup without overwriting the previous document/shortcut. **New in 0.9.37:** the app, installer, file associations and shortcuts share one spreadsheet icon. Some text below describes an older alpha policy; use the [current English README](../../../README.md) or [Portuguese user guide](../../USER_GUIDE.md) for complete 0.9.37 behavior.


<details>
<summary>🌐 دستاویز کی زبان منتخب کریں</summary>

- [EN - English](../../../README.md)
- [ZH - 中文 / Chinese (Simplified) (zh)](../zh/README.md)
- [HI - हिन्दी / Hindi (hi)](../hi/README.md)
- [ES - Español / Spanish (es)](../es/README.md)
- [AR - العربية / Arabic (ar)](../ar/README.md)
- [FR - Français / French (fr)](../fr/README.md)
- [BN - বাংলা / Bengali (bn)](../bn/README.md)
- [PT - Português / Portuguese (pt)](../pt/README.md)
- [ID - Bahasa Indonesia / Indonesian (id)](../id/README.md)
- **UR - اردو / Urdu (ur)** — موجودہ صفحہ
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
- [NL - Nederlands / Dutch (nl)](../nl/README.md)
- [GN - Avañe’ẽ / Guarani (gn)](../gn/README.md)

</details>

**Windows سے مقامی اسپریڈشیٹ فائلیں براہِ راست Google Sheets میں کھولیں۔**

ZagoSheetsWin ایک ہلکی پھلکی Windows ایپ ہے جو مقامی اسپریڈشیٹ کھولنے کے عمل کو آسان بناتی ہے:

**فائل پر ڈبل کلک ← اپ لوڈ اور تبدیل کریں ← Google Sheets میں کھولیں**

کامیاب درآمد کے بعد ZagoSheetsWin اصل مقامی فائل کی جگہ ایک انٹرنیٹ شارٹ کٹ (`.url`) رکھ سکتی ہے جو Google Sheets دستاویز کی طرف لے جاتا ہے، جبکہ اصل فائل کا قابلِ بازیافت مقامی بیک اپ بھی محفوظ رہتا ہے۔

مقصد سادہ ہے: مقامی اسپریڈشیٹ کھولتے وقت Google Sheets کو Windows کی مقامی ایپ جیسا تجربہ بنانا۔

## یہ کیا کرتی ہے

ZagoSheetsWin، Windows کی اسپریڈشیٹ فائلوں کو Google Sheets سے منسلک کرتی ہے۔

معاونت یافتہ مقامی فائل کھولنے پر ایپ:

- اسپریڈشیٹ کی شناخت اور توثیق کر سکتی ہے؛
- قابلِ بازیافت بیک اپ بنا سکتی ہے؛
- Google کے سرکاری API کے ذریعے فائل براہِ راست Google Drive پر اپ لوڈ کر سکتی ہے؛
- اسے Google Sheets کی اصل نوعیت کی دستاویز میں تبدیل کر سکتی ہے؛
- حاصل شدہ اسپریڈشیٹ کو آپ کے ڈیفالٹ براؤزر میں کھول سکتی ہے؛
- Google دستاویز کا مقامی `.url` شارٹ کٹ بنا سکتی ہے؛
- بعد میں وہی فائل دوبارہ کھولنے پر غیر ضروری اپ لوڈ سے بچ سکتی ہے۔

Drive پر دستی اپ لوڈ نہیں۔ براؤزر میں فائل تلاش نہیں۔ بار بار تبدیلی نہیں۔

## معاونت یافتہ فارمیٹس

موجودہ ترقی میں ہدف بنائے گئے فارمیٹس:

- `.xlsx`
- `.xls`
- `.ods`
- `.csv`
- `.tsv`

بعض فارمیٹس میں اضافی مطابقت کی پابندیاں ہو سکتی ہیں۔ جن خصوصیات کو محفوظ طریقے سے برقرار نہیں رکھا جا سکتا، ایسی فائلوں کو احتیاط سے سنبھالا جاتا ہے تاکہ خاموشی سے ڈیٹا ضائع نہ ہو۔

## Windows کے لیے بنایا گیا

ZagoSheetsWin خاص طور پر Windows کے لیے تیار کیا گیا ہے اور آپریٹنگ سسٹم سے ان ذرائع کے ذریعے جڑتا ہے:

- **Open with (اس کے ساتھ کھولیں)**
- فائل کی اقسام کا اندراج
- ڈبل کلک سے فائل کھولنا
- File Explorer کے ساتھ اختیاری انضمام
- Windows کا مقامی انسٹالر

ایپ آپ کی ڈیفالٹ Windows ایپس خاموشی سے تبدیل نہیں کرتی۔ فائل وابستگیوں کا اختیار صارف کے پاس رہتا ہے۔

## ابتدا ہی سے تحفظ

ZagoSheetsWin مقامی فائل کی تبدیلی کو قابلِ بازیافت کارروائی سمجھتا ہے۔

اصل فائل کو اس کے فولڈر سے ہٹانے سے پہلے ایپ تصدیق کرتی ہے کہ:

1. قابلِ بازیافت بیک اپ موجود ہے؛
2. Google Sheets دستاویز کامیابی سے بن گئی ہے؛
3. مقامی وابستگی مستقل طور پر محفوظ ہو چکی ہے؛
4. انٹرنیٹ شارٹ کٹ کامیابی سے لکھا اور جانچا گیا ہے۔

عمل ناکام ہونے کی صورت میں اصل فائل محفوظ رہتی ہے۔

ایپ ایک سادہ اصول پر چلتی ہے:

> صارف کو بتائے بغیر اس کا ڈیٹا کبھی ضائع نہ کریں۔

## بیک اپ

اصل فائلوں کو شارٹ کٹس سے بدلنے سے پہلے ایک نجی مقامی بیک اپ جگہ میں محفوظ کیا جا سکتا ہے۔

بیک اپ کے انتظام میں قابلِ ترتیب ذخیرہ حد، برقرار رکھنے کی پالیسیاں اور صفائی کے کنٹرول شامل ہیں۔

بیک اپ درآمد شدہ اصل فائل کو محفوظ رکھتے ہیں۔ یہ **دو طرفہ ہم وقت سازی نہیں**: Google Sheets میں بعد میں کی گئی تبدیلیاں اصل اسپریڈشیٹ فائل میں واپس نہیں لکھی جاتیں۔

## رازداری اور Google تک رسائی

ZagoSheetsWin آپ کے کمپیوٹر سے براہِ راست Google کے API سے رابطہ کرتی ہے۔

- آپ کی اسپریڈشیٹ کا مواد Zagotools سرور کو نہیں بھیجا جاتا۔
- OAuth ٹوکن مقامی طور پر محفوظ کیے جاتے ہیں اور Windows کے حفاظتی طریقوں سے محفوظ رہتے ہیں۔
- ایپ Google Drive کا `drive.file` اجازت نامہ استعمال کرتی ہے، جس سے رسائی ان فائلوں تک محدود رہتی ہے جو ایپ کے ذریعے بنائی یا کھولی گئی ہیں۔
- اسپریڈشیٹ درآمد کرنے کے عمل کے لیے تجزیاتی ٹریکنگ ضروری نہیں۔

رازداری کی پالیسی اور استعمال کی شرائط:

https://zagotools.top/legal.html

## منصوبے کی موجودہ حالت

ZagoSheetsWin پر فعال ترقی جاری ہے اور اسے فی الحال **الفا مرحلے کا سافٹ ویئر** سمجھنا چاہیے۔

Windows → Google Sheets کا بنیادی طریقۂ کار کام کر رہا ہے۔ تنصیب، بازیافت، فارمیٹ مطابقت، بین الاقوامی زبانوں کی معاونت اور صارف کے تجربے میں بہتری جاری ہے۔

پہلے مستحکم اجرا سے پہلے تبدیلیاں ہو سکتی ہیں۔

## Open in Google سے تعلق

ZagoSheetsWin، [SwatiK425](https://github.com/SwatiK425) کے منصوبے [Open in Google](https://github.com/SwatiK425/open-in-google) پر مبنی اور اسی سے اخذ کیا گیا ہے۔

Open in Google نے اس منصوبے کو ابتدائی بنیاد اور تحریک فراہم کی۔

اس کے بعد ZagoSheetsWin اپنے الگ ڈھانچے، انسٹالر، انٹرفیس، بیک اپ اور بازیافت کے نظام، فائل وابستگی کے طریقۂ کار، فارمیٹس کی معاونت اور مقامی فائلوں کو Google Sheets میں کھولنے کے تجربے کے ساتھ ایک آزاد Windows ایپ بن چکا ہے۔

اصل منصوبہ خود مختار ہے۔ موزوں عمومی بہتریاں اصل منصوبے کو پیش کی جا سکتی ہیں، جبکہ ZagoSheetsWin آزادانہ ترقی جاری رکھے گا۔

## اوپن سورس

ZagoSheetsWin مفت اور اوپن سورس سافٹ ویئر ہے۔

یہ منصوبہ Open in Google کے اصل کوڈ سے متعلق انتساب اور لائسنس کی شرائط برقرار رکھتا ہے اور ZagoSheetsWin / Zagotools کی بعد کی ترقی کو واضح طور پر الگ شناخت دیتا ہے۔

دیکھیے:

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## لائسنس

MIT لائسنس۔

تفصیلات کے لیے [LICENSE](../../../LICENSE) دیکھیں۔

---

**ZagoSheetsWin — Zagotools کا ایک منصوبہ**

حقیقی مسائل کے لیے چھوٹا سافٹ ویئر۔
