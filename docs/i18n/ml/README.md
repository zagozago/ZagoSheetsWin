# ZagoSheetsWin

> ⚠️ **Alpha 0.9.48 — current safety notice in English; full editorial translation pending.**
>
> സ്പ്രെഡ്‌ഷീറ്റ് Google Sheets ൽ തുറന്നു. യഥാർത്ഥ ഫയലിന് പകരം ഒരു കുറുക്കുവഴി സൃഷ്ടിച്ചു. യഥാർത്ഥ ഫയൽ വീണ്ടെടുക്കാൻ ബാക്കപ്പുകൾ തുറക്കുക.
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.48:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Recovery 0.9.44:** a later **independent** opening may start a fresh import only if earlier pending Google document absence is verified; the old backup/history remain protected. Network errors, uncertain responses and concurrent retries do not authorize duplicates. **UI 0.9.45–0.9.48:** refined settings/first use, local 51-language texts, three collapsible backup panels, checkboxes reflecting selection, bulk actions in 32-item batches, visual/installer updates and RTL action insets on Home/Settings. These UI changes do not change the manual backup policy.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [native UI CI](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40121298442) and [protected alpha distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40120263404) passed on earlier 0.9.48 commits; the final RTL hotfix `08dbced` has a [separate CI run](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40126683024), so consult the [current status](../../RELEASE_STATUS.md). Real Google acceptance remains pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 ഡോക്യുമെന്റേഷൻ ഭാഷ · ഭാഷ തിരഞ്ഞെടുക്കുക</summary>

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
- **ML - മലയാളം / Malayalam (ml)** — നിലവിലെ പേജ്
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

**Windows-ലുള്ള പ്രാദേശിക സ്പ്രെഡ്ഷീറ്റ് ഫയലുകൾ Google Sheets-ൽ നേരിട്ട് തുറക്കുക.**

പ്രാദേശിക സ്പ്രെഡ്ഷീറ്റ് ഫയലുകൾ തുറക്കുന്നത് എളുപ്പമാക്കുന്ന ലഘുവായ Windows ആപ്പാണ് ZagoSheetsWin:

**ഫയലിൽ ഇരട്ട ക്ലിക്ക് → അപ്‌ലോഡ് ചെയ്ത് മാറ്റുക → Google Sheets-ൽ തുറക്കുക**

ഇറക്കുമതി വിജയിച്ചശേഷം, യഥാർത്ഥ പ്രാദേശിക ഫയലിന് പകരം Google Sheets പ്രമാണത്തിലേക്ക് നയിക്കുന്ന ഇന്റർനെറ്റ് ഷോർട്ട്കട്ട് (`.url`) ZagoSheetsWin സൃഷ്ടിക്കാം. യഥാർത്ഥ ഫയലിന്റെ വീണ്ടെടുക്കാവുന്ന പ്രാദേശിക ബാക്കപ്പും സൂക്ഷിക്കും.

ലക്ഷ്യം ലളിതമാണ്: പ്രാദേശിക സ്പ്രെഡ്ഷീറ്റ് ഫയലുകൾ തുറക്കുമ്പോൾ Google Sheets ഒരു സാധാരണ Windows ആപ്പ് പോലെ പ്രവർത്തിക്കണം.

## ഇത് എന്താണ് ചെയ്യുന്നത്

ZagoSheetsWin Windows സ്പ്രെഡ്ഷീറ്റ് ഫയലുകളെ Google Sheets-ുമായി ബന്ധിപ്പിക്കുന്നു. പിന്തുണയുള്ള പ്രാദേശിക ഫയൽ തുറക്കുമ്പോൾ ആപ്പിന്:

- സ്പ്രെഡ്ഷീറ്റ് തിരിച്ചറിഞ്ഞ് പരിശോധിക്കാം;
- വീണ്ടെടുക്കാവുന്ന ബാക്കപ്പ് സൃഷ്ടിക്കാം;
- Google-ന്റെ ഔദ്യോഗിക API വഴി നേരിട്ട് Google Drive-ലേക്ക് അപ്‌ലോഡ് ചെയ്യാം;
- Google Sheets-ന്റെ സ്വന്തം പ്രമാണരൂപത്തിലേക്ക് മാറ്റാം;
- ലഭിച്ച ഷീറ്റ് ഡിഫോൾട്ട് ബ്രൗസറിൽ തുറക്കാം;
- Google പ്രമാണത്തിലേക്ക് പ്രാദേശിക `.url` ഷോർട്ട്കട്ട് സൃഷ്ടിക്കാം;
- പിന്നീട് അതേ ഫയൽ തുറക്കുമ്പോൾ വീണ്ടും അപ്‌ലോഡ് ചെയ്യുന്നത് ഒഴിവാക്കാം.

