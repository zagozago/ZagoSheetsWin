# ZagoSheetsWin

> ⚠️ **Alpha 0.9.43 — current safety notice in English; full editorial translation pending.**
>
> સ્પ્રેડશીટ Google Sheets માં ખુલી ગઈ છે. મૂળ ફાઇલની જગ્યાએ શૉર્ટકટ બનાવાયો છે. મૂળ ફાઇલ પાછી મેળવવા બેકઅપ્સ ખોલો.
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.43:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [Local core](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) and [protected distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) passed on commit `9fbec2a`; real Google acceptance is still pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 દસ્તાવેજની ભાષા · ભાષા પસંદ કરો</summary>

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
- **GU - ગુજરાતી / Gujarati (gu)** — વર્તમાન પાનું
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

**Windowsમાંથી સ્થાનિક સ્પ્રેડશીટ ફાઇલો સીધી Google Sheetsમાં ખોલો.**

ZagoSheetsWin એ Windows માટેની હલકી એપ્લિકેશન છે, જે સ્થાનિક સ્પ્રેડશીટ ખોલવાની પ્રક્રિયાને સરળ બનાવે છે:

**ફાઇલ પર બે વાર ક્લિક → અપલોડ અને રૂપાંતર → Google Sheetsમાં ખોલો**

સફળ આયાત પછી ZagoSheetsWin મૂળ સ્થાનિક ફાઇલને Google Sheets દસ્તાવેજ તરફ લઈ જતા ઇન્ટરનેટ શોર્ટકટ (`.url`) વડે બદલી શકે છે, અને મૂળ ફાઇલનો પુનઃપ્રાપ્ત કરી શકાય એવો સ્થાનિક બેકઅપ સાચવી રાખે છે.

હેતુ સરળ છે: સ્થાનિક સ્પ્રેડશીટ ખોલતી વખતે Google Sheets સામાન્ય Windows એપ જેવી સરળ લાગે.

## તે શું કરે છે

ZagoSheetsWin Windowsની સ્પ્રેડશીટ ફાઇલોને Google Sheets સાથે જોડે છે. સમર્થિત સ્થાનિક ફાઇલ ખોલવામાં આવે ત્યારે એપ્લિકેશન:

- સ્પ્રેડશીટને ઓળખી અને ચકાસી શકે છે;
- પુનઃપ્રાપ્ત કરી શકાય એવો બેકઅપ બનાવી શકે છે;
- Googleની સત્તાવાર API દ્વારા સીધું Google Drive પર અપલોડ કરી શકે છે;
- તેને મૂળ Google Sheets દસ્તાવેજમાં રૂપાંતરિત કરી શકે છે;
- પરિણામ ડિફોલ્ટ બ્રાઉઝરમાં ખોલી શકે છે;
- Google દસ્તાવેજ માટે સ્થાનિક `.url` શોર્ટકટ બનાવી શકે છે;
- પછી ફરી એ જ ફાઇલ ખોલતાં ફરી અપલોડ કરવાની જરૂર ટાળી શકે છે.

Drive પર જાતે અપલોડ કરવાની, બ્રાઉઝરમાં ફાઇલ શોધવાની કે વારંવાર રૂપાંતર કરવાની જરૂર નથી.

## સમર્થિત ફોર્મેટ

વર્તમાન વિકાસમાં લક્ષ્ય ફોર્મેટ: `.xlsx`, `.xls`, `.ods`, `.csv`, `.tsv`.

કેટલાક ફોર્મેટમાં વધારાની સુસંગતતા મર્યાદાઓ હોઈ શકે છે. સુરક્ષિત રીતે જાળવી ન શકાય તેવી સુવિધાઓ ધરાવતી ફાઇલોને સાવચેતીપૂર્વક સંભાળવામાં આવે છે, જેથી જાણ કર્યા વિના ડેટા ગુમાય નહીં.

## Windows માટે બનાવેલું

ZagoSheetsWin ખાસ Windows માટે બનાવવામાં આવ્યું છે અને તેમાં **Open with (આની સાથે ખોલો)**, ફાઇલ પ્રકારનું નોંધણીકરણ, બે વાર ક્લિક કરીને ખોલવાની સુવિધા, વૈકલ્પિક File Explorer એકીકરણ અને મૂળ Windows ઇન્સ્ટોલરનો સમાવેશ થાય છે.

એપ્લિકેશન તમારા જાણ્યા વિના Windowsની ડિફોલ્ટ એપ્સ બદલતી નથી. ફાઇલ સંકળાણોનું નિયંત્રણ વપરાશકર્તા પાસે રહે છે.

## શરૂઆતથી જ સુરક્ષા

સ્થાનિક ફાઇલને બદલવાની પ્રક્રિયા પુનઃપ્રાપ્ત કરી શકાય એવી હોવી જોઈએ. મૂળ ફાઇલને તેના ફોલ્ડરમાંથી દૂર કરતાં પહેલાં એપ્લિકેશન ખાતરી કરે છે કે:

