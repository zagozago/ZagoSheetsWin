# Critérios de preparação da versão estável 1.0.0

**Estado:** checklist aberto, não autorização de lançamento. Revisado em 18/10/2026 com referência ao código **0.9.42** ([estado e CI](RELEASE_STATUS.md)). Itens sem evidência comprovada ficam **pendentes**. Uma execução aprovada do GitHub Actions não equivale a homologação end-to-end no Windows e no Google real.

| Área | Critério necessário | Estado nesta revisão |
|---|---|---|
| Versões | Aplicativo, catálogos, instalador e avisos com versão coerente | Código 0.9.42 coerente; **CI Windows do HEAD c1dc8a5 vermelho e distribuição protegida anterior falhou** |
| GitHub | Tag `v1.0.0`, Release, notas e ativos correspondentes a commit aprovado | **Pendente — não existem tags/Releases hoje** |
| OAuth | Revisão Google Cloud: Branding, Audience, Data Access e client desktop | **Pendente — depende do administrador Google** |
| Conta Google | Autorizar, renovar, revogar, desconectar, reconectar, segunda conta e permissões reais | **Pendente — teste manual necessário** |
| Importação | Exemplos reais de XLSX, CSV, TSV, XLS, ODS, interpretação TXT/CP1252, pipe/TAB/aspas/multilinhas, 60k/600k e fidelidade parcial | [Escopo B 0.9.36](IMPORT_REVIEW_0936.md): amostras locais e regressões passaram; **Google real e cenários gerais pendentes** |
| ODS | ODS real simples/complexo/inválido, caudas repetidas, fórmulas/mesclagens, preservação de bytes e riscos de conversão parcial | Parser esparso corrigido em 0.9.36; **ODS real no Google e outros nove ODS não fornecidos permanecem pendentes** |
| Proteção | Upload/retomada, arquivo modificado, readonly, cópia, snapshot, associação antiga, reparo sem alterar remoto | Regressões 0.9.36 com Google simulado e snapshots; **aceite completo Windows/Google real pendente** |
| Armazenamento | 30 dias/200 MB, limpeza obrigatoriamente automática por prazo/cota, autosave, rotação, restauração, corrida de quota, lote de 20 | **Rotação e espera 0.9.42 precisam CI protegido verde e aceite real** |
| Multilíngue | Conferir 51 opções, mensagens de importação/reparo/erros, RTL e DPI, revisão editorial completa | Verificações automatizadas e mensagens 0.9.36 existem; **50 READMEs traduzidos ainda requerem atualização editorial completa** |
| Dependências | Auditoria lockfile/runtime, licenças e elegibilidade de ImageSharp | Gate CI adicionado; **licença e pacote final a conferir** |
| Segurança | Verificação externa, OAuth e auditoria aplicável; nenhuma exposição de segredos | Revisão estática 0.9.30 e alterações posteriores verificadas no código; **não certificada** |
| Distribuição | Local core/Protected alpha distribution verdes no mesmo SHA final, hashes, Authenticode e Windows SmartScreen | **Bloqueado: CI Windows em `c1dc8a5` falhou e distribuição anterior `ceed7fb` falhou no watchdog de lote**; sem prova de assinatura |
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


## Bloqueadores adicionais da preparação 1.0 — 0.9.42

- [ ] **Resolver falha Windows do Local core 39189870249** no SHA `c1dc8a5` e documentar teste específico, causa e correção.
- [ ] **Aprovar Protected alpha distribution no SHA final**, incluindo o lote de 20 arquivos com cota extrema e watchdog atualizado; não aceitar o sucesso do Linux como substituto.
- [ ] Homologar **CSV/TSV v4 de até 2.000.000 células** e o arquivo real de **1.018.712 células** em Windows 11 + Google real (teste local já passou).
- [ ] Validar a regra **CSV/TSV: divergência de valores bloqueia retirada**, e **XLS/ODS: fidelidade parcial pode permitir substituição**.
- [ ] Testar lote com **arquivos diferentes importados em paralelo** quando há espaço e lote sob quota cheia; confirmar que a espera só ocorre em falta real de capacidade e que originais, atalhos e backups pendentes permanecem protegidos.
- [ ] Confirmar a limpeza **automática por prazo e quota sem toggle**, incluindo compatibilidade de configurações antigas, ordenação oldest-first, limite de 1 GB e casos que não podem ser liberados.
- [ ] Verificar o fluxo de `403 rateLimitExceeded`/`userRateLimitExceeded` vs `403 permissionDenied`, respeito a `Retry-After` e recuperação de resposta remota incerta.
- [ ] Rever 51 interfaces e traduzir editorialmente 50 READMEs; garantir informações de limite v4, backup automático e paralelismo, sem publicar informações obsoletas.
- [ ] Retestar ODS complexos/caudas/repetições com amostras reais e validar licenças, OAuth, assinatura e URL de ajuda no site.
