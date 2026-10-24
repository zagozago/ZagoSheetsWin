# ZagoSheetsWin

> ⚠️ **Alpha 0.9.48 — current safety notice in English; full editorial translation pending.**
>
> စာရင်းဇယားကို Google Sheets တွင် ဖွင့်ထားပါသည်။ မူရင်းဖိုင်ကို ဖြတ်လမ်းဖြင့် အစားထိုးထားပါသည်။ မူရင်းဖိုင်ကို ပြန်ရယူရန် အရန်သိမ်းဆည်းမှု များ ကိုဖွင့်ပါ။
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.48:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Recovery 0.9.44:** a later **independent** opening may start a fresh import only if earlier pending Google document absence is verified; the old backup/history remain protected. Network errors, uncertain responses and concurrent retries do not authorize duplicates. **UI 0.9.45–0.9.48:** refined settings/first use, local 51-language texts, three collapsible backup panels, checkboxes reflecting selection, bulk actions in 32-item batches, visual/installer updates and RTL action insets on Home/Settings. These UI changes do not change the manual backup policy.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [native UI CI](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40121298442) and [protected alpha distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40120263404) passed on earlier 0.9.48 commits; the final RTL hotfix `08dbced` has a [separate CI run](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40126683024), so consult the [current status](../../RELEASE_STATUS.md). Real Google acceptance remains pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 စာတမ်းဘာသာစကား · ဘာသာစကားရွေးပါ</summary>

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
- **MY - မြန်မာဘာသာ / Burmese (my)** — လက်ရှိစာမျက်နှာ
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

**Windows မှာရှိတဲ့ spreadsheet ဖိုင်တွေကို Google Sheets နဲ့ တိုက်ရိုက်ဖွင့်ပါ။**

ZagoSheetsWin က Windows အတွက် ပေါ့ပါးတဲ့ အက်ပ်တစ်ခုဖြစ်ပြီး ကွန်ပျူတာထဲက spreadsheet ဖိုင်တွေကို လွယ်လွယ်ကူကူ ဖွင့်နိုင်စေပါတယ်။

**ဖိုင်ကို နှစ်ချက်နှိပ် → အပ်လုဒ်လုပ်ပြီး ပြောင်းလဲ → Google Sheets မှာ ဖွင့်**

ဖိုင်တင်သွင်းမှု အောင်မြင်ပြီးနောက် ZagoSheetsWin က မူရင်းဖိုင်နေရာမှာ Google Sheets စာရွက်ဆီ ညွှန်ပြတဲ့ အင်တာနက် ဖြတ်လမ်း (`.url`) တစ်ခုကို အစားထိုးထားနိုင်ပါတယ်။ ပြန်လည်ရယူနိုင်တဲ့ မူရင်းဖိုင် မိတ္တူကိုလည်း ကွန်ပျူတာထဲမှာ သိမ်းဆည်းထားပါတယ်။

ရည်ရွယ်ချက်က ရိုးရှင်းပါတယ်။ Google Sheets ကို Windows အက်ပ်တစ်ခုလိုပဲ ပြည်တွင်း spreadsheet ဖိုင်တွေ ဖွင့်ရာမှာ အသုံးပြုနိုင်စေဖို့ ဖြစ်ပါတယ်။

## ဘာတွေလုပ်နိုင်သလဲ

ZagoSheetsWin က Windows မှ spreadsheet ဖိုင်တွေကို Google Sheets နဲ့ ချိတ်ဆက်ပေးပါတယ်။ ထောက်ပံ့ထားတဲ့ ဖိုင်ကို ဖွင့်ရင် အက်ပ်က:

- spreadsheet ကို သိရှိပြီး စစ်ဆေးနိုင်တယ်;
- ပြန်လည်ရယူနိုင်တဲ့ အရန်မိတ္တူ ဖန်တီးနိုင်တယ်;
- Google ရဲ့ တရားဝင် API တွေကနေ Google Drive သို့ တိုက်ရိုက် အပ်လုဒ်လုပ်နိုင်တယ်;
- Google Sheets ရဲ့ မူလ စာရွက်ပုံစံသို့ ပြောင်းနိုင်တယ်;
- ပြောင်းပြီးတဲ့ စာရွက်ကို မူရင်း browser မှာ ဖွင့်နိုင်တယ်;
- Google စာရွက်အတွက် `.url` ဖြတ်လမ်း ဖန်တီးနိုင်တယ်;
- နောက်တစ်ကြိမ် အဲဒီဖိုင်ကို ဖွင့်တဲ့အခါ ထပ်မံ အပ်လုဒ်မလုပ်အောင် ကာကွယ်နိုင်တယ်။

