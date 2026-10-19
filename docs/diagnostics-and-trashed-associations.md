# Diagnostics, trashed associations and safe repair (current through 0.9.42)

**Updated 18 October 2026.** For the alpha 0.9.36 implementation evidence see [IMPORT_REVIEW_0936.md](IMPORT_REVIEW_0936.md). The older trashed-association behavior described below remains relevant; new interpretation/revalidation rules must also be considered.

## Opening a local spreadsheet

Opening an existing CSV, TSV, XLSX, XLS or ODS checks its association by account, local file identity and remote ID, never by name. If the bound Google document is confirmed trashed and its association markers match, the old local operation is retired from the active source binding. Its backup, remote mapping and replacement journal remain available as history. A new operation captures the current local bytes and creates a new Google document with a fresh creation marker. Nothing restores, updates or permanently deletes the trashed document.

This check runs before the old snapshot comparison, so an edited local file can be imported as the new document. The new operation retains the normal backup, conversion, validation and shortcut safeguards. Network failures, missing files (HTTP 404), denied access and invalid association markers do not authorize a new upload. Source locks serialize simultaneous openings. A lost upload response is reconciled through the new operation's marker instead of initiating another upload.

A confirmed trashed application folder is similarly retired and replaced by a new folder in the account root. The old folder and its documents remain untouched. Each folder generation and its events are retained in the remote registry.

Direct Internet shortcuts continue to open their recorded URL. Explicit recovery of an old operation does not silently create a different operation.

## Revalidating corrected local data (0.9.36)

Before creating a new import, the app examines whether the previous local source operation has already registered **remote intent**. If the file was edited after a permanent parsing/capacity failure and **no remote intent exists**, the system can retire the unsent association and start a fresh local operation, preserving previous snapshots. If a remote intent or uncertain upload exists, it must be reconciled; the parser cannot silently start another remote creation.

CSV/TSV now keep **versioned parsing options per operation**. Old remote intents remain on version 1; new imports use version 2, including comma/semicolon/TAB/pipe and controlled UTF-8/Windows-1252 behavior. To fix a previously created but incorrectly parsed Google document, the user must explicitly choose **Backups → More actions → Reimport into a new document**. This uses the verified original snapshot and a fresh identity, leaves the old Google document and shortcut untouched, and completes as a **copy**.

## Exporting a diagnostic

**There are two export contexts in alpha 0.9.36 and later:** **Export diagnostics** from the failure window exports JSONL records correlated to the **current attempt ID** (including the current terminal failure, even if normal history logging was unavailable). **Backups → More actions → Export history** exports the available rotated historical JSONL logs to a distinct user-selected file, suggested as `ZSW-history.jsonl`. These contexts must not be described as one common button. Both use bounded, typed entries and safe file handling. Each completed or failed UI attempt includes a bounded `Trace`: step name, outcome, elapsed milliseconds and, where applicable, boolean remote validation checks. It covers preflight, authorization, snapshot preparation, conversion, remote creation/reconciliation/validation, shortcut publication, browser launch and source retirement. Repeated preflight steps may identify **transient-only automatic retries** within an attempt. Persistent text/format/capacity errors are not retried automatically; retry after input correction is deliberately gated by the changed file. Ambiguous delimiters use an explicit per-import chooser.

Each entry includes the product version and, when supplied by the SDK build, its Git revision. Existing failure categories, HRESULT, Google HTTP status and SQLite error code remain available. No exception messages, stack traces, source paths, spreadsheet cells, account identifiers, Google file IDs, OAuth configuration, tokens or upload URLs are exported. Trace names are internal diagnostic codes, not user-facing text.

Trace is captured in memory and appended with the attempt outcome; a process termination before that point can leave only the Started entry. Durable local/remote operation journals remain the authority for recovery. Diagnostic logging failures cannot change the import outcome. The current error window retains its final context for export even if history could not be written.

At most 96 checkpoints are retained per attempt. The existing four files of 64 KiB limit disk usage. Export validates the typed fields, rejects malformed traces and publishes atomically after any approved overwrite. Older entries without trace or revision remain readable. Alpha 0.9.36 adds localized messages for interpretation, recovery and diagnostics in the 51 UI languages; language/editorial quality requires separate review. The parser and diagnostic extensions add categorized limits (text rows/cells/columns, bytes, ODS dimensions/repetition, remote export size) without exposing source content.


## Retentativas remotas e quotas na 0.9.42

O diagnóstico incorpora razões de falha de capacidade/backup separadas de acesso a arquivo. A telemetria da fila global `ImportQueue` permanece legível em históricos, mas **não é uma fila de importação vigente**. `BackupCapacityWait` identifica a espera por espaço real, rechecado a cada 250 ms com cancelamento/prazo.

Nas leituras Google, retries controlados aceitam motivos 403 `rateLimitExceeded`/`userRateLimitExceeded` e regras 429/5xx, com limite de `Retry-After`; não repetir POST de criação. Mensagens de API brutas e planilhas continuam fora do JSONL. O diagnóstico da tentativa continua distinto do histórico geral.

**Estado da distribuição:** `39181854241` (protegida) falhou por watchdog de teste de lote; `39189870249` (Local core HEAD) falhou em Windows e passou em Ubuntu. Não inferir que uma falha de teste é sempre erro de quota real.
