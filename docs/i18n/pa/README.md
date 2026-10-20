# ZagoSheetsWin

> ⚠️ **Alpha 0.9.43 — current safety notice in English; full editorial translation pending.**
>
> ਸਪ੍ਰੈਡਸ਼ੀਟ Google Sheets ਵਿੱਚ ਖੁੱਲ੍ਹ ਗਈ ਹੈ। ਅਸਲ ਫਾਈਲ ਦੀ ਥਾਂ ਸ਼ਾਰਟਕੱਟ ਬਣਾਇਆ ਗਿਆ ਹੈ। ਅਸਲ ਫਾਈਲ ਵਾਪਸ ਲੈਣ ਲਈ ਬੈਕਅੱਪ ਖੋਲ੍ਹੋ।
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.43:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [Local core](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) and [protected distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) passed on commit `9fbec2a`; real Google acceptance is still pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 ਦਸਤਾਵੇਜ਼ ਦੀ ਭਾਸ਼ਾ · ਭਾਸ਼ਾ ਚੁਣੋ</summary>

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
- **PA - ਪੰਜਾਬੀ / Punjabi (pa)** — ਮੌਜੂਦਾ ਸਫ਼ਾ
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

**Windows ਤੋਂ ਆਪਣੀਆਂ ਸਥਾਨਕ ਸਪ੍ਰੈੱਡਸ਼ੀਟ ਫਾਈਲਾਂ ਸਿੱਧੀਆਂ Google Sheets ਵਿੱਚ ਖੋਲ੍ਹੋ।**

ZagoSheetsWin Windows ਲਈ ਇੱਕ ਹਲਕਾ ਐਪ ਹੈ ਜੋ ਸਥਾਨਕ ਸਪ੍ਰੈੱਡਸ਼ੀਟ ਖੋਲ੍ਹਣ ਦੀ ਪ੍ਰਕਿਰਿਆ ਸੌਖੀ ਬਣਾਉਂਦਾ ਹੈ:

**ਫਾਈਲ 'ਤੇ ਡਬਲ-ਕਲਿੱਕ → ਅੱਪਲੋਡ ਅਤੇ ਬਦਲੋ → Google Sheets ਵਿੱਚ ਖੋਲ੍ਹੋ**

ਸਫਲ ਆਯਾਤ ਤੋਂ ਬਾਅਦ ZagoSheetsWin ਮੂਲ ਸਥਾਨਕ ਫਾਈਲ ਦੀ ਥਾਂ Google Sheets ਦਸਤਾਵੇਜ਼ ਵੱਲ ਲੈ ਜਾਣ ਵਾਲਾ ਇੰਟਰਨੈੱਟ ਸ਼ਾਰਟਕੱਟ (`.url`) ਰੱਖ ਸਕਦਾ ਹੈ, ਅਤੇ ਮੂਲ ਫਾਈਲ ਦਾ ਮੁੜ ਪ੍ਰਾਪਤ ਕੀਤਾ ਜਾ ਸਕਣ ਵਾਲਾ ਸਥਾਨਕ ਬੈਕਅੱਪ ਸੰਭਾਲਦਾ ਹੈ।

ਮਕਸਦ ਸੌਖਾ ਹੈ: ਸਥਾਨਕ ਸਪ੍ਰੈੱਡਸ਼ੀਟ ਖੋਲ੍ਹਣ ਵੇਲੇ Google Sheets ਨੂੰ ਆਮ Windows ਐਪ ਵਰਗਾ ਬਣਾਉਣਾ।

## ਇਹ ਕੀ ਕਰਦਾ ਹੈ

ZagoSheetsWin Windows ਦੀਆਂ ਸਪ੍ਰੈੱਡਸ਼ੀਟ ਫਾਈਲਾਂ ਨੂੰ Google Sheets ਨਾਲ ਜੋੜਦਾ ਹੈ। ਸਮਰਥਿਤ ਸਥਾਨਕ ਫਾਈਲ ਖੋਲ੍ਹਣ 'ਤੇ ਐਪ:

