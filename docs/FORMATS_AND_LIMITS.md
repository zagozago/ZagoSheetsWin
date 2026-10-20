# Formatos, ambientes e limites — estado atual

**Referência:** código de `SpreadsheetFormats`, `GoogleImport`, `ConversionVerifier`, `ReplacementCoordinator`, `WindowsLauncher` e políticas Windows no candidato alfa **0.9.43** ([estado e commit](RELEASE_STATUS.md)). Esta é a matriz vigente, não os limites históricos do piloto 0.7 ou a política de fidelidade da 0.9.30.

## Entrada e destino

| Extensão | Preparação antes do Drive | Critério para retirar o original |
|---|---|---|
| `.xlsx` | Validar pacote OOXML e enviar XLSX | Fonte local elegível; backup íntegro, associação e atalho verificados, sem VBA; não há certificação de todos os recursos visuais |
| `.csv` | Parser v4 sem cabeçalho obrigatório, coluna única, `sep=` exato, aspas/CRLF, quatro separadores e prévia | Até **100.000 registros e 2.000.000 células**; **diferença de valores impede substituição** |
| `.tsv` | Parser v4 com campos entre aspas e multilinha, coluna única válida | Até **100.000 registros e 2.000.000 células**, com verificação de valores para substituição; versões antigas mantêm limites originais |
| `.ods` | Validar assinatura/ZIP/mimetype e calcular coordenadas de células sem expandir caudas vazias repetidas; enviar **bytes ODS originais** | Fórmulas, datas, mesclagens e recursos complexos podem não ser preservados; isso não impede por si só substituição após importação confirmada. Estrutura inválida/scripts recusados; teste Google real pendente |
| `.xls` | Binário: ExcelDataReader + NPOI para gerar XLSX. Alguns relatórios HTML nomeados `.xls` têm leitor HTML específico | Fórmulas/valores complexos podem mudar **e a origem ainda pode ser substituída** se a opção de XLS permitir. Macros não são executadas nem preservadas |

**Fora do escopo:** `.xlsm`, `.xlsb`, Word, PowerPoint e qualquer extensão não listada. Arquivo HTML **renomeado para `.ods`** não é um arquivo ODS: a importação é rejeitada sem upload.

## Mudança crítica da política de substituição (0.9.33–0.9.35)

A presença de fórmulas, recursos complexos ou uma **diferença de valores identificada na verificação** já não impõe importação como cópia. A classe `ConversionVerifier` marca a limitação e permite a conclusão quando o documento Google está confirmado, o vínculo remoto é válido, o backup original está íntegro e as demais regras de retirada foram atendidas.

**Isso não garante fidelidade.** O conteúdo no Google Sheets pode ser diferente. Após uma substituição, o arquivo da pasta original vira um **atalho `.url`**; os bytes anteriores continuam no backup e podem ser restaurados separadamente. Se conservar o arquivo no lugar for essencial, selecione **Importar como cópia** antes da operação.

São bloqueios reais, não simples avisos: pacote inválido, script/estrutura recusada, importação remota não confirmada, resposta de exportação com falha, associação/hash inconsistente, ausência/corrupção de backup, alteração conflitante da fonte ou impossibilidade de remover o original. A política de cópia automática para **rede/pastas sincronizadas** e as escolhas do usuário continuam aplicáveis. A 0.9.35 reconhece que o atributo **somente leitura** do Windows pode impedir a retirada; nesse caso, preserva original e backup.

## Limites implementados

| Recurso | Limite |
|---|---:|
| Bytes de entrada e payload normalizado | 20 MiB (`20 × 1024 × 1024` bytes) |
| Células processadas (outros formatos / interpretação antiga) | 500.000 |
| Linhas (outros formatos / interpretação antiga) | 50.000 |
| Células CSV/TSV com interpretação v2 | 600.000 |
| Registros CSV/TSV com interpretação v2 | 60.000 |
| Células CSV/TSV com interpretação v3 | 1.000.000 |
| Registros lógicos CSV/TSV com interpretação v3/v4 | 100.000 |
| Células CSV/TSV com interpretação v4 | **2.000.000** |
| Colunas | 1.000 |
| Abas nos leitores aplicáveis | Até 100 |
| Texto por célula/campo | 32.767 caracteres |
| Conteúdo expandido de pacote ZIP (geral) | 96 MiB |
| Parte XML individual (geral) | 64 MiB |
| Pacote expandido para conferência de CSV/TSV | 160 MiB |
| XML de aba para conferência de CSV/TSV | 128 MiB |
| Resposta de exportação XLSX via Google | 10 MiB |
| Metadados HTTP Google (com restrições adicionais por endpoint) | Até 1 MiB |
| Checkpoints no diagnóstico por tentativa | 96 |
| Rotação de logs | 4 arquivos × 64 KiB |

