# ImageSharp 4.1.3 and assembly-scoped community license

ImageSharp 2.1.13 blocked restore with security advisories. 4.1.3 is pinned
in Directory.Build.props and dependency locks; auditing remains enabled
without suppressions. The 4.1.3 licensing update accepts the assembly-scoped
license issued on 2026-10-10, which the 4.1.2 validator rejected.

The supplied sixlabors.lic is preserved verbatim at the repository root.
Directory.Build.props sets SixLaborsLicenseFile to its absolute location
so nested projects can find it. The provider explicitly permits committing
assembly-scoped community keys; this permission does not apply to unrestricted
community or commercial keys. The license is valid until 2028-01-08 and was
validated by a Release build of SheetsWindows.Infrastructure.

No repository secret is necessary for this key. Existing workflow secret
configuration can still override the file if a nonempty key is configured.
XLS support is retained; no format, translation pack, or UI was removed.

Local Release compilation passes. Localization catalog: 593 messages,
51 locales; all 13 Python tests pass. Windows interface, installer lifecycle
and remaining Windows-specific tests must pass in Local core before protected
distribution is dispatched. No distribution was dispatched for this change.

Sources:
- https://docs.sixlabors.com/articles/imagesharp/index.html
- https://github.com/SixLabors/ImageSharp/releases/tag/v4.1.3