- ਸਪ੍ਰੈੱਡਸ਼ੀਟ ਦੀ ਪਛਾਣ ਅਤੇ ਜਾਂਚ ਕਰ ਸਕਦਾ ਹੈ;
- ਮੁੜ ਪ੍ਰਾਪਤ ਕਰਨ ਯੋਗ ਬੈਕਅੱਪ ਬਣਾ ਸਕਦਾ ਹੈ;
- Google ਦੇ ਅਧਿਕਾਰਤ API ਰਾਹੀਂ ਸਿੱਧਾ Google Drive 'ਤੇ ਅੱਪਲੋਡ ਕਰ ਸਕਦਾ ਹੈ;
- ਫਾਈਲ ਨੂੰ Google Sheets ਦੇ ਮੂਲ ਦਸਤਾਵੇਜ਼ ਵਿੱਚ ਬਦਲ ਸਕਦਾ ਹੈ;
- ਨਤੀਜਾ ਮੂਲ ਬ੍ਰਾਊਜ਼ਰ ਵਿੱਚ ਖੋਲ੍ਹ ਸਕਦਾ ਹੈ;
- Google ਦਸਤਾਵੇਜ਼ ਲਈ ਸਥਾਨਕ `.url` ਸ਼ਾਰਟਕੱਟ ਬਣਾ ਸਕਦਾ ਹੈ;
- ਅਗਲੀ ਵਾਰ ਉਹੀ ਫਾਈਲ ਖੋਲ੍ਹਣ 'ਤੇ ਦੁਬਾਰਾ ਅੱਪਲੋਡ ਤੋਂ ਬਚ ਸਕਦਾ ਹੈ।

Drive 'ਤੇ ਹੱਥੀਂ ਅੱਪਲੋਡ, ਬ੍ਰਾਊਜ਼ਰ ਵਿੱਚ ਫਾਈਲ ਲੱਭਣ ਜਾਂ ਵਾਰ-ਵਾਰ ਬਦਲਣ ਦੀ ਲੋੜ ਨਹੀਂ।

## ਸਮਰਥਿਤ ਫਾਰਮੈਟ

ਮੌਜੂਦਾ ਵਿਕਾਸ ਦੇ ਨਿਸ਼ਾਨੇ: `.xlsx`, `.xls`, `.ods`, `.csv`, `.tsv`।

ਕੁਝ ਫਾਰਮੈਟਾਂ 'ਤੇ ਹੋਰ ਅਨੁਕੂਲਤਾ ਪਾਬੰਦੀਆਂ ਹੋ ਸਕਦੀਆਂ ਹਨ। ਜਿਨ੍ਹਾਂ ਵਿਸ਼ੇਸ਼ਤਾਵਾਂ ਨੂੰ ਸੁਰੱਖਿਅਤ ਤਰੀਕੇ ਨਾਲ ਨਹੀਂ ਰੱਖਿਆ ਜਾ ਸਕਦਾ, ਉਹਨਾਂ ਫਾਈਲਾਂ ਨੂੰ ਧਿਆਨ ਨਾਲ ਸੰਭਾਲਿਆ ਜਾਂਦਾ ਹੈ ਤਾਂ ਜੋ ਬਿਨਾਂ ਦੱਸੇ ਡਾਟਾ ਨਾ ਗੁਆਚੇ।

## Windows ਲਈ ਤਿਆਰ

ZagoSheetsWin ਖ਼ਾਸ Windows ਲਈ ਬਣਿਆ ਹੈ ਅਤੇ **Open with (ਇਸ ਨਾਲ ਖੋਲ੍ਹੋ)**, ਫਾਈਲ ਕਿਸਮ ਰਜਿਸਟ੍ਰੇਸ਼ਨ, ਡਬਲ-ਕਲਿੱਕ ਨਾਲ ਖੋਲ੍ਹਣ, ਚੋਣਵੀਂ File Explorer ਇਕੀਕਰਨ ਅਤੇ ਮੂਲ Windows ਇੰਸਟਾਲਰ ਨਾਲ ਜੁੜਦਾ ਹੈ।

ਐਪ ਚੁੱਪਚਾਪ Windows ਦੇ ਡਿਫਾਲਟ ਐਪ ਨਹੀਂ ਬਦਲਦਾ। ਫਾਈਲ ਸੰਬੰਧਾਂ ਦਾ ਕੰਟਰੋਲ ਵਰਤੋਂਕਾਰ ਕੋਲ ਰਹਿੰਦਾ ਹੈ।

## ਸ਼ੁਰੂ ਤੋਂ ਸੁਰੱਖਿਆ

ਸਥਾਨਕ ਫਾਈਲ ਨੂੰ ਬਦਲਣਾ ਇੱਕ ਅਜਿਹੀ ਕਾਰਵਾਈ ਹੈ ਜਿਸ ਨੂੰ ਮੁੜ ਪ੍ਰਾਪਤ ਕੀਤਾ ਜਾ ਸਕੇ। ਮੂਲ ਫਾਈਲ ਨੂੰ ਫੋਲਡਰ ਤੋਂ ਹਟਾਉਣ ਤੋਂ ਪਹਿਲਾਂ ਐਪ ਜਾਂਚਦਾ ਹੈ ਕਿ:

