# Auditoria e consolidação documental — preparação 1.0.0

**Revisão inicial:** 15/10/2026; **atualização:** 17/10/2026. **Base de código inicial:** `feature/zagosheetswin` commit `6a019318f64c1068ebd5ece4932320535b24f629`. **Base da atualização 0.9.35:** `9929524e67fea42c8940ae27b41f80963479c490`. **Base verificada 0.9.37 (18/10/2026):** `f58b630263cdee07e473a54ee40ff6d3beafe4f6`. **Base 0.9.42 (19/10 UTC):** `c1dc8a501209ce37d1f65c141ad30498146296af` (CI Windows vermelho naquela execução). **Base 0.9.43:** `9fbec2add18c012012405b56a333f6e49248a6e5`, CI/distribuição aprovados. **Base atual 0.9.48:** `08dbced61ed6395534ed6daeff6cf23342c5a4a9`, hotfix RTL com CI próprio iniciado; distribuição anterior validada em outro SHA. **README internacional:** `main` commit `057ef77e0e79af197bb2c6c3f7b28fee1b8c0b02`. **Forma de entrega:** branch isolada e PR para a branch de desenvolvimento; **sem publicar tag/Release 1.0.0**.

## Constatações e decisões

| Constatação | Correção documental |
|---|---|
| `main` tinha README em inglês e 50 READMEs traduzidos; a branch de código não tinha `docs/i18n/` | Preservar seletor internacional e importar exatamente a árvore de traduções de `main` (sem retraduzir automaticamente) |
| README de desenvolvimento em português apresentava versões antigas e estágios passados como guia vigente | Adotar conteúdo canônico atual com links para manual, arquitetura, limites, histórico e changelog |
| `CHANGELOG.md` anterior citava só dois candidatos e não explicava status oficial | Consolidar marcos alfa com links a commits/documentos; assinalar ausência de tags/Releases |
| `docs/ARCHITECTURE.md` descreve arquitetura proposta, não o programa já implementado | Criar `ARCHITECTURE_CURRENT.md` e identificar `ARCHITECTURE.md` como histórico |
| `docs/ROADMAP.md`, `ALPHA_NEXT.md`, `DEVELOPMENT.md` contêm estados históricos | Manter rastreabilidade, adicionar cabeçalho de histórico e indexação `HISTORY.md` |
| `SETUP.md` é instrução do upstream PowerShell (Word/Docs/Slides) | Sinalizar como legado; indicar instalador C# e manual vigente |
| ODS está parcialmente coberto; documento HTML renomeado `.ods` não é ODS | Registrar validação de pacote e **mudança 0.9.33 de substituição mesmo quando há equivalência parcial**; não alegar compatibilidade universal |
| Versões anteriores tinham deriva 0.9.29/0.9.28 entre produto e instalador | Registrar correção do candidato 0.9.30 e exigir validação do artefato exato |
| 51 línguas da interface ≠ 50 READMEs atuais editorialmente revisados | Preservar recursos e avisar que traduções documentais ainda precisam de sincronização antes da estabilidade |
| O botão Ajuda da branch de produto já abre URL Zagotools com idioma, e PR #12 permanece aberto | Descrever a implementação real sem presumir que a página web exista; conciliar PR paralelo |
| Há PR aberto #11 de estrutura de contribuição e documentação | Incorporar conteúdo compatível com base atual e registrar sobreposição para conciliação antes do merge |

## Constatações complementares de 0.9.35

