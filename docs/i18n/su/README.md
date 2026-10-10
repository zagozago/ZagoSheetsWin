# ZagoSheetsWin

<details>
<summary>🌐 Basa dokuméntasi · Pilih basa</summary>

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
- **Basa Sunda** — kaca ayeuna
- [मैथिली](../mai/README.md)

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
