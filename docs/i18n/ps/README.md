# ZagoSheetsWin

> ⚠️ **0.9.35 — Important safety update (English notice; full translation review pending).**
>
> سپریډشیټ په Google Sheets کې پرانیستل شو. اصلي فایل په لنډلار بدل شو. د بېرته ترلاسه کولو لپاره یې بیک‌اپونه پرانیزئ.
>
> **Conversion fidelity:** After a confirmed Google import, the original local file **may be replaced with a `.url` shortcut even when formulas, values or formatting differ**. The original bytes remain in Backups. Choose **Import as copy BEFORE opening** if you need the original to stay in the source folder. Windows **read-only** files may block replacement; in that case, the original is preserved. Some text below describes an older alpha policy; use the [current English README](../../../README.md) or [Portuguese user guide](../../USER_GUIDE.md) for complete 0.9.35 behavior.


<details>
<summary>🌐 د اسنادو ژبه · ژبه وټاکئ</summary>

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
- **PS - پښتو / Pashto (ps)** — اوسنی مخ
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

**په Windows کې خپل محلي سپرېډشېټ فایلونه نېغ په نېغه په Google Sheets کې پرانیزئ.**

ZagoSheetsWin د Windows لپاره یو سپک اپلېکېشن دی چې د محلي سپرېډشېټ فایل پرانیستل اسانه کوي:

**پر فایل دوه ځله کلیک → پورته کول او بدلول → په Google Sheets کې پرانیستل**

له بریالي واردولو وروسته، ZagoSheetsWin کولی شي اصلي محلي فایل د انټرنېټ په یوه لنډلار (`.url`) بدل کړي چې د Google Sheets سند ته ځي، او د اصلي فایل د بېرته ترلاسه کولو وړ محلي خوندي کاپي وساتي.

موخه ساده ده: د محلي سپرېډشېټونو د پرانیستلو لپاره Google Sheets د عادي Windows اپلېکېشن په څېر وکارول شي.

## څه کوي

ZagoSheetsWin د Windows سپرېډشېټ فایلونه له Google Sheets سره نښلوي. کله چې ملاتړ شوی محلي فایل پرانیزئ، اپلېکېشن کولی شي:

- سپرېډشېټ وپېژني او اعتبار یې وڅېړي؛
- د بېرته ترلاسه کولو وړ خوندي کاپي جوړه کړي؛
- د Google د رسمي API له لارې فایل نېغ Google Drive ته پورته کړي؛
- هغه په اصلي Google Sheets سند بدل کړي؛
- پایله په ټاکل شوي براوزر کې پرانیزي؛
- د Google سند لپاره محلي `.url` لنډلار جوړه کړي؛
- وروسته د هماغه فایل د بیا پرانیستلو پر وخت له تکراري پورته کولو ډډه وکړي.

Drive ته د لاس په وسیله پورته کول، په براوزر کې پلټل او بیا بیا بدلول اړین نه دي.

## ملاتړ شوي بڼې

په اوسني پرمختګ کې موخه شوي فایل ډولونه: `.xlsx`، `.xls`، `.ods`، `.csv` او `.tsv`.

ځینې بڼې ښايي د سازګارۍ نور محدودیتونه ولري. هغه فایلونه چې ځینې ځانګړنې یې په خوندي ډول نه شي ساتل کېدای، په احتیاط اداره کېږي څو معلومات په پټه له لاسه ولاړ نه شي.

## د Windows لپاره جوړ شوی

ZagoSheetsWin په ځانګړي ډول د Windows لپاره جوړ شوی او **Open with (له دې سره پرانیستل)**، د فایل ډول ثبتول، په دوه ځله کلیک پرانیستل، له File Explorer سره اختیاري یوځای کېدل او اصلي Windows انسټالر لري.

اپلېکېشن ستاسو له خبرتیا پرته د Windows ډیفالټ اپلېکېشنونه نه بدلوي. د فایل اړیکو کنټرول له کاروونکي سره پاتې کېږي.