- **0.9.31:** cinco formatos sempre disponíveis; ODS no tutorial e registro Windows.
- **0.9.32:** retirada da seção Formatos e texto nas Opções avançadas. Catálogos legados ainda contêm mensagens dessa seção, mas isso não significa que os controles continuem visíveis.
- **0.9.33:** diferença de valores/fórmulas e verificação incompleta deixam de impedir a substituição após importação Google confirmada e validações obrigatórias. A documentação antiga que prometia cópia automática estava **incorreta para esta versão**.
- **0.9.34:** confirmação pós-substituição com Backups e opção persistente; backups com ordenação Arquivo/Data/Tamanho/Estado e regras salvas automaticamente; contraste em tema escuro.
- **0.9.35:** mensagem sobre atributo somente leitura e regressão Windows que preserva original/backup, com retomada após retirar o atributo.
- **Automação:** [Protected alpha distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39059101337) aprovada para `70377cb`; [Local core](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39059144987) aprovado para `9929524`. A distribuição protegida **não foi provada neste levantamento** para o commit mais novo.
- **Limites:** não houve testes adicionais de conta Google real nesta revisão. O futuro `1.0.0` segue pendente de aceite, sem publicação.

## Constatações complementares 0.9.36 e 0.9.37

- **Parser texto 0.9.36:** CSV/TSV detecta separador por conteúdo em `,`, `;`, TAB e `|`. Em ambiguidade oferece escolha **por tentativa**. Fallback CP1252 controlado sem BOM, literalidade das células e 60.000 registros/600.000 células são restritos à semântica de interpretação v2.
- **Compatibilidade e reparo:** operações com intenção remota antiga mantêm interpretador v1; `ImportInterpretations` grava opções imutáveis por operação. `BackupRepairSource` permite criar **outra planilha** sem tocar documento/atalho anterior.
- **ODS esparso:** trata caudas vazias repetidas sem expandir desnecessariamente, mantendo coordenadas/bytes do arquivo original. **Não provar conversão real Google** a partir de um parser local.
- **Erro e diagnóstico:** estado terminal persiste após minimizar/restaurar, ignora progresso atrasado, diagnóstico de tentativa separado do histórico; erros permanentes aguardam alteração do arquivo em vez de repetição automática.
- **Identidade 0.9.37:** arquivo `zagosheetswin.ico` substituído pelo ICO de planilha igual ao dos atalhos, mantendo logos Zagotools e créditos MIT.
- **CI e empacotamento:** [Local core](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39099713350) e [Protected alpha distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39099713370) aprovados **no mesmo SHA f58b630**; não equivale a aceite Google real.
- **Relatório de Escopo B:** `docs/IMPORT_REVIEW_0936.md` contém evidências e limitações datadas da versão 0.9.36. Este PR apenas registra e referencia esse arquivo, que veio da branch do software.
- **Tags/Releases:** ainda inexistentes na consulta de 18/10/2026; 1.0.0 permanece futuro.

## Checagens e limites da auditoria

- Fonte principal: código e documentação do GitHub, branches citadas. Estrutura de arquivos lida a partir das árvores Git e fontes relevantes.
- Foram consultados registros de commits, releases e tags. **Releases: vazio; tags: vazio** durante esta consulta.
- Este trabalho é **documental**. Não modifica fluxo de importação, OAuth, autorização, empacotamento, versão do programa nem catálogos de interface. **A atualização 0.9.35 incorpora a revisão do comportamento implementado em commits da 0.9.31 à 0.9.35; alterações em código vieram da branch do produto, não deste PR.**
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


## Mudanças auditadas até 0.9.48

