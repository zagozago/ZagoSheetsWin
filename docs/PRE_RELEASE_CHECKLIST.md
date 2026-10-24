# Critérios de preparação da versão estável 1.0.0

**Estado:** checklist aberto, não autorização de lançamento. Revisado em 24/10/2026 com referência ao código **0.9.48** ([estado e CI](RELEASE_STATUS.md)). Itens sem evidência comprovada ficam **pendentes**. Uma execução aprovada do GitHub Actions não equivale a homologação end-to-end no Windows e no Google real.

| Área | Critério necessário | Estado nesta revisão |
|---|---|---|
| Versões | Aplicativo, catálogos, instalador e avisos com versão coerente | **0.9.48** no projeto/instalador/catálogo 17; distribuição anterior aprovada, CI do hotfix RTL `08dbced` ainda precisa de conclusão |
| GitHub | Tag `v1.0.0`, Release, notas e ativos correspondentes a commit aprovado | **Pendente — não existem tags/Releases hoje** |
| OAuth | Revisão Google Cloud: Branding, Audience, Data Access e client desktop | **Pendente — depende do administrador Google** |
| Conta Google | Autorizar, renovar, revogar, desconectar, reconectar, segunda conta e permissões reais | **Pendente — teste manual necessário** |
| Importação | Exemplos reais de XLSX, CSV, TSV, XLS, ODS, interpretação TXT/CP1252, pipe/TAB/aspas/multilinhas, 60k/600k e fidelidade parcial | [Escopo B 0.9.36](IMPORT_REVIEW_0936.md): amostras locais e regressões passaram; **Google real e cenários gerais pendentes** |
| ODS | ODS real simples/complexo/inválido, caudas repetidas, fórmulas/mesclagens, preservação de bytes e riscos de conversão parcial | Parser esparso corrigido em 0.9.36; **ODS real no Google e outros nove ODS não fornecidos permanecem pendentes** |
| Proteção | Upload/retomada, arquivo modificado, readonly, cópia, snapshot, associação antiga, reparo sem alterar remoto | Regressões 0.9.36 com Google simulado e snapshots; **aceite completo Windows/Google real pendente** |
| Armazenamento | 30 dias/200 MB, **limpeza somente manual** por seleção ou Limpar expirados, salvamento numérico, quota fail-fast sem apagar originais, lote de 20 | **Regressões e distribuição protegida 0.9.43 aprovadas**; uso real e integridade/restauração em campo ainda pendentes |
| Multilíngue | Conferir 51 opções, mensagens de importação/reparo/erros, RTL e DPI, revisão editorial completa | CSV aprovado de **85×51 mensagens** validado, testes nativos 51 idiomas/4 DPI e 2 temas; hotfix RTL `08dbced` requer validação própria, e **50 READMEs traduzidos ainda precisam de revisão editorial** |
| Dependências | Auditoria lockfile/runtime, licenças e elegibilidade de ImageSharp | Gate CI adicionado; **licença e pacote final a conferir** |
| Segurança | Verificação externa, OAuth e auditoria aplicável; nenhuma exposição de segredos | Revisão estática 0.9.30 e alterações posteriores verificadas no código; **não certificada** |
| Distribuição | Local core/Protected alpha distribution verdes no mesmo SHA, hashes, Authenticode e Windows SmartScreen | **Local core #39346495162 e Protected alpha distribution #39346495298 verdes em `9fbec2a`**; ainda conferir artefato/hashes, assinatura e aceite real |
| Atualização | Atualização a partir de versões alfa usadas, preservação de estado e bloqueio de downgrade | Testes de instalador existem; **teste manual completo pendente** |
| Documentação | README vigente, 50 traduções sincronizadas, manuais, changelog e avisos legais | Consolidação proposta neste PR; **aprovação e traduções pendentes** |
| Ajuda web | Página real, URL validada por idioma e integração correta | Botão do aplicativo já contém URL; **publicação e disponibilidade não verificadas** |

