# ZagoSheetsWin 0.9.41 — CSV/TSV com até 2.000.000 de células

Base: `22fabdf9cc85dab91492a1b6eda4cff1e5b2005e` (0.9.40). Inclui todas as correções de fila, backup e retry da 0.9.40.

## Alteração

O limite de CSV/TSV passa de 1.000.000 para 2.000.000 de células, incluindo células vazias na tabela retangular. Permanecem 100.000 linhas, 1.000 colunas, 32.767 caracteres por célula, 20 MiB por arquivo/normalização e 10 MiB na resposta de exportação. O limite é combinado com essas restrições; não representa garantia de aceitar qualquer conteúdo com dois milhões de células.

A conferência de texto usa agora até 128 MiB por XML de aba e 160 MiB para o pacote descompactado. A leitura continua com XML seguro, sem DTD/resolver, contadores, cancelamento e limite de entradas ZIP. Antes de enviar, o XLSX gerado também é conferido contra esse orçamento, para não aceitar um payload que a própria verificação recusaria.

Os caminhos de ODS/XLS/XLSX não herdam esse aumento: permanecem seus limites anteriores, inclusive os orçamentos XML de 64 MiB e pacote de 96 MiB onde aplicáveis. Não houve mudança no algoritmo de interpretação nem nos bytes de payloads já aceitos; sessões antigas mantêm sua interpretação. Os limites de v1/v2 permanecem preservados. O texto de capacidade e o erro de XML foram atualizados nos 51 idiomas.

## CSV anexado

Arquivo recebido: `GDEX1ADC geo codes no fbo or military(1).csv`.

- 3.317.186 bytes; UTF-8; separador `|` detectado automaticamente.
- 8.782 linhas × 116 colunas = **1.018.712 células**, incluindo cabeçalho e vazias.
- Conversão local para XLSX e conferência de todos os valores: **aprovadas**.
- XLSX normalizado: 4.143.546 bytes; XML de aba: 51.538.776 bytes.
- Execução local isolada: 3.250 ms; pico de memória 145.797.120 bytes (medição Linux, não garantia para todo dispositivo).
- SHA-256 do original: `F2A65B14F37E79FBD347E2DB441AD8D5DC5E3397F9D4ED8E99B60B87A64B686D`.

O conteúdo do CSV não foi incluído no repositório. Esse ensaio não fez upload nem conversão real no Google.

## Regressões

Um teste sintético percorre exatamente 100.000 × 20 = 2.000.000 células com valores distintos, exporta XLSX, exige XML acima do antigo teto de 64 MiB e confere todos os valores. Uma célula adicional em outra aba é recusada antes de gerar payload. CSV automático/explícito e TSV mantêm a recusa acima do novo teto; linhas/colunas e limites dos outros formatos não foram ampliados.

O benchmark nativo do Windows também passa a exercitar dois milhões de células com orçamento de pico de memória de 512 MiB. Os testes antigos que esperavam recusa em um milhão foram ajustados para exceder dois milhões, usando dados compactos para que a checagem exercite células, não o limite de bytes.

Validação local Release: 347 testes aprovados, 43 exclusivos de Windows ignorados, total 390, nenhuma falha. Compilação sem avisos/erros; 14 testes do catálogo e auditoria estática aprovados.

## Resultados do Actions

Código validado: `8d589283e454898fdeeac3a65b2b90da753db017`. [Execução de validação](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39174444610). TRX: Windows 389 aprovados e 1 ignorado; Linux 347 aprovados e 43 ignorados; total 390, nenhuma falha. Interface nativa e 51 idiomas aprovados.

Benchmark nativo `OfflineCsv100000x20RoundTrip`: 2.000.000 de células, 4.867 ms totais (2.333 ms conversão, 2.531 ms conferência), pico de 489.345.024 bytes, abaixo do orçamento de 512 MiB (536.870.912 bytes). Sem rede Google nesse benchmark.

Instalador: compilação, transições nos 51 idiomas, instalação, atualização, preservação na desinstalação e registro de integridade aprovados na mesma execução. Teste real no Google permanece para o aceite no dispositivo.

## Correção do teste de distribuição (18/10/2026)

A distribuição [39175119834](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39175119834) passou compilação, 389 testes Windows, interface nativa com dois milhões de células, empacotamento e os 51 idiomas. Falhou na reinstalação imediata em `Test-Lifecycle.ps1`: Inno retornou código 4, CreateFile com erro 3 ao criar `unins000.dat`.

Os logs comprovam sobreposição: o teste começou a reinstalação às 23:19:11.792 UTC; o clone temporário da desinstalação anterior ainda excluía a pasta às 23:19:12.245, exatamente quando a reinstalação tentou criar seu registro de desinstalação. O log anterior só fechou às 23:19:12.255. Esperar apenas `WaitForExit` do executável registrado não garantia o fim dessa limpeza.

O teste agora exige confirmação de sucesso e fechamento do log do desinstalador, dentro do mesmo orçamento de 120 segundos, antes de reinstalar. A leitura compartilha o log com o processo escritor. Não há retry de instalação, pausa fixa para mascarar a corrida, exclusão manual de pastas, alteração no instalador entregue nem relaxamento de verificações. Duas sequências adicionais de reinstalação/desinstalação imediata repetem as checagens de preservação de dados, atalhos e padrões do Windows.

Referência do comportamento do clone: https://jrsoftware.org/ishelp/topic_uninstexitcodes.htm. A validação Windows da correção será registrada após o Actions. A versão do produto permanece 0.9.41.
