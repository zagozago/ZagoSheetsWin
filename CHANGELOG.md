# Histórico de alterações

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
