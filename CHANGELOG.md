# Histórico de alterações

## 0.9.47 — refinamento dirigido AS IS → TO BE

- G-01–06: marca maior, linha fina global, títulos contextuais e Sobre/Licenças no rodapé, respeitando as exceções do primeiro uso.
- TEL-01/02: ícones e conjuntos centralizados; seleção por checkbox sincronizada com a seleção efetiva, incluindo cabeçalho com estado parcial.
- B-03: três painéis independentes inicialmente recolhidos; ocupação, contagem total e espaço disponível derivados dos dados reais.
- TEL-04–09: títulos, fundos legais, numeração circular, status real, ajuda contextual avançada e hierarquia do primeiro uso.
- CSV autorizado de 85 mensagens em 51 idiomas validado e integrado sem retradução; mapeamento completo em docs/implementation/refinement-20261024-mapping.json.
- Regras de importação, autenticação, recuperação, descarte e persistência preservadas; erros recebem somente o cabeçalho global.

## 0.9.46 — harmonia e alinhamento da interface

- Cabeçalhos com margens, alturas e alinhamento vertical coerentes entre marca, links, idioma e tema.
- Ações principais e linhas de botões alinhadas às mesmas bordas, com espaçamento uniforme.
- Primeiro uso e configurações avançadas mais compactos; tipografia das estatísticas de backup com a mesma legibilidade do texto principal.
- Rodapé legal do primeiro uso alinhado à navegação; painel de erro com menor espaço vazio, mantendo crescimento para mensagens longas.
- Preservados os 51 idiomas, temas claro/escuro e os fluxos funcionais da 0.9.45. Homologação prática do comportamento dos pendentes da 0.9.44 continua em aberto.

## 0.9.45 — refinamento visual

- Cabeçalho, ícones vetoriais próprios, cartões e hierarquia visual compartilhados entre as nove telas de referência.
- Subtítulo da janela principal com as 51 traduções fornecidas; orientação localizada sobre seleção múltipla de backups.
- Configurações agrupadas sem alterar ações, valores ou persistência; lista de backups com linhas discretas, ícones de arquivo e indicador real de uso de espaço.
- Sobre/Licenças em duas colunas, mantendo textos legais integrais, créditos, links e dependências.
- Erros com título destacado e ação de nova tentativa prioritária, preservando todas as condições e mensagens existentes.
- Miniatura do ZagoSheetsWin redesenhada e unificada nos três passos do primeiro uso, com menos espaço vazio e os cinco formatos preservados.
- O novo comportamento dos pendentes da 0.9.44 ainda aguarda homologação prática do usuário; não foi alterado nesta versão.

## 0.9.44

- Backups incompletos aparecem como pendentes nos 51 idiomas.
- Uma nova abertura pode iniciar outra importação após tentar retomar e reconciliar uma operação anterior sem encontrar sua planilha no Google. O backup, a associação remota e o histórico antigos permanecem disponíveis.
- A exceção não se aplica a operações criadas na mesma abertura, operações simultâneas, resultados ambíguos, falhas de rede ou de validação remota. Retentativas automáticas compartilham a mesma abertura.
- Limpeza de backups continua manual; limites, conversão e substituição do original permanecem iguais à 0.9.43.

## 0.9.36 — correções do escopo B

- Detecção de CSV/TSV por conteúdo: vírgula, ponto e vírgula, tabulação e barra vertical, com escolha por importação quando houver ambiguidade.
- Fallback controlado de UTF-8 estrito para Windows-1252 sem BOM; valores permanecem texto literal.
- ODS passa a ignorar a materialização de caudas vazias repetidas, mantendo coordenadas e os bytes originais de envio.
- Interpretação versionada por operação; reparo explícito em novo documento a partir de backup, preservando a planilha anterior.
- Estado de erro persistente, proteção contra progresso atrasado, diagnóstico da tentativa atual e exportação separada do histórico.
- Repetição de falhas permanentes de entrada aguarda edição do arquivo; arquivos corrigidos sem intenção remota podem iniciar nova operação com o backup anterior preservado.
- Limites exclusivos de CSV/TSV: 60.000 registros lógicos e 600.000 células, incluindo a releitura de conferência. Demais limites e políticas de backup/substituição preservados.
- Mensagens novas nos 51 idiomas e regressões automatizadas de leitura, histórico, reparo e interface nativa.

