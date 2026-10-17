# ZagoSheetsWin

> ⚠️ **0.9.35 — Important safety update (English notice; full translation review pending).**
>
> Cədvəl Google Sheets-də açıldı. Orijinal fayl qısayolla əvəz edildi. Onu bərpa etmək üçün Ehtiyat nüsxələr bölməsini açın.
>
> **Conversion fidelity:** After a confirmed Google import, the original local file **may be replaced with a `.url` shortcut even when formulas, values or formatting differ**. The original bytes remain in Backups. Choose **Import as copy BEFORE opening** if you need the original to stay in the source folder. Windows **read-only** files may block replacement; in that case, the original is preserved. Some text below describes an older alpha policy; use the [current English README](../../../README.md) or [Portuguese user guide](../../USER_GUIDE.md) for complete 0.9.35 behavior.


<details>
<summary>🌐 Sənədləşmə dili · Dili seçin</summary>

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
- [IG - Igbo](../ig/README.md)
- **AZ - Azərbaycanca / Azerbaijani (az)** — cari səhifə
- [NL - Nederlands / Dutch (nl)](../nl/README.md)
- [GN - Avañe’ẽ / Guarani (gn)](../gn/README.md)

</details>

**Windows-dan yerli elektron cədvəl fayllarını birbaşa Google Sheets-də açın.**

ZagoSheetsWin yerli elektron cədvəllərin açılmasını asanlaşdıran yüngül Windows tətbiqidir:

**fayla iki dəfə klikləyin → yükləyin və çevirin → Google Sheets-də açın**

Uğurlu idxaldan sonra ZagoSheetsWin ilkin yerli faylı Google Sheets sənədinə aparan internet qısayolu (`.url`) ilə əvəz edə bilər. Eyni zamanda ilkin faylın bərpa edilə bilən yerli ehtiyat nüsxəsini saxlayır.

Məqsəd sadədir: yerli elektron cədvəlləri Google Sheets-də açmağı adi Windows tətbiqindən istifadə etmək qədər rahat etmək.

## Nə edir

ZagoSheetsWin Windows-un elektron cədvəl fayllarını Google Sheets ilə birləşdirir. Dəstəklənən yerli faylı açdıqda tətbiq:

- elektron cədvəli müəyyən edib yoxlaya;
- bərpa edilə bilən ehtiyat nüsxəsi yarada;
- Google-un rəsmi API-lərindən istifadə edərək birbaşa Google Drive-a yükləyə;
- onu doğma Google Sheets sənədinə çevirə;
- nəticəni standart brauzerdə aça;
- Google sənədinə yerli `.url` qısayolu yarada;
- həmin faylın növbəti açılışlarda yenidən yüklənməsinin qarşısını ala bilər.

Drive-a əl ilə yükləməyə, brauzerdə fayl axtarmağa və ya təkrar konvertasiya etməyə ehtiyac yoxdur.

## Dəstəklənən formatlar

Hazırkı inkişaf mərhələsində nəzərdə tutulan formatlar: `.xlsx`, `.xls`, `.ods`, `.csv` və `.tsv`.

Bəzi formatlarda əlavə uyğunluq məhdudiyyətləri ola bilər. Təhlükəsiz şəkildə saxlanıla bilməyən funksiyaları olan fayllar xəbərdarlıq edilmədən məlumat itkisinə yol verməmək üçün ehtiyatla emal olunur.

## Windows üçün hazırlanıb

ZagoSheetsWin xüsusi olaraq Windows üçün yaradılıb və **Open with (Bununla aç)**, fayl növlərinin qeydiyyatı, ikiqat kliklə açma, əlavə File Explorer inteqrasiyası və doğma Windows quraşdırıcısı ilə işləyir.

Tətbiq Windows-un standart proqramlarını istifadəçinin xəbəri olmadan dəyişmir. Fayl assosiasiyalarına nəzarət istifadəçidə qalır.

