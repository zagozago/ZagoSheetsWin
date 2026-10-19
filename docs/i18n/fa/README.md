# ZagoSheetsWin

> ⚠️ **Alpha 0.9.42 — English safety notice; full editorial translation pending.**
>
> صفحه‌گسترده در Google Sheets باز شد. فایل اصلی با یک میان‌بر جایگزین شد. برای بازیابی آن، پشتیبان‌ها را باز کنید.
>
> **CSV/TSV v4:** up to **2,000,000 cells**, **100,000 logical rows**, 1,000 columns and 20 MiB input/payload, subject to other limits. Real delimiter ambiguity shows a local preview. If converted Google cell values differ, **the original must NOT be replaced by a shortcut**.
>
> **XLS/ODS:** complex features may still be converted with partial fidelity and the source **may** be replaced after verified import; the original bytes remain recoverable in Backups. Use **Import as copy** if you must keep the source file in place.
>
> **Backups:** automatic cleanup by age and quota, **no toggle**. Only eligible completed backups are cleaned; quota pressure may delete them before retention expires. Files from different sources can import in parallel; only actual backup-capacity shortages can cause a cancellable wait.
>
> **Release state:** 0.9.42 is **not a stable release**. The latest Windows CI and preceding protected distribution had failing checks; real-Google acceptance remains pending. Some text below describes older alpha behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 زبان مستندات · انتخاب زبان</summary>

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
- **FA - فارسی / Persian (fa)** — صفحهٔ فعلی
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

**فایل‌های صفحه‌گستردهٔ محلی را در ویندوز مستقیماً با Google Sheets باز کنید.**

ZagoSheetsWin برنامه‌ای سبک برای ویندوز است که باز کردن صفحه‌گسترده‌های محلی را آسان می‌کند:

**دوبار کلیک روی فایل ← بارگذاری و تبدیل ← باز کردن در Google Sheets**

پس از وارد کردن موفق، ZagoSheetsWin می‌تواند فایل محلی اصلی را با میان‌بر اینترنتی (`.url`) به سند Google Sheets جایگزین کند و هم‌زمان نسخهٔ پشتیبان محلیِ قابل‌بازیابی از فایل اصلی نگه دارد.

هدف ساده است: باز کردن صفحه‌گسترده‌های محلی در Google Sheets مانند استفاده از یک برنامهٔ بومی ویندوز باشد.

## چه کار می‌کند

ZagoSheetsWin فایل‌های صفحه‌گستردهٔ ویندوز را به Google Sheets متصل می‌کند. هنگام باز کردن یک فایل محلی پشتیبانی‌شده، برنامه می‌تواند:

- صفحه‌گسترده را شناسایی و اعتبارسنجی کند؛
- نسخهٔ پشتیبان قابل‌بازیابی تهیه کند؛
- فایل را مستقیماً با APIهای رسمی Google در Google Drive بارگذاری کند؛
- آن را به سند بومی Google Sheets تبدیل کند؛
- نتیجه را در مرورگر پیش‌فرض باز کند؛
- میان‌بر محلی `.url` به سند Google بسازد؛
- در باز کردن‌های بعدی، از بارگذاری دوبارهٔ همان فایل جلوگیری کند.

دیگر نیازی به بارگذاری دستی در Drive، جست‌وجو در مرورگر یا تبدیل مکرر نیست.

## قالب‌های پشتیبانی‌شده

قالب‌های هدف در توسعهٔ فعلی عبارت‌اند از: `.xlsx`، `.xls`، `.ods`، `.csv` و `.tsv`.

ممکن است بعضی قالب‌ها محدودیت سازگاری بیشتری داشته باشند. فایل‌هایی که برخی قابلیت‌های آن‌ها را نمی‌توان با ایمنی حفظ کرد، با احتیاط پردازش می‌شوند تا اطلاعات بدون اطلاع کاربر از دست نرود.

## طراحی‌شده برای ویندوز

ZagoSheetsWin به‌طور ویژه برای ویندوز ساخته شده است و از **Open with (باز کردن با)**، ثبت نوع فایل، باز کردن با دوبار کلیک، یکپارچگی اختیاری با File Explorer و نصب‌کنندهٔ بومی ویندوز بهره می‌برد.

