# Solução de problemas e recuperação

**Escopo:** ZagoSheetsWin alpha **0.9.35** ([evidências](RELEASE_STATUS.md)) em Windows. Antes de repetir operações, confirme a presença do arquivo original, atalho e backup. **Não exclua `%LOCALAPPDATA%\SheetsWindows` para tentar resolver problemas.** Isso pode eliminar associações ou material necessário à recuperação.

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

1. Confira a codificação do arquivo: UTF-8, UTF-16 com BOM ou Windows-1252 **se essa preferência já estava persistida em uma instalação anterior**. Os seletores de formato, codificação e delimitador foram retirados da interface na 0.9.32.
2. Para CSV ambíguo, **normalize o arquivo antes da importação** para UTF-8 e um delimitador consistente; TSV usa tabulação. Não procure o antigo seletor nas Opções avançadas nem edite arquivos internos.
3. Verifique o limite de **20 MiB**, **500.000 células**, **50.000 linhas** e **1.000 colunas**.
4. O programa mantém CSV/TSV como **texto literal**, inclusive zeros à esquerda e fórmulas escritas como texto. Isso é comportamento intencional.
5. Se a interpretação já foi fixada para operações anteriores, não apague `formats.json` ou o banco; prepare nova fonte ou migração suportada.

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

## 7. Backup não pode ser restaurado

Verifique se a operação possui snapshot disponível e não teve backup excluído por limpeza/descarte confirmado. Restauração usa um destino novo e não sobrescreve arquivo existente. Se a limpeza já apagou o snapshot, o histórico e o Sheets remoto podem continuar, mas **não existe recuperação garantida dos bytes originais**.

O prazo de 30 dias **não elimina backups sozinho**. A limpeza automática é opcional e acontece em operações futuras.

## 8. Documento Google apagado, enviado à lixeira ou sem permissão

- Um `.url` aponta ao documento específico; não pode reparar automaticamente permissões do Google.
- Se o documento foi **confirmadamente enviado à lixeira** e ainda há fonte local, o aplicativo pode iniciar uma nova operação independente, preservando histórico antigo.
- Respostas 404, 403 e falhas de rede **não** autorizam assumir que o remoto está na lixeira.
- Verifique acesso, conta e a lixeira diretamente no Google Drive. Não exclua o histórico para forçar upload.

## 9. Exportar diagnóstico para suporte

1. Abra a janela de erro ou **Backups/Recuperação**.
2. Selecione **Exportar diagnóstico**.
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
