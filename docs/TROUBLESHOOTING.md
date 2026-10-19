# Solução de problemas e recuperação

**Escopo:** ZagoSheetsWin alpha **0.9.42** ([evidências](RELEASE_STATUS.md)) em Windows. Antes de repetir operações, confirme a presença do arquivo original, atalho e backup. **Não exclua `%LOCALAPPDATA%\SheetsWindows` para tentar resolver problemas.** Isso pode eliminar associações ou material necessário à recuperação.

## 1. A planilha não abre por duplo clique

**Condição:** o Windows abre outro aplicativo, ou não há opção ZagoSheetsWin.

1. Abra **Configurações → Aplicativos → Aplicativos padrão**.
2. Procure **ZagoSheetsWin** e escolha as extensões desejadas: XLSX, XLS, ODS, CSV, TSV.
3. Teste com uma cópia descartável. O instalador registra candidatos, mas não altera silenciosamente a escolha do Windows.
4. Se a opção não existir, reexecute **instalação/atualização aprovada**. Não edite `UserChoice` manualmente.

## 2. Conexão Google ausente, revogada ou com erro

1. Abra **Configurações → Conta Google**.
2. Use **Verificar conexão**. Autorização local salva não prova conexão válida.
3. Se necessário, use **Conectar Google** e finalize o consentimento no navegador.
4. Caso tenha trocado a conta, não force o programa a reaproveitar documentos da anterior; operações são delimitadas por conta e cliente OAuth.
5. Se a rede está indisponível, preserve o original e repita somente quando houver conexão.

Se o aviso vier do Google Auth Platform (modo Testing, revisão de marca, conta não autorizada), o responsável pelo projeto deve corrigir a configuração no Google Cloud. Não é possível solucionar isso modificando o arquivo `.url`.

## 3. Erro de ODS ou de formato

1. Confirme que o documento é de fato OpenDocument Spreadsheet e não HTML/ZIP diferente com extensão `.ods`.
2. Abra o arquivo em seu aplicativo de origem e, se possível, exporte uma cópia XLSX simples para testar.
3. **Antes de abrir**, selecione **Importar como cópia** se for essencial conservar o original no mesmo diretório. Uma importação confirmada pode substituir o original mesmo quando fórmulas/valores não se mantêm equivalentes. O backup permite restaurar os bytes originais.
4. Arquivo inválido ou contendo scripts pode ser recusado, sem upload. Exporte diagnóstico se o erro persistir. **ODS ainda não possui promessa de compatibilidade universal.**

## 4. CSV/TSV com colunas incorretas, caracteres inválidos ou limite

1. Confira o arquivo e sua codificação: UTF-8 estrito, UTF-16 com BOM ou fallback limitado para Windows-1252 quando não houver BOM (CSV/TSV v2). Se necessário, escolha a codificação na **janela da tentativa**, não em um controle global antigo.
2. A nova detecção analisa separadores **vírgula, ponto e vírgula, tabulação e pipe (`|`)**. Se surgir mensagem de ambiguidade, selecione o separador na janela de interpretação; confira campos entre aspas e linhas internas. Não edite arquivos internos.
3. Confira limites: **20 MiB**, **100.000 registros lógicos e 2.000.000 células para CSV/TSV v4**, até 1.000 colunas e 32.767 caracteres/célula. Células vazias contam. Outros formatos/versões antigas mantêm tetos anteriores.
4. O programa mantém CSV/TSV como **texto literal**, inclusive zeros à esquerda e fórmulas escritas como texto. Isso é comportamento intencional.
5. Se a interpretação já foi fixada para a operação anterior, **não exclua `formats.json` nem o banco**. Use **Backups → Mais ações → Reimportar em novo documento** para reparar uma importação antiga a partir do backup original, sem sobrescrever o Google Sheets existente.

## 5. O arquivo original continua na pasta

Isso pode ser **resultado correto**, não falha:

