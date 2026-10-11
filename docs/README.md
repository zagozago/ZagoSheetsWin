# ZagoSheetsWin documentation

This folder contains **both engineering references and historical development records**. Most of these notes were written during the alpha implementation in Brazilian Portuguese; they do not need to be translated just to keep the repository open source.

For a new contributor, start with [CONTRIBUTING.md](../CONTRIBUTING.md) for the current source branch, build, tests, localization workflow, data-safety guidelines and handling of credentials.

## Engineering references

These documents can help explain past decisions and current technical concepts. Some were written as **proposals** and may predate the current code; verify the implementation rather than treating them as current runtime specifications.

- [Architecture](ARCHITECTURE.md) — original target design and safety-oriented component boundaries
- [Architecture decisions](adr/DECISIONS.md) — design choices and trade-offs
- [Localization integration](I18N_INTEGRATION.md) — 51-language resources, installer, fallback and RTL behavior
- [XLS conversion](XLS_CONVERSION.md) — format-specific design and compatibility considerations
- [Development](DEVELOPMENT.md) — development history and technical notes; branch references and release status may be dated
- [Upstream audit](AUDIT.md) — static analysis of the original Open in Google code at a recorded revision

## Historical alpha records

The following are **implementation history**, not current installation instructions or a list of still-open tasks:

- `STAGE_1.md` through `STAGE_9.md` — step-by-step implementation history
- `ALPHA_*.md`, `ALPHA_NEXT.md`, `ROADMAP.md` — alpha plans, experiments, and earlier acceptance notes
- `UI_REDESIGN.md`, `UI_TEXTS_PT_BR.md` — interface-design and wording iterations
- `I18N_PREPARATION.md`, `I18N_TRANSLATION_PROGRESS.md`, `LOCALIZATION_AUDIT.md` — language rollout milestones and earlier audits
- `IMAGESHARP_UPDATE.md` — dependency-related work

The original upstream README is retained at [`upstream/README.original.md`](upstream/README.original.md) for attribution and historical context.

## Repository cleanup policy

Before a stable release, review historical files for technical relevance, references from current documentation, licensing requirements, and links used by tooling. Move or retire obsolete notes only after this review. This index makes the distinction visible without renaming or deleting anything during active testing.

Prompts, working conversations and provisional internal checklists are **not prerequisites** for publishing the application's source under the MIT license. Buildable source, appropriate licenses/notices, tests and accurate contributor guidance are more useful.

Removing a tracked file in a later commit does **not** remove it from the public Git history. If a historical document contains a credential or other secret, handle that as a security incident rather than assuming relocation makes it private.
