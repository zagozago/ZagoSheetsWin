# Critérios de preparação da versão estável 1.0.0

**Estado:** checklist aberto, não autorização de lançamento. Revisado em 18/10/2026 com referência ao código **0.9.37** ([estado e CI](RELEASE_STATUS.md)). Itens sem evidência comprovada ficam **pendentes**. Uma execução aprovada do GitHub Actions não equivale a homologação end-to-end no Windows e no Google real.

| Área | Critério necessário | Estado nesta revisão |
|---|---|---|
| Versões | Aplicativo, catálogos, instalador e avisos com versão coerente | 0.9.37 nos arquivos verificados; **CI local e distribuição protegida passaram no mesmo SHA f58b630** |
| GitHub | Tag `v1.0.0`, Release, notas e ativos correspondentes a commit aprovado | **Pendente — não existem tags/Releases hoje** |
| OAuth | Revisão Google Cloud: Branding, Audience, Data Access e client desktop | **Pendente — depende do administrador Google** |
| Conta Google | Autorizar, renovar, revogar, desconectar, reconectar, segunda conta e permissões reais | **Pendente — teste manual necessário** |
| Importação | Exemplos reais de XLSX, CSV, TSV, XLS, ODS, interpretação TXT/CP1252, pipe/TAB/aspas/multilinhas, 60k/600k e fidelidade parcial | [Escopo B 0.9.36](IMPORT_REVIEW_0936.md): amostras locais e regressões passaram; **Google real e cenários gerais pendentes** |
| ODS | ODS real simples/complexo/inválido, caudas repetidas, fórmulas/mesclagens, preservação de bytes e riscos de conversão parcial | Parser esparso corrigido em 0.9.36; **ODS real no Google e outros nove ODS não fornecidos permanecem pendentes** |
| Proteção | Upload/retomada, arquivo modificado, readonly, cópia, snapshot, associação antiga, reparo sem alterar remoto | Regressões 0.9.36 com Google simulado e snapshots; **aceite completo Windows/Google real pendente** |
| Armazenamento | 30 dias/200 MB, limpeza opcional/1 GB, autosave de regras, ordenação das quatro colunas, restauração e desinstalação | Código e regressões automatizadas de 0.9.34; **validação em cenário real pendente** |
| Multilíngue | Conferir 51 opções, mensagens de importação/reparo/erros, RTL e DPI, revisão editorial completa | Verificações automatizadas e mensagens 0.9.36 existem; **50 READMEs traduzidos ainda requerem atualização editorial completa** |
| Dependências | Auditoria lockfile/runtime, licenças e elegibilidade de ImageSharp | Gate CI adicionado; **licença e pacote final a conferir** |
| Segurança | Verificação externa, OAuth e auditoria aplicável; nenhuma exposição de segredos | Revisão estática 0.9.30 e alterações posteriores verificadas no código; **não certificada** |
| Distribuição | Mesmo commit de produto/instalador/CI protegido, hash, manifesto, Authenticode e Windows SmartScreen | **CI Local core e Protected alpha distribution 0.9.37 passaram no mesmo SHA `f58b630`**. Assinatura de publicador e aceitação de distribuição ampla não comprovadas |
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
3. Validar checksum e manifesto do **pacote 0.9.37 no SHA `f58b630`**; conferir executável/instalador e o estado real da assinatura. **Não utilizar checksums de builds anteriores**.
4. Testar em Windows 11 físico ou VM representativa com conta Google real, arquivos descartáveis e origem local.
5. Executar fluxos de falha, recuperação, edição online, reabertura por `.url`, importação como cópia, rede e OneDrive. **Incluir conversão parcial, aviso Backups/Não mostrar novamente, readonly, ambiguidades TAB/pipe/CP1252, correção de arquivo e reparo em documento novo sem alterar o anterior.**
6. Registrar resultados específicos por formato, principalmente ODS e XLS com fórmulas e valores divergentes. **Aceitar conscientemente que a fonte pode virar atalho mesmo com diferença no Sheets.**
7. Revisar privacidade, licença MIT original, licenças de terceiros e apresentação Zagotools.
8. Conferir documentação traduzida e central de ajuda, sem anunciar conteúdo que não existe.
9. Decidir versão de publicação, criar tag **somente após aceite**, e publicar Release real com hashes e limitações.
10. Atualizar o [CHANGELOG](../CHANGELOG.md) com a data e a URL oficiais verificadas.

O release só estará pronto quando os responsáveis registrarem evidência para os critérios obrigatórios. Não alterar o número de versão para simular conclusão.
