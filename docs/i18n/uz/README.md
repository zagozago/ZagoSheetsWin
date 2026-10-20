# ZagoSheetsWin

> ⚠️ **Alpha 0.9.43 — current safety notice in English; full editorial translation pending.**
>
> Jadval Google Sheets’da ochildi. Asl fayl yorliq bilan almashtirildi. Uni tiklash uchun Zaxira nusxalar boʻlimini oching.
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.43:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [Local core](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) and [protected distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) passed on commit `9fbec2a`; real Google acceptance is still pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 Hujjat tili · Tilni tanlang</summary>

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
- **UZ - Oʻzbekcha / Uzbek (uz)** — joriy sahifa
- [SD - سنڌي / Sindhi (sd)](../sd/README.md)
- [NE - नेपाली / Nepali (ne)](../ne/README.md)
- [IG - Igbo](../ig/README.md)
- [AZ - Azərbaycanca / Azerbaijani (az)](../az/README.md)
- [NL - Nederlands / Dutch (nl)](../nl/README.md)
- [GN - Avañe’ẽ / Guarani (gn)](../gn/README.md)

</details>

**Windows orqali mahalliy elektron jadval fayllarini bevosita Google Sheets’da oching.**

ZagoSheetsWin — Windows uchun yengil dastur bo‘lib, kompyuterdagi elektron jadvallarni ochishni soddalashtiradi:

**faylni ikki marta bosing → yuklang va aylantiring → Google Sheets’da oching**

Muvaffaqiyatli importdan so‘ng ZagoSheetsWin asl mahalliy faylni Google Sheets hujjatiga olib boradigan internet yorlig‘i (`.url`) bilan almashtirishi mumkin. Bunda asl faylning qayta tiklanadigan mahalliy zaxira nusxasi saqlanadi.

Maqsad oddiy: mahalliy elektron jadvallarni Google Sheets orqali xuddi Windows’ning odatiy dasturida ochgandek ishlatish.

## Nima qiladi

ZagoSheetsWin Windows’dagi elektron jadval fayllarini Google Sheets bilan bog‘laydi. Qo‘llab-quvvatlanadigan mahalliy fayl ochilganda dastur:

- elektron jadvalni aniqlashi va tekshirishi;
- qayta tiklanadigan zaxira nusxasini yaratishi;
- Google’ning rasmiy API’lari orqali faylni to‘g‘ridan-to‘g‘ri Google Drive’ga yuklashi;
- uni asl Google Sheets hujjatiga aylantirishi;
- natijani standart brauzerda ochishi;
- Google hujjatiga yo‘naltiruvchi mahalliy `.url` yorlig‘ini yaratishi;
- keyingi safar o‘sha faylni qayta yuklashning oldini olishi mumkin.

Drive’ga qo‘lda yuklash, brauzerda fayl izlash yoki uni qayta-qayta aylantirish shart emas.

## Qo‘llab-quvvatlanadigan formatlar

Hozirgi ishlab chiqishdagi maqsadli formatlar: `.xlsx`, `.xls`, `.ods`, `.csv` va `.tsv`.

Ayrim formatlarda qo‘shimcha moslik cheklovlari bo‘lishi mumkin. Xususiyatlari xavfsiz saqlab qolinmaydigan fayllar ma’lumot sezilmasdan yo‘qolmasligi uchun ehtiyotkorlik bilan qayta ishlanadi.

## Windows uchun yaratilgan

ZagoSheetsWin aynan Windows uchun ishlab chiqilgan. U **Open with (Birga ochish)**, fayl turlarini ro‘yxatdan o‘tkazish, ikki marta bosib ochish, ixtiyoriy File Explorer integratsiyasi va Windows uchun mahalliy o‘rnatuvchini taqdim etadi.

Dastur Windows’ning standart ilovalarini bildirmasdan o‘zgartirmaydi. Fayllar bilan bog‘lashni foydalanuvchi boshqaradi.

## Xavfsizlik boshidanoq

Mahalliy faylni almashtirish qayta tiklanadigan amal deb qaraladi. Asl faylni papkasidan olib tashlashdan oldin dastur quyidagilarni tekshiradi:

1. qayta tiklanadigan zaxira nusxasi borligini;
2. Google Sheets hujjati muvaffaqiyatli yaratilganini;
3. mahalliy fayl bog‘lanishi doimiy saqlanganini;
4. internet yorlig‘i yozilgani va tasdiqlanganini.

Amal muvaffaqiyatsiz tugasa, asl fayl saqlab qolinadi.

> Foydalanuvchi ma’lumotlarini uni ogohlantirmasdan hech qachon yo‘q qilmang.

## Zaxira nusxalar

Asl fayllar yorliqlar bilan almashtirilishidan oldin shaxsiy mahalliy zaxira maydonida saqlanishi mumkin. Saqlash hajmi chegarasi, saqlash muddati va tozalash imkoniyatlari sozlanadi.

Zaxira nusxa import paytidagi asl faylni himoya qiladi. Bu **ikki tomonlama sinxronlash emas**: keyinchalik Google Sheets’da qilingan o‘zgarishlar asl elektron jadval fayliga qayta yozilmaydi.

## Maxfiylik va Google’ga kirish

ZagoSheetsWin kompyuteringizdan bevosita Google API’lari bilan bog‘lanadi.

- Elektron jadvallar tarkibi Zagotools serverlariga yuborilmaydi.
- OAuth tokenlari mahalliy saqlanadi va Windows xavfsizlik mexanizmlari bilan himoyalanadi.
- `drive.file` ruxsat doirasi faqat dastur orqali yaratilgan yoki ochilgan fayllarga kirishni cheklaydi.
- Import jarayoni uchun analitika kuzatuvi talab qilinmaydi.

Maxfiylik siyosati va foydalanish shartlari: https://zagotools.top/legal.html

## Loyiha holati

ZagoSheetsWin faol ishlab chiqilmoqda va hozircha **alfa dastur** hisoblanadi. Windows → Google Sheets asosiy jarayoni ishlaydi. O‘rnatish, tiklash, formatlar mosligi, xalqaro tillar va foydalanuvchi tajribasi takomillashtirilmoqda. Birinchi barqaror versiyagacha o‘zgarishlar bo‘lishi mumkin.

## Open in Google bilan aloqasi

ZagoSheetsWin [SwatiK425](https://github.com/SwatiK425) yaratgan [Open in Google](https://github.com/SwatiK425/open-in-google) loyihasiga asoslanadi va undan rivojlangan. Asl loyiha dastlabki texnik poydevor va ilhomni berdi.

Hozir ZagoSheetsWin o‘z arxitekturasi, o‘rnatuvchisi, interfeysi, zaxiralash va tiklash tizimi, fayllarni bog‘lash usuli, formatlarni qayta ishlash hamda Google Sheets’da mahalliy fayllarni ochish tajribasiga ega mustaqil Windows dasturidir. Asl loyiha mustaqil qoladi; umumiy yaxshilanishlar kerak bo‘lganda upstream’ga taklif qilinishi mumkin.

## Ochiq kod

ZagoSheetsWin bepul va ochiq kodli dasturdir. U Open in Google’ning asl kodi mualliflari va litsenziya talablarini saqlab, ZagoSheetsWin / Zagotools tomonidan keyinroq kiritilgan ishlanmalarni alohida ko‘rsatadi.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Litsenziya

MIT litsenziyasi. Tafsilotlar uchun [LICENSE](../../../LICENSE) fayliga qarang.

---

**ZagoSheetsWin — Zagotools loyihasi**

Haqiqiy muammolar uchun kichik dasturlar.