## خوندیتوب له پیله

د محلي فایل بدلول باید د بېرته راګرځولو وړ عملیات وي. مخکې له دې چې اصلي فایل له خپل فولډر څخه لرې شي، اپلېکېشن ګوري چې:

1. د بېرته ترلاسه کولو وړ خوندي کاپي شته؛
2. د Google Sheets سند په بریالیتوب جوړ شوی؛
3. د محلي فایل اړیکه په دوامدار ډول خوندي شوې؛
4. د انټرنېټ لنډلار لیکل شوې او تایید شوې ده.

که عملیات ناکام شي، اصلي فایل خوندي پاتې کېږي.

> د کاروونکي معلومات هېڅکله د هغه له خبرتیا پرته مه له منځه وړئ.

## خوندي کاپۍ

اصلي فایلونه د لنډلارو په وسیله تر بدلولو مخکې په شخصي محلي خوندي ځای کې ساتل کېدای شي. د ذخیرې حد، د ساتنې موده او د پاکولو اختیارونه تنظیمېدلی شي.

خوندي کاپي د واردولو پر وخت اصلي فایل ساتي. دا **دوه اړخیز همغږي کول نه دي**: هغه بدلونونه چې وروسته په Google Sheets کې کېږي، بېرته په اصلي سپرېډشېټ فایل کې نه لیکل کېږي.

## محرمیت او Google ته لاسرسی

ZagoSheetsWin ستاسو له کمپیوټر څخه نېغ د Google له API سره اړیکه نیسي.

- د سپرېډشېټ منځپانګه د Zagotools سرورونو ته نه لېږل کېږي.
- OAuth ټوکنونه په محلي ډول ساتل کېږي او د Windows امنیتي میکانېزمونه یې خوندي کوي.
- د `drive.file` اجازې ساحه لاسرسی هغو فایلونو ته محدودوي چې اپلېکېشن جوړ یا پرانیستي وي.
- د سپرېډشېټ واردولو لپاره د کارونې د تحلیل تعقیب ته اړتیا نشته.

د محرمیت تګلاره او د کارولو شرایط: https://zagotools.top/legal.html

## د پروژې حالت

ZagoSheetsWin لا هم د پرمختګ په حال کې دی او اوس **د الفا پړاو سافټویر** دی. د Windows → Google Sheets اساسي بهیر کار کوي. نصب، بیا رغونه، د فایلونو سازګاري، نړیوالې ژبې او د کاروونکي تجربه لا ښه کېږي. تر لومړۍ ثابتې نسخې مخکې بدلونونه راتلای شي.

## له Open in Google سره اړیکه

ZagoSheetsWin د [SwatiK425](https://github.com/SwatiK425) د [Open in Google](https://github.com/SwatiK425/open-in-google) پر بنسټ جوړ شوی او ترې رامنځته شوی دی. هغه پروژه یې لومړنی بنسټ او الهام و.

اوس ZagoSheetsWin د Windows یو خپلواک اپلېکېشن دی چې خپله جوړښت، انسټالر، انترفېس، خوندي کاپۍ او بیا رغونه، د فایل اړیکې، د فایل بڼو اداره او په Google Sheets کې د محلي فایلونو د پرانیستلو بهیر لري. اصلي پروژه خپلواکه ده او مناسب عمومي اصلاحات upstream ته وړاندې کېدای شي.

## پرانیستې سرچینه

ZagoSheetsWin وړیا او پرانیستې سرچینې سافټویر دی. د Open in Google د اصلي کوډ د لیکوال حق او د جواز شرطونه ساتي او د ZagoSheetsWin / Zagotools وروستي پرمختګونه جلا څرګندوي.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## جواز

د MIT جواز. د نورو معلوماتو لپاره [LICENSE](../../../LICENSE) وګورئ.

---

**ZagoSheetsWin — د Zagotools پروژه**

د رښتینو ستونزو لپاره کوچنی سافټویر.