1. પુનઃપ્રાપ્ત કરી શકાય એવો બેકઅપ હાજર છે;
2. Google Sheets દસ્તાવેજ સફળતાપૂર્વક બનાવાયો છે;
3. સ્થાનિક ફાઇલનું સંકળાણ કાયમી રીતે સાચવાયું છે;
4. ઇન્ટરનેટ શોર્ટકટ લખાયો અને ચકાસાયો છે.

પ્રક્રિયા નિષ્ફળ જાય તો મૂળ ફાઇલ જાળવી રાખવામાં આવે છે.

> વપરાશકર્તાને જાણ કર્યા વિના તેનો ડેટા ક્યારેય નષ્ટ ન કરો.

## બેકઅપ

શોર્ટકટથી બદલતાં પહેલાં મૂળ ફાઇલોને ખાનગી સ્થાનિક બેકઅપ જગ્યામાં રાખી શકાય છે. સ્ટોરેજ મર્યાદા, જાળવણી સમયગાળો અને સફાઈ વિકલ્પો ગોઠવી શકાય છે.

બેકઅપ આયાત કરેલી મૂળ ફાઇલનું રક્ષણ કરે છે. તે **બે-માર્ગીય સમન્વય નથી**: Google Sheetsમાં પછી કરેલા ફેરફારો મૂળ સ્પ્રેડશીટ ફાઇલમાં પાછા લખાતા નથી.

## ગોપનીયતા અને Google ઍક્સેસ

ZagoSheetsWin તમારા કમ્પ્યુટર પરથી સીધું Googleની API સાથે સંચાર કરે છે.

- તમારી સ્પ્રેડશીટની સામગ્રી Zagotoolsના સર્વર પર મોકલાતી નથી.
- OAuth ટોકન સ્થાનિક રીતે સંગ્રહિત થાય છે અને Windowsની સુરક્ષા વ્યવસ્થાથી સુરક્ષિત રહે છે.
- `drive.file` પરવાનગીની મર્યાદા એપ દ્વારા બનાવેલી કે ખોલેલી ફાઇલો સુધી ઍક્સેસ સીમિત કરે છે.
- સ્પ્રેડશીટ આયાત માટે એનાલિટિક્સ ટ્રેકિંગની જરૂર નથી.

ગોપનીયતા નીતિ અને ઉપયોગની શરતો: https://zagotools.top/legal.html

## પ્રોજેક્ટની સ્થિતિ

ZagoSheetsWinનો સક્રિય વિકાસ ચાલુ છે અને હાલમાં તે **આલ્ફા સોફ્ટવેર** છે. Windows → Google Sheetsનો મુખ્ય પ્રવાહ કાર્યરત છે. ઇન્સ્ટોલેશન, પુનઃપ્રાપ્તિ, ફોર્મેટ સુસંગતતા, આંતરરાષ્ટ્રીયકરણ અને વપરાશકર્તા અનુભવ સુધારવામાં આવી રહ્યા છે. પહેલી સ્થિર આવૃત્તિ પહેલાં ફેરફારો થઈ શકે છે.

## Open in Google સાથેનો સંબંધ

ZagoSheetsWin, [SwatiK425](https://github.com/SwatiK425)ના [Open in Google](https://github.com/SwatiK425/open-in-google) પ્રોજેક્ટ પર આધારિત છે અને તેમાંથી વિકસ્યું છે. મૂળ પ્રોજેક્ટે પ્રાથમિક આધાર અને પ્રેરણા પૂરી પાડી.

હવે ZagoSheetsWin પોતાની રચના, ઇન્સ્ટોલર, ઇન્ટરફેસ, બેકઅપ અને પુનઃપ્રાપ્તિ વ્યવસ્થા, ફાઇલ સંકળાણ, ફોર્મેટ સંચાલન અને સ્થાનિક ફાઇલોને Google Sheetsમાં ખોલવાનો અનુભવ ધરાવતી સ્વતંત્ર Windows એપ છે. મૂળ પ્રોજેક્ટ સ્વતંત્ર રહે છે; યોગ્ય સામાન્ય સુધારાઓ upstreamને સૂચવી શકાય છે.

## ઓપન સોર્સ

ZagoSheetsWin મફત અને ઓપન સોર્સ સોફ્ટવેર છે. મૂળ Open in Google કોડના શ્રેય અને લાઇસન્સની શરતો જાળવી રાખવામાં આવે છે તથા ZagoSheetsWin / Zagotoolsના પછીના વિકાસને અલગ દર્શાવવામાં આવે છે.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## લાઇસન્સ

MIT લાઇસન્સ. વિગતો માટે [LICENSE](../../../LICENSE) જુઓ.

---

**ZagoSheetsWin — Zagotools પ્રોજેક્ટ**

વાસ્તવિક સમસ્યાઓ માટે નાનું સોફ્ટવેર.
