# ZagoSheetsWin 0.9.40 — fila de importação e backups automáticos

Base: `e803380bcaa6483a856d4cf52f6957b5b72cdb9c` (0.9.39). Escopo: pedido de 18/10/2026, capturas, JSONL de cota e pacote de diagnóstico de concorrência. Nenhuma alteração de OAuth, escopos, credenciais ou permissões do Google.

## Evidência e decisão

- O JSONL de cota falhou em SnapshotPreparation, antes de um snapshot concluído. O código limpava/verificava espaço antes da captura protegida: processos diferentes podiam usar a mesma estimativa de espaço livre. A captura tinha teto seguro, mas podia recusar um lote apesar de haver backups concluídos para limpar.
- Nos dois JSONL do ZIP, o documento já estava verificado e a falha 403 ocorreu em ConversionVerification. Falta o motivo estruturado da resposta. Limitação temporária é hipótese compatível com o sucesso posterior, não causa comprovada. Dez importações constituem observação do usuário, não limite da API.
- A fila usa um handle exclusivo por perfil local (compartilhado pelas contas desse perfil), antes de qualquer lock de arquivo/conta/manutenção. Só uma importação completa fica ativa: preparação, upload, verificação, atalho e conclusão. Solicitações adicionais aguardam de forma assíncrona, com cancelamento e limite de espera de uma hora; crash libera a posse. Não promete FIFO estrito de chegada. A seleção de backups para limpar continua sendo do mais antigo para o mais recente.
- A fila cobre abrir, cópia, reparar e retomar, inclusive o CLI de desenvolvimento. O caminho interno abrir→cópia não readquire a fila. Nenhuma transação SQLite ou lock de manutenção fica aberto durante a espera na fila ou rede.
- A próxima operação só inicia depois que a anterior conclui/libera a fila. Pode então remover backups concluídos elegíveis. Operações que falharam continuam protegidas: não se apaga evidência necessária para retomada apenas para acomodar um lote.

## Política e interface

Removido o toggle de limpeza. Limpeza por retenção e por cota é automática. Políticas antigas com `AutomaticCleanup=false` passam a carregar com limpeza ativada; o campo é conservado no JSON para compatibilidade. Retenção e cota seguem editáveis com autosave. A descrição de rotação não menciona opção ligada/desligada em nenhum dos 51 idiomas; o tutorial também não ensina mais opt-in.

As estatísticas ficam em bloco com borda, margem superior e fonte em negrito, com rótulos em Title Case onde aplicável. Português: Espaço Usado, Limite, Backups, Protegido.

O erro de cota continua existindo quando o arquivo excede o orçamento ou backups protegidos/desconhecidos ocupam o espaço. Ele explica que a limpeza já foi tentada e que o original permanece, sem afirmar que nunca houve upload em uma operação retomada. Novo código diagnóstico BackupQuota evita classificar esse caso como acesso a arquivo.

## Retry e diagnóstico

Leituras GET de metadados/exportação aceitam 403 como transitório somente para `rateLimitExceeded` e `userRateLimitExceeded`. No máximo três retries com backoff exponencial e jitter, respeitando Retry-After dentro da janela de 30 segundos. Se o servidor exigir espera maior, não se tenta antes do prazo: o erro é devolvido. O prazo total da requisição permanece de 90 segundos. 429/5xx mantêm a política existente. POST de criação não é repetido por essa política.

403 sem motivo reconhecido, com erro permanente, resposta malformada ou motivos mistos não autoriza retry. Falha temporária reconhecida esgotada usa a mensagem de indisponibilidade/limitação Google, sem sugerir autenticação. A retomada mantém operação/documento existentes; não remove originais nem publica atalhos antes da conferência.

O corpo de erro é lido com limite de 16 KiB e descartado. Somente uma lista fechada de motivos e endpoints lógicos entra no diagnóstico. Checkpoints informam tentativas, espera e etapa de fila; o tempo de fila pode ser obtido entre os checkpoints de início e conclusão. Sem tokens, mensagens brutas, caminhos, IDs de conta ou conteúdo de planilha.

Referências oficiais consultadas: https://developers.google.com/workspace/drive/api/guides/handle-errors e https://developers.google.com/workspace/drive/api/guides/limits.

## Validação

Incluídos testes de lotes de 12 e 20 solicitações com cota de 1 MB e arquivos de 600 kB, exigindo uma operação ativa, rotação a cada conclusão, preservação de todos os originais e uso abaixo da cota. Teste real entre processos cobre bloqueio, cancelamento e liberação após encerramento abrupto. Respostas HTTP simuladas cobrem os dois motivos transitórios, permissão, cota diária, armazenamento, motivo desconhecido, corpo inválido, motivos mistos, limite de retry e Retry-After longo, sem POST adicional.

Os testes são locais/simulados, sem Google real. O lote real de 12/20 arquivos permanece critério de aceite no dispositivo do usuário após instalar o candidato. Se falhar, exportar o novo diagnóstico para obter motivo/endpoint/tentativas; não renomear arquivos nem apagar estado para forçar novo envio.

Validação local Release: compilação sem avisos/erros; 345 testes aprovados, 43 exclusivos do Windows ignorados, total 388, nenhuma falha. Contrato de localização: 14 testes aprovados; 619 chaves em 51 idiomas; auditoria estática de segurança aprovada. Validação nativa Windows e ciclo do instalador serão registrados após o Actions.