Drive-ലേക്ക് കൈകൊണ്ട് അപ്‌ലോഡ് ചെയ്യുകയോ ബ്രൗസറിൽ ഫയൽ തിരയുകയോ വീണ്ടും വീണ്ടും മാറ്റുകയോ വേണ്ട.

## പിന്തുണയ്ക്കുന്ന ഫോർമാറ്റുകൾ

ഇപ്പോൾ വികസനത്തിന്റെ ലക്ഷ്യമായ ഫോർമാറ്റുകൾ: `.xlsx`, `.xls`, `.ods`, `.csv`, `.tsv`.

ചില ഫോർമാറ്റുകൾക്ക് അധിക അനുയോജ്യതാപരിധികൾ ഉണ്ടായേക്കാം. സുരക്ഷിതമായി നിലനിർത്താനാവാത്ത സവിശേഷതകളുള്ള ഫയലുകൾ ഉപയോക്താവറിയാതെ വിവരനഷ്ടം സംഭവിക്കാതിരിക്കാൻ ശ്രദ്ധാപൂർവം കൈകാര്യം ചെയ്യുന്നു.

## Windows-നായി രൂപകൽപ്പന ചെയ്തത്

ZagoSheetsWin പ്രത്യേകിച്ച് Windows-നായി വികസിപ്പിച്ചതാണ്. **Open with (ഇത് ഉപയോഗിച്ച് തുറക്കുക)**, ഫയൽതരം രജിസ്ട്രേഷൻ, ഇരട്ട ക്ലിക്കിലൂടെ തുറക്കൽ, ഐച്ഛിക File Explorer സംയോജനം, Windows-ന്റെ സ്വന്തം ഇൻസ്റ്റാളർ എന്നിവ ഇതിലുണ്ട്.

ഉപയോക്താവറിയാതെ Windows ഡിഫോൾട്ട് ആപ്പുകൾ മാറ്റില്ല. ഫയൽ ബന്ധങ്ങളുടെ നിയന്ത്രണം ഉപയോക്താവിനാണ്.

## രൂപകൽപ്പനയിൽ തന്നെ സുരക്ഷ

പ്രാദേശിക ഫയൽ മാറ്റിസ്ഥാപിക്കൽ വീണ്ടെടുക്കാവുന്ന പ്രവർത്തനമായിരിക്കണം. യഥാർത്ഥ ഫയൽ അതിന്റെ ഫോൾഡറിൽനിന്ന് നീക്കം ചെയ്യുന്നതിനുമുമ്പ് ആപ്പ് ഉറപ്പാക്കുന്നത്:

1. വീണ്ടെടുക്കാവുന്ന ബാക്കപ്പ് നിലവിലുണ്ട്;
2. Google Sheets പ്രമാണം വിജയകരമായി സൃഷ്ടിച്ചിട്ടുണ്ട്;
3. പ്രാദേശിക ഫയൽ ബന്ധം സ്ഥിരമായി സൂക്ഷിച്ചിട്ടുണ്ട്;
4. ഇന്റർനെറ്റ് ഷോർട്ട്കട്ട് എഴുതുകയും പരിശോധിക്കുകയും ചെയ്തിട്ടുണ്ട്.

പ്രവർത്തനം പരാജയപ്പെട്ടാൽ യഥാർത്ഥ ഫയൽ നിലനിർത്തും.

> ഉപയോക്താവറിയാതെ അവരുടെ വിവരങ്ങൾ ഒരിക്കലും നശിപ്പിക്കരുത്.

## ബാക്കപ്പുകൾ

യഥാർത്ഥ ഫയലുകൾ ഷോർട്ട്കട്ടുകൾ ഉപയോഗിച്ച് മാറ്റുന്നതിനുമുമ്പ് സ്വകാര്യ പ്രാദേശിക ബാക്കപ്പ് സ്ഥലത്ത് സൂക്ഷിക്കാം. സംഭരണപരിധി, സൂക്ഷിക്കുന്ന കാലാവധി, വൃത്തിയാക്കൽ എന്നിവ ക്രമീകരിക്കാം.

ബാക്കപ്പ് ഇറക്കുമതി ചെയ്ത സമയത്തെ യഥാർത്ഥ ഫയലിനെ സംരക്ഷിക്കുന്നു. ഇത് **രണ്ടുവഴിയുള്ള സമന്വയമല്ല**: പിന്നീട് Google Sheets-ൽ വരുത്തുന്ന മാറ്റങ്ങൾ യഥാർത്ഥ സ്പ്രെഡ്ഷീറ്റ് ഫയലിലേക്ക് തിരികെ എഴുതില്ല.

