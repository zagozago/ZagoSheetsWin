# ZagoSheetsWin

> ⚠️ **Alpha 0.9.43 — current safety notice in English; full editorial translation pending.**
>
> Arkusz został otwarty w Google Sheets. Oryginalny plik zastąpiono skrótem. Aby go odzyskać, otwórz Kopie zapasowe.
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.43:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [Local core](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) and [protected distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) passed on commit `9fbec2a`; real Google acceptance is still pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 Język dokumentacji · Wybierz język</summary>

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
- **PL - Polski / Polish (pl)** — bieżąca strona
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

**Otwieraj lokalne arkusze kalkulacyjne bezpośrednio w Arkuszach Google z poziomu Windows.**

ZagoSheetsWin to lekka aplikacja dla Windows, która upraszcza otwieranie lokalnych arkuszy:

**dwukrotne kliknięcie pliku → przesłanie i konwersja → otwarcie w Arkuszach Google**

Po udanym imporcie ZagoSheetsWin może zastąpić oryginalny plik lokalny skrótem internetowym (`.url`) prowadzącym do dokumentu w Arkuszach Google. Jednocześnie przechowuje możliwą do odzyskania lokalną kopię zapasową pliku źródłowego.

Cel jest prosty: sprawić, by Arkusze Google otwierały lokalne pliki tak wygodnie jak natywna aplikacja Windows.

## Co robi aplikacja

ZagoSheetsWin integruje arkusze kalkulacyjne Windows z Arkuszami Google. Przy otwieraniu obsługiwanego pliku lokalnego aplikacja może:

- rozpoznać i sprawdzić arkusz;
- utworzyć kopię zapasową możliwą do odzyskania;
- przesłać plik bezpośrednio na Dysk Google za pomocą oficjalnych interfejsów API;
- przekonwertować go na natywny dokument Arkuszy Google;
- otworzyć wynik w domyślnej przeglądarce;
- utworzyć lokalny skrót `.url` do dokumentu Google;
- uniknąć ponownego przesyłania tego samego pliku przy kolejnych otwarciach.

Nie trzeba ręcznie przesyłać plików na Dysk, wyszukiwać ich w przeglądarce ani wielokrotnie konwertować.

## Obsługiwane formaty

Formaty uwzględnione w obecnym planie rozwoju: `.xlsx`, `.xls`, `.ods`, `.csv`, `.tsv`.

Niektóre formaty mogą mieć dodatkowe ograniczenia zgodności. Pliki zawierające funkcje, których nie można bezpiecznie zachować, są obsługiwane ostrożnie, aby nie dopuścić do niezauważonej utraty danych.

## Zaprojektowano dla Windows

ZagoSheetsWin powstał specjalnie dla Windows. Integruje się z systemem poprzez **Otwórz za pomocą**, rejestrację typów plików, otwieranie dwukrotnym kliknięciem, opcjonalną integrację z Eksploratorem plików i natywny instalator Windows.

Aplikacja nie zmienia potajemnie programów domyślnych. Użytkownik zachowuje kontrolę nad skojarzeniami plików.

## Bezpieczeństwo od początku

Zastępowanie pliku lokalnego jest operacją, którą można odwrócić. Przed usunięciem oryginału z folderu aplikacja sprawdza, czy:

1. istnieje kopia zapasowa możliwa do odzyskania;
2. dokument Arkuszy Google został poprawnie utworzony;
3. lokalne skojarzenie zostało trwale zapisane;
4. skrót internetowy został zapisany i zweryfikowany.

Jeśli operacja się nie powiedzie, oryginalny plik pozostaje na miejscu.

> Nigdy nie niszcz danych użytkownika bez jego wiedzy.

## Kopie zapasowe

Oryginalne pliki można przechowywać w prywatnej lokalnej przestrzeni kopii zapasowych przed zastąpieniem skrótami. Dostępne są konfigurowalne limity miejsca, zasady przechowywania i opcje czyszczenia.

Kopie chronią plik źródłowy w chwili importu. **Nie jest to synchronizacja dwukierunkowa**: zmiany wprowadzone później w Arkuszach Google nie są zapisywane w oryginalnym pliku.

## Prywatność i dostęp do Google

ZagoSheetsWin komunikuje się z interfejsami API Google bezpośrednio z komputera użytkownika.

- Zawartość arkuszy nie jest wysyłana na serwery Zagotools.
- Tokeny OAuth są przechowywane lokalnie i zabezpieczane mechanizmami bezpieczeństwa Windows.
- Zakres uprawnień `drive.file` ogranicza dostęp do plików utworzonych lub otwartych przez aplikację.
- Import arkuszy nie wymaga śledzenia analitycznego.

Polityka prywatności i warunki użytkowania: https://zagotools.top/legal.html

## Stan projektu

ZagoSheetsWin jest aktywnie rozwijany i obecnie należy go traktować jako **oprogramowanie w wersji alfa**. Główny proces Windows → Arkusze Google działa. Trwają ulepszenia instalacji, odzyskiwania, zgodności formatów, internacjonalizacji i wygody użytkowania. Do pierwszego stabilnego wydania mogą występować zmiany.

## Związek z Open in Google

ZagoSheetsWin bazuje na projekcie [Open in Google](https://github.com/SwatiK425/open-in-google) autorstwa [SwatiK425](https://github.com/SwatiK425) i jest jego rozwinięciem. Oryginalny projekt dostarczył podstaw technicznych i inspiracji.

ZagoSheetsWin stał się niezależną aplikacją Windows z własną architekturą, instalatorem, interfejsem, systemem kopii zapasowych i odzyskiwania, obsługą skojarzeń i formatów plików oraz sposobem otwierania lokalnych arkuszy w Arkuszach Google. Projekt źródłowy pozostaje niezależny, a ogólne usprawnienia można proponować projektowi upstream, gdy będzie to stosowne.

## Otwarte oprogramowanie

ZagoSheetsWin to bezpłatne oprogramowanie open source. Zachowuje informacje o autorach i warunki licencji pierwotnego kodu Open in Google, a także wyraźnie rozróżnia późniejsze prace ZagoSheetsWin / Zagotools.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Licencja

Licencja MIT. Szczegółowe informacje znajdują się w [LICENSE](../../../LICENSE).

---

**ZagoSheetsWin — projekt Zagotools**

Małe oprogramowanie do rzeczywistych problemów.
