# ZagoSheetsWin

Abre planilhas locais no Google Sheets e, após confirmação, substitui o original por um atalho na mesma pasta, com backup recuperável.

**Candidato atual: 0.9.32.** A publicação 1.0 depende do aceite final no Windows/Google real e das pendências externas de distribuição.

- [Alterações](CHANGELOG.md)
- [Revisão de segurança, OAuth e pendências externas](docs/SECURITY_REVIEW_0930.md)
- [Plano do projeto](docs/ALPHA_NEXT.md)

Para obter o pacote oficial, use o workflow **Protected alpha distribution**, na branch feature/zagosheetswin, após seus testes ficarem verdes. O Environment zagosheets-distribution exige aprovação. O artifact ZagoSheetsWin-Setup-win-x64 contém o instalador, SHA256SUMS.txt e build-info.json. Confira versão, commit e hash antes de executar.

Os builds comuns de CI validam código e interface com respostas Google simuladas. O build protegido adiciona a configuração OAuth desktop oficial. O instalador é por usuário e preserva dados locais ao atualizar/desinstalar.

Formatos: CSV, TSV, XLSX, XLS e ODS. O nome/extensão não prova o formato: HTML renomeado para ODS é recusado. Recursos sem verificação de fidelidade podem exigir importação como cópia, preservando o original. O produto não sincroniza edições do Sheets de volta ao arquivo local.

Os scripts PowerShell herdados são referência histórica; o fluxo atual usa o aplicativo C#.

## Relationship with Open in Google