## സ്വകാര്യതയും Google പ്രവേശനവും

ZagoSheetsWin നിങ്ങളുടെ കമ്പ്യൂട്ടറിൽനിന്ന് നേരിട്ട് Google API-കളുമായി ആശയവിനിമയം നടത്തുന്നു.

- സ്പ്രെഡ്ഷീറ്റിലെ ഉള്ളടക്കം Zagotools സെർവറുകളിലേക്ക് അയയ്ക്കില്ല.
- OAuth ടോക്കണുകൾ പ്രാദേശികമായി സൂക്ഷിച്ച് Windows സുരക്ഷാസംവിധാനങ്ങൾ ഉപയോഗിച്ച് സംരക്ഷിക്കും.
- `drive.file` അനുമതി ആപ്പ് വഴി സൃഷ്ടിച്ചതോ തുറന്നതോ ആയ ഫയലുകളിലേക്കുള്ള പ്രവേശനം പരിമിതപ്പെടുത്തുന്നു.
- ഇറക്കുമതി പ്രക്രിയയ്ക്ക് അനലിറ്റിക്സ് ട്രാക്കിംഗ് ആവശ്യമില്ല.

സ്വകാര്യതാ നയവും ഉപയോഗനിബന്ധനകളും: https://zagotools.top/legal.html

## പ്രോജക്റ്റ് സ്ഥിതി

ZagoSheetsWin ഇപ്പോഴും സജീവ വികസനത്തിലുള്ള **ആൽഫ സോഫ്റ്റ്‌വെയർ** ആണ്. Windows → Google Sheets പ്രധാന പ്രവർത്തനരീതി നിലവിൽ പ്രവർത്തിക്കുന്നു. ഇൻസ്റ്റാളേഷൻ, വീണ്ടെടുക്കൽ, ഫോർമാറ്റ് അനുയോജ്യത, അന്തർദേശീയവൽക്കരണം, ഉപയോക്തൃ അനുഭവം എന്നിവ മെച്ചപ്പെടുത്തിവരുന്നു. ആദ്യ സ്ഥിരതയുള്ള പതിപ്പിന് മുമ്പ് മാറ്റങ്ങളുണ്ടാകാം.

## Open in Google-ുമായുള്ള ബന്ധം

ZagoSheetsWin, [SwatiK425](https://github.com/SwatiK425) വികസിപ്പിച്ച [Open in Google](https://github.com/SwatiK425/open-in-google) അടിസ്ഥാനമാക്കിയുള്ളതാണ്. അതാണ് പ്രാരംഭ അടിത്തറയും പ്രചോദനവും നൽകിയത്.

ഇപ്പോൾ സ്വന്തം ഘടന, ഇൻസ്റ്റാളർ, ഇന്റർഫേസ്, ബാക്കപ്പ്-വീണ്ടെടുക്കൽ സംവിധാനം, ഫയൽ ബന്ധങ്ങൾ, ഫോർമാറ്റ് കൈകാര്യംചെയ്യൽ, പ്രാദേശിക ഫയലുകൾ Google Sheets-ൽ തുറക്കുന്ന പ്രവർത്തനരീതി എന്നിവയുള്ള സ്വതന്ത്ര Windows ആപ്പാണ് ZagoSheetsWin. മൂല പ്രോജക്റ്റ് സ്വതന്ത്രമായി തുടരുന്നു; അനുയോജ്യമായ പൊതുമെച്ചപ്പെടുത്തലുകൾ upstream-ലേക്ക് നിർദേശിക്കാം.

## ഓപ്പൺ സോഴ്‌സ്

ZagoSheetsWin സൗജന്യ ഓപ്പൺ സോഴ്‌സ് സോഫ്റ്റ്‌വെയറാണ്. യഥാർത്ഥ Open in Google കോഡിന്റെ അംഗീകാരവും ലൈസൻസ് വ്യവസ്ഥകളും സംരക്ഷിക്കുകയും ZagoSheetsWin / Zagotools പിന്നീട് നടത്തിയ വികസനം വേർതിരിച്ചുകാണിക്കുകയും ചെയ്യുന്നു.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## ലൈസൻസ്

MIT ലൈസൻസ്. വിവരങ്ങൾക്ക് [LICENSE](../../../LICENSE) കാണുക.

---

**ZagoSheetsWin — Zagotools പ്രോജക്റ്റ്**

യഥാർത്ഥ പ്രശ്നങ്ങൾക്ക് ചെറിയ സോഫ്റ്റ്‌വെയർ.
