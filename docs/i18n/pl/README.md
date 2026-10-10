# ZagoSheetsWin

<details>
<summary>🌐 Język dokumentacji · Wybierz język</summary>

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
- **Polski** — bieżąca strona
- [Basa Sunda](../su/README.md)
- [मैथिली](../mai/README.md)
- [Українська](../uk/README.md)
- [Afaan Oromoo](../om/README.md)
- [Oʻzbekcha](../uz/README.md)
- [سنڌي](../sd/README.md)
- [नेपाली](../ne/README.md)
- [Asụsụ Igbo](../ig/README.md)
- [Azərbaycan dili](../az/README.md)
- [Nederlands](../nl/README.md)
- [Avañe’ẽ](../gn/README.md)

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