- A opção “Importar como cópia” está habilitada.
- O arquivo está em pasta sincronizada ou rede.
- Ocorreu uma falha efetiva de importação, integridade, verificação remota ou acesso; **simples falta de equivalência de fórmulas/valores não força mais a cópia**.
- A importação, validação, backup, atalho ou abertura do navegador falhou.
- O programa detectou mudança da fonte entre leitura e retirada.

Procure o atalho em **Pasta de atalhos das cópias** quando aplicável. Não exclua o original antes de confirmar o Google Sheets e o backup.

### Depois da substituição, a planilha no Google ficou diferente

Desde a 0.9.33, a conversão pode ser **parcial** e ainda concluir a substituição. A confirmação da 0.9.34 informa onde recuperar o original; **não** significa que todas as fórmulas ou dados foram preservados.

1. Abra o documento no Google Sheets e confira os dados.
2. Abra **Backups**, selecione a operação e use **Restaurar em…** para recuperar os bytes originais em um local livre.
3. Para a próxima importação sensível a fidelidade, use **Importar como cópia** antes da abertura e mantenha o arquivo de origem.
4. Se marcou **Não mostrar novamente**, isso oculta confirmações futuras, **não altera a política de backup nem a substituição**.

### Arquivo original marcado como somente leitura (0.9.35)

O Windows pode impedir a retirada do arquivo marcado **Somente leitura**. A interface inclui essa hipótese na mensagem genérica de erro em 51 idiomas. O arquivo original e os backups existentes **devem permanecer preservados**.

1. Confira o atributo do arquivo em **Propriedades** no Windows.
2. Se for adequado, desmarque **Somente leitura**. Não faça isso em arquivo de terceiros sem autorização.
3. Retome a **operação já existente** pela janela Backups/Recuperação; não force reupload apagando registros.
4. Caso queira manter a origem, use importação como cópia. Se houver restrições de acesso, trate-as na própria pasta.

## 6. O upload parou ou a internet caiu

O programa mantém associação, upload resumable e journal. Falhas transitórias podem receber até duas tentativas adicionais do processamento; uma nova execução explícita pode recuperar a **mesma operação**. A criação do documento remoto não é repetida cegamente.

1. Use **Backups/Recuperação**.
2. Selecione a operação existente.
3. Escolha **Retomar como cópia** para conservar a fonte, ou **Concluir substituição** se o programa permitir e a origem continuar elegível.
4. Se o sistema exigir reconciliação por resultado remoto incerto, não apague bancos/sessões nem reimporte com outro nome como tentativa automática de “conserto”.

### Erro permanente, botão Tentar novamente e janela de erro (0.9.36)

- **Erro permanente de leitura/capacidade:** o programa não executa novas tentativas automáticas enquanto o arquivo permanece igual. Corrija/salve a fonte; a tentativa manual passa a estar disponível quando a alteração é detectada.
- **CSV/TSV ambíguo:** escolha explicitamente delimitador/codificação na janela contextual. Não é obrigatório reconfigurar o aplicativo globalmente.
- **Depois de minimizar/restaurar a janela:** a mensagem de falha permanece, mesmo com progresso atrasado. Se a mensagem desaparecer em uma instalação antiga, use 0.9.37 e exporte evidência sanitizada.
- **Exportar diagnóstico** na tela de falha: exporta **a tentativa atual**. Em Backups → Mais ações → **Exportar histórico**, o arquivo contém o histórico geral. O formato é JSONL e mantém exclusões de dados sensíveis.

## 7. Backup não pode ser restaurado

Verifique se a operação possui snapshot disponível e não teve backup excluído por limpeza/descarte confirmado. Restauração usa um destino novo e não sobrescreve arquivo existente. Se a limpeza já apagou o snapshot, o histórico e o Sheets remoto podem continuar, mas **não existe recuperação garantida dos bytes originais**.

A limpeza de backups concluídos agora é **automática por prazo e cota**, sem toggle, durante operações. Um backup concluído elegível pode ser removido antes dos 30 dias para liberar espaço. Backups pendentes/incertos continuam protegidos.

