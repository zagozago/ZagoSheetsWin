# Changelog — ZagoSheetsWin

As alterações significativas são registradas aqui em ordem cronológica inversa, seguindo as categorias **Added**, **Changed**, **Fixed**, **Security** e **Known limitations** do [Keep a Changelog](https://keepachangelog.com/en/1.1.0/). As versões anteriores a 1.0.0 são **marcos do desenvolvimento alfa**, não GitHub Releases. Este histórico foi revisado em **18/10/2026** usando commits, código, documentos de fase e metadados do repositório.

**Não há tags de versão nem Releases publicadas no GitHub em 18/10/2026.** GitHub Actions produz candidatos e artefatos temporários, mas uma execução bem-sucedida não constitui lançamento público nem aceite com Google real. Não confundir versão declarada no código com Release publicada.

## [Unreleased] — preparação para 1.0.0

- **Documentation:** consolidar README internacional, guias atuais, histórico, formatos, recuperação, segurança e checklist de homologação.
- **Pending:** homologar fluxos reais de OAuth/Drive no Windows, compatibilidade de ODS complexos e importações com conversão parcial, validar reparo e seleção de interpretador com conta Google real, revisão editorial dos READMEs internacionais, elegibilidade de licenças e eventual assinatura do instalador.
- **Not done:** publicar 1.0.0, criar tag ou Release, certificar segurança, afirmar suporte universal a ODS.
- Alterações de documentação **não** aumentam a versão do aplicativo.

## [0.9.37] — candidato alfa, 18/10/2026

Commit: [f58b630](https://github.com/zagozago/ZagoSheetsWin/commit/f58b630263cdee07e473a54ee40ff6d3beafe4f6).

### Changed
- Uniformizar **ícone do programa, janelas e instalador** com o ícone de planilha fornecido pelo usuário. O ICO inclui frames de 16, 24, 32, 48, 64, 128 e 256 pixels.
- Reutilizar o mesmo desenho já empregado nos arquivos associados e nos atalhos de planilha. Manter as marcas do cabeçalho Zagotools e as atribuições Open in Google/Swati K intactas.
- Atualizar `branding/app-icon-source.png`, `branding/zagosheetswin.ico`, versão do catálogo e fallback do Inno Setup para 0.9.37.

### Verificação
- [Local core 0.9.37](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39099713350): **sucesso**.
- [Protected alpha distribution 0.9.37](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39099713370): **sucesso** no **mesmo commit f58b630**.
- Testes automatizados e build protegido não equivalem à homologação de conteúdo e importação com conta Google real.

## [0.9.36] — candidato alfa, 17–18/10/2026

Commits principais: [ae27b88](https://github.com/zagozago/ZagoSheetsWin/commit/ae27b8819d50815c96cb51681bcbf4f051373c45), [4bceb7a](https://github.com/zagozago/ZagoSheetsWin/commit/4bceb7aaac739c2dbd13ab7730fe292f0bf3b673), [3aae63c](https://github.com/zagozago/ZagoSheetsWin/commit/3aae63c1397f716abe9eeee7edb8722e4adff6d4), [f6b6a4f](https://github.com/zagozago/ZagoSheetsWin/commit/f6b6a4f4852b00e0dc07a565fbe70092bf2e92bb). Relatório detalhado: [Escopo B](docs/IMPORT_REVIEW_0936.md).

### Added
- **CSV/TSV:** detecção de `,`, `;`, TAB e `|` pelo conteúdo, interpretação manual **por importação** quando a leitura é ambígua, suporte a aspas/multilinhas e fallback controlado para Windows-1252 quando UTF-8 estrito falha em arquivo sem BOM.
- **Interpretação versionada:** versão e opções de leitura gravadas por operação, com compatibilidade para operações anteriores que já possuem intenção remota. Não alterar silenciosamente o significado de um documento existente.
- **Reimportar em novo documento:** reparo explícito a partir do backup SHA-256 verificado, com nova identidade e documento Google; **não sobrescreve o documento, atalho, operação ou histórico anteriores**.
- **Diagnósticos:** códigos de capacidade/estrutura mais específicos; exportação da tentativa atual isolada do histórico completo.

### Fixed
- Em ODS, calcular coordenadas sem expandir longas caudas vazias repetidas; manter as posições das células relevantes e enviar os **bytes originais** do ODS.
- Preservar mensagem terminal de erro e sua altura após minimizar/restaurar a janela modal; impedir que atualizações de progresso atrasadas substituam a falha exibida.
- Evitar repetição automática de erros permanentes de entrada. Para esses erros, reabilitar tentativa manual depois que o arquivo muda; em ambiguidade CSV, abrir escolha do interpretador.
- Para arquivo local corrigido **sem intenção remota**, permitir nova operação mantendo snapshot/histórico anteriores. Se há intenção remota, reconciliar em vez de recriar cegamente.
- Ajustar limites **somente para CSV/TSV v2**: até **60.000 registros lógicos** e **600.000 células**, inclusive na verificação do XLSX exportado. Demais formatos conservam os limites anteriores.
- Instrumentar o ciclo de instalação/reinstalação com logs e códigos de saída, após uma ocorrência anterior não reproduzida; **não declarar causa confirmada para a falha antiga**.

### Verificação
- [Local core](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39087900152) e [Protected alpha distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39087900310) do commit **f6b6a4f**: **sucesso**.
- Revisão Scope B registra no Windows Release **338 testes aprovados, 1 não executado**; Linux **297 aprovados, 42 não executados** no [CI 39087296881](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39087296881).
- Amostras locais reais de texto e um ODS fornecido foram conferidos **sem chamadas Google**. Outros arquivos ODS e a conversão real permanecem pendentes. Nenhuma certificação de compatibilidade ou segurança é inferida.

## [0.9.35] — candidato alfa, 17/10/2026

Commit de produto: [70377cb](https://github.com/zagozago/ZagoSheetsWin/commit/70377cb17cd515149c03777aae1bb4fd87a2a11b), com verificação de layout adicional em [9929524](https://github.com/zagozago/ZagoSheetsWin/commit/9929524e67fea42c8940ae27b41f80963479c490).

### Fixed
- Mensagem genérica em **51 idiomas** inclui a possibilidade de arquivo marcado como **somente leitura**. O Windows pode negar a retirada nessa condição; a operação falha de forma recuperável, mantendo o original e o backup.
- Novo teste Windows verifica erro nativo de acesso negado, preservação de bytes/atributo de somente leitura e retomada segura **depois que o atributo é removido pelo usuário**.
- Verificação nativa de layout garante que a mensagem ampliada caiba na tela de erro nos 51 idiomas.

### Verification status
- [Local core 0.9.35 / commit 9929524](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39059144987): Windows e Ubuntu **concluídos com sucesso**.
- [Protected alpha distribution 0.9.35 / commit 70377cb](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39059101337): execução protegida **concluída com sucesso**, anterior ao commit de verificação visual 9929524.
- A execução protegida e o HEAD da branch **não são o mesmo SHA**; produzir pacote no commit definitivo antes da futura Release. Esses testes automatizados não substituem a homologação com Google real.

## [0.9.34] — candidato alfa, 17/10/2026

Commits: [961a180](https://github.com/zagozago/ZagoSheetsWin/commit/961a180ceda24cc9a54df7c91a7f37cf43f8563f) e [eb9c137](https://github.com/zagozago/ZagoSheetsWin/commit/eb9c1371ecfad1271c7f738f01bd2935de4d41cd).

### Changed
- Depois de **cada substituição concluída**, exibir confirmação em vez de um alerta exclusivo para conversão parcial: a mensagem informa que o original foi substituído por atalho e oferece acesso a **Backups**.
- Adicionar **Não mostrar novamente**, persistido por usuário; impedir sobreposição de múltiplas confirmações simultâneas.
- Salvar automaticamente retenção, quota e limpeza na tela de backups, inclusive edições feitas antes de um salvamento anterior terminar.
- Remover caixas de seleção das linhas de backup; seleção múltipla direta e **ordenação ascendente/descendente por Arquivo, Data, Tamanho e Estado**, com data/tamanho comparados pelo tipo.
- Melhorar contraste de textos de botões desativados no tema escuro; validação de salvamento automático em interface nativa.

## [0.9.33] — candidato alfa, 16/10/2026

Commit: [6754b70](https://github.com/zagozago/ZagoSheetsWin/commit/6754b70150a5582574239e7c7f9120afe291ebc9).

### Changed
- **Mudança importante de política de dados:** após criação nativa do Google Sheets, vínculo remoto confirmado e **backup íntegro**, falhas de equivalência de valores/fórmulas e ausência de verificação integral deixam de impor importação como cópia. O original **pode ser substituído** por `.url` conforme consentimento e preferências existentes.
- O código `ConversionVerifier` marca diferença de valores suportados ou impossibilidade de comparação como advertência, mas continua rejeitando falhas graves de integridade, importação remota e associação.
- Retomada passa a respeitar preferência global de cópia, mantendo associação ao documento já criado.
- Fluxos e testes automatizados de XLS/ODS, restauração binária e erros remotos foram atualizados. **Não representam equivalência garantida nem homologação de todos os documentos reais**.

## [0.9.32] — candidato alfa, 16/10/2026

Commit: [a3b1f9c](https://github.com/zagozago/ZagoSheetsWin/commit/a3b1f9ca0d504dee193553bd879fb364e910edf5).

- Restaurar padrão de **9 pt** para botões de ação, com a família de fontes selecionada para o idioma.
- Remover da janela de Opções avançadas a seção **Formatos e texto**, inclusive seletores de codificação/delimitador; **preservar as preferências previamente gravadas e os cinco formatos suportados**.
- Ajustar altura da janela avançada ao conteúdo restante.

## [0.9.31] — candidato alfa, 16/10/2026

Commit: [d37c419](https://github.com/zagozago/ZagoSheetsWin/commit/d37c419364e24ddab598a245c28120d62ad3d020).

- Adicionar ODS à ilustração do tutorial e à descrição de integração de Windows, nos 51 idiomas.
- Remover o antigo controle de habilitação de formatos; suporte aos cinco formatos passa a ser sempre oferecido.
- Alteração tipográfica intermediária de 10 pt, **substituída pelo padrão de 9 pt na 0.9.32**.

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
