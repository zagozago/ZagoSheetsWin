# Changelog — ZagoSheetsWin

As alterações significativas são registradas aqui em ordem cronológica inversa, seguindo as categorias **Added**, **Changed**, **Fixed**, **Security** e **Known limitations** do [Keep a Changelog](https://keepachangelog.com/en/1.1.0/). As versões anteriores a 1.0.0 são **marcos do desenvolvimento alfa**, não GitHub Releases. Este histórico foi revisado em **24/10/2026** usando commits, código, documentos de fase e metadados do repositório.

**Não há tags de versão nem Releases publicadas no GitHub na consulta de 24/10/2026.** GitHub Actions produz candidatos e artefatos temporários, mas uma execução bem-sucedida não constitui lançamento público nem aceite com Google real. Não confundir versão declarada no código com Release publicada.

## [Unreleased] — preparação para 1.0.0

- **Documentation:** consolidar README internacional, guias atuais, histórico, formatos, recuperação, segurança e checklist de homologação.
- **Pending:** homologar fluxos reais de OAuth/Drive no Windows, compatibilidade de ODS complexos e importações com conversão parcial, validar reparo e seleção de interpretador com conta Google real, revisão editorial dos READMEs internacionais, elegibilidade de licenças e eventual assinatura do instalador.
- **Not done:** publicar 1.0.0, criar tag ou Release, certificar segurança, afirmar suporte universal a ODS.
- Alterações de documentação **não** aumentam a versão do aplicativo.

## [0.9.48] — candidato alfa, 24/10/2026

Fontes: [implementação detalhada e aceite visual](docs/implementation/final-ui-20261024.md), [CHANGELOG da branch do produto](https://github.com/zagozago/ZagoSheetsWin/blob/08dbced61ed6395534ed6daeff6cf23342c5a4a9/CHANGELOG.md), commits [d6f6f15](https://github.com/zagozago/ZagoSheetsWin/commit/d6f6f150b7), [1738188](https://github.com/zagozago/ZagoSheetsWin/commit/173818851e), [74e2846](https://github.com/zagozago/ZagoSheetsWin/commit/74e28469e8), [a932a13](https://github.com/zagozago/ZagoSheetsWin/commit/a932a1389e), [08dbced](https://github.com/zagozago/ZagoSheetsWin/commit/08dbced61ed6395534ed6daeff6cf23342c5a4a9).

### Interface, localização e instalação
- Seletor do instalador com área cliente dimensionada para DPI, margens equilibradas e arte lateral oficial reproduzível. Logos/créditos MIT e Open in Google preservados.
- Home: ícone da pasta alinhado, labels e ícones centrados; botão Mais ações tem seta junto à última palavra quando a linha quebra, inclusive em idiomas longos. Prefixos de Backups/Disponível e dicas receberam destaque tipográfico sem modificar as traduções.
- Backups: **seleção real com checkboxes por linha**, coluna auxiliar estreita e nenhum checkbox redundante no cabeçalho. **Selecionar todos / Limpar seleção** agem em lotes nativos de 32, com spinner transitório e trava de reentrada, preservando os estados permitidos. Barra ocupa a largura interna do cartão, bordas fechadas; lateral passou de 240 para 285 unidades. Fonte do corpo dos painéis reduzida em 2 pt sem alterar títulos.
- Mensagens de erro mantêm o mesmo conteúdo/ordem e têm quebras naturais calculadas pela largura e fonte disponíveis; comandos/retry não mudaram.
- Ajuste específico RTL no HEAD `08dbced`: `InsetFlowLayoutPanel` mantém espaçamento horizontal físico das ações em **Home** e **Configurações** apesar da espelhagem WinForms; verificação nativa checa ambos os recuos. Não altera código de importação/backup.
- Limites, OAuth, importação, recuperação, seleção elegível e armazenamento continuam com a política da 0.9.43/0.9.44.

### Evidência
- [Local core #40110813671](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40110813671): sucesso (Windows 407 aprovados/1 ignorado; Linux 364 aprovados/44 ignorados) num commit anterior `1738188`, com matriz nativa de 51 idiomas e quatro escalas DPI.
- [Protected alpha distribution #40120263404](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40120263404): sucesso em `df6f4d6`, **antes** do hotfix RTL final.
- [Local core #40121298442](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40121298442): sucesso em `cacba045`, **antes** do hotfix RTL final.
- O hotfix **`08dbced`** iniciou [Local core #40126683024](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40126683024); resultado **pendente no momento da revisão**. Não afirmar Protected distribution verde nesse SHA até verificar.
- Nenhuma validação completa com Google real foi declarada; screenshots e smoke tests nativos são evidências de UI, não homologação da conversão.

## [0.9.47] — candidato alfa, 24/10/2026

Referência: [refinamento e mapeamento dos 85 textos × 51 idiomas](docs/implementation/refinement-20261024.md) e commit [f3c931b](https://github.com/zagozago/ZagoSheetsWin/commit/f3c931b3ed).

- Ajustes G-01–06/TEL-01–09: cabeçalhos, marca/títulos, rodapé legal, ícones centralizados, botões, fonte contextual, sobre/licenças em cartões, estados reais da conta, descrição e toggles de configurações/avançadas, miniatura do tutorial e primeira instalação em três passos.
- Backups: seleção nativa espelhada em checkboxes individuais, cabeçalho com estado parcial nesta fase, três painéis independentes inicialmente fechados, métricas reais e largura adequada para temas e RTL/DPI. O cabeçalho redundante foi **retirado na 0.9.48**.
- CSV autorizado com **4.335 linhas: 85 mensagens × 51 locais**, validado sem retradução. Foram 65 IDs reutilizados, 18 criados e dois não aplicáveis.
- Matriz de UI offline em temas claro/escuro, 51 idiomas e DPI 100%, 125%, 150% e 200%, incluindo RTL e teclado/Shift. [Local core #40099207181](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40099207181) passou.

## [0.9.46] — candidato alfa, 21/10/2026

- Padronização de alinhamento entre marca, seletor de idioma, tema, títulos e rodapé; linhas de botões e altura de ações ajustadas; primeiro uso e configurações avançadas mais compactos.
- Erros exibem conteúdo completo em painel menor, permitindo expansão para mensagens longas. Funcionalidade do backend e 51 idiomas preservados.

## [0.9.45] — candidato alfa, 21/10/2026

- Estilo visual compartilhado nos principais formulários, ícones vetoriais, cartões suaves, hierarquia tipográfica, configurações agrupadas e Sobre/Licenças reorganizado sem reduzir conteúdo legal. Erros dão ênfase à ação correta, preservando lógica/condições.
- Primeiro uso com imagem unificada nos três passos e cinco formatos disponíveis. Versão 0.9.44 permanece com pendentes operacionais aguardando aceite.

## [0.9.44] — candidato alfa, 20–21/10/2026

- Backups incompletos passam a exibir estado **pendente**, sem pressupor que podem ser eliminados; não modificar política manual de limpeza.
- Uma **nova abertura independente**, depois de tentar retomar/reconciliar uma operação antiga sem conseguir localizar seu Google Sheets, pode iniciar uma **nova operação**, mantendo backup/associação remota/histórico anteriores. **Não** se aplica a upload ou resposta ambígua, falha de rede/permissão, operações concorrentes, mesma abertura ou retry automático. Fonte no código: `SpreadsheetOpening` e estados registrados.
- Testes [Regression0944Tests](tests/SheetsWindows.Tests/Regression0944Tests.cs) cobrem elegibilidade/identidade; homologação em máquina com Google real permanece pendente.

## [0.9.43] — candidato alfa, 19/10/2026 (20/10 UTC)

Referência de código: [9fbec2a](https://github.com/zagozago/ZagoSheetsWin/commit/9fbec2add18c012012405b56a333f6e49248a6e5). Commits de produto: [4555ead](https://github.com/zagozago/ZagoSheetsWin/commit/4555eadc67aa0473412ca788472f815017e57fa9), [038ce08](https://github.com/zagozago/ZagoSheetsWin/commit/038ce08d3a9a5acfb68c5f5baac5bde33b53de36), [788b4c7](https://github.com/zagozago/ZagoSheetsWin/commit/788b4c7f1f31c62f8a751944c19fbe61988d6faf), [1fba728](https://github.com/zagozago/ZagoSheetsWin/commit/1fba7286f447694fe7711753bfce5c04e9935c3e) e ajuste final [9fbec2a](https://github.com/zagozago/ZagoSheetsWin/commit/9fbec2add18c012012405b56a333f6e49248a6e5). Documento técnico [REGRESSION_0943.md](docs/REGRESSION_0943.md).

### Changed — política de backups, **revogando a limpeza automática da 0.9.40–0.9.42**
- **Limpeza exclusivamente manual:** nenhuma rotação de backups concluídos por quota, nenhuma limpeza por prazo após importação, nenhuma espera por liberação de capacidade. A ação **Limpar expirados** usa os dias de retenção configurados; também é possível selecionar e apagar backups concluídos de forma explícita. Estados pendentes/incertos são preservados.
- **Quota imediata:** se não houver capacidade suficiente, a captura falha prontamente, preserva o original e orienta limpeza manual em Backups. Lock curto de snapshot e checagem atômica de quota continuam protegendo capturas concorrentes; **uploads de fontes diferentes permanecem paralelos**, sem fila global/limite arbitrário de quantidade.
- Configurações numéricas de retenção/quota seguem com salvamento automático; campo legado `AutomaticCleanup` persiste para leitura de arquivos antigos, mas é carregado como **false**. Sem toggle na interface.

### Fixed — memória, associação e interface
- Usar um único buffer de tamanho exato para os bytes do snapshot enviados ao importador, reduzindo uma duplicação de memória; remover varreduras de manutenção antes/depois de cada importação.
- Conferir a pasta Google existente **uma única vez** sob a validação atual, eliminando GET redundante. Em resposta remota incerta, até **duas consultas limitadas** pelo marcador da operação, **sem outra criação/sessão**. O diagnóstico registra apenas status numérico dos ACKs.
- Falhas por falta de memória seguem caminho de baixo custo, sem serialização diagnóstica e sem construir a UI de retry; a janela de processamento oferece **Fechar** com texto de erro previamente preparado. Isso não promete memória ilimitada para conversões paralelas.
- Manter janela inicial com borda fixa, minimizar disponível, maximizar/redimensionar indisponíveis, posicionamento responsivo e tratamento seguro de minimização/restauração.
- Ajuste final do commit `9fbec2a`: reduzir **apenas a altura do cliente da janela Home de 240 para 192 px** para eliminar espaço inferior excessivo, sem mudar o processamento de arquivos.

### Verificação — aprovada no SHA de referência
- [Local core #39346495162](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) em `9fbec2a`: **Windows e Ubuntu concluídos com sucesso**.
- [Protected alpha distribution #39346495298](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) no **mesmo SHA `9fbec2a`**: **concluída com sucesso**. Publicou artefatos de instalador `ZagoSheetsWin-Setup-win-x64`, launcher `ZagoSheetsWin-win-x64`, diagnósticos de teste e conferência de idioma. **Artefatos de Actions não são GitHub Releases.**
- [Relatório 0.9.43](docs/REGRESSION_0943.md) registra validação anterior no `788b4c7`: **398 testes aprovados + 1 ignorado no Windows; 355 aprovados + 44 ignorados no Linux**, catálogos/segurança/instalação/51 idiomas; resultados específicos do HEAD final são indicados pelos workflows bem-sucedidos, sem assumir a mesma contagem.
- **Ainda pendente:** homologação de lotes no Windows do usuário e importação Google real, casos complexos de ODS, revisão editorial das traduções, avaliação de assinatura/infra e aceite para 1.0.

## [0.9.42] — candidato alfa, 18/10/2026 (19/10 UTC)

Commits: [3cc48e0](https://github.com/zagozago/ZagoSheetsWin/commit/3cc48e0bf2d77e5e04f3523b085293c1c23514e9), [e2096c7](https://github.com/zagozago/ZagoSheetsWin/commit/e2096c7c2c61ba3c1aaf2cbad94a98a56731b391), [ceed7fb](https://github.com/zagozago/ZagoSheetsWin/commit/ceed7fbd897019e4afb0c1661e43c9f5182ac40b), [c1dc8a5](https://github.com/zagozago/ZagoSheetsWin/commit/c1dc8a501209ce37d1f65c141ad30498146296af). [Relatório datado](docs/REGRESSION_0942.md).

### Changed
- Remover a fila global que serializava as importações na 0.9.40. Abrir, importar como cópia, retomar e reparar arquivos **diferentes** podem avançar paralelamente. Locks por origem e operações permanecem.
- Manter lock curto de manutenção na captura do backup; **esperar somente quando a quota está efetivamente ocupada**. Aguardar capacidade com checagens a cada 250 ms, cancelamento e prazo interno de até **2 minutos**; não bloquear a rede nem a conclusão de outras operações.
- Coalescer esperas/limpezas simultâneas e evitar escrita/preparação repetida enquanto o espaço permanece indisponível. Liberar backups concluídos elegíveis mais antigos; não apagar pendências ou material protegido.
- Ajustar apenas o **watchdog do teste extremo de lote** de 45 segundos para 3 minutos, mantendo os prazos do produto; repetir o lote de 20 arquivos com a quota previamente ocupada.

### Verification status — NÃO HOMOLOGADO
- [Actions 39181030201](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39181030201) no commit `e2096c7`: **CI Windows e Linux passaram** (392/1 Windows; 350/43 Linux).
- [Actions 39181854229](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39181854229) no commit `ceed7fb`: **Local core passou**.
- [Protected distribution 39181854241](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39181854241) no `ceed7fb`: **falhou** no watchdog de 45 s do teste extremo de quota.
- [Local core 39189870249](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39189870249) no HEAD `c1dc8a5`: **Linux passou; Windows falhou** na etapa de testes. A correção do watchdog de 3 min **não pode ser declarada aprovada**. Não há distribuição protegida concluída com sucesso para o SHA final consultado.

## [0.9.41] — candidato alfa, 18/10/2026

Commit: [8d58928](https://github.com/zagozago/ZagoSheetsWin/commit/8d589283e454898fdeeac3a65b2b90da753db017). [Relatório](docs/REGRESSION_0941.md).

- Aumentar **somente CSV/TSV novos** de 1.000.000 para **2.000.000 células**; manter 100.000 linhas, 1.000 colunas, 32.767 caracteres/célula e limite de 20 MiB de entrada/payload.
- Conferência de XML de aba de texto até 128 MiB e ZIP expandido de texto até 160 MiB; demais formatos mantêm seus tetos anteriores.
- Testes locais do CSV real `GDEX1ADC geo codes no fbo or military(1).csv`: 8.782 × 116 = **1.018.712 células**, conversão e comparação local aprovadas; **não testado no Google real**.
- [Local core 39174444610](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39174444610) passou. A primeira [distribuição 39175119834](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39175119834) falhou no ciclo desinstalação/reinstalação, por corrida de limpeza no ambiente de teste. O teste foi corrigido para aguardar confirmação final do desinstalador; [CI 39176358735](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39176358735) passou e [distribuição 39177203588](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39177203588) concluiu com sucesso no commit posterior `f8c7639`.

## [0.9.40] — candidato alfa, 18/10/2026

Commits: [a5bf4ea](https://github.com/zagozago/ZagoSheetsWin/commit/a5bf4eafbb71f34036e7d02c83ba540e26865cd9), [624a2a8](https://github.com/zagozago/ZagoSheetsWin/commit/624a2a8da81463af04ac170105859da87a707c7f), [22fabdf](https://github.com/zagozago/ZagoSheetsWin/commit/22fabdf9cc85dab91492a1b6eda4cff1e5b2005e). [Relatório](docs/REGRESSION_0940.md).

- Remover o toggle **Limpeza automática**: limpeza por prazo/cota passa a ser automática; retenção/quota mantêm autosave. O campo `AutomaticCleanup` permanece no JSON apenas para compatibilidade.
- Introduzir **fila global de importações por perfil** para reduzir colisões do lote, **posteriormente substituída pela execução paralela da 0.9.42**.
- Retry seguro de leituras Google (metadados/exportação) para 403 exclusivamente com motivos `rateLimitExceeded` / `userRateLimitExceeded`, até 3 tentativas extras, `Retry-After` limitado a 30 s; não repetir POST de criação e não tratar 403 de permissão como transitório.
- Novos motivos estruturados e diagnósticos de quota/limitação Google sem expor dados da planilha. [CI 39172166238](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39172166238) passou.

## [0.9.39] — candidato alfa, 18/10/2026

Commits: [4e081b5](https://github.com/zagozago/ZagoSheetsWin/commit/4e081b58a2adaa954219011c20d0bab3c0dc0315), [b759a97](https://github.com/zagozago/ZagoSheetsWin/commit/b759a97020104768dd3d682a5e525acace52b54d), [bcd482f](https://github.com/zagozago/ZagoSheetsWin/commit/bcd482f8d70fe554d3a52507c3229c19c7f5c1fe). [Relatório](docs/REGRESSION_0939.md).

- Destacar **Escolher separador** apenas nos casos genuinamente ambíguos, com prévia de leitura; demais erros mantêm ação comum.
- Corrigir CSV de coluna única com delimitadores internos citados, campos multilinha, e CRLF/LF no caminho CSV/TSV v4 sem `trim` nem perda de espaços; operações antigas mantêm payload.
- Em nova substituição, **verificar CSV/TSV antes de publicar atalho**. Recusar diferença de valores e preservar o original; XLS/ODS permanecem sob política própria de fidelidade parcial.
- Recuperar atalho ausente em journal incompleto **sem novo upload**, mantendo proteção do original/backup e sem sobrescrever atalho de terceiros.
- Limpeza por cota pode liberar backups concluídos mais antigos mesmo sem expiração, sem descartar pendências.
- [CI 39157306007](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39157306007) passou.

## [0.9.38] — candidato alfa, 18/10/2026

Commits: [21ba191](https://github.com/zagozago/ZagoSheetsWin/commit/21ba191705ee671750fdded120ee9ddae167b8a5), [67d1986](https://github.com/zagozago/ZagoSheetsWin/commit/67d1986f6d90891d05c638df894dacec002f89ed), [d12844d](https://github.com/zagozago/ZagoSheetsWin/commit/d12844d7a6d6223840dc236056c469828246a492). [Relatório](docs/REGRESSION_0938.md) e [matriz 26+78](docs/REGRESSION_MATRIX_0938.md).

- Interpretador v3: CSV sem cabeçalho, coluna única TSV, hints `sep=` exatos, análise de gramática/largura antes de alocar dados, prévia local de 3 linhas e 12 colunas; versões anteriores preservadas.
- Limite experimental v3 de 100.000 linhas/1.000.000 células, **superado por v4/0.9.41**; writers respeitam XML/ZIP e compatibilidade do payload legado.
- Reconciliar respostas de criação/sessão remota incertas via marcador de operação, **sem reupload cego**. Telemetria de reconciliação, códigos estruturados e tentativas correlacionadas.
- ODS com repetição fora da grade é rejeitado conservadoramente; **sem normalizar silenciosamente os bytes originais**.
- Conferência CSV/TSV com divergência **bloqueia substituição** (política específica mais estrita que XLS/ODS).
- [CI 39145165210](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39145165210) passou; homologação Google real permanece pendente.

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


### Atualização de preparação CI da 0.9.48 (24/10/2026)

O commit [df01b85](https://github.com/zagozago/ZagoSheetsWin/commit/df01b85856a904cafddcaa7f4bab51fb483c5cef) altera **apenas comentário de identificação do workflow** para o candidato RTL; **nenhum código funcional ou visual adicional** foi modificado depois do hotfix `08dbced`.

- [Local core #40127989036](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40127989036) no SHA `df01b85`: **em execução** na última consulta.
- [Protected alpha distribution #40127989020](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40127989020) no mesmo SHA: **aguardando aprovação do ambiente protegido**.
- O sucesso de workflows em commits anteriores à mudança RTL **não aprova automaticamente** o SHA final. Os testes Google reais continuam pendentes.


### Hotfix RTL aprovado no CI e registro de aceitação (24/10/2026)

- [**Local core #40126683024**](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40126683024) do hotfix `08dbced`: **sucesso em Windows e Linux**, incluindo verificação de margens de botões, cartões e linhas em **5 idiomas RTL** (árabe, urdu, persa, pashto, sindi). A matriz dos 51 idiomas permaneceu coberta. Ver [relatório de recuos RTL](docs/implementation/rtl-insets-20261024.md).
- [**ee09a90**](https://github.com/zagozago/ZagoSheetsWin/commit/ee09a9065047a5840a404c55f3fd4b717ec897c3): commit **somente documental** com a validação; [CI #40128293029](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40128293029) iniciado. Não altera o comportamento do aplicativo.
- A [distribuição protegida #40127989020](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40127989020) continua **aguardando aprovação** na consulta. CI aprovado do hotfix não equivale à liberação protegida do instalador.