## Diferenças acrescentadas para o aceite 0.9.36–0.9.37

- [ ] Testar no Google real as **oito amostras** documentadas no [relatório do Escopo B](IMPORT_REVIEW_0936.md), observando posições de células, acentos, `19,90`, aspas, 53.941 × 10 e limites novos.
- [ ] Testar ODS com quatro abas e caudas vazias sem confundir parser local aprovado com conversão Google concluída. Não alegar validação dos outros nove ODS não fornecidos.
- [ ] Testar a escolha de delimitador **por importação** e persistência da interpretação imutável após o vínculo remoto.
- [ ] Testar **Backups → Mais ações → Reimportar em novo documento** preservando Google Sheets antigo, link anterior e edições online.
- [ ] Testar mensagem de erro após minimizar/restaurar, progresso atrasado, correção do arquivo e os dois modos de diagnóstico (tentativa/histórico).
- [ ] Inspecionar **novo ícone comum** no executável, janelas, instalador, extensões associadas e atalhos `.url`, em diferentes tamanhos e escalas de Windows.
- [x] Confirmar que **Local core e Protected alpha distribution 0.9.37** concluíram com sucesso no mesmo SHA `f58b630`; é verificação automatizada, não aceite de campo.

## Procedimento de aceite de release

1. Fixar o commit candidato para testes (não usar automaticamente HEAD de uma branch mutável).
2. Rodar CI Windows/Linux, auditoria de dependências, verificação de recursos e pacote protegido no mesmo commit.
3. Validar checksum e manifesto de novo **pacote final ainda não aprovado** no SHA escolhido. Não reutilizar pacotes/hashes anteriores nem promover o HEAD com CI vermelho.
4. Testar em Windows 11 físico ou VM representativa com conta Google real, arquivos descartáveis e origem local.
5. Executar fluxos de falha, recuperação, edição online, reabertura por `.url`, importação como cópia, rede e OneDrive. **Incluir conversão parcial, aviso Backups/Não mostrar novamente, readonly, ambiguidades TAB/pipe/CP1252, correção de arquivo e reparo em documento novo sem alterar o anterior.**
6. Registrar resultados específicos por formato, principalmente ODS e XLS com fórmulas e valores divergentes. **Aceitar conscientemente que a fonte pode virar atalho mesmo com diferença no Sheets.**
7. Revisar privacidade, licença MIT original, licenças de terceiros e apresentação Zagotools.
8. Conferir documentação traduzida e central de ajuda, sem anunciar conteúdo que não existe.
9. Decidir versão de publicação, criar tag **somente após aceite**, e publicar Release real com hashes e limitações.
10. Atualizar o [CHANGELOG](../CHANGELOG.md) com a data e a URL oficiais verificadas.

O release só estará pronto quando os responsáveis registrarem evidência para os critérios obrigatórios. Não alterar o número de versão para simular conclusão.


## Aceite complementar antes da 1.0 — baseline 0.9.43

