# ImageSharp 4.1.2: dependency update awaiting license

The 2.1.13 dependency blocks restore because of five security advisories.
4.1.2 restores successfully with the NuGet audit enabled and no suppressions.
Release compilation requires a Six Labors community or commercial license key.

Request a community key at https://licensing.sixlabors.com/ using the truthful
eligibility details and exact assembly name `SheetsWindows.Infrastructure`.
The repository is currently private; do not claim a public repository URL or
eligibility that does not apply. The provider also supports hobbyist applications.

Add the full key/file contents as repository Actions secret
`ZAGOSHEETS_SIXLABORS_LICENSE_KEY` (not just the JSON Key field).
Both Local core and Protected alpha distribution pass it as
`SixLaborsLicenseKey` to MSBuild. Never commit the key or license file.
If a same-named environment secret exists, it takes precedence in distribution.

Validation so far: restore with regenerated dependency locks passes. Release
build, conversion tests, native locale checks and installer validation remain
pending the key. No release-ready claim is made.

Sources:
- https://docs.sixlabors.com/articles/imagesharp/index.html
- https://sixlabors.com/posts/licence-enforcement-changes/
- https://github.com/SixLabors/ImageSharp/security/advisories/GHSA-jjfr-hcj7-qf5w
