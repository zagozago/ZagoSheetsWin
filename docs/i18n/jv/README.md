# ZagoSheetsWin

<details>
<summary>🌐 Basa dokumentasi · Pilih basa</summary>

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
- **Basa Jawa** — kaca saiki
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

</details>

**Bukak berkas spreadsheet lokal langsung nganggo Google Sheets saka Windows.**

ZagoSheetsWin iku aplikasi Windows sing entheng, supaya mbukak spreadsheet ing komputer dadi luwih gampang:

**klik kaping pindho berkas → unggah lan owahi format → bukak ing Google Sheets**

Sawisé impor kasil, ZagoSheetsWin bisa ngganti berkas asli ing komputer nganggo trabasan Internet (`.url`) sing ngarah menyang dokumen Google Sheets. Salinan cadangan lokal saka berkas asli sing bisa dipulihaké tetep disimpen.

Tujuané prasaja: supaya Google Sheets bisa dienggo kaya aplikasi Windows lumrah kanggo mbukak spreadsheet lokal.

## Apa sing ditindakake

ZagoSheetsWin nyambungaké berkas spreadsheet ing Windows karo Google Sheets. Nalika mbukak berkas lokal sing didhukung, aplikasi iki bisa:

- ngenali lan mriksa spreadsheet;
- nggawé salinan cadangan sing bisa dipulihaké;
- ngunggah langsung menyang Google Drive nganggo API resmi Google;
- ngowahi dadi dokumen Google Sheets asli;
- mbukak asilé ing browser baku;
- nggawé trabasan `.url` lokal menyang dokumen Google;
- ngindhari unggahan bola-bali kanggo berkas sing padha.

Ora perlu unggah manual menyang Drive, nggoleki dokumen liwat browser, utawa mbaleni konversi.

## Format sing didhukung

Format sing dadi sasaran pangembangan saiki yaiku `.xlsx`, `.xls`, `.ods`, `.csv` lan `.tsv`.

Sawetara format bisa nduwé watesan kompatibilitas tambahan. Berkas sing nduwé fitur sing ora bisa dijaga kanthi aman bakal ditangani kanthi ati-ati supaya data ora ilang tanpa kabar.

## Dirancang kanggo Windows

ZagoSheetsWin dirancang mligi kanggo Windows. Aplikasi iki nyedhiyakake **Open with (Bukak nganggo)**, registrasi jinis berkas, bukak nganggo klik kaping pindho, integrasi File Explorer opsional lan pemasang Windows asli.

Aplikasi ora bakal ngowahi aplikasi baku Windows tanpa kawruh pangguna. Hubungan berkas karo aplikasi tetep dikendhalèkaké pangguna.

## Aman wiwit dirancang

Ngganti berkas lokal dianggep operasi sing kudu bisa dipulihaké. Sadurungé berkas asli dibusak saka folderé, aplikasi mriksa manawa:

1. salinan cadangan sing bisa dipulihaké wis ana;
2. dokumen Google Sheets wis kasil digawé;
3. hubungan berkas lokal wis kasimpen kanthi ajeg;
4. trabasan Internet wis ditulis lan divalidasi.

Yèn proses gagal, berkas asli tetep disimpen.

> Aja nganti ngrusak data pangguna meneng-meneng.

## Salinan cadangan

Berkas asli bisa disimpen ing panggonan cadangan lokal pribadi sadurungé diganti trabasan. Wates panyimpenan, wektu nyimpen lan pilihan ngresiki bisa diatur.

Cadangan nglindhungi berkas asli nalika diimpor. Iki **dudu sinkronisasi loro arah**: owah-owahan sing mengko digawé ing Google Sheets ora ditulis bali menyang berkas spreadsheet asli.

## Privasi lan akses Google

ZagoSheetsWin nyambung langsung saka komputer panjenengan menyang API Google.

- Isi spreadsheet ora dikirim menyang server Zagotools.
- Token OAuth disimpen sacara lokal lan dilindhungi mekanisme keamanan Windows.
- Cakupan idin `drive.file` matesi akses mung kanggo berkas sing digawé utawa dibukak liwat aplikasi.
- Proses impor ora mbutuhaké pelacakan analitik.

Kabijakan Privasi lan Katemtuan Panggunaan: https://zagotools.top/legal.html

## Status proyek

ZagoSheetsWin isih dikembangaké lan saiki kalebu **piranti lunak tahap alfa**. Alur utama Windows → Google Sheets wis mlaku. Instalasi, pemulihan, kompatibilitas format, panyengkuyung manéka basa lan pengalaman pangguna terus dibecikaké. Isih bisa ana owah-owahan sadurungé rilis stabil kapisan.

## Hubungan karo Open in Google

ZagoSheetsWin asalé saka [Open in Google](https://github.com/SwatiK425/open-in-google) gawéan [SwatiK425](https://github.com/SwatiK425), sing nyedhiyakake dhasar lan inspirasi awal.

Saiki ZagoSheetsWin dadi aplikasi Windows mandiri kanthi arsitektur, pemasang, antarmuka, sistem cadangan lan pemulihan, hubungan jinis berkas, pangolahan format lan cara mbukak berkas lokal ing Google Sheets dhewe. Proyek asli tetep mandiri, lan perbaikan umum sing trep bisa diwènèhaké marang upstream.

## Sumber kabuka

ZagoSheetsWin iku piranti lunak gratis lan sumber kabuka. Kredit lan katemtuan lisensi kode Open in Google asli tetep dijaga, déné pangembangan ZagoSheetsWin / Zagotools sabanjuré diandharaké kanthi cetha.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Lisensi

Lisensi MIT. Delengen [LICENSE](../../../LICENSE) kanggo katrangan.

---

**ZagoSheetsWin — proyek Zagotools**

Piranti lunak cilik kanggo masalah nyata.
