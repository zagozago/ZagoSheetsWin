# ZagoSheetsWin

> ⚠️ **Alpha 0.9.43 — current safety notice in English; full editorial translation pending.**
>
> فُتح جدول البيانات في Google Sheets. استُبدل الملف الأصلي باختصار. لاستعادته، افتح النسخ الاحتياطية.
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.43:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [Local core](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) and [protected distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) passed on commit `9fbec2a`; real Google acceptance is still pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 لغات التوثيق · اختر لغتك</summary>

- [EN - English](../../../README.md)
- [ZH - 中文 / Chinese (Simplified) (zh)](../zh/README.md)
- [HI - हिन्दी / Hindi (hi)](../hi/README.md)
- [ES - Español / Spanish (es)](../es/README.md)
- **AR - العربية / Arabic (ar)** — الصفحة الحالية
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
- [NL - Nederlands / Dutch (nl)](../nl/README.md)
- [GN - Avañe’ẽ / Guarani (gn)](../gn/README.md)

</details>

**افتح ملفات جداول البيانات المحلية مباشرةً في جداول بيانات Google من Windows.**

ZagoSheetsWin تطبيق خفيف لنظام Windows يجعل فتح جدول بيانات محلي عملية بسيطة:

**انقر نقرًا مزدوجًا على الملف ← ارفع الملف وحوّله ← افتحه في جداول بيانات Google**

بعد الاستيراد بنجاح، يمكن لتطبيق ZagoSheetsWin استبدال الملف المحلي الأصلي باختصار إنترنت (`.url`) يؤدي إلى مستند جداول بيانات Google، مع الاحتفاظ بنسخة احتياطية محلية من الملف الأصلي يمكن استعادتها.

الهدف بسيط: جعل جداول بيانات Google تبدو كتطبيق Windows أصلي عند فتح ملفات جداول البيانات المحلية.

## ماذا يفعل؟

يربط ZagoSheetsWin ملفات جداول البيانات في Windows بخدمة جداول بيانات Google.

عند فتح ملف محلي مدعوم، يمكن للتطبيق:

- التعرّف على جدول البيانات والتحقق منه؛
- إنشاء نسخة احتياطية قابلة للاستعادة؛
- رفع الملف مباشرةً إلى Google Drive باستخدام واجهات Google البرمجية الرسمية؛
- تحويله إلى مستند أصلي من جداول بيانات Google؛
- فتح جدول البيانات الناتج في المتصفح الافتراضي؛
- إنشاء اختصار `.url` محلي لمستند Google؛
- تجنّب رفع الملف نفسه مرة أخرى عند فتحه لاحقًا.

لا حاجة إلى الرفع اليدوي إلى Drive، أو التنقل بين صفحات المتصفح، أو تكرار عملية التحويل.

## التنسيقات المدعومة

التنسيقات المستهدفة في التطوير الحالي:

- `.xlsx`
- `.xls`
- `.ods`
- `.csv`
- `.tsv`

قد تخضع بعض التنسيقات لقيود توافق إضافية. وتُعالَج الملفات التي تحتوي على ميزات لا يمكن الحفاظ عليها بأمان بحذر لتجنّب فقدان البيانات دون تنبيه.

## مصمم لنظام Windows

صُمم ZagoSheetsWin خصيصًا لنظام Windows، ويتكامل مع نظام التشغيل من خلال:

- **فتح باستخدام (Open with)**
- تسجيل أنواع الملفات
- فتح الملفات بالنقر المزدوج
- تكامل اختياري مع مستكشف الملفات
- مثبّت أصلي لنظام Windows

لا يغيّر التطبيق التطبيقات الافتراضية في Windows دون علمك. يظل التحكم في ارتباطات الملفات بيد المستخدم.

## الأمان جزء من التصميم

يتعامل ZagoSheetsWin مع استبدال الملفات المحلية بوصفه عملية قابلة للاستعادة.

