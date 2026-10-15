# Histórico de alterações

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