## Təhlükəsizlik əsas prinsipdir

Yerli faylın əvəzlənməsi bərpa edilə bilən əməliyyat kimi nəzərdə tutulub. İlkin faylı qovluqdan çıxarmazdan əvvəl tətbiq yoxlayır ki:

1. bərpa edilə bilən ehtiyat nüsxəsi mövcuddur;
2. Google Sheets sənədi uğurla yaradılıb;
3. yerli fayl assosiasiyası davamlı şəkildə saxlanılıb;
4. internet qısayolu yazılıb və yoxlanılıb.

Proses uğursuz olarsa, ilkin fayl saxlanılır.

> İstifadəçinin məlumatlarını onun xəbəri olmadan heç vaxt məhv etməyin.

## Ehtiyat nüsxələri

İlkin fayllar qısayolla əvəzlənməzdən əvvəl şəxsi yerli ehtiyat nüsxəsi sahəsində saxlanıla bilər. Yaddaş hədləri, saxlama müddətləri və təmizləmə qaydaları tənzimlənir.

Ehtiyat nüsxəsi idxal edilən ilkin faylı qoruyur. Bu, **ikitərəfli sinxronizasiya deyil**: Google Sheets-də sonradan edilən dəyişikliklər ilkin elektron cədvəl faylına geri yazılmır.

## Məxfilik və Google-a giriş

ZagoSheetsWin kompüterinizdən birbaşa Google API-ləri ilə əlaqə saxlayır.

- Elektron cədvəllərin məzmunu Zagotools serverlərinə göndərilmir.
- OAuth tokenləri yerli saxlanılır və Windows təhlükəsizlik mexanizmləri ilə qorunur.
- `drive.file` icazə sahəsi girişi tətbiq vasitəsilə yaradılan və ya açılan fayllarla məhdudlaşdırır.
- Elektron cədvəllərin idxalı üçün analitik izləmə tələb olunmur.

Məxfilik siyasəti və istifadə şərtləri: https://zagotools.top/legal.html

## Layihənin vəziyyəti

ZagoSheetsWin fəal inkişaf mərhələsindədir və hazırda **alfa proqram təminatı** sayılmalıdır. Windows → Google Sheets əsas iş prosesi fəaliyyət göstərir. Quraşdırma, bərpa, format uyğunluğu, beynəlmiləlləşdirmə və istifadəçi təcrübəsi təkmilləşdirilir. İlk stabil versiyadan əvvəl dəyişikliklər mümkündür.

## Open in Google ilə əlaqə

ZagoSheetsWin [SwatiK425](https://github.com/SwatiK425) tərəfindən yaradılan [Open in Google](https://github.com/SwatiK425/open-in-google) layihəsinə əsaslanır və ondan törəyib. Bu layihə ilkin texniki təməli və ilhamı təmin edib.

Bu gün ZagoSheetsWin öz arxitekturası, quraşdırıcısı, interfeysi, ehtiyat nüsxələmə və bərpa sistemi, fayl assosiasiyaları, formatların emalı və yerli faylları Google Sheets-də açma təcrübəsi olan müstəqil Windows tətbiqidir. Orijinal layihə müstəqildir; uyğun ümumi təkmilləşdirmələr upstream-ə təqdim edilə bilər.

## Açıq mənbə

ZagoSheetsWin pulsuz və açıq mənbəli proqram təminatıdır. İlkin Open in Google kodunun müəlliflik bildirişləri və lisenziya şərtləri qorunur; ZagoSheetsWin / Zagotools tərəfindən sonrakı inkişaf açıq şəkildə fərqləndirilir.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Lisenziya

MIT lisenziyası. Ətraflı məlumat üçün [LICENSE](../../../LICENSE) faylına baxın.

---

**ZagoSheetsWin — Zagotools layihəsi**

Real problemlər üçün kiçik proqramlar.
