# Auditoria 0.9.37 → candidato 0.9.38

Baseline: `f58b630263cdee07e473a54ee40ff6d3beafe4f6`, branch `feature/zagosheetswin`.
Este é um candidato de validação; publicação não autorizada.

## Correções e decisões

- Parser v3 em novas importações: análise completa de gramática/largura com contadores antes de alocar a tabela. Alternativas inválidas não inventam falhas de capacidade. Vírgulas decimais em campos numéricos de tabela regular com ponto e vírgula são evidência; empates reais 13/14 exigem escolha. Nenhuma exigência de cabeçalho.
- TSV sem tabulação estrutural aceita coluna única com pontuação irregular. CSV irregular permanece rejeitado. Aspas estritas, valores literais e escape OpenXML preservados.
- `sep=,`, `sep=;`, `sep=|` e `sep=<TAB>` só na primeira linha exata: removida do payload normalizado e preservada no backup. A tabela seguinte deve ser regular e ter mais de uma coluna; conflito com escolha explícita é erro.
- Delimitador efetivo, encoding e geração de interpretação persistidos por operação. Intenções antigas mantêm v1/v2 e os mesmos bytes; nenhuma migração silenciosa de upload pendente.
- Prévia local de até 3 linhas/12 colunas após escolher separador, sem envio de conteúdo. A confirmação fica bloqueada até uma interpretação válida.
- CSV/TSV v3: 100.000 linhas totais (cabeçalho incluído), 1.000.000 células; 20 MiB de entrada/payload e 32.767 caracteres por célula mantidos. Outros formatos continuam 50.000 linhas/500.000 células. XML por aba continua limitado a 64 MiB, pacote descompactado total a 96 MiB. O writer v3 omite xml:space apenas em valores sem nenhum whitespace; valores com espaços/quebras permanecem marcados, e writers v1/v2 mantêm exatamente os bytes anteriores.
- Escrita XLSX valida linhas/células/colunas/caracteres e cancelamento antes de publicar payload. Divergência de valores remotos em CSV/TSV agora impede retirada do original e publicação do atalho. A política anterior para recursos complexos de XLS/ODS não foi alterada.
- Resposta de conclusão incompleta tenta reconciliar exatamente o mesmo marcador, sem nova sessão. Nenhum resultado ou múltiplos resultados continuam bloqueados; sessão expirada não provoca outra criação. Hash, conta, tamanho, MIME, payload e marcador protegem a retomada. Sessões anteriores sem o novo campo de marcador mantêm validações existentes.
- Telemetria distingue SessionInitiated/SessionAck, lookup vazio/múltiplo e reconciliação; código RemoteReconciliation precede IOException. ID opaco da operação após snapshot e linha lógica de erro são permitidos no JSONL; sem nomes, caminhos, dados, contas, URLs de sessão ou tokens. Started pode não ter ID ainda; AttemptId correlaciona a tentativa.
- Sete novos motivos locais nos 51 idiomas: aspas abertas/mal posicionadas, vazio, dica conflitante, largura desigual, repetição/dimensão ODS e estrutura ODS. XML ODS malformado e parte ausente têm códigos próprios. A instrução de recuperação remota aponta aos botões localizados. Sem nova evidência de regressão minimize/restore; teste existente mantido.
- ODS 03/06: decisão conservadora de rejeitar atributos fora da grade. Não basta ignorar vazios localmente e mandar os atributos enormes ao Google; normalização fiel fica fora deste candidato. Não há mudança nos bytes originais/backup.

## Caso A-01

Os 29 JSONL confirmam 28 falhas e uma reconciliação na 0.9.37. O caso multiline parou em RemoteCandidate Started após intenção persistida. Isso não demonstra defeito do parser, duplicação nem indisponibilidade Google. A causa real da sessão do dispositivo segue não comprovada. Os testes simulam respostas perdidas/incompletas, retomada 308, expiração, lookup nenhum/único/múltiplo, conta/payload divergentes e reinício.

No dispositivo: instalar candidato, manter o estado e o backup existentes, tentar recuperar a mesma operação. Se lookup continuar incerto, permanecer bloqueado; não apagar estado, não reimportar manualmente e não compartilhar `.session`.

## Evidência e gates

Benchmark isolado Linux Release, sintético 100.000×10: conversão 1.401 ms, total 3.484 ms, CPU 3.830 ms, pico 261.541.888 bytes (~249 MiB), alocações cumulativas da thread 555.144.120 bytes, XLSX 3.401.121 bytes. Alocação cumulativa não equivale a memória simultânea. Orçamento do teste nativo Windows: pico de processo <512 MiB; tempo local por caso <2 min. Não foi simulado um PC Windows de pouca RAM.

Os catálogos foram preservados e a [matriz dos 78 casos](REGRESSION_MATRIX_0938.md) revisada. Os 26 anteriores constam como aprovados **pelo usuário na baseline**, não como 26 novos reprocessamentos nesta iteração. As regressões automáticas sintetizam as formas relevantes, sem publicar fixtures pessoais. Os sete volumes devem ser testados no Google real, em cópias, comparando início/meio/fim, dimensões, backup e atalho. Capacidade permanece experimental até esse aceite. OAuth, assinatura e publicação seguem gates separados.

Hashes SHA-256 das evidências recebidas:

- `78index.html`: `d79e9045b72d4d98c4b918bb7e3221675d7a69f5e9da7e93b7f8a5daf6c398ad`
- `26catalogo.html`: `f946c1cac2820c7e5173e898156bf9ce889a6dddf3fbec622901fea0cfcabf14`
- `ZSW-diagn-26e78.zip`: `185b244d5936d962bcc3acceb9e6f5c4b50507e07b4be5dddc6305b1e16fa8e9`

Validação local Release final: 323 testes aprovados, 42 específicos do Windows ignorados, 0 falhas (365 casos). Solução Windows/Linux compilada sem avisos. Contrato de localização: 14 testes aprovados; catálogo de 617 chaves/51 idiomas validado; auditoria estática de segurança aprovada. A validação nativa Windows e o ciclo de instalador são executados separadamente pelo Actions; seus resultados não são inferidos do build local.
