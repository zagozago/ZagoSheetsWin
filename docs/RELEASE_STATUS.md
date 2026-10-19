# Estado da versão — referência vigente

**Versão declarada no código: `0.9.42` (alfa).** Não é lançamento estável, Release, homologação ou certificação.  
**Branch do produto:** `feature/zagosheetswin`.  
**Commit mais recente conferido:** [c1dc8a501209](https://github.com/zagozago/ZagoSheetsWin/commit/c1dc8a501209ce37d1f65c141ad30498146296af), de **19/10/2026 UTC** (18/10 à noite no Brasil).  
**Não existem tags de versão nem GitHub Releases publicados** na consulta desta revisão.

## Coerência de código, instalador e catálogo

- [Projeto C#](../src/SheetsWindows.Windows/SheetsWindows.Windows.csproj): `<Version>0.9.42</Version>`.
- [Inno Setup](../installer/SheetsWindows.iss): fallback `0.9.42`, com distribuição parametrizada pela versão do projeto.
- [Catálogo](../i18n/source.json): `productVersion = 0.9.42`; 51 opções de idioma.
- Licenças/atribuições anteriores permanecem; documentação **não** adiciona credenciais ou informações privadas.

## Execuções de CI e pendências do candidato

| Evidência | Commit | Resultado verificado |
|---|---|---|
| [Validação 0.9.42 anterior](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39181030201) | `e2096c7` | Local core Windows/Linux **sucesso**, 392 aprovados + 1 ignorado no Windows, 350 aprovados + 43 ignorados Linux |
| [Local core após preparação de distribuição](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39181854229) | `ceed7fb` | **Sucesso** |
| [Protected alpha distribution](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39181854241) | `ceed7fb` | **Falha**: watchdog externo de 45 s no ensaio de 20 arquivos/quota extrema; 391 outros testes haviam passado |
| [Local core mais recente](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39189870249) | `c1dc8a5` | **Falha**: Windows falhou na etapa `dotnet test`; Ubuntu passou. O motivo detalhado do teste mais recente não foi confirmado nesta revisão |

**O teste no HEAD da branch 0.9.42 NÃO está verde.** A ampliação do watchdog de teste para 3 minutos e a repetição do lote de 20 arquivos constam do código, mas não se pode declarar a correção aprovada. Nenhum workflow protegido **concluído com sucesso no SHA atual** foi identificado na consulta. Não apresentar a 0.9.42 como versão estável ou certificada, nem confundir falha do teste com diagnóstico conclusivo de defeito do software.

## Contrato funcional atual da 0.9.42

1. **CSV/TSV v4:** até **2.000.000 células** (incluindo vazias), **100.000 linhas**, **1.000 colunas**, **32.767 caracteres/célula**; entrada e payload até **20 MiB**. Interpretações v1–v3 conservam seus limites/comportamento. Texto usa orçamento de até 128 MiB por XML de aba e 160 MiB de expansão ZIP, sem alterar limites de XLS/ODS/XLSX.
2. **Parser:** `,`, `;`, TAB e `|`; cabeçalho não obrigatório; entrada de coluna única válida nos casos suportados; aspas, multilinhas, `sep=` inicial exato e seleção com prévia até três linhas/doze colunas em ambiguidade. CP1252 estrito/controlado quando aplicável.
3. **Fidelidade:** para **CSV/TSV**, divergências de valores na exportação Google impedem retirada da fonte e publicação do atalho; para **XLS/ODS**, recursos complexos com equivalência parcial ainda podem gerar aviso e substituição após importação confirmada, conforme consentimentos e requisitos de integridade.
4. **Recuperação:** interpretar opções/versionamento por operação; reparar backup em **novo documento** sem sobrescrever o antigo; reconciliar uploads incertos com o mesmo marcador; restaurar atalho ausente apenas sob journal seguro.
5. **Backup:** padrão de **30 dias/200 MB**, configurável entre 1–365 dias e 1–1.000 MB (o campo de quota usa unidades decimais em bytes). Limpeza por prazo e quota **automática e sem toggle**; só concluídos verificados são elegíveis. Pendentes, desconhecidos, alterados e material de reparo protegido são preservados.
6. **Concorrência:** a fila global introduzida na 0.9.40 foi retirada em 0.9.42. Arquivos diferentes podem importar **em paralelo**; o lock por fonte previne operações concorrentes sobre a mesma identidade. Uma captura mantém lock de manutenção curto. **Somente falta de quota comprovada** aciona espera cancelável de até dois minutos entre tentativas, com checagem a cada 250 ms e limpeza coordenada.
7. **Google rate-limit:** leitura de metadados/exportações aceita retry com backoff apenas em 403 com motivo permitido (`rateLimitExceeded`, `userRateLimitExceeded`), além das regras para 429/5xx; `Retry-After` maior que 30 s impede nova tentativa antecipada. **Não repete POST de criação às cegas.**
8. **UI:** erro terminal persistente, prévia de importação e botão de escolha para ambiguidade, ação Backups/reparo, ícone de planilha unificado (0.9.37), estatísticas de backup com fonte 8 pt (0.9.42) e textos atualizados em 51 idiomas.
9. **ODS:** rejeita pacotes inválidos e repetições fora da grade sem materializá-las; bytes originais enviados em casos aceitos, mas não há garantia de conversão complexa fiel nem homologação com Google real.

## Antes da futura 1.0.0

- Investigar e resolver o **CI Windows vermelho do commit `c1dc8a5`**; exigir **Local core e Protected alpha distribution verdes no mesmo SHA final**.
- Homologar Google real e arquivos de teste, sobretudo ODS, CSV/TSV perto de 2M de células, falhas, retentativas, cotas e reparo.
- Inspecionar scripts de reinstalação, assinaturas/hashes do pacote, configuração pública OAuth, licenças e suporte web.
- Revisar documentação traduzida integralmente; mensagens provisórias dos READMEs internacionais ainda não têm revisão editorial.
- Após aceite explícito, preparar tag/Release oficiais e registrar 1.0.0 no [CHANGELOG](../CHANGELOG.md). **Não publicar automaticamente**.

Relatórios por versão: [0.9.38](REGRESSION_0938.md), [0.9.39](REGRESSION_0939.md), [0.9.40](REGRESSION_0940.md), [0.9.41](REGRESSION_0941.md), [0.9.42](REGRESSION_0942.md). São evidências datadas, não garantias do build corrente.
