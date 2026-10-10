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
- **Bahasa Indonesia** — halaman ini
- [اردو](../ur/README.md)
- [Русский](../ru/README.md)
- [Deutsch](../de/README.md)

</details>

**Buka file spreadsheet lokal langsung di Google Spreadsheet dari Windows.**

ZagoSheetsWin adalah aplikasi Windows ringan yang menyederhanakan proses membuka spreadsheet lokal:

**klik dua kali file → unggah dan konversi → buka di Google Spreadsheet**

Setelah impor berhasil, ZagoSheetsWin dapat mengganti file lokal asli dengan pintasan Internet (`.url`) yang mengarah ke dokumen Google Spreadsheet, sambil menyimpan cadangan lokal file asli yang dapat dipulihkan.

Tujuannya sederhana: membuat Google Spreadsheet terasa seperti aplikasi Windows bawaan saat membuka file spreadsheet lokal.

## Apa yang dilakukan

ZagoSheetsWin mengintegrasikan file spreadsheet di Windows dengan Google Spreadsheet.

Saat Anda membuka file lokal yang didukung, aplikasi dapat:

- mendeteksi dan memvalidasi spreadsheet;
- membuat cadangan yang dapat dipulihkan;
- mengunggah file langsung ke Google Drive melalui API resmi Google;
- mengonversinya menjadi dokumen Google Spreadsheet asli;
- membuka spreadsheet hasil konversi di peramban bawaan Anda;
- membuat pintasan `.url` lokal ke dokumen Google;
- menghindari pengunggahan ulang file yang sama saat dibuka kembali.

Tanpa unggahan manual ke Drive. Tanpa mencari dokumen lewat peramban. Tanpa konversi berulang.

## Format yang didukung

Format yang ditargetkan dalam pengembangan saat ini:

- `.xlsx`
- `.xls`
- `.ods`
- `.csv`
- `.tsv`

Beberapa format mungkin memiliki batasan kompatibilitas tambahan. File dengan fitur yang tidak dapat dipertahankan secara aman ditangani secara hati-hati untuk mencegah kehilangan data tanpa pemberitahuan.

## Dirancang untuk Windows

ZagoSheetsWin dibuat khusus untuk Windows dan terintegrasi dengan sistem operasi melalui:

- **Buka dengan (Open with)**
- pendaftaran jenis file
- pembukaan file dengan klik dua kali
- integrasi opsional dengan File Explorer
- penginstal Windows asli

Aplikasi tidak mengubah aplikasi bawaan Windows secara diam-diam. Pengguna tetap mengendalikan asosiasi file.

## Aman sejak perancangan

ZagoSheetsWin memperlakukan penggantian file lokal sebagai operasi yang dapat dipulihkan.

Sebelum menghapus file asli dari foldernya, aplikasi memverifikasi bahwa:

1. cadangan yang dapat dipulihkan tersedia;
2. dokumen Google Spreadsheet telah dibuat dengan berhasil;
3. asosiasi lokal telah disimpan secara permanen;
4. pintasan Internet telah ditulis dan divalidasi.

Jika proses gagal, file asli tetap dipertahankan.

Aplikasi mengikuti satu aturan sederhana:

> Jangan pernah menghilangkan data pengguna tanpa pemberitahuan.

## Cadangan

File asli dapat disimpan di area cadangan lokal privat sebelum diganti dengan pintasan.

Pengelolaan cadangan mencakup batas penyimpanan yang dapat diatur, kebijakan retensi, dan kontrol pembersihan.

Cadangan melindungi file asli yang diimpor. Cadangan **bukan sistem sinkronisasi dua arah**: perubahan yang dibuat kemudian di Google Spreadsheet tidak akan ditulis kembali ke file spreadsheet asli.

## Privasi dan akses Google

ZagoSheetsWin berkomunikasi langsung dari komputer Anda dengan API Google.

- Isi spreadsheet Anda tidak dikirim ke server Zagotools.
- Token OAuth disimpan secara lokal dan dilindungi oleh mekanisme keamanan Windows.
- Aplikasi menggunakan cakupan izin Google Drive `drive.file`, sehingga akses dibatasi pada file yang dibuat atau dibuka melalui aplikasi.
- Proses impor spreadsheet tidak memerlukan analitik.

Kebijakan Privasi dan Ketentuan Penggunaan:

https://zagotools.top/legal.html

## Status proyek

ZagoSheetsWin sedang aktif dikembangkan dan saat ini harus dianggap sebagai **perangkat lunak alfa**.

Alur utama Windows → Google Spreadsheet telah berfungsi dan terus ditingkatkan dalam pemasangan, pemulihan, kompatibilitas format, internasionalisasi, dan pengalaman pengguna.

Perubahan dapat terjadi sebelum rilis stabil pertama.

## Hubungan dengan Open in Google

ZagoSheetsWin didasarkan pada dan dikembangkan dari [Open in Google](https://github.com/SwatiK425/open-in-google) karya [SwatiK425](https://github.com/SwatiK425).

Open in Google menjadi dasar dan inspirasi awal proyek ini.

Sejak itu, ZagoSheetsWin berkembang menjadi aplikasi Windows independen dengan arsitektur, penginstal, antarmuka, sistem pencadangan dan pemulihan, alur asosiasi file, penanganan format, dan pengalaman membuka file lokal di Google Spreadsheet miliknya sendiri.

Proyek asal tetap independen. Peningkatan umum dapat diusulkan kembali ke proyek asal apabila sesuai, sementara ZagoSheetsWin terus berkembang secara mandiri.

## Sumber terbuka

ZagoSheetsWin adalah perangkat lunak gratis dan sumber terbuka.

Proyek ini mempertahankan atribusi dan ketentuan lisensi kode asli Open in Google, sekaligus dengan jelas mengidentifikasi pengembangan ZagoSheetsWin / Zagotools selanjutnya.

Lihat:

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Lisensi

Lisensi MIT.

Lihat [LICENSE](../../../LICENSE) untuk detailnya.

---

**ZagoSheetsWin — proyek Zagotools**

Perangkat lunak kecil untuk masalah nyata.
