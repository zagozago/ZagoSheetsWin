# ZagoSheetsWin

> ⚠️ **Alpha 0.9.48 — current safety notice in English; full editorial translation pending.**
>
> स्प्रेडशीट Google Sheets मध्ये उघडली आहे. मूळ फाइलच्या जागी शॉर्टकट तयार केला आहे. मूळ फाइल परत मिळवण्यासाठी बॅकअप उघडा.
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.48:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Recovery 0.9.44:** a later **independent** opening may start a fresh import only if earlier pending Google document absence is verified; the old backup/history remain protected. Network errors, uncertain responses and concurrent retries do not authorize duplicates. **UI 0.9.45–0.9.48:** refined settings/first use, local 51-language texts, three collapsible backup panels, checkboxes reflecting selection, bulk actions in 32-item batches, visual/installer updates and RTL action insets on Home/Settings. These UI changes do not change the manual backup policy.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [native UI CI](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40121298442) and [protected alpha distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40120263404) passed on earlier 0.9.48 commits; the final RTL hotfix `08dbced` has a [separate CI run](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40126683024), so consult the [current status](../../RELEASE_STATUS.md). Real Google acceptance remains pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 दस्तऐवजाची भाषा · भाषा निवडा</summary>

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
- **MR - मराठी / Marathi (mr)** — सध्याचे पृष्ठ
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

**Windows मधील स्थानिक स्प्रेडशीट फाइल्स थेट Google Sheets मध्ये उघडा.**

ZagoSheetsWin हे Windows साठी हलके अॅप्लिकेशन आहे, जे स्थानिक स्प्रेडशीट उघडण्याची प्रक्रिया सोपी करते:

**फाइलवर डबल-क्लिक → अपलोड आणि रूपांतर → Google Sheets मध्ये उघडा**

यशस्वी आयातीनंतर ZagoSheetsWin मूळ स्थानिक फाइलच्या जागी Google Sheets दस्तऐवजाकडे नेणारा इंटरनेट शॉर्टकट (`.url`) तयार करू शकते आणि मूळ फाइलचा पुनर्प्राप्त करता येण्याजोगा स्थानिक बॅकअप जतन करते.

उद्दिष्ट सोपे आहे: स्थानिक स्प्रेडशीट उघडताना Google Sheets हे Windows मधील नेहमीच्या अॅपसारखे वाटावे.

## हे काय करते

ZagoSheetsWin Windows मधील स्प्रेडशीट फाइल्सना Google Sheets शी जोडते. समर्थित स्थानिक फाइल उघडल्यावर अॅप पुढील गोष्टी करू शकते:

- स्प्रेडशीट ओळखणे आणि तपासणे;
- पुनर्प्राप्त करता येण्याजोगा बॅकअप तयार करणे;
- Google च्या अधिकृत API वापरून थेट Google Drive वर अपलोड करणे;
- मूळ Google Sheets स्वरूपातील दस्तऐवजात रूपांतर करणे;
- परिणामी स्प्रेडशीट डीफॉल्ट ब्राउझरमध्ये उघडणे;
- Google दस्तऐवजासाठी स्थानिक `.url` शॉर्टकट तयार करणे;
- पुढील वेळी तीच फाइल उघडताना पुन्हा अपलोड टाळणे.

Drive वर स्वतः अपलोड करणे, ब्राउझरमध्ये फाइल शोधणे किंवा वारंवार रूपांतर करणे आवश्यक नाही.

## समर्थित स्वरूपे

सध्याच्या विकासात लक्ष असलेली स्वरूपे: `.xlsx`, `.xls`, `.ods`, `.csv`, `.tsv`.

काही स्वरूपांना अतिरिक्त सुसंगतता मर्यादा असू शकतात. सुरक्षितपणे जतन न होणारी वैशिष्ट्ये असलेल्या फाइल्स काळजीपूर्वक हाताळल्या जातात, जेणेकरून नकळत माहिती नष्ट होऊ नये.

## Windows साठी तयार केलेले

ZagoSheetsWin खास Windows साठी तयार केले आहे. यात **Open with (यासह उघडा)**, फाइल प्रकारांची नोंदणी, डबल-क्लिकने उघडणे, ऐच्छिक File Explorer एकत्रीकरण आणि मूळ Windows इंस्टॉलर यांचा समावेश आहे.

अॅप परवानगीशिवाय Windows चे डीफॉल्ट अॅप बदलत नाही. फाइल संबंधांवर वापरकर्त्याचे नियंत्रण राहते.

