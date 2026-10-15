# Changelog — ZagoSheetsWin

As alterações significativas são registradas aqui em ordem cronológica inversa, seguindo as categorias **Added**, **Changed**, **Fixed**, **Security** e **Known limitations** do [Keep a Changelog](https://keepachangelog.com/en/1.1.0/). As versões anteriores a 1.0.0 são **marcos do desenvolvimento alfa**, não GitHub Releases. Este histórico foi revisado em **15/10/2026** usando commits, código, documentos de fase e metadados do repositório.

**Não há tags de versão nem Releases publicadas no GitHub nesta data.** GitHub Actions produz candidatos e artefatos temporários, mas uma execução bem-sucedida não constitui lançamento público nem aceite com Google real. Não confundir versão declarada no código com Release publicada.

## [Unreleased] — preparação para 1.0.0

- **Documentation:** consolidar README internacional, guias atuais, histórico, formatos, recuperação, segurança e checklist de homologação.
- **Pending:** homologar fluxos reais de OAuth/Drive no Windows, comportamento de ODS complexos, critérios de fidelidade, revisão editorial dos idiomas, configuração de distribuição pública, elegibilidade de licenças e eventual assinatura do instalador.
- **Not done:** publicar 1.0.0, criar tag ou Release, certificar segurança, afirmar suporte universal a ODS.
- Alterações de documentação **não** aumentam a versão do aplicativo.

## [0.9.30] — candidato alfa, 15/10/2026

Base observada: [6a019318](https://github.com/zagozago/ZagoSheetsWin/commit/6a019318f64c1068ebd5ece4932320535b24f629); revisão técnica em [SECURITY_REVIEW_0930.md](docs/SECURITY_REVIEW_0930.md).

### Fixed
- Validar assinatura e `mimetype` de ODS; rejeitar arquivos HTML renomeados como `.ods`.
- Corrigir verificações de integridade e limites de expansão de pacotes compactados.
- Unificar versão do aplicativo, catálogo e instalador no pipeline de distribuição.
- Revisar rótulos e fluxos de recuperação, incluindo restauração e exclusão de backups.

### Security
- Endurecer processamento de callback/respostas OAuth, validação de escopos, revogação e configuração do cliente desktop.
- Fixar dependências corrigidas da família .NET 10.0.13 e reforçar auditoria de dependências; workflow de distribuição usa SDK 10.0.402.
- Produzir informações de build, checksum SHA-256 e evidência do estado de assinatura; checksum não substitui Authenticode.

### Changed
- Sugerir nome de exportação JSONL com basename local sanitizado (o nome do arquivo exportado pode revelar o documento de origem; o conteúdo do diagnóstico mantém sua política de exclusão de dados).

### Known limitations
- ODS com fórmulas/recursos não verificáveis continua conservador, podendo exigir modo de cópia. A validação estrutural de dois exemplos não representa cobertura geral de ODS.
- Sem canal de atualização automática assinado e sem comprovação de homologação completa com o Google real.

## [0.9.29] — candidatos alfa, 11–14/10/2026

Commits selecionados: [bcd2bb3](https://github.com/zagozago/ZagoSheetsWin/commit/bcd2bb3dac), [f06814a](https://github.com/zagozago/ZagoSheetsWin/commit/f06814a7a3), [f76ea12](https://github.com/zagozago/ZagoSheetsWin/commit/f76ea12fe06d9c19a8ed035045d7df398ac16d69).

### Added
- Configurações e Opções avançadas reorganizadas, persistência automática de preferências e ajuda contextual.
- Assistente de primeira utilização com três páginas; tutorial independente com quatro páginas.
- Diagnóstico de etapas, duração, validações remotas limitadas e contexto da tentativa atual.

### Changed
- Verificação de conexão, apresentação do e-mail autorizado quando disponível, desconexão e reconexão Google.
- Redirecionamento de abertura não autorizada à configuração, sem iniciar a importação pendente.

### Fixed
- Recuperação de falhas transitórias com nova tentativa delimitada.
- Nova importação de arquivo local quando a planilha associada está **confirmadamente na lixeira**, preservando histórico e associação anterior; falhas de acesso/404 não autorizam reupload.
- Correções na interface WinForms, estados e exportação segura de diagnósticos.

**Observação histórica:** uma execução de distribuição nesta fase combinou aplicativo 0.9.29 e instalador 0.9.28; a correção foi incorporada ao candidato 0.9.30.

## [0.9.23–0.9.28] — outubro de 2026

Marcos: [0.9.23](docs/ALPHA_0_9_23.md), [assistente de idiomas e retries](https://github.com/zagozago/ZagoSheetsWin/commit/2485609d8c), [reorganização do primeiro uso](https://github.com/zagozago/ZagoSheetsWin/commit/43eca48672).

- Seletor de idiomas do instalador em ordem canônica, com passagem da preferência ao aplicativo.
- Retomada e repetição controlada de consultas Drive; operações de criação não são repetidas cegamente.
- Preferências de atalho/cópia, telas nativas, ajustes de recuperação e validação de interface.

## [0.9.20–0.9.22] — outubro de 2026

Evidências: [integração de 51 idiomas](https://github.com/zagozago/ZagoSheetsWin/commit/fbc168474d), [integração documentada](docs/I18N_INTEGRATION.md).

- Catálogos internacionais, fallback, ordem canônica T51, interface e instalador com 51 opções.
- Suporte de layout RTL onde aplicável, validações de catálogos e controles nativos.
- Tradução presente no pacote não implica revisão editorial concluída para cada idioma.

## [0.9.13–0.9.19] — outubro de 2026

Documentos de referência: [XLS_CONVERSION](docs/XLS_CONVERSION.md), [histórico alfa](docs/HISTORY.md).

- Conversão local de XLS binário usando NPOI e verificação de valores com ExcelDataReader; recursos avançados não verificáveis mantêm o original.
- Reconhecimento de relatórios HTML gravados com extensão `.xls`, sem executar scripts.
- Evolução de interface, tutorial, preferências, conta Google e créditos/licenças.

## [0.9.1–0.9.12] — outubro de 2026

Evidências: [etapa 9](docs/STAGE_9.md), [CSV/limites 0.9.2](docs/ALPHA_1_CSV.md), [XLS 0.9.3](docs/ALPHA_2_XLS.md), [backups 0.9.6](docs/ALPHA_6_BACKUPS.md).

- Identidade ZagoSheetsWin/Zagotools, aplicação Windows instalável, Google OAuth, backup e atalho `.url`.
- Ampliação de capacidade para **20 MiB**, **500.000 células**, **50.000 linhas** e **1.000 colunas**.
- Política de retenção padrão 30 dias e cota de 200 MB (até 1.000 MB), limpeza opcional mediante consentimento.
- Distribuição alfa com OAuth desktop embutido via ambiente protegido do GitHub Actions.

## [0.1–0.9.0] — fundação e fases iniciais, 01/10/2026

Evidências: [auditoria](docs/AUDIT.md), [decisões](docs/adr/DECISIONS.md), [fases 1–9](docs/HISTORY.md).

- Fork independente do projeto Open in Google de Swati K; copyright e MIT preservados.
- Arquitetura C#/.NET, SQLite e journal, importação via Drive, associação de arquivos Windows e recuperação segura.
- Histórico inicial incluiu especificações e pilotos que não devem substituir o código atual como referência operacional.

---

### Regras para próximas versões

1. Inserir funcionalidades novas primeiro em **Unreleased** somente após implementadas ou indicar explicitamente `Planned`.
2. Ao criar uma versão, registrar data **verificada**, commit, tag/Release **se existirem**, alterações, migração e limitações.
3. Não antecipar `[1.0.0]` como lançamento antes de publicar a tag e a Release reais.
4. Não registrar sucesso de CI como equivalência a teste de ponta a ponta com conta Google real.
5. Preservar crédito ao Open in Google e avisos de terceiros.
