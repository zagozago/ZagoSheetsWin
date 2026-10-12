# Diagnostics and trashed associations

## Opening a local spreadsheet

Opening an existing CSV, TSV, XLSX, XLS or ODS checks its association by account, local file identity and remote ID, never by name. If the bound Google document is confirmed trashed and its association markers match, the old local operation is retired from the active source binding. Its backup, remote mapping and replacement journal remain available as history. A new operation captures the current local bytes and creates a new Google document with a fresh creation marker. Nothing restores, updates or permanently deletes the trashed document.

This check runs before the old snapshot comparison, so an edited local file can be imported as the new document. The new operation retains the normal backup, conversion, validation and shortcut safeguards. Network failures, missing files (HTTP 404), denied access and invalid association markers do not authorize a new upload. Source locks serialize simultaneous openings. A lost upload response is reconciled through the new operation's marker instead of initiating another upload.

A confirmed trashed application folder is similarly retired and replaced by a new folder in the account root. The old folder and its documents remain untouched. Each folder generation and its events are retained in the remote registry.

Direct Internet shortcuts continue to open their recorded URL. Explicit recovery of an old operation does not silently create a different operation.

## Exporting a diagnostic

The existing export button produces one JSONL file containing rotated history, current attempt context and an export timestamp. Each completed or failed UI attempt includes a bounded `Trace`: step name, outcome, elapsed milliseconds and, where applicable, boolean remote validation checks. It covers preflight, authorization, snapshot preparation, conversion, remote creation/reconciliation/validation, shortcut publication, browser launch and source retirement. Repeated preflight steps identify automatic retries inside the same UI attempt.

Each entry includes the product version and, when supplied by the SDK build, its Git revision. Existing failure categories, HRESULT, Google HTTP status and SQLite error code remain available. No exception messages, stack traces, source paths, spreadsheet cells, account identifiers, Google file IDs, OAuth configuration, tokens or upload URLs are exported. Trace names are internal diagnostic codes, not user-facing text.

Trace is captured in memory and appended with the attempt outcome; a process termination before that point can leave only the Started entry. Durable local/remote operation journals remain the authority for recovery. Diagnostic logging failures cannot change the import outcome. The current error window retains its final context for export even if history could not be written.

At most 96 checkpoints are retained per attempt. The existing four files of 64 KiB limit disk usage. Export validates the typed fields, rejects malformed traces and publishes atomically after any approved overwrite. Older entries without trace or revision remain readable. No new UI messages or translations are introduced by this backend change.
