# ZagoSheetsWin

<details>
<summary>🌐 Belge dili · Dil seçin</summary>

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
- **Türkçe** — geçerli sayfa
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
- [Azərbaycan dili](../az/README.md)
- [Nederlands](../nl/README.md)
- [Avañe’ẽ](../gn/README.md)

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