- **0.9.38:** interpretação v3 até 1M células, escolha de separador com prévia e gramática ampliada; **CSV/TSV passam a exigir conferência de valores para retirar o original**, diferente de XLS/ODS.
- **0.9.39:** leitor v4 corrige coluna única/campos citados/CRLF, ação `Escolher separador` e restauração segura de atalho perdido; rotação da cota por backups concluídos.
- **0.9.40:** retirada do toggle de limpeza; prazo/cota automáticos; fila global de importação temporária; retry de leituras Google com motivos 403 de rate-limit permitidos. **A fila global não é política vigente.**
- **0.9.41:** CSV/TSV até **2.000.000 células** com 100.000 registros; XML/ZIP de texto aumentados; arquivo real de 1.018.712 células validado localmente, não no Google. Correção de corrida de desinstalação/reinstalação no teste CI.
- **0.9.42:** removida fila global, importações diferentes paralelas e espera **apenas quando falta capacidade real de backup**, com coordenação/cancelamento/prazo. Último CI Windows falhou no commit `c1dc8a5`, distribuição anterior falhou no watchdog de quota. **Não afirmar candidato pronto**.
- Fontes: [0.9.38](REGRESSION_0938.md), [0.9.39](REGRESSION_0939.md), [0.9.40](REGRESSION_0940.md), [0.9.41](REGRESSION_0941.md), [0.9.42](REGRESSION_0942.md), além de fontes C# e workflow GitHub. Nenhuma dessas evidências representa homologação Google real, assinatura ou certificação.
- Histórico de licenças, 51 idiomas, créditos MIT/Swati K e identidade Zagotools mantidos; nenhuma credencial alterada pelo PR.

### Conciliação da versão 0.9.43 (19/10 local — 20/10 UTC)

O [relatório `REGRESSION_0943.md`](REGRESSION_0943.md) registra **rollback explícito** da política de backups: não se aplica mais limpeza por prazo após importações nem rotação por quota. **Limpeza apenas manual.** `BackupPolicy.AutomaticCleanup` volta a `false` ao carregar preferências antigas e o toggle segue ausente. Quota excedida retorna falha imediata preservando original/backup, enquanto capturas usam trava curta atômica.

A versão manteve importações paralelas entre arquivos diferentes e os limites de 2 milhões de células CSV/TSV. Resposta de sessão remota ambígua admite até duas consultas somente-leitura ao marcador, sem duplicação de criação; byte buffer de upload foi simplificado, leitura de pasta existente não duplica GET e erros por falta de RAM usam UI mínima com **Fechar**. Janela principal sem redimensionamento ganhou ajuste de `ClientSize` de 500×240 para **500×192**, restrito à Home.

**Evidência verificável:** [Local core #39346495162](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) e [Protected alpha distribution #39346495298](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) aprovaram o mesmo commit `9fbec2a`, com pacotes de instalador/launcher e testes de idioma. Não se infere sucesso de testes Google reais, assinatura pública, compatibilidade universal de ODS nem conclusão de revisão editorial.

### Constatações 0.9.44–0.9.48

**0.9.44:** operações pendentes são recuperáveis. Nova abertura posterior/independente pode reimportar só com remoto comprovadamente ausente após reconciliação, preservando backup/associação/histórico antigos. A mesma abertura/retry, concorrência, rede indisponível ou resposta ambígua **não** autorizam nova criação. Ver `SpreadsheetOpening` e `Regression0944Tests`.

**0.9.45–0.9.46:** refinamentos de UI, temas, cabeçalhos, rodapés legais, cartões, primeiro uso e botões sem alterar a política de backup/importação.

**0.9.47:** CSV aprovado com **4.335 linhas = 85 mensagens × 51 idiomas**, mapeado (65 IDs reaproveitados, 18 criados, dois não aplicáveis); nova seleção/checkbox, três cartões de recuperação recolhidos e métricas reais. `Ctrl+A` não foi acrescentado. Fonte: [implementação](implementation/refinement-20261024.md).

**0.9.48:** dez requisitos de UI implementados, incluindo instalador, alinhamentos, lote de 32 com spinner, checkbox redundante removido, barra e sidebar, corpo -2 pt, erro de quebra automática, prefixos e seta de Mais ações. Hotfix posterior `08dbced` conserva recuos físicos de botões em RTL apenas na Home/Configurações via `InsetFlowLayoutPanel`, sem alteração no backend. Fonte: [revisão](implementation/final-ui-20261024.md).

**Evidências no momento da revisão:** [distribuição protegida #40120263404](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40120263404) passou em `df6f4d6`; [Local core #40121298442](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40121298442) passou em `cacba045`. Hotfix [#40126683024](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40126683024) estava em execução; nenhuma evidência de Google real ou Release estável foi inferida.
