# Critérios de preparação da versão estável 1.0.0

**Estado:** checklist aberto, não autorização de lançamento. Revisado em 15/10/2026 com referência ao código 0.9.30. Itens sem evidência comprovada ficam **pendentes**. Uma execução aprovada do GitHub Actions não equivale a homologação end-to-end no Windows e no Google real.

| Área | Critério necessário | Estado nesta revisão |
|---|---|---|
| Versões | Aplicativo, catálogos, instalador e avisos com versão coerente | Corrigido no candidato 0.9.30; verificar artefato final |
| GitHub | Tag `v1.0.0`, Release, notas e ativos correspondentes a commit aprovado | **Pendente — não existem tags/Releases hoje** |
| OAuth | Revisão Google Cloud: Branding, Audience, Data Access e client desktop | **Pendente — depende do administrador Google** |
| Conta Google | Autorizar, renovar, revogar, desconectar, reconectar, segunda conta e permissões reais | **Pendente — teste manual necessário** |
| Importação | Exemplos reais de XLSX, CSV, TSV, XLS e ODS, inclusive regressões | **Pendente — evidência de campo parcial** |
| ODS | ODS simples, complexo, inválido, fórmulas, mesclagens e perdas; política de cópia | **Pendente — não declarar suporte universal** |
| Proteção | Interrupção de upload, erro no navegador, snapshot, atalho, concorrência, restauração e conflito local | Testes simulados existem; **aceite em Windows real pendente** |
| Armazenamento | Política de 30 dias/200 MB, limpeza opcional, limite 1 GB, restauração e desinstalação | Código e testes existem; **validação manual pendente** |
| Multilíngue | Conferir 51 opções, texto, fonte, RTL e DPI; revisão editorial adequada | Catálogos e verificações automáticas existem; **revisão editorial não certificada** |
| Dependências | Auditoria lockfile/runtime, licenças e elegibilidade de ImageSharp | Gate CI adicionado; **licença e pacote final a conferir** |
| Segurança | Verificação externa, OAuth e auditoria aplicável; nenhuma exposição de segredos | Revisão estática 0.9.30; **não certificada** |
| Distribuição | Checksum, manifesto, decisão de assinatura Authenticode e Windows SmartScreen | Hash/manifesto implementados; **assinatura não configurada** |
| Atualização | Atualização a partir de versões alfa usadas, preservação de estado e bloqueio de downgrade | Testes de instalador existem; **teste manual completo pendente** |
| Documentação | README vigente, 50 traduções sincronizadas, manuais, changelog e avisos legais | Consolidação proposta neste PR; **aprovação e traduções pendentes** |
| Ajuda web | Página real, URL validada e idioma, PR de navegação decidido | **Pendente — URL proposta não é página verificada** |

## Procedimento de aceite de release

1. Fixar o commit candidato para testes (não usar automaticamente HEAD de uma branch mutável).
2. Rodar CI Windows/Linux, auditoria de dependências, verificação de recursos e pacote protegido no mesmo commit.
3. Validar checksum do artefato e manifesto do build; registrar o estado real de assinatura.
4. Testar em Windows 11 físico ou VM representativa com conta Google real, arquivos descartáveis e origem local.
5. Executar fluxos de falha, recuperação, edição online, reabertura por `.url`, importação como cópia, rede e OneDrive.
6. Registrar resultados específicos por formato, principalmente ODS e XLS com fórmulas.
7. Revisar privacidade, licença MIT original, licenças de terceiros e apresentação Zagotools.
8. Conferir documentação traduzida e central de ajuda, sem anunciar conteúdo que não existe.
9. Decidir versão de publicação, criar tag **somente após aceite**, e publicar Release real com hashes e limitações.
10. Atualizar o [CHANGELOG](../CHANGELOG.md) com a data e a URL oficiais verificadas.

O release só estará pronto quando os responsáveis registrarem evidência para os critérios obrigatórios. Não alterar o número de versão para simular conclusão.
