# ZagoSheetsWin

> ⚠️ **Alpha 0.9.48 — current safety notice in English; full editorial translation pending.**
>
> स्प्रेडसिट Google Sheets मा खोलिएको छ। मूल फाइलको ठाउँमा सर्टकट बनाइएको छ। मूल फाइल फिर्ता पाउन जगेडा खोल्नुहोस्।
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.48:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Recovery 0.9.44:** a later **independent** opening may start a fresh import only if earlier pending Google document absence is verified; the old backup/history remain protected. Network errors, uncertain responses and concurrent retries do not authorize duplicates. **UI 0.9.45–0.9.48:** refined settings/first use, local 51-language texts, three collapsible backup panels, checkboxes reflecting selection, bulk actions in 32-item batches, visual/installer updates and RTL action insets on Home/Settings. These UI changes do not change the manual backup policy.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [native UI CI](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40121298442) and [protected alpha distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40120263404) passed on earlier 0.9.48 commits; the final RTL hotfix `08dbced` has a [separate CI run](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40126683024), so consult the [current status](../../RELEASE_STATUS.md). Real Google acceptance remains pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 कागजातको भाषा · भाषा छान्नुहोस्</summary>

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
- **NE - नेपाली / Nepali (ne)** — हालको पृष्ठ
- [IG - Igbo](../ig/README.md)
- [AZ - Azərbaycanca / Azerbaijani (az)](../az/README.md)
- [NL - Nederlands / Dutch (nl)](../nl/README.md)
- [GN - Avañe’ẽ / Guarani (gn)](../gn/README.md)

</details>

**Windows बाट आफ्ना स्थानीय स्प्रेडसिट फाइलहरू सिधै Google Sheets मा खोल्नुहोस्।**

ZagoSheetsWin Windows का लागि हलुका अनुप्रयोग हो, जसले स्थानीय स्प्रेडसिट खोल्ने प्रक्रियालाई सरल बनाउँछ:

**फाइलमा डबल-क्लिक → अपलोड र रूपान्तरण → Google Sheets मा खोल्नुहोस्**

सफल आयातपछि ZagoSheetsWin ले मूल स्थानीय फाइलको सट्टा Google Sheets कागजातमा लैजाने इन्टरनेट सर्टकट (`.url`) राख्न सक्छ। साथै, मूल फाइलको पुनःप्राप्त गर्न सकिने स्थानीय ब्याकअप सुरक्षित राखिन्छ।

उद्देश्य सरल छ: स्थानीय स्प्रेडसिट खोल्दा Google Sheets लाई Windows को सामान्य अनुप्रयोगजस्तै प्रयोग गर्न सकियोस्।

## यसले के गर्छ

ZagoSheetsWin ले Windows का स्प्रेडसिट फाइल र Google Sheets लाई जोड्छ। समर्थित स्थानीय फाइल खोल्दा अनुप्रयोगले:

- स्प्रेडसिट पहिचान र प्रमाणीकरण गर्न सक्छ;
- पुनःप्राप्त गर्न सकिने ब्याकअप बनाउन सक्छ;
- Google का आधिकारिक API प्रयोग गरी सिधै Google Drive मा अपलोड गर्न सक्छ;
- फाइललाई मूल Google Sheets कागजातमा रूपान्तरण गर्न सक्छ;
- नतिजा पूर्वनिर्धारित ब्राउजरमा खोल्न सक्छ;
- Google कागजातका लागि स्थानीय `.url` सर्टकट बनाउन सक्छ;
- पछि त्यही फाइल खोल्दा पुनः अपलोड हुनबाट बचाउन सक्छ।

Drive मा आफैँ अपलोड गर्ने, ब्राउजरमा फाइल खोज्ने वा बारम्बार रूपान्तरण गर्ने आवश्यकता पर्दैन।

## समर्थित ढाँचाहरू

हाल विकासको लक्ष्यमा रहेका ढाँचाहरू: `.xlsx`, `.xls`, `.ods`, `.csv` र `.tsv`।

केही ढाँचामा थप अनुकूलता सीमाहरू हुन सक्छन्। सुरक्षित रूपमा जोगाउन नसकिने विशेषता भएका फाइलहरूलाई जानकारी नदिई डेटा हराउने जोखिम कम गर्न सावधानीपूर्वक व्यवस्थापन गरिन्छ।

## Windows का लागि बनाइएको

ZagoSheetsWin विशेष रूपमा Windows का लागि बनाइएको हो। यसमा **Open with (यससँग खोल्नुहोस्)**, फाइल प्रकार दर्ता, डबल-क्लिकद्वारा खोल्ने सुविधा, वैकल्पिक File Explorer एकीकरण र Windows को आफ्नै इन्स्टलर समावेश छन्।

