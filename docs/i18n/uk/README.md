# ZagoSheetsWin

> ⚠️ **0.9.37 — Important safety update (English notice; full translation review pending).**
>
> Таблицю відкрито в Google Sheets. Початковий файл замінено ярликом. Щоб відновити його, відкрийте Резервні копії.
>
> **Conversion fidelity:** After a confirmed Google import, the original local file **may be replaced with a `.url` shortcut even when formulas, values or formatting differ**. The original bytes remain in Backups. Choose **Import as copy BEFORE opening** if you need the original to stay in the source folder. Windows **read-only** files may block replacement; in that case, the original is preserved. **New in 0.9.36:** CSV/TSV detects comma, semicolon, TAB and pipe; a per-import chooser resolves ambiguity. The bounded Windows-1252 fallback and higher text limits (60,000 logical rows / 600,000 cells) apply to new interpretations. **Backups → More actions → Reimport into a new document** repairs from the verified original backup without overwriting the previous document/shortcut. **New in 0.9.37:** the app, installer, file associations and shortcuts share one spreadsheet icon. Some text below describes an older alpha policy; use the [current English README](../../../README.md) or [Portuguese user guide](../../USER_GUIDE.md) for complete 0.9.37 behavior.


<details>
<summary>🌐 Мова документації · Оберіть мову</summary>

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
- **UK - Українська / Ukrainian (uk)** — поточна сторінка
- [OM - Afaan Oromoo / Oromo (om)](../om/README.md)
- [UZ - Oʻzbekcha / Uzbek (uz)](../uz/README.md)
- [SD - سنڌي / Sindhi (sd)](../sd/README.md)
- [NE - नेपाली / Nepali (ne)](../ne/README.md)
- [IG - Igbo](../ig/README.md)
- [AZ - Azərbaycanca / Azerbaijani (az)](../az/README.md)
- [NL - Nederlands / Dutch (nl)](../nl/README.md)
- [GN - Avañe’ẽ / Guarani (gn)](../gn/README.md)

</details>

**Відкривайте локальні файли електронних таблиць безпосередньо в Google Таблицях із Windows.**

ZagoSheetsWin — легка програма для Windows, яка спрощує відкриття локальних електронних таблиць:

**двічі клацніть файл → завантажте й конвертуйте → відкрийте в Google Таблицях**

Після успішного імпорту ZagoSheetsWin може замінити початковий локальний файл інтернет-ярликом (`.url`), що веде до документа Google Таблиць, зберігаючи локальну резервну копію оригіналу, яку можна відновити.

Мета проста: зробити відкриття локальних таблиць у Google Таблицях таким самим зручним, як у звичайній програмі Windows.

## Що робить програма

ZagoSheetsWin поєднує файли електронних таблиць у Windows із Google Таблицями. Під час відкриття підтримуваного локального файлу програма може:

- розпізнати й перевірити електронну таблицю;
- створити резервну копію, яку можна відновити;
- завантажити файл безпосередньо на Google Диск через офіційні API Google;
- перетворити його на документ у власному форматі Google Таблиць;
- відкрити отриману таблицю в браузері за замовчуванням;
- створити локальний ярлик `.url` на документ Google;
- не завантажувати повторно той самий файл під час наступних відкриттів.

Не потрібно вручну завантажувати файл на Диск, шукати його в браузері або повторювати конвертацію.

## Підтримувані формати

Формати, на які орієнтована поточна розробка: `.xlsx`, `.xls`, `.ods`, `.csv` та `.tsv`.

Для деяких форматів можуть діяти додаткові обмеження сумісності. Файли з функціями, які неможливо безпечно зберегти, обробляються обережно, щоб уникнути непомітної втрати даних.

## Створено для Windows

ZagoSheetsWin розроблено спеціально для Windows. Програма інтегрується з операційною системою через **«Відкрити за допомогою» (Open with)**, реєстрацію типів файлів, відкриття подвійним клацанням, необов’язкову інтеграцію з Провідником і власний інсталятор Windows.

Програма не змінює застосунки за замовчуванням без вашого відома. Користувач зберігає контроль над зіставленнями файлів.

## Безпека за задумом

ZagoSheetsWin розглядає заміну локального файлу як операцію, яку можна відновити. Перш ніж прибрати оригінал із папки, програма перевіряє, що:

1. існує резервна копія, яку можна відновити;
2. документ Google Таблиць успішно створено;
3. локальне зіставлення надійно збережено;
4. інтернет-ярлик успішно записано та перевірено.

Якщо операція завершується невдало, оригінальний файл залишається на місці.

> Ніколи не знищувати дані користувача без його відома.

## Резервні копії

Перед заміною ярликами оригінальні файли можна зберігати у приватному локальному сховищі резервних копій. Передбачено налаштування обмежень обсягу, термінів зберігання та очищення.

Копія захищає оригінальний файл на момент імпорту. Це **не двостороння синхронізація**: подальші зміни в Google Таблицях не записуються назад до початкового файлу.

## Конфіденційність і доступ до Google

ZagoSheetsWin взаємодіє з API Google безпосередньо з вашого комп’ютера.

- Вміст ваших таблиць не надсилається на сервери Zagotools.
- Токени OAuth зберігаються локально та захищаються механізмами безпеки Windows.
- Програма використовує область дозволів Google Диска `drive.file`, яка обмежує доступ файлами, створеними або відкритими через неї.
- Для імпорту таблиць не потрібне аналітичне відстеження.

Політика конфіденційності й умови використання: https://zagotools.top/legal.html

## Стан проєкту

ZagoSheetsWin активно розробляється, тому зараз його слід вважати **програмним забезпеченням на стадії альфа**. Основний процес Windows → Google Таблиці працює. Триває вдосконалення встановлення, відновлення, сумісності форматів, інтернаціоналізації та зручності використання. До першого стабільного випуску можливі зміни.

## Зв’язок з Open in Google

ZagoSheetsWin заснований на [Open in Google](https://github.com/SwatiK425/open-in-google) авторства [SwatiK425](https://github.com/SwatiK425) і є похідним проєктом. Open in Google надав початкову технічну основу й натхнення.

Відтоді ZagoSheetsWin став незалежною програмою Windows із власною архітектурою, інсталятором, інтерфейсом, резервним копіюванням і відновленням, зіставленнями файлів, обробкою форматів та способом відкриття локальних файлів у Google Таблицях. Початковий проєкт залишається незалежним; загальні вдосконалення можуть передаватися до upstream, коли це доречно.

## Відкритий вихідний код

ZagoSheetsWin — безкоштовне програмне забезпечення з відкритим вихідним кодом. Проєкт зберігає авторські повідомлення та ліцензійні вимоги початкового коду Open in Google, чітко позначаючи подальші напрацювання ZagoSheetsWin / Zagotools.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Ліцензія

Ліцензія MIT. Подробиці наведено у файлі [LICENSE](../../../LICENSE).

---

**ZagoSheetsWin — проєкт Zagotools**

Невеликі програми для реальних проблем.
