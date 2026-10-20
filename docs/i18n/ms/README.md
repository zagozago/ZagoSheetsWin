# ZagoSheetsWin

> ⚠️ **Alpha 0.9.43 — current safety notice in English; full editorial translation pending.**
>
> Hamparan dibuka dalam Google Sheets. Fail asal digantikan dengan pintasan. Untuk memulihkannya, buka Sandaran.
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.43:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [Local core](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) and [protected distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) passed on commit `9fbec2a`; real Google acceptance is still pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

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
- **MS - Bahasa Melayu / Malay (ms)** — halaman semasa
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
