# 0.9.43 — backup admission rollback

Imports no longer poll for backup capacity, rotate completed backups, or run post-import maintenance. A quota failure immediately preserves the original and points to manual cleanup in Backups. The snapshot capture lock and atomic quota check remain: simultaneous captures cannot exceed the configured quota. Uploads do not hold that lock. No profile import queue or file-count cap was introduced.

Retention days still determine the explicit **Clean expired** action. Cleanup is manual, including for profiles saved with the former automatic-cleanup flag. Pending/unverified backups remain protected; restore, discard journals, completed-backup selection, autosaved numerical limits and the statistics panel are preserved. Existing quota error translations were restored from the pre-rotation revision; the recovery panel uses the translated cleanup tip.

The 2,000,000-cell CSV/TSV limit, source identity/hash checks, remote conversion verification, resumable upload binding, and read-only SQLite schema optimization remain unchanged. Snapshot upload bytes now use one exact-sized buffer instead of a growing MemoryStream plus a second complete copy. Removal of the unconditional maintenance scans also avoids catalog-wide work after each file. An existing account folder is fetched and fully validated once, instead of repeating the same GET while holding the folder lock.

The supplied 4,251-byte XLSX has a small, structurally valid OOXML package with accented text. Its log reaches upload session acknowledgments, not conversion verification; no capacity wait was recorded. Its roughly 91 MiB peak differs from the separate multi-window crash log, which reports OutOfMemoryException at SQLite construction. Neither log proves backup locking caused the exhaustion. Ambiguous upload completion now gets two bounded, read-only marker rechecks; no second creation or upload session is initiated. Session acknowledgments record only the numeric HTTP status to make future failures diagnosable.

Low-memory processing exceptions bypass diagnostic serialization and the retry/error-dialog construction path. The processing window offers Close using precomputed error text. This is graceful failure handling, not a promise that unlimited simultaneous large conversions fit in system memory.

The home window uses a fixed border, disables maximize/resizing, retains minimize and no longer mixes a client-size initial value with an outer-size minimum. Header fitting ignores minimized bounds. Native regression checks repeatedly minimize/restore it and test a simulated low-memory failure.

Validation results and installer candidate will be recorded after checks complete. Real Google behavior and user-machine batch timing still require the user's retest; offline tests cannot prove those.