Based on / derived from [Open in Google](https://github.com/SwatiK425/open-in-google), de Swati K. Base auditada: `124419b9ffce1f42c696ab9068fa9db6a4a9c199`. Os três scripts, SETUP.md, LICENSE e .gitignore foram preservados nesta etapa. O README original, com links ajustados à pasta de arquivo, está em docs/upstream/README.original.md. A licença MIT e o copyright original permanecem em LICENSE.

A evolução pertence ao fork e não depende de PRs aceitos. Correções genéricas poderão voltar ao upstream em branches independentes.

## Licença, termos e privacidade

ZagoSheetsWin é distribuído sob MIT, preservando o copyright e a licença de
Open in Google / Swati K e identificando separadamente as modificações do
Zagotools. Consulte [LICENSE](LICENSE), [ATTRIBUTION.md](ATTRIBUTION.md),
[NOTICE.md](NOTICE.md) e [third-party/NOTICE.md](third-party/NOTICE.md).

Na conexão Google, o aplicativo solicita somente
`https://www.googleapis.com/auth/drive.file`. Tokens ficam localmente,
protegidos por Windows DPAPI, e as planilhas são enviadas diretamente às APIs
do Google para executar a função solicitada; elas não passam por servidor do
Zagotools.

- Política de Privacidade: https://zagotools.top/legal.html#privacidade
- Termos de Uso: https://zagotools.top/legal.html#termos
- Licenciamento Zagotools: https://zagotools.top/legal.html#licencas

## Documentação

- [Auditoria](docs/AUDIT.md)
- [Arquitetura](docs/ARCHITECTURE.md)
- [Decisões](docs/adr/DECISIONS.md)
- [Roadmap](docs/ROADMAP.md)
- [Desenvolvimento e fork](docs/DEVELOPMENT.md)
- [Conclusão da etapa 1](docs/STAGE_1.md)
- [Núcleo local e testes — etapa 2](docs/STAGE_2.md)
- [OAuth, importação e piloto — etapa 3](docs/STAGE_3.md)
- [Atalho, retirada e recuperação — etapa 4](docs/STAGE_4.md)
- [Launcher e Abrir com — etapa 5](docs/STAGE_5.md)
- [Instalação, configuração e restauração — etapa 6](docs/STAGE_6.md)
- [Formatos e ambientes — etapa 7](docs/STAGE_7.md)
- [Robustez, recuperação e atualização — etapa 8](docs/STAGE_8.md)
- [Consolidação ZagoSheetsWin e piloto final — etapa 9](docs/STAGE_9.md)
- [Autoria e licença](ATTRIBUTION.md)

O MVP inicial cobre XLSX em pasta local não sincronizada, com OAuth, snapshot, backup, journal, lock, conversão verificada e atalho .url. A fase 7 amplia formatos e adiciona cópia de origens compartilhadas conforme a matriz do guia; a retirada nesses ambientes permanece bloqueada. Não há sincronização bidirecional. O backup guarda os bytes da importação inicial, não edições online futuras.

### Piloto instalável — fase 6

A branch `feature/installable-pilot` produz o instalador por usuário **SheetsWindows-Setup-win-x64** no GitHub Actions. [Pacote histórico 0.6](https://github.com/zagozago/ZagoSheetsWin/actions/runs/36866831248/artifacts/11163847812). Configuração visual, OAuth, escolha do padrão e restauração offline estão no aplicativo. [Guia de instalação e recuperação](docs/STAGE_6.md). A desinstalação conserva backups, configuração, atalhos e documentos Google. O aceite manual completo no Windows 11 com Google real ainda está pendente.

### Novos formatos — fase 7

Usar a branch `feature/formats-environments` e habilitar os formatos na configuração visual. [Baixar o instalador 0.7 validado](https://github.com/zagozago/ZagoSheetsWin/actions/runs/36875411056/artifacts/11168732158). [Matriz de comportamento e limites](docs/STAGE_7.md). A retirada automática em XLS, ODS complexos, OneDrive e rede permanece bloqueada; esses casos usam cópia com original preservado.

### Robustez — fase 8

Usar a branch `feature/robustness`, versão 0.8. [Baixar instalador 0.8 aprovado](https://github.com/zagozago/ZagoSheetsWin/actions/runs/36882411612/artifacts/11171354492). [Retomada, matriz de falhas e atualização](docs/STAGE_8.md). Sessões de upload são protegidas por DPAPI; a recuperação reaproveita a operação existente. Diagnósticos têm limite de 256 KiB e não incluem caminhos, contas ou conteúdo. O instalador preserva os dados e bloqueia downgrade. O teste manual com Google real continua pendente.

### ZagoSheetsWin — etapa 9

Versão 0.9.1 na branch `feature/zagosheetswin`. [Baixar instalador aprovado](https://github.com/zagozago/ZagoSheetsWin/actions/runs/36926691912/artifacts/11193993461). CI Windows/Linux aprovado, incluindo instalação/atualização/remoção e revisão visual das quatro telas. Logos e créditos incorporados ao instalador e às telas; Sobre / MIT identifica Open in Google e Swati K (SwatiK425). O nome novo preserva a instalação/estado anteriores por compatibilidade. [Pacote e roteiro final](docs/STAGE_9.md). A licença MIT original permanece íntegra.

### XLS local (0.9.13)

Arquivos XLS são convertidos localmente em XLSX com NPOI 2.7.6 antes da importação. O backup conserva o XLS original; ExcelDataReader continua responsável pela leitura e conferência. Fórmulas não verificáveis e recursos complexos preservam o original. Consulte [limitações da conversão](docs/XLS_CONVERSION.md) e [licenças das dependências](third-party/NOTICE.md).

Desde 0.9.15, relatórios HTML salvos com extensão `.xls` também são reconhecidos e convertidos; filtros externos à tabela ficam na aba Informações. O XLS binário continua utilizando NPOI.

## 0.9.16 — Ajuda e configuração simplificadas

O tutorial usa quatro passos ilustrados. As configurações seguem Conta Google, Aplicativos padrão e Pastas sincronizadas (opcional). Novas instalações podem abrir arquivos locais sem escolher uma pasta; a confirmação de substituição com backup continua necessária. Instalações existentes preservam a pasta anterior até alteração explícita. Arquivos em pastas sincronizadas identificadas ou de rede são importados como cópia.

A conexão distingue autorização salva, conexão verificada e necessidade de nova autorização. Em Backups, marque vários arquivos ou use Selecionar todos para apagar os concluídos após confirmação; operações pendentes continuam protegidas.