Drive ကို ကိုယ်တိုင် အပ်လုဒ်လုပ်စရာ၊ browser မှာ ဖိုင်ရှာစရာ၊ အကြိမ်ကြိမ် ပြောင်းလဲစရာ မလိုပါဘူး။

## ထောက်ပံ့ထားသော ဖိုင်ပုံစံများ

လက်ရှိ ဖွံ့ဖြိုးမှု ရည်မှန်းထားတဲ့ ဖိုင်ပုံစံတွေက `.xlsx`, `.xls`, `.ods`, `.csv`, `.tsv` တို့ဖြစ်ပါတယ်။

တချို့ပုံစံတွေမှာ လိုက်ဖက်မှု ကန့်သတ်ချက်တွေ ရှိနိုင်ပါတယ်။ မူလလုပ်ဆောင်ချက်တွေကို လုံခြုံစွာ ထိန်းသိမ်းလို့မရတဲ့ ဖိုင်တွေကို သတိထား စီမံပြီး အချက်အလက်တွေ အသိမပေးဘဲ ပျောက်ဆုံးခြင်း မဖြစ်အောင် ကာကွယ်ပါတယ်။

## Windows အတွက် ဒီဇိုင်းလုပ်ထားသည်

ZagoSheetsWin ကို Windows အတွက်ပဲ တည်ဆောက်ထားပြီး **Open with (ဤအက်ပ်ဖြင့် ဖွင့်ရန်)**၊ ဖိုင်အမျိုးအစား မှတ်ပုံတင်ခြင်း၊ နှစ်ချက်နှိပ်ပြီး ဖွင့်ခြင်း၊ ရွေးချယ်နိုင်တဲ့ File Explorer ချိတ်ဆက်မှုနဲ့ Windows installer တို့ ပါဝင်ပါတယ်။

Windows ရဲ့ ပုံမှန်ဖွင့်တဲ့ အက်ပ်တွေကို အသိမပေးဘဲ မပြောင်းပါဘူး။ ဖိုင်ချိတ်ဆက်မှုတွေကို အသုံးပြုသူက ထိန်းချုပ်နိုင်ပါတယ်။

## လုံခြုံရေးကို အခြေခံထားသည်

မူရင်းဖိုင်ကို အစားထိုးတဲ့ လုပ်ငန်းစဉ်ဟာ ပြန်လည်ရယူနိုင်ရပါမယ်။ ဖိုင်ကို မူရင်းဖိုလ်ဒါကနေ မဖယ်ရှားခင် အက်ပ်က စစ်ဆေးမယ့် အချက်တွေက:

1. ပြန်လည်ရယူနိုင်တဲ့ အရန်မိတ္တူ ရှိနေရမယ်;
2. Google Sheets စာရွက်ကို အောင်မြင်စွာ ဖန်တီးပြီးဖြစ်ရမယ်;
3. ပြည်တွင်းဖိုင်နဲ့ ချိတ်ဆက်မှုကို အမြဲတမ်း သိမ်းဆည်းပြီးဖြစ်ရမယ်;
4. အင်တာနက် ဖြတ်လမ်းကို ရေးပြီး စစ်ဆေးပြီးဖြစ်ရမယ်။

လုပ်ငန်းစဉ် မအောင်မြင်ရင် မူရင်းဖိုင်ကို ဆက်လက်ထားရှိပါတယ်။

> အသုံးပြုသူ မသိဘဲ သူ့အချက်အလက်တွေကို ဘယ်တော့မှ မဖျက်ဆီးပါနဲ့။

## အရန်မိတ္တူများ

မူရင်းဖိုင်တွေကို ဖြတ်လမ်းနဲ့ မအစားထိုးခင် ကိုယ်ပိုင် အရန်မိတ္တူ နေရာမှာ သိမ်းဆည်းနိုင်ပါတယ်။ သိမ်းဆည်းနိုင်တဲ့ ပမာဏ၊ ထိန်းသိမ်းကာလနဲ့ သန့်ရှင်းရေး ရွေးချယ်မှုတွေကို သတ်မှတ်နိုင်ပါတယ်။

အရန်မိတ္တူက စတင်တင်သွင်းချိန်က မူရင်းဖိုင်ကို ကာကွယ်ပေးပါတယ်။ **နှစ်ဖက်လုံး အလိုအလျောက် တစ်ပြိုင်တည်း ပြောင်းပေးတဲ့ စနစ် မဟုတ်ပါဘူး**။ Google Sheets မှာ နောက်ပိုင်း ပြင်ဆင်တာတွေကို မူရင်း spreadsheet ဖိုင်ထဲ ပြန်မရေးပါဘူး။