برنامه، نرم‌افزارهای پیش‌فرض ویندوز را بدون اطلاع شما تغییر نمی‌دهد. کنترل ارتباط فایل‌ها با برنامه‌ها در اختیار کاربر می‌ماند.

## ایمنی از مرحلهٔ طراحی

جایگزینی فایل محلی عملیاتی قابل‌بازیابی در نظر گرفته می‌شود. پیش از برداشتن فایل اصلی از پوشه، برنامه بررسی می‌کند که:

1. نسخهٔ پشتیبان قابل‌بازیابی وجود دارد؛
2. سند Google Sheets با موفقیت ساخته شده است؛
3. ارتباط محلی فایل به‌صورت پایدار ذخیره شده است؛
4. میان‌بر اینترنتی نوشته و اعتبارسنجی شده است.

اگر فرایند ناموفق باشد، فایل اصلی حفظ می‌شود.

> هرگز داده‌های کاربر را بدون اطلاع او از بین نبرید.

## نسخه‌های پشتیبان

فایل‌های اصلی می‌توانند پیش از جایگزینی با میان‌برها در فضای خصوصیِ پشتیبان‌گیری محلی ذخیره شوند. محدودیت حجم، دورهٔ نگهداری و گزینه‌های پاک‌سازی قابل تنظیم‌اند.

نسخهٔ پشتیبان از فایل اصلی در لحظهٔ وارد کردن محافظت می‌کند. این قابلیت **همگام‌سازی دوطرفه نیست**: تغییرات بعدی در Google Sheets به فایل صفحه‌گستردهٔ اصلی بازنویسی نمی‌شوند.

## حریم خصوصی و دسترسی به Google

ZagoSheetsWin مستقیماً از رایانهٔ شما با APIهای Google ارتباط برقرار می‌کند.

- محتوای صفحه‌گسترده‌ها به سرورهای Zagotools فرستاده نمی‌شود.
- توکن‌های OAuth به‌صورت محلی ذخیره شده و با سازوکارهای امنیتی ویندوز محافظت می‌شوند.
- محدودهٔ مجوز `drive.file` دسترسی را به فایل‌هایی محدود می‌کند که از طریق برنامه ایجاد یا باز شده‌اند.
- فرایند وارد کردن به ردیابی تحلیلی نیاز ندارد.

سیاست حفظ حریم خصوصی و شرایط استفاده: https://zagotools.top/legal.html

## وضعیت پروژه

ZagoSheetsWin در حال توسعهٔ فعال است و فعلاً باید آن را **نرم‌افزار مرحلهٔ آلفا** دانست. روند اصلی Windows → Google Sheets کار می‌کند و نصب، بازیابی، سازگاری قالب‌ها، بین‌المللی‌سازی و تجربهٔ کاربری بهبود می‌یابند. ممکن است پیش از اولین نسخهٔ پایدار تغییراتی رخ دهد.

## ارتباط با Open in Google

ZagoSheetsWin از پروژهٔ [Open in Google](https://github.com/SwatiK425/open-in-google) ساختهٔ [SwatiK425](https://github.com/SwatiK425) مشتق شده و بر آن استوار است. آن پروژه پایه و الهام اولیه را فراهم کرد.

اکنون ZagoSheetsWin برنامه‌ای مستقل برای ویندوز است که معماری، نصب‌کننده، رابط، پشتیبان‌گیری و بازیابی، ارتباط فایل‌ها، مدیریت قالب‌ها و تجربهٔ باز کردن فایل‌های محلی در Google Sheets را به‌صورت اختصاصی دارد. پروژهٔ اصلی مستقل است و بهبودهای عمومی را می‌توان در صورت مناسب بودن به upstream پیشنهاد کرد.

## متن‌باز

ZagoSheetsWin نرم‌افزاری رایگان و متن‌باز است. اعتبار پدیدآورندگان و شرایط مجوز کد اصلی Open in Google حفظ می‌شوند و توسعه‌های بعدی ZagoSheetsWin / Zagotools جداگانه مشخص شده‌اند.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## مجوز

مجوز MIT. برای جزئیات [LICENSE](../../../LICENSE) را ببینید.

---

**ZagoSheetsWin — پروژه‌ای از Zagotools**

نرم‌افزار کوچک برای مشکلات واقعی.
