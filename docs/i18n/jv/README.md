# ZagoSheetsWin

> ⚠️ **Alpha 0.9.42 — English safety notice; full editorial translation pending.**
>
> Spreadsheet wis dibukak ing Google Sheets. File asli diganti nganggo trabasan. Kanggo mulihake, bukak Cadangan.
>
> **CSV/TSV v4:** up to **2,000,000 cells**, **100,000 logical rows**, 1,000 columns and 20 MiB input/payload, subject to other limits. Real delimiter ambiguity shows a local preview. If converted Google cell values differ, **the original must NOT be replaced by a shortcut**.
>
> **XLS/ODS:** complex features may still be converted with partial fidelity and the source **may** be replaced after verified import; the original bytes remain recoverable in Backups. Use **Import as copy** if you must keep the source file in place.
>
> **Backups:** automatic cleanup by age and quota, **no toggle**. Only eligible completed backups are cleaned; quota pressure may delete them before retention expires. Files from different sources can import in parallel; only actual backup-capacity shortages can cause a cancellable wait.
>
> **Release state:** 0.9.42 is **not a stable release**. The latest Windows CI and preceding protected distribution had failing checks; real-Google acceptance remains pending. Some text below describes older alpha behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 Basa dokumentasi · Pilih basa</summary>

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
- **JV - Basa Jawa / Javanese (jv)** — kaca saiki
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