## 0.9.35

- Clarify that the Windows read-only file attribute can prevent completion; keep the original protected.
- Add native Windows regression coverage for read-only retirement failure and recovery without changing the original.

## 0.9.34

- Confirm successful original replacement for every supported format, with persistent opt-out and one notice at a time per user session.
- Hide backup row switches, auto-save retention/quota/cleanup preferences, and enable typed ascending/descending column sorting.
- Render disabled button text legibly in dark mode.

## 0.9.33 — candidato de 2026-10-16

- Importações confirmadas com equivalência parcial deixam de forçar cópia: fórmulas, dados ou recursos cuja preservação não pode ser confirmada geram aviso, mantendo backup e substituindo o original por atalho na pasta de origem conforme as preferências do usuário.
- O aviso explica como recuperar o original e oferece acesso direto a Backups; seu título não indica incorretamente importação como cópia. Mensagem traduzida nos 51 idiomas.
- A retomada respeita a preferência global de cópia e reutiliza o documento existente.
- Mantidas as verificações de identidade local, hash, backup, publicação do atalho, vínculo remoto e importação nativa confirmada. Falhas de importação ou integridade impedem a retirada do original.
- Regressões com os exemplos equivalentes XLS/ODS, restauração binária, configurações, retomada e falha de conversão remota.

## 0.9.32 — candidato de 2026-10-16

- Corrigida a referência tipográfica: o tamanho original de 9 pt da navegação principal passa a ser o padrão dos botões de ação em todas as telas, incluindo ações principais e pop-ups de informação, com a família do idioma selecionado.
- Removida a seção Formatos e texto das opções avançadas, incluindo a lista, os seletores indisponíveis e os textos associados à seção. As preferências persistidas e o suporte aos cinco formatos são preservados.
- Janela de opções avançadas reduzida para acompanhar o conteúdo restante.

## 0.9.31 — candidato de 2026-10-16

- ODS incluído junto aos demais formatos na ilustração do tutorial e na descrição de integração do Windows em todos os idiomas.
- Botões da tela principal herdam a fonte padrão de 10 pt, com a família do idioma selecionado.
- Removido o toggle de habilitação de formatos: suporte é parte do aplicativo. Lista informativa mantém os cinco formatos; preferências de leitura existentes são preservadas.

## 0.9.30 — candidato de 2026-10-15

- Validação explícita de assinatura ODS; HTML renomeado para .ods é rejeitado antes do upload, com código de diagnóstico específico. Cancelamento propagado à leitura ODS.
- Diagnósticos sugerem ZSW-diagn-{nomeextensaoconcatenados}.jsonl com basename sanitizado e limitado; conteúdo permanece sem nomes e caminhos.
- Restaurar passa para o início da linha de ações de backup; Restaurar e Apagar usam rótulos curtos nos 51 idiomas.
- Respostas OAuth limitadas, callback reforçado, JSONs com tokens pessoais recusados, escopos adicionais rejeitados e erros de revogação tratados explicitamente.
- Limite de expansão ZIP verificado sem soma suscetível a overflow.
- Versão 0.9.30 coerente entre aplicativo, catálogo e instalador; distribuição usa a versão do projeto, com teste contra deriva.
- .NET atualizado para runtime/pacotes 10.0.13 e SDK 10.0.402, corrigindo os avisos de segurança publicados em 13/10/2026.
- Auditoria de dependências transitivas e checagens de segurança entram no CI. Instalador acompanha checksum SHA-256, versão, commit e estado da assinatura.
- Revisão técnica e pendências externas documentadas em docs/SECURITY_REVIEW_0930.md.

## 0.9.29 — consolidação anterior da alpha

- Reorganização de Configurações e Opções avançadas, ícone informativo, e-mail da conta, desconexão real e feedback visual de ações desabilitadas.
- Planilhas abertas sem conexão encaminham a Configurações; primeiro uso termina na terceira página.
- Diagnósticos com etapas, tempos e verificações seguras; exportação atômica e recuperação da tentativa atual.
- Nova importação local quando o documento associado é confirmado na lixeira, preservando histórico e backups.
- Traduções e validação nativa dos 51 idiomas, regressões de CSV, upload, conversão e ciclo do instalador.