अनुप्रयोगले जानकारी नदिई Windows का पूर्वनिर्धारित अनुप्रयोगहरू परिवर्तन गर्दैन। फाइल सम्बन्धहरूको नियन्त्रण प्रयोगकर्ताकै हातमा रहन्छ।

## डिजाइनदेखि नै सुरक्षा

स्थानीय फाइल बदल्ने कार्य पुनःप्राप्त गर्न सकिने हुनुपर्छ। मूल फाइललाई फोल्डरबाट हटाउनुअघि अनुप्रयोगले निम्न कुराहरू जाँच गर्छ:

1. पुनःप्राप्त गर्न सकिने ब्याकअप उपलब्ध छ;
2. Google Sheets कागजात सफलतापूर्वक बनेको छ;
3. स्थानीय फाइल सम्बन्ध स्थायी रूपमा सुरक्षित गरिएको छ;
4. इन्टरनेट सर्टकट लेखेर जाँच गरिएको छ।

प्रक्रिया असफल भएमा मूल फाइल सुरक्षित रहन्छ।

> प्रयोगकर्तालाई जानकारी नदिई उनको डेटा कहिल्यै नष्ट नगर्नुहोस्।

## ब्याकअपहरू

मूल फाइलहरू सर्टकटले बदल्नुअघि निजी स्थानीय ब्याकअप स्थानमा राख्न सकिन्छ। भण्डारण सीमा, राख्ने अवधि र सफा गर्ने विकल्पहरू मिलाउन सकिन्छ।

ब्याकअपले आयात गरिँदाको मूल फाइललाई जोगाउँछ। यो **दुईतर्फी समक्रमण होइन**: Google Sheets मा पछि गरिएका परिवर्तनहरू मूल स्प्रेडसिट फाइलमा फिर्ता लेखिँदैनन्।

## गोपनीयता र Google पहुँच

ZagoSheetsWin ले तपाईंको कम्प्युटरबाट सिधै Google API सँग सम्पर्क गर्छ।

- स्प्रेडसिटको सामग्री Zagotools सर्भरमा पठाइँदैन।
- OAuth टोकन स्थानीय रूपमा भण्डारण गरिन्छन् र Windows सुरक्षा संयन्त्रबाट सुरक्षित रहन्छन्।
- `drive.file` अनुमति दायराले अनुप्रयोगमार्फत सिर्जना वा खोलिएका फाइलहरूमा मात्र पहुँच सीमित गर्छ।
- स्प्रेडसिट आयात गर्न प्रयोग विश्लेषण ट्र्याकिङ आवश्यक पर्दैन।

गोपनीयता नीति र प्रयोगका सर्तहरू: https://zagotools.top/legal.html

## परियोजनाको अवस्था

ZagoSheetsWin सक्रिय रूपमा विकसित हुँदैछ र हाल यसलाई **अल्फा सफ्टवेयर** मान्नुपर्छ। Windows → Google Sheets को मुख्य प्रक्रिया काम गरिरहेको छ। स्थापना, पुनःप्राप्ति, ढाँचा अनुकूलता, अन्तर्राष्ट्रियकरण र प्रयोगकर्ता अनुभव सुधार भइरहेका छन्। पहिलो स्थिर संस्करणअघि परिवर्तन हुन सक्छन्।

## Open in Google सँगको सम्बन्ध

ZagoSheetsWin, [SwatiK425](https://github.com/SwatiK425) को [Open in Google](https://github.com/SwatiK425/open-in-google) परियोजनामा आधारित र त्यसैबाट विकसित गरिएको हो। मूल परियोजनाले सुरुवाती आधार र प्रेरणा दिएको हो।

अहिले ZagoSheetsWin आफ्नै संरचना, इन्स्टलर, इन्टरफेस, ब्याकअप र पुनःप्राप्ति, फाइल सम्बन्ध, ढाँचा व्यवस्थापन र स्थानीय फाइल Google Sheets मा खोल्ने अनुभव भएको स्वतन्त्र Windows अनुप्रयोग बनेको छ। मूल परियोजना स्वतन्त्र रहन्छ; उपयुक्त सामान्य सुधारहरू upstream मा प्रस्ताव गर्न सकिन्छ।

## खुला स्रोत

ZagoSheetsWin निःशुल्क र खुला स्रोत सफ्टवेयर हो। यसले मूल Open in Google कोडको श्रेय र इजाजतपत्रका सर्तहरू जोगाउँछ, साथै ZagoSheetsWin / Zagotools को पछिल्लो विकासलाई स्पष्ट छुट्याउँछ।

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## इजाजतपत्र

MIT इजाजतपत्र। विवरणका लागि [LICENSE](../../../LICENSE) हेर्नुहोस्।

---

**ZagoSheetsWin — Zagotools परियोजना**

वास्तविक समस्याका लागि सानो सफ्टवेयर।