Todos os limites pertinentes se aplicam simultaneamente. Um arquivo abaixo de 20 MiB pode falhar por excesso de linhas, células, XML expandido ou exportação. O Google também aplica regras próprias. Nenhum limite representa compromisso de fidelidade total.

## CSV e TSV

- Automático na interpretação v2: UTF-8 estrito, com ou sem BOM; UTF-16 LE/BE somente com BOM. Se **não houver BOM** e UTF-8 estrito falhar, um **fallback controlado para Windows-1252** é permitido, excluindo bytes indefinidos e controles incompatíveis com texto. Operações antigas com semântica v1 continuam com as regras de leitura anteriores.
- Windows-1252 também pode ser escolhido **na janela específica da importação ou reparo**; não há seletor global atual em Opções avançadas. A seleção é rejeitada se conflitar com BOM.
- CSV/TSV v4: reconhecer **vírgula, ponto e vírgula, TAB e barra vertical (pipe)**, cabeçalho opcional, coluna única quando válida; ambiguidade real usa escolha com prévia local. `sep=` só é aceito na primeira linha exata.
- TSV com operação legada v1 preserva interpretação TAB da versão anterior.
- Parser suporta campos vazios, aspas escapadas e quebras de linha em campo citado; largura irregular é rejeitada. **Registros lógicos não equivalem ao número de linhas físicas de arquivo**.
- Todos os valores CSV/TSV são mantidos como **texto literal**, incluindo zeros à esquerda, números longos, datas escritas como texto e cadeias com `=`, `+`, `-` ou `@`. Essa regra evita execução de fórmulas na importação, mas também exige conversões manuais posteriores no Sheets quando desejadas.

## Interpretação versionada e reparo seguro (0.9.36)

O interpretador CSV/TSV registra **opções e versão imutáveis** por operação. Importações antigas com intenção remota preservam a versão 1, inclusive suas limitações; a versão 2 aplica nova detecção e limites. A conferência do XLSX exportado usa os limites específicos para texto e respeita o interpretador da própria operação.

Para corrigir um CSV/TSV antigo, usar **Backups → Mais ações → Reimportar em novo documento**. O programa verifica o hash do backup original, cria **nova operação e documento Google** e mantém intactos a planilha anterior, o atalho anterior e o vínculo remoto. O reparo é **como cópia**, não faz retirada da fonte. Correção de conteúdo local antes de qualquer intenção remota pode iniciar operação nova sem descartar o snapshot anterior.

Em erros permanentes de entrada, repetir automaticamente sem alterar o arquivo não corrige a causa; a repetição manual é condicionada a mudança do arquivo. Ambiguidade de separador usa escolha por tentativa, não edição de `formats.json`.

## XLS e relatórios HTML

O `.xls` binário usa leitura independente e conversão para XLSX. Recursos como VBA, desenho, proteção, formatação condicional e rich text **não** têm fidelidade garantida. Depois de importação Google confirmada, a substituição segue a preferência de XLS e os controles de integridade **mesmo que haja equivalência apenas parcial**. Para conservar o original no mesmo local, use o modo de cópia ou desative substituição de XLS. Arquivos HTML de exportadores antigos com extensão `.xls` são tratados como **HTML**, sem executar scripts ou carregar URLs externas; tabelas são convertidas para abas e informações externas podem ficar em uma aba específica. Isso **não** equivale a suporte amplo a qualquer HTML.

## ODS: pendência explícita

- ODS válido é um pacote OpenDocument identificável por estrutura, `mimetype` e conteúdo. Arquivos inválidos não se tornam válidos pela extensão.
- ODS simples com valores conferíveis pode ser importado e, se elegível, substituído.
- ODS com fórmulas, datas/horários, objetos, mesclagens, estilos ou outros elementos **sem equivalência comprovada pode ser importado e ter o original substituído**, conforme preferências e após importação confirmada. O backup mantém os bytes originais; **não pressupor que o Sheets terá os mesmos dados ou fórmulas**. Estruturas inválidas e scripts podem ser recusados.
- O exame estrutural de exemplos reais não prova que todos os documentos ODS funcionem. **ODS continua sob validação de compatibilidade e homologação real.**

