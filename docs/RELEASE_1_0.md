# ZagoSheetsWin 1.0 — preparação e conferência

Data: 2026-10-25. Distribuição decidida pelo mantenedor: EXE tradicional, por usuário, no GitHub Releases, sem assinatura Authenticode. Microsoft Store/MSIX fica fora deste lançamento. O site é tratado separadamente pelo mantenedor.

## Base e escopo

- Base funcional: feature/zagosheetswin em ee09a9065047a5840a404c55f3fd4b717ec897c3 (0.9.48), testes manuais concluídos segundo o mantenedor.
- Base documental: main em 057ef77e0e79af197bb2c6c3f7b28fee1b8c0b02. Os 50 READMEs traduzidos e o seletor de 51 idiomas são preservados. As traduções documentais contêm contexto histórico; o README raiz e as notas da Release indicam a distribuição atual.
- Nesta preparação: apenas versão, recursos gerados com nova versão, documentação e comentário de identificação do workflow. Não mudar dependências, algoritmos, preferências, cliente OAuth, AppId do instalador, caminhos de dados ou associações.
- O workflow continua com nome histórico Protected alpha distribution e o mesmo Environment/secret. Nenhuma proteção é removida.

## Verificações de saída

- [ ] Catálogo, CSV autorizado, contratos e auditoria de versões/segredos aprovados no commit final.
- [ ] Local core: Linux e Windows aprovados, incluindo interface nativa e idiomas.
- [ ] Build protegido no commit final, com configuração OAuth oficial existente e auditoria transitiva de dependências.
- [ ] Instalação, atualização e desinstalação aprovadas pelo workflow; preservação de configurações, backups, journal e atalhos.
- [ ] Instalador final: versão 1.0.0, Authenticode NotSigned, SHA-256 e build-info.json correspondentes aos mesmos bytes/commit/run.
- [ ] Conferir o eventual alerta explícito de malware reportado anteriormente: não atribuí-lo automaticamente à ausência de assinatura. Não recomendar desativar segurança.
- [ ] Registrar limitações: CI usa Google simulado; aceite funcional Google/Windows real é do mantenedor. O fixture de upgrade do CI usa versão antiga sintética do instalador e não substitui evidência manual de upgrade da instalação anterior real.
- [ ] Consolidar main com os commits aprovados, sem reescrever histórico; definir tag v1.0.0 no commit verificado.
- [ ] Preparar Release com EXE, SHA256SUMS.txt e build-info.json. Não recompilar após calcular os hashes.
- [ ] Aprovação final do mantenedor para tornar a Release pública, depois da conferência dos artefatos.

## Notas da Release — minuta

Primeira versão pública estável do ZagoSheetsWin para Windows 11 x64. Abre XLSX, XLS, ODS, CSV e TSV no Google Sheets, com configuração inicial e interface em 51 idiomas.

Após importação confirmada, o original pode ser substituído por um atalho .url na mesma pasta, preservando cópia recuperável conforme as preferências. Não há sincronização das alterações online de volta ao arquivo local. Conversões podem alterar recursos não suportados pelo Google Sheets.

Instalador por usuário, sem assinatura Authenticode (NotSigned). Avisos de publicador desconhecido/reputação podem ocorrer. Confira procedência, versão e SHA-256; assinatura ou hash não garantem ausência de vulnerabilidades. Não desative mecanismos de segurança. Relate detecções explícitas de malware para investigação.

Arquivos esperados: ZagoSheetsWin-Setup-win-x64.exe, SHA256SUMS.txt e build-info.json. O manifesto identifica o commit e a execução do build oficial. Atualizações são feitas com novo instalador oficial. Desinstalação preserva dados de recuperação e não apaga documentos Google.

Licença MIT, com atribuições e licenças de terceiros incluídas no pacote. Somente escopo Google drive.file; tokens locais protegidos por DPAPI, planilhas enviadas diretamente ao Google.
