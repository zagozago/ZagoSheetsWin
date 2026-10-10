# ZagoSheetsWin 0.9.23

- The installer replaces the native alphabetical language dialog with a custom
  unsorted selector inside the same executable. Captions and order are generated
  from i18n/locales.json using the same formatting as LanguageSettings.Caption.
  Unicode names, English names, display codes and canonical codes are preserved.
- A different selection restarts the same installer with /LANG before installation;
  loader-only process handles are filtered while ordinary arguments are preserved.
  Cancellation installs nothing. Silent operation and explicit /LANG bypass the
  picker, preserving existing unattended install behavior.
- Explicit interactive selection persists the language before --first-use opens
  the application, including upgrades with an older preference. Silent installation
  without an explicit selection marker initializes only an absent preference.
- Applying a language in the app restores and activates the existing owner window
  after its modal picker closes. This is not a process restart. Maximized windows
  retain their state; minimized windows return to normal.
- Read-only Drive metadata/export and existing resumable sessions use bounded
  transient retries (429, 5xx and network request failures), waiting 2/4/8 seconds
  or bounded Retry-After. Cancellation and the overall request deadline still apply.
  Creation POST requests are not repeated, and session recovery queries the remote
  offset before sending more content. Permanent errors remain visible.

Local validation: Release build passes; 224 functional tests pass on Linux,
36 Windows-only tests skipped; 14 catalog tests pass, 593 messages in 51 locales.
Native Windows checks cover restored language windows. The installer selector
check reads all 51 real combo entries and tests cancellation; lifecycle checks
verify explicit installer language handoff and unattended upgrade preservation.
Windows CI must pass before protected distribution is queued.

The user reported a transient XLSX error without its message; these changes cover
identified gaps in retry handling and do not establish that error's exact cause.