## सुरक्षेला प्राधान्य

स्थानिक फाइल बदलण्याची प्रक्रिया पुनर्प्राप्त करता येण्याजोगी असणे आवश्यक आहे. मूळ फाइल तिच्या फोल्डरमधून काढण्यापूर्वी अॅप खात्री करते की:

1. पुनर्प्राप्त करता येणारा बॅकअप उपलब्ध आहे;
2. Google Sheets दस्तऐवज यशस्वीपणे तयार झाला आहे;
3. स्थानिक फाइलचा संबंध कायमस्वरूपी जतन झाला आहे;
4. इंटरनेट शॉर्टकट लिहिला आणि तपासला गेला आहे.

प्रक्रिया अयशस्वी झाल्यास मूळ फाइल सुरक्षित ठेवली जाते.

> वापरकर्त्याची माहिती त्याला न कळवता कधीही नष्ट करू नका.

## बॅकअप

मूळ फाइल्स शॉर्टकटने बदलण्यापूर्वी खाजगी स्थानिक बॅकअप जागेत ठेवता येतात. संचय मर्यादा, जतन कालावधी आणि साफसफाईचे पर्याय कॉन्फिगर करता येतात.

बॅकअप आयातीच्या वेळी असलेली मूळ फाइल सुरक्षित ठेवतो. हे **दोन दिशांचे समक्रमण नाही**: Google Sheets मध्ये नंतर केलेले बदल मूळ स्प्रेडशीट फाइलमध्ये परत लिहिले जात नाहीत.

## गोपनीयता आणि Google प्रवेश

ZagoSheetsWin तुमच्या संगणकावरून थेट Google API शी संवाद साधते.

- स्प्रेडशीटमधील माहिती Zagotools च्या सर्व्हरवर पाठवली जात नाही.
- OAuth टोकन स्थानिकपणे साठवले जातात आणि Windows च्या सुरक्षा यंत्रणेने संरक्षित केले जातात.
- `drive.file` परवानगी व्याप्तीमुळे अॅपद्वारे तयार किंवा उघडलेल्या फाइल्सपुरताच प्रवेश मर्यादित राहतो.
- स्प्रेडशीट आयात करण्यासाठी विश्लेषणात्मक ट्रॅकिंग आवश्यक नाही.

गोपनीयता धोरण आणि वापर अटी: https://zagotools.top/legal.html

## प्रकल्पाची स्थिती

ZagoSheetsWin चा सक्रिय विकास सुरू असून हे सध्या **अल्फा सॉफ्टवेअर** आहे. Windows → Google Sheets हा मुख्य प्रवाह कार्यरत आहे. स्थापना, पुनर्प्राप्ती, स्वरूप सुसंगतता, आंतरराष्ट्रीयीकरण आणि वापरकर्ता अनुभव सुधारले जात आहेत. पहिल्या स्थिर आवृत्तीपूर्वी बदल होऊ शकतात.

## Open in Google शी संबंध

ZagoSheetsWin हे [SwatiK425](https://github.com/SwatiK425) यांच्या [Open in Google](https://github.com/SwatiK425/open-in-google) प्रकल्पावर आधारित विकसित झाले आहे. या मूळ प्रकल्पाने प्रारंभिक तांत्रिक पाया आणि प्रेरणा दिली.

आता ZagoSheetsWin हे स्वतंत्र Windows अॅप आहे. त्याची स्वतःची रचना, इंस्टॉलर, इंटरफेस, बॅकअप आणि पुनर्प्राप्ती प्रणाली, फाइल संबंध, स्वरूप हाताळणी आणि स्थानिक फाइल Google Sheets मध्ये उघडण्याचा अनुभव आहे. मूळ प्रकल्प स्वतंत्र आहे; योग्य सर्वसाधारण सुधारणा upstream ला सुचवता येतात.

## मुक्त स्रोत

ZagoSheetsWin हे विनामूल्य आणि मुक्त-स्रोत सॉफ्टवेअर आहे. Open in Google च्या मूळ कोडचे श्रेय आणि परवाना अटी जतन केल्या जातात; ZagoSheetsWin / Zagotools ची नंतरची कामे स्वतंत्रपणे नमूद केली जातात.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## परवाना

MIT परवाना. तपशीलांसाठी [LICENSE](../../../LICENSE) पाहा.

---

**ZagoSheetsWin — Zagotools प्रकल्प**

खऱ्या समस्यांसाठी छोटे सॉफ्टवेअर.
