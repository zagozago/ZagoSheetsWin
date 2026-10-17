# ZagoSheetsWin

> ⚠️ **0.9.35 — Important safety update (English notice; full translation review pending).**
>
> স্প্রেডশিটটি Google Sheets-এ খোলা হয়েছে। মূল ফাইলের বদলে একটি শর্টকাট তৈরি হয়েছে। মূল ফাইল ফিরিয়ে পেতে ব্যাকআপ(B) খুলুন।
>
> **Conversion fidelity:** After a confirmed Google import, the original local file **may be replaced with a `.url` shortcut even when formulas, values or formatting differ**. The original bytes remain in Backups. Choose **Import as copy BEFORE opening** if you need the original to stay in the source folder. Windows **read-only** files may block replacement; in that case, the original is preserved. Some text below describes an older alpha policy; use the [current English README](../../../README.md) or [Portuguese user guide](../../USER_GUIDE.md) for complete 0.9.35 behavior.


<details>
<summary>🌐 নথির ভাষা নির্বাচন করুন</summary>

- [EN - English](../../../README.md)
- [ZH - 中文 / Chinese (Simplified) (zh)](../zh/README.md)
- [HI - हिन्दी / Hindi (hi)](../hi/README.md)
- [ES - Español / Spanish (es)](../es/README.md)
- [AR - العربية / Arabic (ar)](../ar/README.md)
- [FR - Français / French (fr)](../fr/README.md)
- **BN - বাংলা / Bengali (bn)** — বর্তমান পৃষ্ঠা
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
- [GN - Avañe’ẽ / Guarani (gn)](../gn/README.md)

</details>

**Windows থেকে স্থানীয় স্প্রেডশিট ফাইল সরাসরি Google Sheets-এ খুলুন।**

ZagoSheetsWin হলো Windows-এর জন্য একটি হালকা অ্যাপ, যা স্থানীয় স্প্রেডশিট খোলার প্রক্রিয়াকে সহজ করে:

**ফাইলে ডাবল-ক্লিক → আপলোড ও রূপান্তর → Google Sheets-এ খুলুন**

সফলভাবে আমদানির পর ZagoSheetsWin মূল স্থানীয় ফাইলের পরিবর্তে Google Sheets নথির লিংকযুক্ত একটি ইন্টারনেট শর্টকাট (`.url`) রাখতে পারে। একই সঙ্গে মূল ফাইলের একটি পুনরুদ্ধারযোগ্য স্থানীয় ব্যাকআপ সংরক্ষিত থাকে।

লক্ষ্য সহজ: স্থানীয় স্প্রেডশিট খোলার ক্ষেত্রে Google Sheets-কে Windows-এর একটি স্বাভাবিক অ্যাপের মতো ব্যবহারযোগ্য করে তোলা।

## এটি কী করে

ZagoSheetsWin Windows-এর স্প্রেডশিট ফাইলকে Google Sheets-এর সঙ্গে যুক্ত করে।

সমর্থিত স্থানীয় ফাইল খুললে অ্যাপটি:

- স্প্রেডশিট শনাক্ত ও যাচাই করতে পারে;
- পুনরুদ্ধারযোগ্য ব্যাকআপ তৈরি করতে পারে;
- Google-এর সরকারি API ব্যবহার করে সরাসরি Google Drive-এ আপলোড করতে পারে;
- ফাইলটিকে Google Sheets-এর নিজস্ব নথিতে রূপান্তর করতে পারে;
- তৈরি হওয়া স্প্রেডশিট ডিফল্ট ব্রাউজারে খুলতে পারে;
- Google নথির জন্য একটি স্থানীয় `.url` শর্টকাট তৈরি করতে পারে;
- পরে একই ফাইল আবার খোলার সময় পুনরায় আপলোড এড়াতে পারে।

Drive-এ হাতে আপলোড নয়। ব্রাউজারে নথি খুঁজে বেড়ানো নয়। বারবার রূপান্তর নয়।

## সমর্থিত ফরম্যাট

বর্তমান উন্নয়নের লক্ষ্য:

- `.xlsx`
- `.xls`
- `.ods`
- `.csv`
- `.tsv`

কিছু ফরম্যাটে অতিরিক্ত সামঞ্জস্য-সংক্রান্ত সীমাবদ্ধতা থাকতে পারে। যেসব বৈশিষ্ট্য নিরাপদে সংরক্ষণ করা সম্ভব নয়, সেসব ফাইল সতর্কভাবে পরিচালনা করা হয়, যাতে অজান্তে তথ্য হারিয়ে না যায়।

## Windows-এর জন্য তৈরি

ZagoSheetsWin বিশেষভাবে Windows-এর জন্য তৈরি এবং অপারেটিং সিস্টেমের সঙ্গে যুক্ত হয়:

- **Open with (অন্য অ্যাপ দিয়ে খুলুন)**
- ফাইলের ধরন নিবন্ধন
- ডাবল-ক্লিক করে ফাইল খোলা
- ঐচ্ছিক File Explorer সমন্বয়
- Windows-এর নিজস্ব ইনস্টলার

অ্যাপটি নীরবে Windows-এর ডিফল্ট অ্যাপ পরিবর্তন করে না। কোন ধরনের ফাইল কোন অ্যাপে খুলবে, সেই নিয়ন্ত্রণ ব্যবহারকারীর কাছেই থাকে।

## নকশাতেই নিরাপত্তা