### Importação antiga interpretou colunas erradas — reparo explícito (0.9.36)

1. Abra **Backups** e localize a operação com o backup íntegro.
2. Selecione uma linha e use **Mais ações → Reimportar em novo documento**.
3. Informe o separador e a codificação adequados à fonte.
4. Confira o **novo** Google Sheets criado.

A operação anterior, o antigo atalho, a planilha e quaisquer edições remotas anteriores continuam preservados. O reparo não reinterpreta nem atualiza o mesmo documento. Sem backup disponível, esse reparo não é garantido.

## 8. Documento Google apagado, enviado à lixeira ou sem permissão

- Um `.url` aponta ao documento específico; não pode reparar automaticamente permissões do Google.
- Se o documento foi **confirmadamente enviado à lixeira** e ainda há fonte local, o aplicativo pode iniciar uma nova operação independente, preservando histórico antigo.
- Respostas 404, 403 e falhas de rede **não** autorizam assumir que o remoto está na lixeira.
- Verifique acesso, conta e a lixeira diretamente no Google Drive. Não exclua o histórico para forçar upload.

## 9. Exportar diagnóstico para suporte

1. Abra a janela de erro ou **Backups/Recuperação**.
2. Na janela de falha, selecione **Exportar diagnóstico** para a **tentativa atual**. Para o histórico geral, use **Backups → Mais ações → Exportar histórico**.
3. Salve o arquivo `.jsonl` e revise **o nome sugerido**. Pode conter um identificador sanitizado do nome do documento local.
4. Não envie backups, planilhas, tokens, `auth/`, `uploads/` ou pastas inteiras de dados.
5. Compartilhe apenas o diagnóstico e o resumo do procedimento, removendo qualquer dado adicional que não deseje expor.

O JSONL exportado contém eventos tipados e etapas de processamento limitadas. Não contém células, caminhos locais, e-mails, tokens nem IDs Google. Diagnósticos não substituem o journal de recuperação.

## 10. Atualização, downgrade ou reinstalação

1. Obtenha um instalador de commit aprovado e confira seu checksum.
2. Feche o aplicativo e execute o instalador.
3. Não instale versão mais antiga sobre estado migrado. O instalador pode bloquear downgrade.
4. Não execute os scripts de ciclo de vida do CI em seu perfil pessoal.
5. A desinstalação conserva dados do usuário por padrão. Se desejar remover os dados manualmente, faça isso somente depois de recuperar e verificar tudo o que importa.

Abra um [issue no GitHub](https://github.com/zagozago/ZagoSheetsWin/issues) com versão, procedimento, resultado esperado e diagnóstico sanitizado, sem documentos privados ou segredos.


## Casos novos 0.9.38–0.9.42

**CSV/TSV com diferenças:** desde 0.9.38, valores incompatíveis com a planilha exportada do Google **bloqueiam substituição e publicação de novo atalho**. ODS/XLS conservam política separada de fidelidade parcial. Delimitadores ambíguos recebem **Escolher separador** com prévia de 3 linhas/12 colunas. CSV de coluna única, sem cabeçalho, e hint `sep=` exato podem ser válidos.

**Várias planilhas:** 0.9.42 permite imports em paralelo de arquivos diferentes. `Aguardando espaço no backup…` indica falta real de quota; espera cancelável de até dois minutos. Backups concluídos podem ser rotacionados automaticamente. Se um único arquivo for maior que a quota, é recusado de imediato. Não apague journals, fontes ou arquivos `.session` para forçar importação.

**403 Google:** só `rateLimitExceeded` e `userRateLimitExceeded` permitem retry controlado de leituras; erro de permissão não é rate-limit. `Retry-After` longo não pode ser antecipado. Criação incerta exige reconciliação pelo marcador anterior, sem novo upload cego.

**Status do candidato:** CI Windows do commit `c1dc8a5` e distribuição protegida anterior `ceed7fb` não concluíram com sucesso. Consulte [estado de versão](RELEASE_STATUS.md) antes de considerar esse build validado.
