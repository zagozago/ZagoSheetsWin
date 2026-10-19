# ZagoSheetsWin

> ⚠️ **Alpha 0.9.42 — English safety notice; full editorial translation pending.**
>
> Spreadsheet dibuka di Google Sheets. File asli diganti dengan pintasan. Untuk memulihkannya, buka Cadangan.
>
> **CSV/TSV v4:** up to **2,000,000 cells**, **100,000 logical rows**, 1,000 columns and 20 MiB input/payload, subject to other limits. Real delimiter ambiguity shows a local preview. If converted Google cell values differ, **the original must NOT be replaced by a shortcut**.
>
> **XLS/ODS:** complex features may still be converted with partial fidelity and the source **may** be replaced after verified import; the original bytes remain recoverable in Backups. Use **Import as copy** if you must keep the source file in place.
>
> **Backups:** automatic cleanup by age and quota, **no toggle**. Only eligible completed backups are cleaned; quota pressure may delete them before retention expires. Files from different sources can import in parallel; only actual backup-capacity shortages can cause a cancellable wait.
>
> **Release state:** 0.9.42 is **not a stable release**. The latest Windows CI and preceding protected distribution had failing checks; real-Google acceptance remains pending. Some text below describes older alpha behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 Bahasa dokumentasi · Pilih bahasa</summary>

- [EN - English](../../../README.md)
- [ZH - 中文 / Chinese (Simplified) (zh)](../zh/README.md)
- [HI - हिन्दी / Hindi (hi)](../hi/README.md)
- [ES - Español / Spanish (es)](../es/README.md)
- [AR - العربية / Arabic (ar)](../ar/README.md)
- [FR - Français / French (fr)](../fr/README.md)
- [BN - বাংলা / Bengali (bn)](../bn/README.md)
- [PT - Português / Portuguese (pt)](../pt/README.md)
- **ID - Bahasa Indonesia / Indonesian (id)** — halaman ini
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