قبل إزالة الملف الأصلي من مجلده، يتحقق التطبيق مما يلي:

1. وجود نسخة احتياطية قابلة للاستعادة؛
2. إنشاء مستند جداول بيانات Google بنجاح؛
3. حفظ ارتباط الملف المحلي بصورة دائمة؛
4. كتابة اختصار الإنترنت والتحقق منه بنجاح.

إذا فشلت العملية، يُحتفَظ بالملف الأصلي.

يلتزم التطبيق بقاعدة بسيطة:

> لا تُتلِف بيانات المستخدم مطلقًا دون علمه.

## النسخ الاحتياطية

يمكن حفظ الملفات الأصلية في مساحة خاصة للنسخ الاحتياطية المحلية قبل استبدالها باختصارات.

تتضمن إدارة النسخ الاحتياطية حدود تخزين قابلة للضبط، وسياسات للاحتفاظ بالنسخ، وخيارات للتنظيف.

تحمي النسخ الاحتياطية الملف الأصلي عند استيراده. وهي **ليست نظام مزامنة ثنائي الاتجاه**: لا تُكتب التعديلات التي تُجرى لاحقًا في جداول بيانات Google إلى ملف جدول البيانات الأصلي.

## الخصوصية والوصول إلى Google

يتواصل ZagoSheetsWin مباشرةً من حاسوبك مع واجهات Google البرمجية.

- لا يُرسَل محتوى جداول البيانات إلى خوادم Zagotools.
- تُخزَّن رموز OAuth محليًا مع حمايتها بآليات الأمان في Windows.
- يستخدم التطبيق نطاق إذن Google Drive المسمى `drive.file`، ما يقيّد وصوله إلى الملفات التي أُنشئت أو فُتحت عبر التطبيق.
- لا تتطلب عملية استيراد جداول البيانات أدوات تحليل أو تتبّع.

سياسة الخصوصية وشروط الاستخدام:

https://zagotools.top/legal.html

## حالة المشروع

يخضع ZagoSheetsWin للتطوير النشط، ويجب اعتباره حاليًا **برنامجًا في مرحلة ألفا**.

يعمل المسار الأساسي من Windows إلى جداول بيانات Google، ويجري تحسين التثبيت والاستعادة وتوافق التنسيقات ودعم اللغات وتجربة المستخدم.

قد تحدث تغييرات قبل صدور أول إصدار مستقر.

## العلاقة بمشروع Open in Google

يعتمد ZagoSheetsWin على مشروع [Open in Google](https://github.com/SwatiK425/open-in-google) الذي أنشأته [SwatiK425](https://github.com/SwatiK425)، وهو مشروع مشتق منه.

قدّم Open in Google الأساس والإلهام الأوليين لهذا المشروع.

تطوّر ZagoSheetsWin منذ ذلك الحين إلى تطبيق Windows مستقل، ببنيته ومثبّته وواجهته ونظام النسخ الاحتياطي والاستعادة وارتباطات الملفات ومعالجة التنسيقات وتجربة فتح الملفات المحلية في جداول بيانات Google.

يظل المشروع الأصلي مستقلًا. ويمكن تقديم التحسينات العامة المناسبة إلى المشروع الأصلي، بينما يواصل ZagoSheetsWin التطور بصورة مستقلة.

## المصدر المفتوح

ZagoSheetsWin برنامج مجاني ومفتوح المصدر.

يحافظ المشروع على نسب الفضل ومتطلبات الترخيص الخاصة بشفرة Open in Google الأصلية، ويوضح التطوير اللاحق الذي أنجزه ZagoSheetsWin / Zagotools.

راجع:

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## الترخيص

رخصة MIT.

للتفاصيل، راجع [LICENSE](../../../LICENSE).

---

**ZagoSheetsWin — مشروع من Zagotools**

برامج صغيرة لمشكلات حقيقية.
