# Critérios de preparação da versão estável 1.0.0

**Estado:** checklist aberto, não autorização de lançamento. Revisado em 17/10/2026 com referência ao código **0.9.35** ([estado e CI](RELEASE_STATUS.md)). Itens sem evidência comprovada ficam **pendentes**. Uma execução aprovada do GitHub Actions não equivale a homologação end-to-end no Windows e no Google real.

| Área | Critério necessário | Estado nesta revisão |
|---|---|---|
| Versões | Aplicativo, catálogos, instalador e avisos com versão coerente | Código 0.9.35 coerente nos arquivos conferidos; **empacotamento final deve ter o mesmo commit** |
| GitHub | Tag `v1.0.0`, Release, notas e ativos correspondentes a commit aprovado | **Pendente — não existem tags/Releases hoje** |
| OAuth | Revisão Google Cloud: Branding, Audience, Data Access e client desktop | **Pendente — depende do administrador Google** |
| Conta Google | Autorizar, renovar, revogar, desconectar, reconectar, segunda conta e permissões reais | **Pendente — teste manual necessário** |
| Importação | Exemplos reais de XLSX, CSV, TSV, XLS e ODS, inclusive regressões e perda parcial de fidelidade | Regressões offline/Windows estão no código; **homologação Google real pendente** |
| ODS | ODS simples/complexo/inválido, fórmulas, mesclagens, dados e riscos de conversão parcial | **Pendente — desde 0.9.33, importações confirmadas podem substituir a origem mesmo com divergência** |
| Proteção | Interrupção de upload, navegador, snapshot, atalho, readonly, concorrência, restauração e conflito local | Teste nativo readonly adicionado em 0.9.35; **aceite real e caso só leitura pendentes** |
| Armazenamento | 30 dias/200 MB, limpeza opcional/1 GB, autosave de regras, ordenação das quatro colunas, restauração e desinstalação | Código e regressões automatizadas de 0.9.34; **validação em cenário real pendente** |
| Multilíngue | Conferir 51 opções, texto, fonte, RTL e DPI; revisão editorial adequada | Catálogos e verificações automáticas existem; **revisão editorial não certificada** |
| Dependências | Auditoria lockfile/runtime, licenças e elegibilidade de ImageSharp | Gate CI adicionado; **licença e pacote final a conferir** |
| Segurança | Verificação externa, OAuth e auditoria aplicável; nenhuma exposição de segredos | Revisão estática 0.9.30 e alterações posteriores verificadas no código; **não certificada** |
| Distribuição | Mesmo commit de produto/instalador/CI protegido, hash, manifesto, Authenticode e Windows SmartScreen | Protected 0.9.35 foi aprovado no SHA `70377cb`; HEAD `9929524` teve CI local aprovado. **Rodar distribuição no SHA congelado; assinatura não configurada** |
| Atualização | Atualização a partir de versões alfa usadas, preservação de estado e bloqueio de downgrade | Testes de instalador existem; **teste manual completo pendente** |
| Documentação | README vigente, 50 traduções sincronizadas, manuais, changelog e avisos legais | Consolidação proposta neste PR; **aprovação e traduções pendentes** |
| Ajuda web | Página real, URL validada por idioma e integração correta | Botão do aplicativo já contém URL; **publicação e disponibilidade não verificadas** |

## Procedimento de aceite de release

1. Fixar o commit candidato para testes (não usar automaticamente HEAD de uma branch mutável).
2. Rodar CI Windows/Linux, auditoria de dependências, verificação de recursos e pacote protegido no mesmo commit.
3. Validar checksum do artefato e manifesto do build; registrar o estado real de assinatura. **Não reutilizar o instalador 70377cb como prova de pacote do HEAD 9929524.**
4. Testar em Windows 11 físico ou VM representativa com conta Google real, arquivos descartáveis e origem local.
5. Executar fluxos de falha, recuperação, edição online, reabertura por `.url`, importação como cópia, rede e OneDrive. **Incluir substituição confirmada com perda parcial de fidelidade, aviso de Backups/Não mostrar novamente e erro de arquivo somente leitura com retomada segura.**
6. Registrar resultados específicos por formato, principalmente ODS e XLS com fórmulas e valores divergentes. **Aceitar conscientemente que a fonte pode virar atalho mesmo com diferença no Sheets.**
7. Revisar privacidade, licença MIT original, licenças de terceiros e apresentação Zagotools.
8. Conferir documentação traduzida e central de ajuda, sem anunciar conteúdo que não existe.
9. Decidir versão de publicação, criar tag **somente após aceite**, e publicar Release real com hashes e limitações.
10. Atualizar o [CHANGELOG](../CHANGELOG.md) com a data e a URL oficiais verificadas.

O release só estará pronto quando os responsáveis registrarem evidência para os critérios obrigatórios. Não alterar o número de versão para simular conclusão.