ZagoSheetsWin স্থানীয় ফাইল প্রতিস্থাপনকে একটি পুনরুদ্ধারযোগ্য প্রক্রিয়া হিসেবে বিবেচনা করে।

মূল ফাইলটি তার ফোল্ডার থেকে সরানোর আগে অ্যাপটি যাচাই করে:

1. পুনরুদ্ধারযোগ্য ব্যাকআপ আছে;
2. Google Sheets নথি সফলভাবে তৈরি হয়েছে;
3. স্থানীয় ফাইলের সংযোগ স্থায়ীভাবে সংরক্ষিত হয়েছে;
4. ইন্টারনেট শর্টকাট সফলভাবে তৈরি ও যাচাই হয়েছে।

প্রক্রিয়া ব্যর্থ হলে মূল ফাইল অক্ষত রাখা হয়।

অ্যাপটির মূল নীতি:

> ব্যবহারকারীকে না জানিয়ে কখনোই তার তথ্য নষ্ট করা যাবে না।

## ব্যাকআপ

মূল ফাইলকে শর্টকাট দিয়ে প্রতিস্থাপনের আগে একটি ব্যক্তিগত স্থানীয় ব্যাকআপ স্থানে রাখা যেতে পারে।

ব্যাকআপ ব্যবস্থাপনায় সংরক্ষণের সীমা, কত দিন রাখা হবে তার নীতি এবং পরিষ্কারের নিয়ন্ত্রণ নির্ধারণ করা যায়।

ব্যাকআপ আমদানি করা মূল ফাইল সুরক্ষিত রাখে। এটি **দ্বিমুখী সিঙ্ক্রোনাইজেশন নয়**: পরে Google Sheets-এ করা পরিবর্তন মূল স্প্রেডশিট ফাইলে ফিরে লেখা হয় না।

## গোপনীয়তা ও Google-এ প্রবেশাধিকার

ZagoSheetsWin আপনার কম্পিউটার থেকে সরাসরি Google API-এর সঙ্গে যোগাযোগ করে।

- আপনার স্প্রেডশিটের বিষয়বস্তু Zagotools-এর কোনো সার্ভারে পাঠানো হয় না।
- OAuth টোকেন স্থানীয়ভাবে রাখা হয় এবং Windows-এর নিরাপত্তা ব্যবস্থা দিয়ে সুরক্ষিত থাকে।
- অ্যাপটি Google Drive-এর `drive.file` অনুমতি ব্যবহার করে, যা অ্যাপের মাধ্যমে তৈরি বা খোলা ফাইলেই প্রবেশাধিকার সীমিত রাখে।
- স্প্রেডশিট আমদানির প্রক্রিয়ায় বিশ্লেষণভিত্তিক ট্র্যাকিংয়ের প্রয়োজন নেই।

গোপনীয়তা নীতি ও ব্যবহারের শর্ত:

https://zagotools.top/legal.html

## প্রকল্পের অবস্থা

ZagoSheetsWin সক্রিয়ভাবে উন্নয়নাধীন এবং বর্তমানে এটিকে **আলফা পর্যায়ের সফটওয়্যার** হিসেবে বিবেচনা করা উচিত।

Windows → Google Sheets-এর মূল প্রক্রিয়া কার্যকর। ইনস্টলেশন, পুনরুদ্ধার, ফরম্যাটের সামঞ্জস্য, আন্তর্জাতিকীকরণ এবং ব্যবহারকারীর অভিজ্ঞতা উন্নত করা হচ্ছে।

প্রথম স্থিতিশীল সংস্করণ প্রকাশের আগে পরিবর্তন হতে পারে।

## Open in Google-এর সঙ্গে সম্পর্ক

ZagoSheetsWin [SwatiK425](https://github.com/SwatiK425)-এর [Open in Google](https://github.com/SwatiK425/open-in-google) প্রকল্পের ওপর ভিত্তি করে তৈরি ও বিকশিত।

Open in Google এই প্রকল্পের প্রাথমিক ভিত্তি ও অনুপ্রেরণা জুগিয়েছে।

এরপর ZagoSheetsWin নিজস্ব স্থাপত্য, ইনস্টলার, ইন্টারফেস, ব্যাকআপ ও পুনরুদ্ধার ব্যবস্থা, ফাইল সংযোগের প্রক্রিয়া, ফরম্যাট ব্যবস্থাপনা এবং স্থানীয় ফাইল থেকে Google Sheets-এ কাজ করার অভিজ্ঞতাসহ একটি স্বাধীন Windows অ্যাপে পরিণত হয়েছে।

মূল প্রকল্পটি স্বতন্ত্র রয়েছে। উপযুক্ত সাধারণ উন্নতি মূল প্রকল্পে অবদান হিসেবে পাঠানো যেতে পারে, আর ZagoSheetsWin স্বাধীনভাবে বিকশিত হতে থাকবে।

## মুক্ত উৎস

ZagoSheetsWin বিনামূল্যের মুক্ত উৎস সফটওয়্যার।

মূল Open in Google কোডের স্বীকৃতি ও লাইসেন্সের শর্ত বজায় রেখে ZagoSheetsWin / Zagotools-এর পরবর্তী উন্নয়নকেও স্পষ্টভাবে চিহ্নিত করা হয়।

দেখুন:

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## লাইসেন্স

MIT লাইসেন্স।

বিস্তারিত জানতে [LICENSE](../../../LICENSE) দেখুন।

---

**ZagoSheetsWin — Zagotools-এর একটি প্রকল্প**

বাস্তব সমস্যার জন্য ছোট সফটওয়্যার।