1. ਮੁੜ ਪ੍ਰਾਪਤ ਹੋ ਸਕਣ ਵਾਲਾ ਬੈਕਅੱਪ ਮੌਜੂਦ ਹੈ;
2. Google Sheets ਦਸਤਾਵੇਜ਼ ਸਫਲਤਾ ਨਾਲ ਬਣਿਆ ਹੈ;
3. ਸਥਾਨਕ ਫਾਈਲ ਦਾ ਸੰਬੰਧ ਪੱਕੇ ਤੌਰ 'ਤੇ ਸੰਭਾਲਿਆ ਗਿਆ ਹੈ;
4. ਇੰਟਰਨੈੱਟ ਸ਼ਾਰਟਕੱਟ ਲਿਖਿਆ ਅਤੇ ਜਾਂਚਿਆ ਗਿਆ ਹੈ।

ਪ੍ਰਕਿਰਿਆ ਅਸਫਲ ਹੋਣ 'ਤੇ ਮੂਲ ਫਾਈਲ ਸੁਰੱਖਿਅਤ ਰਹਿੰਦੀ ਹੈ।

> ਵਰਤੋਂਕਾਰ ਦਾ ਡਾਟਾ ਬਿਨਾਂ ਦੱਸੇ ਕਦੇ ਵੀ ਨਸ਼ਟ ਨਾ ਕਰੋ।

## ਬੈਕਅੱਪ

ਮੂਲ ਫਾਈਲਾਂ ਨੂੰ ਸ਼ਾਰਟਕੱਟ ਨਾਲ ਬਦਲਣ ਤੋਂ ਪਹਿਲਾਂ ਨਿੱਜੀ ਸਥਾਨਕ ਬੈਕਅੱਪ ਥਾਂ ਵਿੱਚ ਸੰਭਾਲਿਆ ਜਾ ਸਕਦਾ ਹੈ। ਸਟੋਰੇਜ ਦੀ ਹੱਦ, ਸੰਭਾਲਣ ਦੀ ਮਿਆਦ ਅਤੇ ਸਫ਼ਾਈ ਦੇ ਵਿਕਲਪ ਸੈੱਟ ਕੀਤੇ ਜਾ ਸਕਦੇ ਹਨ।

ਬੈਕਅੱਪ ਆਯਾਤ ਕੀਤੀ ਮੂਲ ਫਾਈਲ ਦੀ ਰੱਖਿਆ ਕਰਦਾ ਹੈ। ਇਹ **ਦੋ-ਪਾਸੀਂ ਸਿੰਕ ਨਹੀਂ**: Google Sheets ਵਿੱਚ ਬਾਅਦ ਦੇ ਬਦਲਾਅ ਮੂਲ ਸਪ੍ਰੈੱਡਸ਼ੀਟ ਫਾਈਲ ਵਿੱਚ ਵਾਪਸ ਨਹੀਂ ਲਿਖੇ ਜਾਂਦੇ।

## ਗੋਪਨੀਯਤਾ ਅਤੇ Google ਪਹੁੰਚ

ZagoSheetsWin ਤੁਹਾਡੇ ਕੰਪਿਊਟਰ ਤੋਂ ਸਿੱਧੇ Google API ਨਾਲ ਸੰਪਰਕ ਕਰਦਾ ਹੈ।

- ਸਪ੍ਰੈੱਡਸ਼ੀਟ ਦੀ ਸਮੱਗਰੀ Zagotools ਸਰਵਰ 'ਤੇ ਨਹੀਂ ਭੇਜੀ ਜਾਂਦੀ।
- OAuth ਟੋਕਨ Windows ਸੁਰੱਖਿਆ ਪ੍ਰਣਾਲੀ ਨਾਲ ਸੁਰੱਖਿਅਤ ਹੋ ਕੇ ਸਥਾਨਕ ਤੌਰ 'ਤੇ ਸੰਭਾਲੇ ਜਾਂਦੇ ਹਨ।
- `drive.file` ਇਜਾਜ਼ਤ ਐਪ ਰਾਹੀਂ ਬਣਾਈਆਂ ਜਾਂ ਖੋਲ੍ਹੀਆਂ ਫਾਈਲਾਂ ਤੱਕ ਪਹੁੰਚ ਸੀਮਿਤ ਕਰਦੀ ਹੈ।
- ਆਯਾਤ ਲਈ ਵਰਤੋਂ ਵਿਸ਼ਲੇਸ਼ਣ ਟ੍ਰੈਕਿੰਗ ਦੀ ਲੋੜ ਨਹੀਂ।

