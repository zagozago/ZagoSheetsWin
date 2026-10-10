# ZagoSheetsWin

<details>
<summary>🌐 Lingua della documentazione · Scegli una lingua</summary>

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
- **Italiano** — pagina corrente
- [ไทย](../th/README.md)
- [Filipino](../fil/README.md)
- [Bahasa Melayu](../ms/README.md)
- [Kiswahili](../sw/README.md)
- [Nigerian Pidgin](../pcm/README.md)

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