## Localização do arquivo

- Pasta local, estável e não sincronizada, com acesso de escrita: candidata à substituição.
- OneDrive, pasta sincronizada reconhecida, UNC e unidade de rede: **cópia**, sem remoção da fonte.
- Arquivo somente na nuvem ou referência simbólica incompatível: bloqueio até estar disponível localmente, ou recusa.
- A detecção de sincronização é uma proteção, **não uma certificação para todos os provedores**.

O Google Sheets criado é a cópia de trabalho online. O backup da importação inicial não recebe edições posteriores. Se o Google Sheets associado for confirmado na lixeira e a fonte local ainda existir, uma **nova** operação pode ser criada preservando os registros antigos. Erros 404, acesso negado ou falha de rede não significam “na lixeira”.

## Critério de verificação

Quando o formato tem valores esperados, o aplicativo pode exportar o Sheets como XLSX e compará-los. Na **0.9.42**, diferenças `ConversionMismatchException` em **CSV/TSV** **bloqueiam a substituição**. Apenas formatos **não textuais**, como XLS/ODS, podem tratar diferenças de equivalência como advertência. Falhas reais na exportação, associação remota, snapshot ou conteúdo estrutural ainda bloqueiam. A comparação não abrange gráficos, macros, formatação nem a semântica total das fórmulas.

**Interface atual:** os cinco formatos estão habilitados sem toggle. A seção **global** de formato/codificação/delimitador foi removida das Opções avançadas em 0.9.32; escolhas `formats.json` pré-existentes permanecem aplicáveis. Desde a 0.9.36 há um **seletor contextual de interpretação por importação** para casos ambíguos ou reparo, sem modificar operações remotas anteriores.


## Especificações de CSV/TSV v4 (vigentes na 0.9.43)

O teto de **2.000.000 células** (incluindo vazias) aplica-se a CSV/TSV **v4**, até **100.000 registros**, **1.000 colunas**, 32.767 caracteres por célula e 20 MiB de origem/payload. XLS/ODS/XLSX não herdam esse aumento. Conferência textual: XML de aba de até **128 MiB** e pacote expandido de até **160 MiB**; não textuais: 64 MiB/96 MiB. Operações v1/v2/v3 mantêm formato de upload e semântica de interpretação previamente vinculados.

Para CSV/TSV, **a comparação de valores deve passar para substituir o original**; diferentemente de XLS/ODS com recursos complexos, divergência textual é falha e não somente aviso. A exportação/comparação ocorre antes de publicar atalho novo.

A limpeza por prazo e capacidade do backup é **manual**, sem toggle de limpeza automática. **A 0.9.43 retirou a rotação de backups concluídos e a espera por capacidade.** Importações de fontes diferentes continuam avançando em paralelo; a captura consulta atomicamente a cota sob lock curto. Se faltar capacidade, falha prontamente, preservando o original e orientando **Limpar expirados** ou exclusão manual de backups concluídos elegíveis. O limite de cota do backup em MB usa bytes decimais; o tamanho de arquivo em MiB usa bytes binários.


## Política de backup e requisitos de memória (0.9.43)

O tamanho máximo de entrada (20 MiB), o limite atual CSV/TSV v4 (2 milhões de células) e os orçamentos XML/ZIP **não mudaram** nesta versão. Entretanto, o armazenamento de backup já usado deve ter capacidade suficiente antes de capturar a origem. A cota é checada atomicamente, de modo que capturas simultâneas não podem ultrapassá-la; **nenhuma exclusão, espera ou nova tentativa automática por capacidade** é realizada. Nem o prazo de retenção nem a cota autorizam remoção automática de backups concluídos.

O usuário deve gerenciar o espaço em **Backups**, selecionando backups concluídos ou executando **Limpar expirados** quando aplicável. Backups pendentes ou não verificados não são apagados por esse fluxo. O controle `AutomaticCleanup` permanece apenas para ler configurações antigas; o código atual normaliza para **false**.

A implementação usa um buffer exato para upload de bytes do snapshot, evitando uma cópia integral redundante, e tenta reduzir alocações adicionais ao lidar com `OutOfMemoryException`. **Não é uma garantia de baixa utilização de RAM** para conversões com dois milhões de células ou lotes paralelos. O aceite de desempenho/compatibilidade com Google real permanece necessário.