ਗੋਪਨੀਯਤਾ ਨੀਤੀ ਅਤੇ ਵਰਤੋਂ ਦੀਆਂ ਸ਼ਰਤਾਂ: https://zagotools.top/legal.html

## ਪ੍ਰੋਜੈਕਟ ਦੀ ਸਥਿਤੀ

ZagoSheetsWin ਦਾ ਸਰਗਰਮ ਵਿਕਾਸ ਜਾਰੀ ਹੈ ਅਤੇ ਇਸ ਵੇਲੇ ਇਹ **ਅਲਫਾ ਸਾਫਟਵੇਅਰ** ਹੈ। Windows → Google Sheets ਦਾ ਮੁੱਖ ਕੰਮ ਚੱਲ ਰਿਹਾ ਹੈ। ਇੰਸਟਾਲੇਸ਼ਨ, ਮੁੜ ਪ੍ਰਾਪਤੀ, ਫਾਰਮੈਟ ਅਨੁਕੂਲਤਾ, ਭਾਸ਼ਾਈ ਸਹਾਇਤਾ ਅਤੇ ਵਰਤੋਂਕਾਰ ਅਨੁਭਵ ਸੁਧਾਰੇ ਜਾ ਰਹੇ ਹਨ। ਪਹਿਲੇ ਸਥਿਰ ਰਿਲੀਜ਼ ਤੋਂ ਪਹਿਲਾਂ ਬਦਲਾਅ ਹੋ ਸਕਦੇ ਹਨ।

## Open in Google ਨਾਲ ਸੰਬੰਧ

ZagoSheetsWin, [SwatiK425](https://github.com/SwatiK425) ਦੇ [Open in Google](https://github.com/SwatiK425/open-in-google) ਤੋਂ ਬਣਿਆ ਪ੍ਰੋਜੈਕਟ ਹੈ। ਉਸ ਪ੍ਰੋਜੈਕਟ ਨੇ ਸ਼ੁਰੂਆਤੀ ਬੁਨਿਆਦ ਅਤੇ ਪ੍ਰੇਰਣਾ ਦਿੱਤੀ ਸੀ।

ਹੁਣ ZagoSheetsWin ਆਪਣੀ ਬਣਤਰ, ਇੰਸਟਾਲਰ, ਇੰਟਰਫੇਸ, ਬੈਕਅੱਪ ਅਤੇ ਮੁੜ ਪ੍ਰਾਪਤੀ, ਫਾਈਲ ਸੰਬੰਧ, ਫਾਰਮੈਟ ਸੰਭਾਲ ਅਤੇ ਸਥਾਨਕ ਫਾਈਲਾਂ ਨੂੰ Google Sheets ਵਿੱਚ ਖੋਲ੍ਹਣ ਦੇ ਤਰੀਕੇ ਵਾਲਾ ਸੁਤੰਤਰ Windows ਐਪ ਹੈ। ਮੂਲ ਪ੍ਰੋਜੈਕਟ ਵੀ ਸੁਤੰਤਰ ਹੈ; ਢੁੱਕਵੇਂ ਆਮ ਸੁਧਾਰ upstream ਨੂੰ ਸੁਝਾਏ ਜਾ ਸਕਦੇ ਹਨ।

## ਓਪਨ ਸੋਰਸ

ZagoSheetsWin ਮੁਫ਼ਤ ਓਪਨ ਸੋਰਸ ਸਾਫਟਵੇਅਰ ਹੈ। ਮੂਲ Open in Google ਕੋਡ ਦੇ ਸਿਹਰੇ ਅਤੇ ਲਾਇਸੈਂਸ ਦੀਆਂ ਸ਼ਰਤਾਂ ਕਾਇਮ ਰੱਖੀਆਂ ਜਾਂਦੀਆਂ ਹਨ ਅਤੇ ZagoSheetsWin / Zagotools ਦੇ ਬਾਅਦਲੇ ਵਿਕਾਸ ਨੂੰ ਵੱਖਰਾ ਦਰਸਾਇਆ ਜਾਂਦਾ ਹੈ।

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## ਲਾਇਸੈਂਸ

MIT ਲਾਇਸੈਂਸ। ਵੇਰਵੇ ਲਈ [LICENSE](../../../LICENSE) ਵੇਖੋ।

---

**ZagoSheetsWin — Zagotools ਦਾ ਪ੍ਰੋਜੈਕਟ**

ਅਸਲੀ ਸਮੱਸਿਆਵਾਂ ਲਈ ਛੋਟਾ ਸਾਫਟਵੇਅਰ।
