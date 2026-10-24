# ZagoSheetsWin

> ⚠️ **Alpha 0.9.48 — current safety notice in English; full editorial translation pending.**
>
> Pe kuatia renda ojepe'a Google Sheets-pe. Pe archivo ypykue rendaguépe oñemoĩ peteĩ jeike pya'e. Eguerujey hag̃ua, eipe'a Ñongatupyre joapy.
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.48:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Recovery 0.9.44:** a later **independent** opening may start a fresh import only if earlier pending Google document absence is verified; the old backup/history remain protected. Network errors, uncertain responses and concurrent retries do not authorize duplicates. **UI 0.9.45–0.9.48:** refined settings/first use, local 51-language texts, three collapsible backup panels, checkboxes reflecting selection, bulk actions in 32-item batches, visual/installer updates and RTL action insets on Home/Settings. These UI changes do not change the manual backup policy.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [native UI CI](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40121298442) and [protected alpha distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40120263404) passed on earlier 0.9.48 commits; the final RTL hotfix `08dbced` has a [separate CI run](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40126683024), so consult the [current status](../../RELEASE_STATUS.md). Real Google acceptance remains pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 Kuatiañe’ẽ · Eiporavo ne ñe’ẽ</summary>

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
- **GN - Avañe’ẽ / Guarani (gn)** — ko kuatiarogue

</details>

**Eipe'a umi spreadsheet rembiapokue oĩva ne komputadórape directamente Google Sheets-pe Windows guive.**

ZagoSheetsWin ha'e peteĩ Windows rembiporu ivevúiva, ombohapéva spreadsheet rembiapokue local jepe'a:

**eikutu mokõi jey archivo rehe → ehupi ha emoambue → eipe'a Google Sheets-pe**

Oñemoinge porã rire, ZagoSheetsWin ikatu omyengovia archivo local ypykue peteĩ Internet rape mbyky (`.url`) rehe, omog̃uahẽva Google Sheets kuatiápe. Avei oñongatu peteĩ copia de seguridad local archivo ypykue rehegua, ikatúva ojegueru jey.

Hembipota hasy'ỹva: Google Sheets toiko peteĩ Windows rembiporu jepiguáicha ojepe'a hag̃ua umi spreadsheet archivo local.

## Mba'épa ojapo

ZagoSheetsWin ombojoaju Windows spreadsheet archivo ha Google Sheets. Eipe'áramo archivo ojeguerohorýva, pe rembiporu ikatu:

- oikuaa ha ohechajey spreadsheet;
- ojapo copia de seguridad ikatúva ojegueru jey;
- ohupi archivo directamente Google Drive-pe Google API oficial rupive;
- omoambue Google Sheets kuatia tee-pe;
- oipe'a osẽva navegador ojeporúva jepi rupi;
- ojapo `.url` rape mbyky local Google kuatiápe;
- oheja ojehupi jey archivo peteĩchagua oñeipe'a vove ambue jey.

Natekotevẽi rehupi archivo nde pópe Drive-pe, reheka navegador-pe térã rejapo jey ñemoambue.

## Archivo formato oipytyvõva

Ko'ág̃a oñemba'apo ko'ã formato rehe: `.xlsx`, `.xls`, `.ods`, `.csv` ha `.tsv`.

Oĩ formato ikatúva oguereko ambue joajuha ñemboty. Umi archivo oguerekóva mba'e ndaikatúiva oñeñongatu porã ojeporu oñeñangarekóvo, ani hag̃ua opyta rei dato okañy hag̃ua avave oikuaa'ỹre.

## Ojejapo Windows-pe g̃uarã

ZagoSheetsWin ojejapo Windows-pe g̃uarã ha oike sistema-pe **Open with (Eipe'a ko'ãva rupive)**, archivo tipo ñemboheraguapy, mokõi jey eikutu ha jepe'a, File Explorer joajurã ikatúva ojeporavo ha Windows installer rupive.

Rembiporu nomoambuéi Windows app ojeporúva jepi reikuaa'ỹre. Pe oiporúva oreko archivo joajuha ñangareko.

## Tekorosã oñepyrũ guive

Archivo local ñemyengovia ha'e tembiapo ikatúva oñemyatyrõ jey. Ojeipe'a mboyve archivo ypykue ifoldéragui, pe app ohecha:

1. oĩpa copia de seguridad ikatúva ojegueru jey;
2. ojejapópa porã Google Sheets kuatia;
3. oñongatúpa are hag̃ua archivo local joajuha;
4. ojehaípa ha ojehechajey Internet rape mbyky.

Tembiapo ndoikóiramo, archivo ypykue opyta oñeñongatupy.

> Ani rembyai dato pe oiporúva oikuaa'ỹre.

## Copia de seguridad

Umi archivo ypykue ikatu oñongatu peteĩ renda ñemiháme ne komputadórape oñemyengovia mboyve rape mbykýre. Ikatu reiporavo ñeñongatu renda tuichakue, ára mboypa oñongatúta ha ñemopotĩrã.

Pe copia de seguridad oñangareko archivo ypykue rehe oñemoinge jave. Péva **ndaha'éi mokõive gotyo sincronización**: Google Sheets-pe upe rire rejapóva ñemoambue ndojehai jeýi archivo spreadsheet ypykuépe.

## Ñemigua ha Google jeike

ZagoSheetsWin oñe'ẽ directamente Google API ndive ne komputadóra guive.

- Spreadsheet ryepýpe oĩva noñemondói Zagotools servidor-pe.
- OAuth token oñongatu local ha Windows tekorosã rupive oñeñangareko hese.
- `drive.file` permiso omoĩ límite umi archivo ojejapo térã ojeipe'áva app rupive rehe.
- Archivo ñemoinge natekotevẽi seguimiento analítico.

Ñemigua política ha Jiporu rehegua ñemohenda: https://zagotools.top/legal.html

## Proyecto rehegua mba'éichapa oĩ

ZagoSheetsWin rehe oñemba'apo gueteri, upévare ha'e **software alpha** ko'ág̃a. Windows → Google Sheets tembiapo tenondegua omba'apo, ha oñemyatyrõ ñemohenda, ñemyatyrõjey, formato joaju, heta ñe’ẽ oipytyvõ ha puruhára rekove. Ikatu oĩ ñemoambue osẽ mboyve versión estable peteĩha.

## Open in Google ndive joaju

ZagoSheetsWin ou [Open in Google](https://github.com/SwatiK425/open-in-google) rembiapokuégui, [SwatiK425](https://github.com/SwatiK425) ojapova'ekue. Upe proyecto ome'ẽ ñepyrũrã pyenda ha temiandu.

Ko'ág̃a ZagoSheetsWin ha'e peteĩ Windows rembiporu ijeheguíva, oguerekóva arquitectura, installer, interfaz, copia de seguridad ha ñemyatyrõjey, archivo joajuha, formato ñangareko ha archivo local Google Sheets-pe jepe'a. Proyecto ypykue opyta ijehegui; umi mejora opavavépe g̃uarã ikatu oñeme'ẽ upstream-pe oikotevẽ vove.

## Código abierto

ZagoSheetsWin ha'e software reiete ha código abierto. Oñangareko Open in Google código ypykue ojapova'ekuére ñemboaje ha licencia rehegua ñemohenda, ha ohechauka porã umi tembiapo ZagoSheetsWin / Zagotools ojapo rire.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Licencia

MIT licencia. Ehecha [LICENSE](../../../LICENSE) marandu hetave hag̃ua.

---

**ZagoSheetsWin — Zagotools proyecto**

Software michĩva apañuãi añeteguápe g̃uarã.
