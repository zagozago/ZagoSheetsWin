# ZagoSheetsWin

> ⚠️ **Alpha 0.9.43 — current safety notice in English; full editorial translation pending.**
>
> Таблица открыта в Google Sheets. Исходный файл заменён ярлыком. Чтобы восстановить его, откройте Резервные копии.
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.43:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [Local core](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) and [protected distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) passed on commit `9fbec2a`; real Google acceptance is still pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 Язык документации · Выберите язык</summary>

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
- **RU - Русский / Russian (ru)** — текущая страница
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

**Открывайте локальные файлы электронных таблиц прямо в Google Таблицах из Windows.**

ZagoSheetsWin — лёгкое приложение для Windows, которое упрощает открытие локальных электронных таблиц:

**двойной щелчок по файлу → загрузка и преобразование → открытие в Google Таблицах**

После успешного импорта ZagoSheetsWin может заменить исходный локальный файл интернет-ярлыком (`.url`) со ссылкой на документ Google Таблиц, сохранив восстанавливаемую локальную резервную копию оригинала.

Цель проста: сделать открытие локальных таблиц в Google Таблицах таким же привычным, как в обычном приложении Windows.

## Возможности

ZagoSheetsWin связывает файлы электронных таблиц в Windows с Google Таблицами.

При открытии поддерживаемого локального файла приложение может:

- определить и проверить электронную таблицу;
- создать резервную копию с возможностью восстановления;
- загрузить файл напрямую в Google Диск через официальные API Google;
- преобразовать его в документ Google Таблиц;
- открыть полученную таблицу в браузере по умолчанию;
- создать локальный ярлык `.url` на документ Google;
- избежать повторной загрузки того же файла при последующих открытиях.

Без ручной загрузки на Диск. Без поиска нужного документа в браузере. Без повторного преобразования.

## Поддерживаемые форматы

Форматы, на которые ориентирована текущая разработка:

- `.xlsx`
- `.xls`
- `.ods`
- `.csv`
- `.tsv`

Для некоторых форматов действуют дополнительные ограничения совместимости. Файлы с возможностями, которые нельзя безопасно сохранить, обрабатываются осторожно, чтобы не допустить незаметной потери данных.

## Создано для Windows

ZagoSheetsWin разрабатывается специально для Windows и интегрируется с операционной системой через:

- **Открыть с помощью**
- регистрацию типов файлов
- открытие двойным щелчком
- необязательную интеграцию с Проводником
- собственный установщик для Windows

Приложение не меняет программы по умолчанию без ведома пользователя. Настройка сопоставлений файлов остаётся под вашим контролем.

## Безопасность заложена в основу

ZagoSheetsWin рассматривает замену локального файла как операцию с возможностью восстановления.

Прежде чем удалить исходный файл из его папки, приложение проверяет, что:

1. существует восстанавливаемая резервная копия;
2. документ Google Таблиц успешно создан;
3. локальное сопоставление надёжно сохранено;
4. интернет-ярлык успешно записан и проверен.

Если операция завершается с ошибкой, исходный файл сохраняется.

Приложение следует простому правилу:

> Никогда не уничтожать данные пользователя без предупреждения.

## Резервные копии

Исходные файлы могут сохраняться в закрытом локальном хранилище резервных копий до замены ярлыками.

Управление резервными копиями предусматривает настраиваемые ограничения объёма, сроки хранения и средства очистки.

Резервные копии защищают исходный импортированный файл. Это **не двусторонняя синхронизация**: изменения, внесённые позднее в Google Таблицах, не записываются обратно в оригинальный файл.

## Конфиденциальность и доступ к Google

ZagoSheetsWin взаимодействует с API Google напрямую с вашего компьютера.

- Содержимое ваших таблиц не отправляется на серверы Zagotools.
- Токены OAuth хранятся локально и защищены механизмами безопасности Windows.
- Приложение использует область разрешений Google Диска `drive.file`, ограничивающую доступ файлами, созданными или открытыми через приложение.
- Для импорта таблиц аналитика не требуется.

Политика конфиденциальности и условия использования:

https://zagotools.top/legal.html

## Статус проекта

ZagoSheetsWin активно разрабатывается и пока должен считаться **альфа-версией программного обеспечения**.

Основной сценарий Windows → Google Таблицы работает. Продолжается улучшение установки, восстановления, совместимости форматов, локализации и удобства использования.

До первого стабильного выпуска возможны изменения.

## Связь с Open in Google

ZagoSheetsWin основан на проекте [Open in Google](https://github.com/SwatiK425/open-in-google) автора [SwatiK425](https://github.com/SwatiK425) и является его производной версией.

Open in Google стал первоначальной технической основой и источником вдохновения.

С тех пор ZagoSheetsWin превратился в независимое приложение Windows с собственной архитектурой, установщиком, интерфейсом, системой резервного копирования и восстановления, механизмом сопоставления файлов, обработкой форматов и способом открытия локальных файлов в Google Таблицах.

Исходный проект продолжает развиваться независимо. При необходимости общие улучшения могут быть предложены основному проекту, а ZagoSheetsWin продолжит самостоятельное развитие.

## Открытый исходный код

ZagoSheetsWin — бесплатное программное обеспечение с открытым исходным кодом.

Проект сохраняет сведения об авторстве и лицензионные требования исходного кода Open in Google и отдельно указывает последующую разработку ZagoSheetsWin / Zagotools.

См. также:

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Лицензия

Лицензия MIT.

Подробнее см. [LICENSE](../../../LICENSE).

---

**ZagoSheetsWin — проект Zagotools**

Небольшие программы для реальных задач.
