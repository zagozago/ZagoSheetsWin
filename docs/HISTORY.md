# Histórico preservado do desenvolvimento alfa

**Este índice reúne documentos históricos. Eles não são instruções atuais de instalação nem declaração de funcionalidades já disponíveis.** Não remover ou mover arquivos antigos sem revalidar links de PRs, commits, issues, READMEs traduzidos e licenças.

## Fundamentos e decisões (histórico)

- [Auditoria do upstream PowerShell](AUDIT.md): revisão de Open in Google em um commit fixo.
- [Arquitetura-alvo da época](ARCHITECTURE.md): usa termos de proposta; consulte a [arquitetura atual](ARCHITECTURE_CURRENT.md) para a implementação.
- [Decisões arquiteturais](adr/DECISIONS.md): decisões numeradas tomadas nas fases iniciais.
- [Roadmap inicial](ROADMAP.md) e [próximas evoluções alfa](ALPHA_NEXT.md): cronogramas e critérios relativos à época; algumas entregas já foram implementadas e outras permanecem abertas.
- [Notas de desenvolvimento/fork](DEVELOPMENT.md): comandos e nomes de branches históricos.

## Implementações por fase

- [STAGE_1](STAGE_1.md) — base do fork.
- [STAGE_2](STAGE_2.md) — núcleo local e testes.
- [STAGE_3](STAGE_3.md) — OAuth, importação e piloto.
- [STAGE_4](STAGE_4.md) — atalho, retirada, recuperação.
- [STAGE_5](STAGE_5.md) — Windows launcher, associação.
- [STAGE_6](STAGE_6.md) — instalador por usuário.
- [STAGE_7](STAGE_7.md) — formatos e ambientes, limites anteriores às amplificações posteriores.
- [STAGE_8](STAGE_8.md) — upload resumable, recuperação, logs e atualização.
- [STAGE_9](STAGE_9.md) — identidade Zagotools / ZagoSheetsWin.

## Evoluções alfa

- [ALPHA_1_CSV](ALPHA_1_CSV.md) — capacidade ampliada e parser CSV.
- [ALPHA_2_XLS](ALPHA_2_XLS.md) — substituição XLS e ícone.
- [ALPHA_3_SETUP](ALPHA_3_SETUP.md) — instalação e primeiro uso.
- [ALPHA_4_UI](ALPHA_4_UI.md) — interface/tema/processamento.
- [ALPHA_6_BACKUPS](ALPHA_6_BACKUPS.md) — gestão de backups e quota.
- [ALPHA_7_OAUTH](ALPHA_7_OAUTH.md) — cliente OAuth de distribuição.
- [ALPHA_0_9_23](ALPHA_0_9_23.md) — seletor de idiomas e política de retries.
- [XLS_CONVERSION](XLS_CONVERSION.md) — evolução da conversão de XLS binário e HTML com extensão XLS.
- [IMPORT_REVIEW_0936](IMPORT_REVIEW_0936.md) — aceitação técnica do Escopo B: CSV/TSV, ODS esparso, reparo versionado, interface, diagnósticos e limites; **o teste real Google está pendente**.
- [Branding/README](../branding/README.md) — evolução dos ícones incluindo a unificação da 0.9.37.
- [REGRESSION_0938](REGRESSION_0938.md), [matriz](REGRESSION_MATRIX_0938.md) — parser v3 e segurança da exportação.
- [REGRESSION_0939](REGRESSION_0939.md) — coluna única/CRLF, rotação, reconciliação de atalho.
- [REGRESSION_0940](REGRESSION_0940.md) — fila de importação histórica, limpeza automática e retries Google.
- [REGRESSION_0941](REGRESSION_0941.md) — 2M células CSV/TSV, correção do teste de reinstalação.
- [REGRESSION_0942](REGRESSION_0942.md) — paralelismo restaurado, espera por quota e correção da contenção SQLite (**comportamento de quota/limpeza substituído na 0.9.43**).
- [REGRESSION_0943](REGRESSION_0943.md) — limpeza manual, falha rápida de quota, memória/OOM e reconciliação (dimensões da Home evoluíram posteriormente); distribuição protegida [#39346495298](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298).
- [Refinamento da UI 0.9.47](implementation/refinement-20261024.md) — CSV aprovado de 85 mensagens × 51 idiomas, seleção de backups, primeira instalação e acessibilidade.
- [Acabamento 0.9.48](implementation/final-ui-20261024.md) — margens do instalador, seleção em lote, métricas, erros, alinhamentos e arte; hotfix RTL em `08dbced` no histórico Git.

## Interface, traduções e dependências — registros de trabalho

- [UI_REDESIGN](UI_REDESIGN.md) e [UI_TEXTS_PT_BR](UI_TEXTS_PT_BR.md).
- [I18N_PREPARATION](I18N_PREPARATION.md), [I18N_TRANSLATION_PROGRESS](I18N_TRANSLATION_PROGRESS.md), [LOCALIZATION_AUDIT](LOCALIZATION_AUDIT.md).
- [IMAGESHARP_UPDATE](IMAGESHARP_UPDATE.md).
- [Métricas históricas alpha 0.9.5](metrics/alpha-0.9.5-native.json).
- [README original do upstream](upstream/README.original.md).
- [SETUP.md da origem PowerShell](../SETUP.md), **não aplicável ao instalador nativo ZagoSheetsWin**.

Os arquivos `I18N_INTEGRATION.md`, `diagnostics-and-trashed-associations.md` e `SECURITY_REVIEW_0930.md` são referências técnicas datadas e podem auxiliar a implementação, mas devem sempre ser comparados ao código do commit investigado.

**Documentos vigentes:** [índice atual](README.md), [manual](USER_GUIDE.md), [limites](FORMATS_AND_LIMITS.md) e [CHANGELOG](../CHANGELOG.md).
