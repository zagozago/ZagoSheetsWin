# ZagoSheetsWin

> ⚠️ **0.9.35 — Important safety update (English notice; full translation review pending).**
>
> Spreadsheet parantos dibuka dina Google Sheets. Berkas asli diganti ku potong kompas. Pikeun mulangkeunana, buka Cadangan.
>
> **Conversion fidelity:** After a confirmed Google import, the original local file **may be replaced with a `.url` shortcut even when formulas, values or formatting differ**. The original bytes remain in Backups. Choose **Import as copy BEFORE opening** if you need the original to stay in the source folder. Windows **read-only** files may block replacement; in that case, the original is preserved. Some text below describes an older alpha policy; use the [current English README](../../../README.md) or [Portuguese user guide](../../USER_GUIDE.md) for complete 0.9.35 behavior.


<details>
<summary>🌐 Basa dokuméntasi · Pilih basa</summary>

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
- **SU - Basa Sunda / Sundanese (su)** — kaca ayeuna
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

**Buka berkas spreadsheet lokal langsung dina Google Sheets tina Windows.**

ZagoSheetsWin téh aplikasi Windows nu hampang pikeun ngagampangkeun muka spreadsheet nu aya dina komputer:

**klik dua kali berkas → unggah jeung konvérsi → buka dina Google Sheets**

Sanggeus impor hasil, ZagoSheetsWin bisa ngaganti berkas lokal aslina ku potong kompas Internét (`.url`) nu ngarah ka dokumén Google Sheets, bari nyimpen salinan cadangan lokal tina berkas asli nu bisa dipulangkeun.

Tujuanana basajan: sangkan Google Sheets karasa siga aplikasi Windows biasa waktu muka berkas spreadsheet lokal.

## Naon nu dilakukeun

ZagoSheetsWin nyambungkeun berkas spreadsheet dina Windows jeung Google Sheets. Lamun muka berkas lokal nu dirojong, aplikasi bisa:

- mikawanoh jeung mariksa spreadsheet;
- nyieun cadangan nu bisa dipulangkeun;
- unggah berkas langsung ka Google Drive maké API resmi Google;
- ngarobah jadi dokumén Google Sheets asli;
- muka hasilna dina panyungsi standar;
- nyieun potong kompas lokal `.url` ka dokumén Google;
- nyingkahan unggahan deui waktu berkas nu sarua dibuka engké.

Teu kudu unggah sorangan ka Drive, neangan berkas dina panyungsi, atawa ngulang konvérsi.

## Format nu dirojong

Format nu jadi udagan pangwangunan ayeuna: `.xlsx`, `.xls`, `.ods`, `.csv`, `.tsv`.

Sababaraha format bisa boga wates kasaluyuan tambahan. Berkas nu boga fitur nu teu bisa dijaga kalayan aman bakal diurus sacara taliti sangkan data teu leungit tanpa béja.

## Dirancang pikeun Windows

ZagoSheetsWin dijieun husus pikeun Windows, kalayan **Open with (Buka maké)**, pendaptaran jinis berkas, muka ku klik dua kali, sambungan pilihan jeung File Explorer, sarta pamasang Windows asli.

Aplikasi henteu ngaganti aplikasi standar Windows bari teu ngabéjaan. Hubungan berkas jeung aplikasi tetep dikawasa ku pamaké.

## Kaamanan ti mimiti

Ngaganti berkas lokal dianggap pagawéan nu kudu bisa dipulangkeun. Saméméh ngaleungitkeun berkas asli tina polderna, aplikasi mariksa yén:

1. cadangan nu bisa dipulangkeun geus aya;
2. dokumén Google Sheets geus hasil dijieun;
3. hubungan berkas lokal geus disimpen sacara permanén;
4. potong kompas Internét geus ditulis jeung dipariksa.

Lamun prosés gagal, berkas asli tetep disimpen.

> Ulah kungsi ngancurkeun data pamaké tanpa ngabéjaan.

## Cadangan

Berkas asli bisa disimpen dina tempat cadangan lokal pribadi saméméh diganti ku potong kompas. Wates panyimpenan, lilana nyimpen jeung pilihan beberesih bisa diatur.

Cadangan ngajaga berkas asli dina waktu impor. Ieu **lain sinkronisasi dua arah**: parobahan nu engké dilakukeun dina Google Sheets moal ditulis balik kana berkas spreadsheet aslina.

## Privasi jeung aksés Google

ZagoSheetsWin komunikasi langsung tina komputer anjeun ka API Google.

- Eusi spreadsheet teu dikirim ka server Zagotools.
- Token OAuth disimpen sacara lokal jeung ditangtayungan ku mékanisme kaamanan Windows.
- Wengkuan idin `drive.file` ngawatesan aksés ka berkas nu dijieun atawa dibuka ngaliwatan aplikasi.
- Prosés impor henteu merlukeun palacakan analitik.

Kabijakan Privasi jeung Sarat Pamakéan: https://zagotools.top/legal.html

## Kaayaan proyék

ZagoSheetsWin keur terus dimekarkeun sarta ayeuna masih **parangkat lunak tahap alfa**. Prosés utama Windows → Google Sheets geus jalan. Pamasangan, pamulihan, kasaluyuan format, dukungan rupa-rupa basa, jeung pangalaman pamaké terus dironjatkeun. Bisa aya parobahan saméméh vérsi stabil munggaran.

## Hubungan jeung Open in Google

ZagoSheetsWin dimekarkeun tina [Open in Google](https://github.com/SwatiK425/open-in-google) ku [SwatiK425](https://github.com/SwatiK425), nu méré dadasar jeung inspirasi munggaran.

Ayeuna ZagoSheetsWin jadi aplikasi Windows mandiri nu boga arsitéktur, pamasang, antarbeungeut, cadangan jeung pamulihan, hubungan berkas, pangolahan format, sarta cara muka berkas lokal dina Google Sheets sorangan. Proyék asal tetep mandiri; parobahan umum nu cocog bisa diajukeun deui ka upstream.

## Sumber kabuka

ZagoSheetsWin téh parangkat lunak gratis jeung sumber kabuka. Kredit jeung syarat lisénsi kode Open in Google asli dijaga, bari pangwangunan salajengna ku ZagoSheetsWin / Zagotools dijelaskeun misah.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Lisénsi

Lisénsi MIT. Baca [LICENSE](../../../LICENSE) kanggo katerangan.

---

**ZagoSheetsWin — proyék Zagotools**

Parangkat lunak leutik pikeun masalah nyata.
