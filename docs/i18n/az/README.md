# ZagoSheetsWin

<details>
<summary>🌐 Sənədləşmə dili · Dili seçin</summary>

- [English](../../../README.md)
- [简体中文](../zh/README.md)
- [हिन्दी](../hi/README.md)
- [Español](../es/README.md)
- [العربية](../ar/README.md)
- [Français](../fr/README.md)
- [বাংলা](../bn/README.md)
- [Português (Brasil)](../pt/README.md)
- [Bahasa Indonesia](../id/README.md)
- [اردو](../ur/README.md)
- [Русский](../ru/README.md)
- [Deutsch](../de/README.md)
- [日本語](../ja/README.md)
- [Tiếng Việt](../vi/README.md)
- [Türkçe](../tr/README.md)
- [한국어](../ko/README.md)
- [Italiano](../it/README.md)
- [ไทย](../th/README.md)
- [Filipino](../fil/README.md)
- [Bahasa Melayu](../ms/README.md)
- [Kiswahili](../sw/README.md)
- [Nigerian Pidgin](../pcm/README.md)
- [मराठी](../mr/README.md)
- [తెలుగు](../te/README.md)
- [Hausa](../ha/README.md)
- [ਪੰਜਾਬੀ](../pa/README.md)
- [தமிழ்](../ta/README.md)
- [粵語](../yue/README.md)
- [فارسی](../fa/README.md)
- [አማርኛ](../am/README.md)
- [Basa Jawa](../jv/README.md)
- [ગુજરાતી](../gu/README.md)
- [ಕನ್ನಡ](../kn/README.md)
- [Yorùbá](../yo/README.md)
- [भोजपुरी](../bho/README.md)
- [پښتو](../ps/README.md)
- [ଓଡ଼ିଆ](../or/README.md)
- [မြန်မာ](../my/README.md)
- [മലയാളം](../ml/README.md)
- [Polski](../pl/README.md)
- [Basa Sunda](../su/README.md)
- [मैथिली](../mai/README.md)
- [Українська](../uk/README.md)
- [Afaan Oromoo](../om/README.md)
- [Oʻzbekcha](../uz/README.md)
- [سنڌي](../sd/README.md)
- [नेपाली](../ne/README.md)
- [Asụsụ Igbo](../ig/README.md)
- **Azərbaycan dili** — cari səhifə
- [Nederlands](../nl/README.md)
- [Avañe’ẽ](../gn/README.md)

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