- [x] **Resolver falha SQLite/CI Windows da baseline 0.9.42:** corrigida pela abertura de schemas atualizados sem DDL repetida, documentada em [REGRESSION_0942.md](REGRESSION_0942.md); Local core 0.9.43 aprovado.
- [x] **Local core e Protected alpha distribution aprovados no commit 0.9.43 `9fbec2a`**; [CI](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) e [distribuição](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) passaram. Uma futura 1.0 exige revalidar seu próprio SHA de lançamento.
- [ ] Homologar **CSV/TSV v4 de até 2.000.000 células** e o arquivo real de **1.018.712 células** em Windows 11 + Google real (teste local já passou).
- [ ] Validar a regra **CSV/TSV: divergência de valores bloqueia retirada**, e **XLS/ODS: fidelidade parcial pode permitir substituição**.
- [ ] Testar lote real com **arquivos diferentes importados em paralelo** e quota cheia; confirmar **falha imediata com orientação de limpeza manual**, sem espera por capacidade/rotação/eliminação automática e sem alterar originais, atalhos ou backups anteriores.
- [ ] Confirmar a limpeza **somente manual**, inclusive após importação bem-sucedida; testar **Limpar expirados**, exclusão selecionada, compatibilidade com configurações antigas (`AutomaticCleanup=false`), quota de 1 GB e proteção de pendências.
- [ ] Verificar o fluxo de `403 rateLimitExceeded`/`userRateLimitExceeded` vs `403 permissionDenied`, respeito a `Retry-After` e recuperação de resposta remota incerta.
- [ ] Rever 51 interfaces e traduzir editorialmente 50 READMEs; garantir informações de limite v4, **backup exclusivamente manual**, falha rápida por quota, memória e paralelismo, sem publicar conteúdo obsoleto.
- [ ] Retestar ODS complexos/caudas/repetições com amostras reais e validar licenças, OAuth, assinatura e URL de ajuda no site.

## Verificações específicas dos candidatos 0.9.43–0.9.48

- [x] [Local core 39346495162](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) — Windows e Ubuntu verdes no commit `9fbec2a`.
- [x] [Protected alpha distribution 39346495298](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) — instalador e distribuição protegida verdes no **mesmo SHA**.
- [ ] Reproduzir no Windows do usuário a quota excedida por arquivo pequeno/médio, sem espera/polling nem rotação, e realizar a limpeza **manual** de um backup concluído.
- [ ] Testar lotes paralelos de 12/20 arquivos, inclusive 2 milhões de células e memória limitada, verificando que `OutOfMemoryException` oferece **Fechar** e não ocasiona exclusão da fonte.
- [ ] Testar a recuperação de upload com resposta ambígua, verificando que duas reconsultas não criam segundo Google Sheets.
- [ ] Conferir a altura reduzida **somente da janela Home** (500×192 de client size), minimizar/restaurar em 51 idiomas e DPI 100/125/150/200%, sem botões cortados.
- [ ] Confirmar no arquivo oficial os hashes/checksum e o estado de assinatura de publicador, além da efetiva distribuição fora do Actions quando planejada.

## Aceite final da interface 0.9.44–0.9.48

- [ ] Confirmar que abertura **independente** de operação pendente permite nova importação somente com remoto **comprovadamente ausente** após reconciliação; preservar backup, associação e histórico, e negar nova importação por erro de rede, resultado ambíguo, concorrência ou retry da mesma abertura.
- [ ] Comparar Home, Configurações, Opções avançadas, três etapas de primeiro uso, Recuperação e Sobre/Licenças com [0.9.47](implementation/refinement-20261024.md) e [0.9.48](implementation/final-ui-20261024.md).
- [ ] Validar catálogo de **85 textos × 51 idiomas** com placeholders, créditos MIT/upstream e títulos legais preservados.
- [ ] Testar lista com **313+ backups**: checkboxes espelham seleção real, seleção/limpeza em lotes de 32 com spinner, nenhuma perda de estado, sem checkbox extra no cabeçalho e sem exclusão indevida de pendentes.
- [ ] Inspecionar painéis inicialmente recolhidos (Configurações/Espaço utilizado/Dica), métricas reais, barra dentro do cartão, lateral 285 unidades, fonte do corpo -2 pt, prefixos em negrito e ícone da pasta.
- [ ] Confirmar seta de **Mais ações** alinhada à última palavra, mensagens de erro longas com quebra automática e telas 51 idiomas × 2 temas × DPI 100/125/150/200.
- [ ] **Hotfix RTL** `08dbced`: verificar recuos físicos esquerdo/direito em Home e Configurações, completar CI do commit e repetir distribuição protegida no mesmo SHA congelado caso seja promovido.
- [ ] Homologar com Google real o caso de backup pendente recuperado em uma nova abertura; CI nativo não substitui esse aceite.
