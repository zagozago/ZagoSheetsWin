# Auditoria e consolidação documental — preparação 1.0.0

**Revisão:** 15/10/2026. **Base de código:** `feature/zagosheetswin` commit `6a019318f64c1068ebd5ece4932320535b24f629`. **README internacional:** `main` commit `057ef77e0e79af197bb2c6c3f7b28fee1b8c0b02`. **Forma de entrega:** branch isolada e PR para a branch de desenvolvimento; **sem publicar tag/Release 1.0.0**.

## Constatações e decisões

| Constatação | Correção documental |
|---|---|
| `main` tinha README em inglês e 50 READMEs traduzidos; a branch de código não tinha `docs/i18n/` | Preservar seletor internacional e importar exatamente a árvore de traduções de `main` (sem retraduzir automaticamente) |
| README de desenvolvimento em português apresentava versões antigas e estágios passados como guia vigente | Adotar conteúdo canônico atual com links para manual, arquitetura, limites, histórico e changelog |
| `CHANGELOG.md` anterior citava só dois candidatos e não explicava status oficial | Consolidar marcos alfa com links a commits/documentos; assinalar ausência de tags/Releases |
| `docs/ARCHITECTURE.md` descreve arquitetura proposta, não o programa já implementado | Criar `ARCHITECTURE_CURRENT.md` e identificar `ARCHITECTURE.md` como histórico |
| `docs/ROADMAP.md`, `ALPHA_NEXT.md`, `DEVELOPMENT.md` contêm estados históricos | Manter rastreabilidade, adicionar cabeçalho de histórico e indexação `HISTORY.md` |
| `SETUP.md` é instrução do upstream PowerShell (Word/Docs/Slides) | Sinalizar como legado; indicar instalador C# e manual vigente |
| ODS está parcialmente coberto; documento HTML renomeado `.ods` não é ODS | Registrar validação de pacote, política de cópia e regressões pendentes; sem alegar compatibilidade universal |
| Versões anteriores tinham deriva 0.9.29/0.9.28 entre produto e instalador | Registrar correção do candidato 0.9.30 e exigir validação do artefato exato |
| 51 línguas da interface ≠ 50 READMEs atuais editorialmente revisados | Preservar recursos e avisar que traduções documentais ainda precisam de sincronização antes da estabilidade |
| Guia futuro no domínio Zagotools aparece no PR #12 | Separar URL proposta de link confirmado; não publicar hiperlink como manual existente |
| Há PR aberto #11 de estrutura de contribuição e documentação | Incorporar conteúdo compatível com base atual e registrar sobreposição para conciliação antes do merge |

## Checagens e limites da auditoria

- Fonte principal: código e documentação do GitHub, branches citadas. Estrutura de arquivos lida a partir das árvores Git e fontes relevantes.
- Foram consultados registros de commits, releases e tags. **Releases: vazio; tags: vazio** durante esta consulta.
- Este trabalho é **documental**. Não modifica fluxo de importação, OAuth, autorização, empacotamento, versão do programa nem catálogos de interface.
- Não foram executados testes locais adicionais nem homologação Google real nesta revisão.
- Uma tradução importada de `main` pode estar menos atualizada que o README canônico. É necessário processo futuro de tradução/revisão e não apenas conservar links.
- Não houve validação da publicação do futuro caminho de ajuda no domínio; links legais existentes permanecem identificados como configurados no código.
- O status de vulnerabilidades e a elegibilidade de dependências são sensíveis ao momento e devem ser revalidados no build candidato oficial.
- Não há certificação formal ASD-STE100: textos portugueses seguem princípios de **Português Técnico Controlado**.

## O que não foi alterado

- Arquivos `LICENSE`, `ATTRIBUTION.md`, `NOTICE.md`, `third-party/` e fontes de interface i18n.
- Histórico alfa nem arquivos de origem PowerShell além do **aviso** inicial de obsolescência em `SETUP.md`.
- `main`, releases, tags, OAuth, configurações de GitHub ou credenciais.

## Revisão de PRs em paralelo

- [PR #11](https://github.com/zagozago/ZagoSheetsWin/pull/11): propõe `CONTRIBUTING.md` e `docs/README.md`. Este PR incorpora versões atualizadas desses conceitos. Conciliar ou encerrar PR #11 separadamente **após revisão**, sem presumir sua aprovação.
- [PR #12](https://github.com/zagozago/ZagoSheetsWin/pull/12): propõe botão Ajuda abrindo URL futura. Este PR não modifica essa ação; o domínio/caminho deve ser confirmado primeiro.