## ကိုယ်ရေးလုံခြုံမှုနှင့် Google ဝင်ရောက်ခွင့်

ZagoSheetsWin က သင့်ကွန်ပျူတာကနေ Google API တွေနဲ့ တိုက်ရိုက် ဆက်သွယ်ပါတယ်။

- spreadsheet အကြောင်းအရာတွေကို Zagotools server သို့ မပို့ပါဘူး။
- OAuth token တွေကို Windows လုံခြုံရေးစနစ်နဲ့ ကာကွယ်ပြီး ကွန်ပျူတာထဲမှာ သိမ်းပါတယ်။
- `drive.file` ခွင့်ပြုချက်က အက်ပ်ကနေ ဖန်တီးထားတဲ့ သို့မဟုတ် ဖွင့်ထားတဲ့ ဖိုင်တွေကိုသာ ဝင်ရောက်ခွင့် ကန့်သတ်ပေးပါတယ်။
- ဖိုင်တင်သွင်းဖို့ analytics tracking မလိုအပ်ပါဘူး။

ကိုယ်ရေးလုံခြုံမှု မူဝါဒနှင့် အသုံးပြုမှု စည်းကမ်းချက်များ: https://zagotools.top/legal.html

## ပရောဂျက်အခြေအနေ

ZagoSheetsWin ကို ဆက်လက် ဖွံ့ဖြိုးနေပြီး လောလောဆယ် **alpha အဆင့် ဆော့ဖ်ဝဲ** ဖြစ်ပါတယ်။ Windows → Google Sheets အခြေခံလုပ်ငန်းစဉ် အလုပ်လုပ်နေပါတယ်။ တပ်ဆင်ခြင်း၊ ပြန်လည်ရယူခြင်း၊ ဖိုင်လိုက်ဖက်မှု၊ ဘာသာစကားစုံနဲ့ အသုံးပြုရလွယ်ကူမှုကို ပိုမိုကောင်းမွန်စေနေပါတယ်။ ပထမဆုံး တည်ငြိမ်တဲ့ ဗားရှင်းမထွက်ခင် ပြောင်းလဲနိုင်ပါတယ်။

## Open in Google နှင့် ဆက်နွယ်မှု

ZagoSheetsWin ကို [SwatiK425](https://github.com/SwatiK425) ရဲ့ [Open in Google](https://github.com/SwatiK425/open-in-google) ပရောဂျက်အပေါ် အခြေခံ တီထွင်ထားတာပါ။ မူလပရောဂျက်က နည်းပညာအခြေခံနဲ့ စိတ်ကူးစိတ်သန်းကို ပေးခဲ့ပါတယ်။

ယခု ZagoSheetsWin မှာ ကိုယ်ပိုင် ဖွဲ့စည်းပုံ၊ installer၊ interface၊ backup နဲ့ recovery၊ ဖိုင်ချိတ်ဆက်မှု၊ ဖိုင်ပုံစံစီမံခန့်ခွဲမှုနဲ့ Google Sheets မှာ ဖွင့်နိုင်တဲ့ စနစ်ရှိပြီး သီးခြား Windows အက်ပ် ဖြစ်လာပါတယ်။ မူလပရောဂျက်က သီးခြား ဆက်လက်ရှိနေပြီး သင့်လျော်တဲ့ အထွေထွေတိုးတက်မှုတွေကို upstream သို့ အကြံပြုနိုင်ပါတယ်။

## ပွင့်လင်းရင်းမြစ်

ZagoSheetsWin က အခမဲ့ ပွင့်လင်းရင်းမြစ် ဆော့ဖ်ဝဲဖြစ်ပါတယ်။ Open in Google မူလကုဒ်ရဲ့ မူပိုင်ရှင် အသိအမှတ်ပြုမှုနဲ့ လိုင်စင်လိုအပ်ချက်တွေကို ထိန်းသိမ်းထားပြီး ZagoSheetsWin / Zagotools ရဲ့ နောက်ပိုင်း ဖွံ့ဖြိုးမှုတွေကို သီးခြား ဖော်ပြပါတယ်။

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## လိုင်စင်

MIT လိုင်စင်။ အသေးစိတ်ကို [LICENSE](../../../LICENSE) မှာ ကြည့်ပါ။

---

**ZagoSheetsWin — Zagotools ပရောဂျက်**

တကယ့်ပြဿနာတွေအတွက် ဆော့ဖ်ဝဲလေးတွေ။
