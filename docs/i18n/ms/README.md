# ZagoSheetsWin

<details>
<summary>🌐 Bahasa dokumentasi · Pilih bahasa</summary>

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
- **Bahasa Melayu** — halaman semasa
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

**Buka fail hamparan tempatan terus dalam Google Sheets melalui Windows.**

ZagoSheetsWin ialah aplikasi Windows yang ringan dan memudahkan proses membuka hamparan tempatan:

**dwiklik fail → muat naik dan tukar format → buka dalam Google Sheets**

Selepas import berjaya, ZagoSheetsWin boleh menggantikan fail tempatan asal dengan pintasan Internet (`.url`) yang menuju ke dokumen Google Sheets, sambil menyimpan sandaran tempatan fail asal yang boleh dipulihkan.

Matlamatnya mudah: menjadikan Google Sheets seolah-olah aplikasi Windows asli apabila membuka fail hamparan tempatan.

## Apa yang dilakukannya

ZagoSheetsWin menghubungkan fail hamparan Windows dengan Google Sheets. Apabila fail yang disokong dibuka, aplikasi boleh:

- mengesan dan mengesahkan hamparan;
- mencipta sandaran yang boleh dipulihkan;
- memuat naik fail terus ke Google Drive melalui API rasmi Google;
- menukarnya menjadi dokumen Google Sheets asli;
- membuka hasilnya dalam pelayar lalai;
- mencipta pintasan `.url` tempatan ke dokumen Google;
- mengelakkan muat naik semula fail yang sama apabila dibuka lagi.

Tidak perlu muat naik manual ke Drive, mencari fail dalam pelayar atau mengulangi penukaran.

## Format yang disokong

Format sasaran pembangunan semasa: `.xlsx`, `.xls`, `.ods`, `.csv` dan `.tsv`.

Sesetengah format mempunyai had keserasian tambahan. Fail yang mempunyai ciri yang tidak boleh dikekalkan dengan selamat akan dikendalikan secara berhati-hati bagi mengelakkan kehilangan data tanpa disedari.

## Direka untuk Windows

ZagoSheetsWin dibangunkan khusus untuk Windows dan berintegrasi melalui **Open with (Buka dengan)**, pendaftaran jenis fail, pembukaan dengan dwiklik, integrasi File Explorer pilihan dan pemasang Windows asli.

Aplikasi tidak menukar aplikasi lalai Windows secara senyap. Pengguna kekal mengawal perkaitan fail.

## Selamat sejak reka bentuk

Penggantian fail tempatan dianggap sebagai operasi yang boleh dipulihkan. Sebelum mengeluarkan fail asal daripada folder, aplikasi mengesahkan bahawa:

1. sandaran yang boleh dipulihkan wujud;
2. dokumen Google Sheets berjaya diwujudkan;
3. perkaitan fail tempatan telah disimpan secara kekal;
4. pintasan Internet telah ditulis dan disahkan.

Jika proses gagal, fail asal dikekalkan.

> Jangan sekali-kali memusnahkan data pengguna tanpa memaklumkannya.

## Sandaran

Fail asal boleh disimpan dalam ruang sandaran tempatan peribadi sebelum digantikan dengan pintasan. Had storan, tempoh simpanan dan kawalan pembersihan boleh ditetapkan.

Sandaran melindungi fail asal yang diimport. Ia **bukan penyegerakan dua hala**: perubahan kemudian dalam Google Sheets tidak ditulis semula ke fail hamparan asal.

## Privasi dan akses Google

ZagoSheetsWin berhubung terus dari komputer anda dengan API Google.

- Kandungan hamparan tidak dihantar ke pelayan Zagotools.
- Token OAuth disimpan secara tempatan dan dilindungi menggunakan mekanisme keselamatan Windows.
- Skop kebenaran `drive.file` mengehadkan akses kepada fail yang dicipta atau dibuka melalui aplikasi.
- Proses import tidak memerlukan penjejakan analitik.

Dasar Privasi dan Terma Penggunaan: https://zagotools.top/legal.html

## Status projek

ZagoSheetsWin sedang dibangunkan secara aktif dan kini dianggap **perisian alfa**. Aliran utama Windows → Google Sheets sudah berfungsi. Pemasangan, pemulihan, keserasian format, pengantarabangsaan dan pengalaman pengguna terus ditambah baik. Perubahan mungkin berlaku sebelum keluaran stabil pertama.

## Hubungan dengan Open in Google

ZagoSheetsWin dibangunkan berasaskan [Open in Google](https://github.com/SwatiK425/open-in-google) oleh [SwatiK425](https://github.com/SwatiK425), yang menyediakan asas dan inspirasi asal.

Sejak itu, ZagoSheetsWin menjadi aplikasi Windows bebas dengan seni bina, pemasang, antara muka, sistem sandaran dan pemulihan, perkaitan fail, pengendalian format dan pengalaman membuka fail tempatan dalam Google Sheets sendiri. Projek asal kekal bebas, dan penambahbaikan umum boleh dicadangkan kepada projek asal apabila sesuai.

## Sumber terbuka

ZagoSheetsWin ialah perisian percuma dan sumber terbuka. Projek mengekalkan atribusi serta syarat lesen kod Open in Google asal, sambil mengenal pasti pembangunan kemudian oleh ZagoSheetsWin / Zagotools.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Lesen

Lesen MIT. Rujuk [LICENSE](../../../LICENSE) untuk maklumat lanjut.

---

**ZagoSheetsWin — projek Zagotools**

Perisian kecil untuk masalah sebenar.
