# Estado da versão — referência vigente

**Versão do código: `0.9.43` (alfa).** Esta é uma build distribuída pelo fluxo protegido, **não** uma Release estável 1.0.0 nem certificação.  
**Branch de produto:** `feature/zagosheetswin`.  
**Commit conferido:** [9fbec2add18c](https://github.com/zagozago/ZagoSheetsWin/commit/9fbec2add18c012012405b56a333f6e49248a6e5) em **20/10/2026 UTC** (19/10/2026 no fuso brasileiro).  
**Tags e GitHub Releases:** nenhuma encontrada na consulta desta revisão.

## Versão e artefatos

- [Projeto C#](../src/SheetsWindows.Windows/SheetsWindows.Windows.csproj): `<Version>0.9.43</Version>`.
- [Inno Setup](../installer/SheetsWindows.iss): fallback `0.9.43`; distribuição protegida injeta versão do projeto.
- [Fonte internacional](../i18n/source.json): `productVersion = 0.9.43`, revisão 16; 51 idiomas.
- Preservados branding Zagotools, ícone de planilha, licença MIT, créditos Open in Google/Swati K e avisos de terceiros.

## Estado de validação no commit exato

| Evidência | Commit | Estado |
|---|---|---|
| [Local core #39346495162](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495162) | `9fbec2a` | **Sucesso Windows e Ubuntu** |
| [Protected alpha distribution #39346495298](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39346495298) | `9fbec2a` | **Sucesso**, execução completa, com empacotamento e checagens de instalador |
| [Relatório de regressão 0.9.43](REGRESSION_0943.md) | `788b4c7` (anterior) | Windows **398 aprovados / 1 ignorado**, Linux **355 aprovados / 44 ignorados**; os números são do SHA citado, não do HEAD |

A execução protegida disponibilizou artefatos **ZagoSheetsWin-Setup-win-x64**, **ZagoSheetsWin-win-x64**, resultados de testes/linguagem e prévia de branding. A presença desses artefatos **não** equivale a GitHub Release publicada, assinatura de publicador comprovada ou homologação com uma conta Google real. A própria documentação de regressão distingue testes offline/simulados de campo.

## Comportamento vigente da 0.9.43

1. **Backup manual e cota:** política padrão de **30 dias de retenção e 200 MB**, configurável até 365 dias e 1.000 MB. **A limpeza automática foi retirada**, inclusive por idade ou espaço. O usuário usa **Limpar expirados** ou seleciona backups concluídos para apagar. O campo `AutomaticCleanup` carregado de configurações anteriores é normalizado para **false**. Backups pendentes/não verificados continuam protegidos; números das regras salvam automaticamente.
2. **Sem espera por capacidade:** capturas de snapshot mantêm checagem atômica da quota sob lock curto, mas **não fazem polling, rotação ou espera**. Falta de espaço gera erro prontamente e orienta limpeza manual, **preservando arquivo original e os backups anteriores**. Arquivos maiores que a quota também são recusados.
3. **Concorrência:** arquivos de origem **diferentes** podem importar em paralelo; sem fila de perfil nem limite arbitrário de quantidade, com locks de fonte/associação/backup nos pontos necessários. A memória disponível e o serviço Google continuam impondo limites reais.
4. **CSV/TSV v4:** até **2.000.000 células** (vazias contam), **100.000 registros lógicos**, **1.000 colunas**, **32.767 caracteres por célula**, com **20 MiB** para origem/payload. Parser `,`, `;`, TAB e `|`, `sep=` exato, aspas/linhas internas, prévia de ambiguidade. Versões antigas mantêm seus limites.
5. **Fidelidade:** diferenças de valores CSV/TSV exportados do Google **impedem substituição e publicação de atalho novo**; XLS/ODS de recursos complexos continuam sob política distinta de conversão parcial e backup.
6. **Resiliência:** snapshot enviado com um **buffer de tamanho exato**, sem segunda cópia integral; pastas Google existentes têm verificação única; confirmação remota incerta realiza até **duas reconsultas read-only** pelo marcador sem duplicar criação/sessão. ACKs registram apenas status numérico.
7. **Baixa memória e interface:** exceções de memória insuficiente têm encerramento simples com **Fechar**, sem custo extra de serialização de logs ou construção de retry. A janela Home mantém minimizar, bloqueia maximizar/redimensionar e agora tem **ClientSize 500 × 192 px**, corrigindo margem inferior excessiva; testes nativos cobrem minimizar/restaurar e DPI/51 idiomas.
8. **Diagnóstico e recuperação:** conserva journal/reconciliação remota, restauração em novo local, reparo CSV/TSV em novo documento, atributos somente leitura e fluxos protegidos de atalho.

## Preparação da futura 1.0.0

O antigo bloqueador **CI Windows vermelho na 0.9.42** foi superado por **Local core e Protected alpha distribution verdes no mesmo SHA 0.9.43**. Ainda restam validações que **não são provadas por CI offline**:

- Instalar o pacote protegido exato no Windows do usuário e testar lotes de 12/20 arquivos simultâneos, memória limitada, quota cheia e **limpeza manual**, sem remoções inesperadas.
- Homologar importação Google real, inclusive CSV/TSV de milhões de células e ODS complexo, vinculação e reconciliação remota.
- Testar o retorno após falha por memória insuficiente e verificar dimensões da janela Home, temas, fontes e escalonamento.
- Revisar integralmente as 50 páginas de README traduzidas, URL de ajuda, OAuth público, licenças e estado de assinatura/SmartScreen.
- Depois de aceite formal, congelar commit, validar artefatos/hashes e então planejar tag/Release 1.0.0. **Não publicar automaticamente.**

Referências históricas: [0.9.38](REGRESSION_0938.md), [0.9.39](REGRESSION_0939.md), [0.9.40](REGRESSION_0940.md), [0.9.41](REGRESSION_0941.md), [0.9.42](REGRESSION_0942.md) e [0.9.43](REGRESSION_0943.md). Relatórios antigos permanecem datados; não representam política vigente quando alterada por versão posterior.
