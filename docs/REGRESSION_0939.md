# Ajustes 0.9.38 → candidato 0.9.39

Baseline: `2c275590fad46bfc1246ab5f47df3d81f8824c0f`. Evidências: ZIP com 4 diagnósticos e os 3 CSVs originais; screenshot anexado. Publicação não autorizada.

## Alterações

- Somente ambiguidade real (T013/T014): ação principal “Escolher separador”, verde escuro, texto branco em negrito. Abre a mesma escolha com prévia. O botão interno continua “Tentar novamente”; outros erros mantêm sua ação/estilo. Traduções nos 51 idiomas. Separador personalizado adiado: exigiria ampliar validação, interpretação persistida e prévia; os quatro conhecidos permanecem.
- Cota de backups: liberar espaço pelos concluídos mais antigos antes da nova captura, mesmo com expiração automática desligada. O toggle agora descreve exclusivamente a limpeza por prazo. Pendentes, associações não verificadas, backups desconhecidos e a operação atual continuam protegidos. Não se apagam originais, atalhos ou planilhas Google. Se não houver espaço recuperável, a captura continua bloqueada.
- T012: delimitadores exclusivamente dentro de aspas são texto de uma coluna; a geração v4 aceita o CSV real preservando pontuação e os três registros. CSV irregular com delimitadores fora de aspas continua rejeitado.
- T027: o arquivo REAL tem `sep=;`, cabeçalho de 3 colunas e uma linha inteira entre aspas, `"1;Ana;1,20"`, que é uma única célula CSV. A rejeição é correta; não reinterpretar o interior de um campo como três campos. O controle válido usa `1;Ana;1,20` sem as aspas envolvendo a linha inteira. Ambos têm testes independentes; o mestre não foi editado.
- T031: o writer XLSX anterior normalizava CRLF para LF, gerando diferença na conferência. Writer v4 usa XML Entitize para preservar CR; na conferência de CSV/TSV CRLF e LF são equivalentes como quebra lógica. Nenhum trim, mudança de caixa, remoção de espaços, conversão de tipos ou normalização de CR isolado. Diferenças nos demais caracteres continuam bloqueando a retirada. Backup retém os bytes originais.
- Intenções/sessões v1/v2/v3 preservam seus payloads e interpretações. O verificador pode conferir o documento T031 anterior sem reenviá-lo. Não se apaga nem recria remotamente uma operação incerta.
- T031 segunda tentativa: JSONL informa FileMissing depois de Replacement Started, sem caminho (privacidade). Não prova qual arquivo faltou. Foi reproduzido e corrigido o caso de atalho removido em journal 1/2: recriar apenas a URL registrada, após verificar/segurar original e backup, sem sobrescrever arquivo alheio e sem outro upload. Arquivo original alterado bloqueia a recuperação.
- Nova substituição verifica a conversão ANTES de publicar o atalho. Uma diferença não deixa novo atalho de importação incompleta. Operação concluída nunca retira um original recriado.
- Diagnóstico de diferença de valores usa ConversionValues, não FileAccess. Enum aditivo; sem conteúdo/caminhos/tokens nos JSONL.

## Validação

A solução compila localmente em Release sem avisos. Contrato de localização: 14 testes aprovados, 619 chaves e 51 idiomas. Testes incluem bytes equivalentes dos três anexos, CRLF/LF e CR isolado, T027 inválido/válido, recuperação de atalho ausente com original preservado/alterado, proteção de atalho alheio e rotação com expiração automática ligada/desligada. CI Windows/Linux e inspeção nativa são gates separados. Google real do dispositivo ainda requer aceite após instalar o candidato.

No dispositivo, retomar o T031 pelo MESMO caminho/operação; não renomear para forçar importação, não apagar estado e não enviar arquivos `.session`. Se persistir divergência, exportar o novo diagnóstico.

## Hashes SHA-256 dos CSVs recebidos

- `12_csv_coluna_unica_aspas.csv`: `98ce7407a817bd480eaaf81060779c631c386fcfc548f27dca33f081ff6dc0a2`
- `27_csv_sep_excel_hint.csv`: `52bff58a8a677a86fec4e5c0c1554c55fdb30b0365748757348a4a1cdddc4e2f`
- `31_csv_aspas_multilinha_crlf.csv`: `26bebc12f846cb99ace016bf61298b902df08d4966bc1beaba800c0218d97b4b`

Resultado local Release: 332 testes aprovados, 42 exclusivos do Windows ignorados, 0 falhas (374 casos). A suíte anterior de 365 casos continua incluída.
