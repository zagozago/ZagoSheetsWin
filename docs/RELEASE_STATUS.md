# Estado da versão — referência vigente

**Versão declarada do aplicativo: `0.9.48` (alfa).** Nenhuma tag/Release 1.0.0 foi publicada. A última atualização disponível na branch inclui **o hotfix RTL mencionado pelo usuário**.  
**Branch de produto:** `feature/zagosheetswin`.  
**Commit conferido:** [08dbced61ed6](https://github.com/zagozago/ZagoSheetsWin/commit/08dbced61ed6395534ed6daeff6cf23342c5a4a9), em **24/10/2026**; na data da revisão, ainda aguardava o resultado do CI do próprio hotfix.  
**Documentação:** PR #13 em branch separada, sem merge/lançamento.

## Coerência de projeto e internacionalização

- [Projeto Windows](../src/SheetsWindows.Windows/SheetsWindows.Windows.csproj): `<Version>0.9.48</Version>`.
- Instalador: versão/fallback `0.9.48`; marca lateral oficial refeita e seletor com margens e escala DPI ajustadas.
- [Fonte de tradução](../i18n/source.json): `productVersion = 0.9.48`, catálogo revisão 17 e 51 idiomas. O [CSV dirigido](../i18n/refinement-20261024.csv) de 85 mensagens × 51 locales foi importado e verificado, sem gerar traduções novas; consultar [mapeamento de 85 IDs](implementation/refinement-20261024-mapping.json).
- MIT, atribuição à base Open in Google/Swati K e logos Zagotools mantidos.

## CI e distribuição

| Execução / evidência | SHA | Resultado confirmado na consulta |
|---|---|---|
| [Local core #40110813671](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40110813671) | `1738188` | **Sucesso** Windows 407 aprovados/1 ignorado; Linux 364 aprovados/44 ignorados; matriz nativa 51 locais e quatro escalas |
| [Local core #40121298442](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40121298442) | `cacba045` | **Sucesso** na versão 0.9.48 antes do último hotfix |
| [Protected alpha distribution #40120263404](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40120263404) | `df6f4d6` | **Sucesso**, commit anterior ao hotfix RTL |
| [Local core #40126683024](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40126683024) | `08dbced` | **Em execução** no momento da consulta. Não confundir com o sucesso no commit anterior |

**Não existe confirmação de nova distribuição protegida no SHA `08dbced`** nesta revisão. Compilação e captura de tela offline, mesmo nos 51 idiomas, não substituem testes no Windows do usuário/Google real nem aprovação formal de 1.0.0.

## Comportamento atual 0.9.48

1. **Importação/conversão preservadas:** cinco formatos `.xlsx/.xls/.ods/.csv/.tsv`. CSV/TSV v4 até 2.000.000 células (incluindo vazias), 100.000 registros, 1.000 colunas e 20 MiB/payload. Divergência de valores **CSV/TSV** impede substituição; XLS/ODS podem conservar risco de fidelidade parcial após Google confirmado.
2. **Backup manual:** 30 dias/200 MB por padrão, cota até 1.000 MB, autosave numérico; limpeza **somente manual**. Falta de quota gera erro direto com preservação do original, sem limpeza automática ou espera por capacidade. Backups pendentes/não verificados continuam protegidos.
3. **Pendências — exceção controlada 0.9.44:** operação antiga reconciliada, com remoto confirmado ausente, pode permitir nova importação numa **nova abertura independente** sem remover associação, backup e histórico anteriores. O fluxo **não** age em falha de rede/validação, resultado ambíguo, mesmo clique/retry ou operação concorrente.
4. **Interface 0.9.45–0.9.47:** cabeçalhos, ícones, rodapés, cartões Sobre/Licenças, preferências, informações contextuais, assistente de três telas e miniaturas padronizadas. Backups mostra métricas reais de uso e capacidade em painéis separados inicialmente recolhidos.
5. **Interface 0.9.48:** seleção de backups por checkboxes por linha espelhando `SelectedItems` (sem checkbox extra no cabeçalho); selecionar/limpar todos em lotes curtos de 32 com spinner e proteção de reentrada. Fonte do corpo de painéis reduzida 2 pt, sem alterar títulos; barra de ocupação limitada ao cartão, lateral de 285 unidades, textos de erro com quebra automática.
6. **Ajustes finais:** ícone da pasta da Home alinhado em um pixel; seta de Mais ações associada à última palavra para quebras de linha; prefixos de Backups/Disponível e dicas em negrito. `InsetFlowLayoutPanel` no hotfix `08dbced` mantém **recuos físicos horizontais de botões/painéis na Home e Configurações em RTL**; não toca backend. A Home usa agora `ClientSize 540×190` no código atual, mantém minimizar e não pode ser maximizada/redimensionada.
7. **Segurança/reliability 0.9.43:** buffer exato para bytes de snapshot, tratamento simplificado de baixa memória, reconciliador remoto sem segundo POST, locks por origem e importações independentes em paralelo; tudo preservado na série visual conforme registros e código.

## Evidências e itens para 1.0

- [Refinamento 0.9.47](implementation/refinement-20261024.md) — plano aprovado, CSV e testes nativos.
- [Acabamento 0.9.48](implementation/final-ui-20261024.md) — 10 requisitos localizados, screenshots e ajustes finais.
- [Regressão funcional 0.9.43](REGRESSION_0943.md) e [casos pendentes 0.9.44](../tests/SheetsWindows.Tests/Regression0944Tests.cs): riscos de importação não foram homologados em conta Google real nesta etapa.
- **Gates:** concluir CI do hotfix RTL, gerar/validar distribuição protegida no SHA congelado (se a versão final for esse hotfix), testes reais de backup/pendências, vários arquivos e Google, suporte de ODS, revisão editorial de 50 páginas traduzidas, assinatura/SmartScreen, OAuth público, central de ajuda.
- **Sem** inventar uma Release publicada ou inferir aprovação da 1.0.0.

Veja [checklist estável](PRE_RELEASE_CHECKLIST.md) e [CHANGELOG histórico](../CHANGELOG.md).
