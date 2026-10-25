# ZagoSheetsWin

<details>
<summary>🌐 Belge dili · Dil seçin</summary>

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
- **TR - Türkçe / Turkish (tr)** — geçerli sayfa
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

**Windows'taki yerel elektronik tablo dosyalarını doğrudan Google E-Tablolar'da açın.**

ZagoSheetsWin, yerel elektronik tabloları açmayı kolaylaştıran hafif bir Windows uygulamasıdır:

**dosyaya çift tıkla → yükle ve dönüştür → Google E-Tablolar'da aç**

Başarılı içe aktarmadan sonra uygulama, özgün yerel dosyanın yerine Google E-Tablolar belgesini açan bir İnternet kısayolu (`.url`) koyabilir ve geri yüklenebilir yerel yedeğini saklar.

Amaç, Google E-Tablolar'ın yerel elektronik tablo dosyalarını açarken yerel bir Windows uygulaması gibi çalışmasını sağlamaktır.

## Ne yapar?

ZagoSheetsWin, Windows'taki elektronik tablo dosyalarını Google E-Tablolar ile bütünleştirir. Desteklenen bir dosya açıldığında şunları yapabilir:

- elektronik tabloyu algılamak ve doğrulamak;
- geri yüklenebilir yedek oluşturmak;
- Google'ın resmî API'leriyle doğrudan Google Drive'a yüklemek;
- yerel Google E-Tablolar belgesine dönüştürmek;
- sonucu varsayılan tarayıcıda açmak;
- Google belgesine yerel `.url` kısayolu oluşturmak;
- sonraki açılışlarda aynı dosyayı yeniden yüklememek.

Drive'a elle yükleme, tarayıcıda dosya arama veya tekrar tekrar dönüştürme yok.

## Desteklenen biçimler

Geliştirmede hedeflenen biçimler: `.xlsx`, `.xls`, `.ods`, `.csv`, `.tsv`.

Bazı biçimlerde ek uyumluluk sınırlamaları vardır. Güvenli biçimde korunamayan özellikler içeren dosyalar, sessiz veri kaybını önlemek için dikkatli işlenir.

## Windows için tasarlandı

ZagoSheetsWin, Windows'a özel geliştirilmiştir. **Birlikte aç (Open with)**, dosya türü kaydı, çift tıklamayla açma, isteğe bağlı Dosya Gezgini bütünleşmesi ve yerel Windows yükleyicisi sağlar.

Varsayılan uygulamaları kullanıcıya haber vermeden değiştirmez. Dosya ilişkilendirmeleri kullanıcının kontrolündedir.

## Tasarımdan gelen güvenlik

Yerel dosyanın değiştirilmesi geri alınabilir bir işlemdir. Özgün dosya klasöründen kaldırılmadan önce uygulama şunları doğrular:

1. geri yüklenebilir bir yedeğin bulunduğunu;
2. Google E-Tablolar belgesinin başarıyla oluşturulduğunu;
3. yerel dosya ilişkisinin kalıcı kaydedildiğini;
4. İnternet kısayolunun yazılıp doğrulandığını.

İşlem başarısız olursa özgün dosya korunur.

> Kullanıcı verilerini asla sessizce yok etme.

## Yedekler

Özgün dosyalar, kısayolla değiştirilmeden önce özel yerel yedek alanında saklanabilir. Depolama sınırı, saklama süresi ve temizleme ayarları yapılandırılabilir.

Yedekler ilk içe aktarılan dosyayı korur. **Çift yönlü eşitleme değildir**: Google E-Tablolar'da sonradan yapılan değişiklikler özgün elektronik tabloya geri yazılmaz.

## Gizlilik ve Google erişimi

ZagoSheetsWin, bilgisayarınızdan doğrudan Google API'leriyle iletişim kurar.

- Elektronik tablo içerikleri Zagotools sunucularına gönderilmez.
- OAuth belirteçleri yerel saklanır ve Windows güvenlik mekanizmalarıyla korunur.
- `drive.file` yetki kapsamı, erişimi uygulama aracılığıyla oluşturulan veya açılan dosyalarla sınırlar.
- İçe aktarma süreci analiz takibi gerektirmez.

Gizlilik Politikası ve Kullanım Koşulları: https://zagotools.top/legal.html

## Proje durumu

ZagoSheetsWin etkin biçimde geliştirilen **alfa yazılımdır**. Windows → Google E-Tablolar temel akışı çalışır. Kurulum, kurtarma, biçim uyumluluğu, uluslararasılaştırma ve kullanıcı deneyimi iyileştirilmektedir. İlk kararlı sürümden önce değişiklikler olabilir.

## Open in Google ile ilişkisi

ZagoSheetsWin, [SwatiK425](https://github.com/SwatiK425) tarafından geliştirilen [Open in Google](https://github.com/SwatiK425/open-in-google) projesinden türetilmiştir. Bu proje ilk temeli ve ilhamı sağlamıştır.

ZagoSheetsWin artık kendi mimarisi, yükleyicisi, arayüzü, yedekleme ve kurtarma sistemi, dosya ilişkilendirme akışı, biçim işleme özellikleri ve yerel dosyadan Google E-Tablolar'a geçiş deneyimi olan bağımsız Windows uygulamasıdır. Kaynak proje bağımsızdır; uygun genel iyileştirmeler upstream'e sunulabilir.

## Açık kaynak

ZagoSheetsWin ücretsiz ve açık kaynaklı yazılımdır. Özgün Open in Google kodunun atıf ve lisans koşulları korunur; ZagoSheetsWin / Zagotools geliştirmeleri ayrıca belirtilir.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Lisans

MIT Lisansı. Ayrıntılar için [LICENSE](../../../LICENSE).

---

**ZagoSheetsWin — bir Zagotools projesi**

Gerçek sorunlar için küçük yazılımlar.
