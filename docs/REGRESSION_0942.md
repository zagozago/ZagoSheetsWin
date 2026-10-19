# ZagoSheetsWin 0.9.42 — abertura paralela e espera restrita à capacidade do backup

Base: `f8c76397e6765f7dff74f66f89e8d66b9a2a410e` (0.9.41). Mantém os dois milhões de células CSV/TSV, demais limites, retry seguro de leituras Google e a correção do teste de reinstalação.

## Problema e mudança

A fila de perfil da 0.9.40 abrangia a operação inteira, incluindo upload e conferência no Google. Portanto, até arquivos pequenos com bastante espaço livre esperavam toda a importação anterior. Esse bloqueio foi removido de abrir, copiar, retomar, reparar e CLI. Arquivos diferentes podem avançar simultaneamente. Locks por identidade continuam impedindo importações duplicadas do mesmo arquivo; OAuth e associação da pasta conservam suas proteções específicas.

A captura mantém somente o lock local curto de manutenção, necessário para conferir o uso real e gravar um snapshot sem ultrapassar a cota entre processos. Esse lock não abrange conversão, rede, conferência remota, navegador nem a operação inteira.

A primeira tentativa de captura não entra em fila de importações. Apenas uma recusa por falta real de capacidade ativa o caminho de limpeza e nova tentativa local. A limpeza escolhe backups concluídos e verificados, do mais antigo para o mais recente; pendentes, desconhecidos, alterados e a origem de uma reparação continuam protegidos. Capturas concorrentes reconferem a cota sob lock antes de gravar. Não há retry de upload nesse mecanismo.

A espera por capacidade reavalia os backups a cada 250 ms e admite cancelamento. Os processos que aguardam não repetem a preparação enquanto o espaço continua ocupado. Um único aguardante por perfil inspeciona o catálogo por vez; se o espaço ainda está protegido, ele mantém somente esse lock de limpeza por um intervalo de 250 ms, liberando os locks de origem e manutenção para que uploads e conclusões avancem. A captura ocorre fora desse lock de limpeza, preservando a ordem dos locks em reparações. Ela tem limite de espera de dois minutos entre tentativas; operações locais conservam também seus próprios timeouts. Se não houver espaço recuperável, a mensagem de cota continua necessária, preservando o original. Um arquivo sozinho maior que a cota é recusado imediatamente, sem criar uma operação nem apagar backups.

A limpeza automática pula fontes ocupadas, em vez de esperar seus locks. Isso evita inversão de locks com reparações que precisam manter seu backup de origem protegido. A exclusão manual conserva sua espera e suas verificações. A retenção automática é aplicada depois de uma importação bem-sucedida; rotação por capacidade ocorre somente quando falta espaço.

## Interface

Durante a espera real: “Aguardando espaço no backup… Dica: libere espaço no backup para evitar esperas demoradas.” A dica também integra o erro final de cota sem afirmar que uma tentativa encerrada ainda está aguardando. Ambos os textos estão nos 51 idiomas; catálogo revisão 15, 621 chaves. A telemetria distingue `BackupCapacityWait`; o valor antigo `ImportQueue` é preservado apenas para ler históricos.

Somente o quadro de estatísticas de Backups passa de 10 para 8 pontos. Negrito, contorno, espaçamento e os demais tamanhos permanecem.

## Validação

- Lotes de 12 e 20 arquivos com espaço: todos alcançam uma barreira que representa uploads ainda pendentes antes de qualquer upload poder terminar. Nenhuma espera por capacidade é acionada.
- Lotes de 12 e 20 arquivos de 600.000 bytes com cota de 1.000.000: todos concluem com rotação, sem ultrapassar a cota e preservando os originais.
- Backup pendente protegido: espera cancelável sem reter source/maintenance locks.
- Backup concluído com fonte ocupada: limpeza automática retorna sem bloqueá-la; uma passagem posterior o remove após liberar o lock.
- Os testes de cota, integridade, preservação e recusa de arquivo maior que o limite continuam obrigatórios.
- Catálogo: 14 testes aprovados; geração dos 51 idiomas e auditoria estática aprovadas.

Validação local Release: 350 testes aprovados, 43 exclusivos de Windows ignorados, total 393, nenhuma falha. Os resultados Windows/instalador serão registrados após o Actions. Os ensaios de concorrência usam uma barreira local; não medem latência real nem garantem disponibilidade/cota do Google.

A primeira validação Windows (39180414738) detectou estouro do orçamento de 45 segundos no lote de 20 arquivos com cota cheia; 391 outros testes passaram. A coordenação foi refinada para evitar novas escritas de falha e varreduras concorrentes enquanto a cota permanece ocupada. O orçamento do teste foi mantido.
