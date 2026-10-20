# ZagoSheetsWin

> ⚠️ **Alpha 0.9.43 — current safety notice in English; full editorial translation pending.**
>
> Il foglio è stato aperto in Google Sheets. Il file originale è stato sostituito da un collegamento. Per recuperarlo, apri Backup.
>
> **CSV/TSV v4:** up to **2,000,000 cells**, 100,000 logical rows, 1,000 columns and 20 MiB input/payload, subject to other limits. A mismatch in Google-exported cell values **blocks source replacement**. For XLS/ODS, complex content may change even when a verified import replaces the local original with a shortcut.
>
> **Backups in 0.9.43:** **cleanup is MANUAL ONLY** — not automatic by age or quota. When there is not enough backup capacity, import fails promptly and **preserves the original file and existing backups**. Use **Backups → Clean expired** or explicitly remove eligible completed backups to free space. No quota wait, automatic rotation, or global import queue. Different files may be processed in parallel.
>
> **Reliability:** lower-memory upload buffering, bounded remote-marker rechecks without duplicate creation, minimal **Close** screen on low-memory failures, and a shorter fixed-size home window. [Local core](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) and [protected distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) passed on commit `9fbec2a`; real Google acceptance is still pending. This is **not** a 1.0.0 release. Some content below may describe older behavior. Read the [current English README](../../../README.md) and [Portuguese user guide](../../USER_GUIDE.md) for current instructions.

<details>
<summary>🌐 Lingua della documentazione · Scegli una lingua</summary>

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
- **IT - Italiano / Italian (it)** — pagina corrente
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

**Apri i file di fogli di calcolo locali direttamente in Fogli Google da Windows.**

ZagoSheetsWin è una leggera applicazione Windows che semplifica l'apertura dei fogli di calcolo locali:

**doppio clic sul file → caricamento e conversione → apertura in Fogli Google**

Dopo un'importazione riuscita, può sostituire il file locale originale con un collegamento Internet (`.url`) che punta al documento di Fogli Google e conservare una copia di sicurezza locale recuperabile.

L'obiettivo è far funzionare Fogli Google come un'applicazione Windows nativa per aprire i fogli di calcolo locali.

## Cosa fa

ZagoSheetsWin integra i fogli di calcolo di Windows con Fogli Google. Quando apri un file supportato, l'applicazione può:

- rilevare e convalidare il foglio di calcolo;
- creare una copia di sicurezza recuperabile;
- caricare il file direttamente su Google Drive tramite le API ufficiali di Google;
- convertirlo in un documento nativo di Fogli Google;
- aprire il risultato nel browser predefinito;
- creare un collegamento locale `.url` al documento Google;
- evitare un nuovo caricamento dello stesso file alle aperture successive.

Non serve caricare manualmente su Drive, cercare il file nel browser o ripetere le conversioni.

## Formati supportati

I formati previsti dallo sviluppo attuale sono `.xlsx`, `.xls`, `.ods`, `.csv` e `.tsv`.

Alcuni possono avere restrizioni di compatibilità. I file con caratteristiche non conservabili in sicurezza vengono trattati con cautela per evitare perdite di dati non segnalate.

## Progettato per Windows

ZagoSheetsWin è sviluppato appositamente per Windows. Offre **Apri con**, registrazione dei tipi di file, apertura con doppio clic, integrazione facoltativa con Esplora file e programma di installazione nativo.

Non modifica di nascosto le app predefinite di Windows. Il controllo delle associazioni resta all'utente.

## Sicurezza fin dalla progettazione

La sostituzione di un file locale è un'operazione recuperabile. Prima di rimuovere l'originale dalla sua cartella, l'applicazione verifica che:

1. esista una copia di sicurezza recuperabile;
2. il documento di Fogli Google sia stato creato correttamente;
3. l'associazione locale sia stata salvata in modo persistente;
4. il collegamento Internet sia stato scritto e convalidato.

Se l'operazione fallisce, il file originale viene conservato.

> Non distruggere mai i dati dell'utente senza avvisarlo.

## Copie di sicurezza

I file originali possono essere archiviati in un'area privata di backup locale prima di essere sostituiti con collegamenti. Sono disponibili limiti di spazio configurabili, politiche di conservazione e strumenti di pulizia.

I backup proteggono il file originale importato. **Non sono una sincronizzazione bidirezionale**: le modifiche apportate successivamente in Fogli Google non vengono riscritte nel file originale.

## Privacy e accesso a Google

ZagoSheetsWin comunica direttamente dal computer dell'utente con le API Google.

- I contenuti dei fogli di calcolo non vengono inviati ai server Zagotools.
- I token OAuth sono archiviati in locale e protetti dai meccanismi di sicurezza di Windows.
- L'ambito `drive.file` limita l'accesso ai file creati o aperti tramite l'applicazione.
- Per l'importazione non occorrono strumenti di analisi.

Informativa sulla privacy e termini di utilizzo: https://zagotools.top/legal.html

## Stato del progetto

ZagoSheetsWin è in sviluppo attivo ed è attualmente **software in fase alpha**. Il flusso Windows → Fogli Google funziona già; installazione, recupero, compatibilità dei formati, internazionalizzazione ed esperienza utente vengono migliorati. Sono possibili modifiche prima della prima versione stabile.

## Relazione con Open in Google

ZagoSheetsWin deriva da [Open in Google](https://github.com/SwatiK425/open-in-google) di [SwatiK425](https://github.com/SwatiK425), che ha fornito la base e l'ispirazione iniziali.

Da allora è diventato un'applicazione Windows indipendente con architettura, installatore, interfaccia, backup e recupero, associazioni dei file, gestione dei formati ed esperienza di apertura dei fogli locali in Fogli Google propri. Il progetto originale resta indipendente e può ricevere contributi con miglioramenti generici.

## Open source

ZagoSheetsWin è software gratuito e open source. Conserva attribuzioni e condizioni di licenza del codice originale di Open in Google, distinguendo gli sviluppi successivi di ZagoSheetsWin / Zagotools.

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## Licenza

Licenza MIT. Consulta [LICENSE](../../../LICENSE) per i dettagli.

---

**ZagoSheetsWin — un progetto Zagotools**

Piccoli software per problemi reali.
