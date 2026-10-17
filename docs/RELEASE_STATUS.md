# Estado da versão — única referência para documentação vigente

**Versão do produto no código: `0.9.35` — candidata alfa.** Não é lançamento estável nem certificação.  
**Branch de referência:** `feature/zagosheetswin`.  
**Commit mais recente conferido:** [9929524e67fe](https://github.com/zagozago/ZagoSheetsWin/commit/9929524e67fea42c8940ae27b41f80963479c490), em 17/10/2026.  
**Sem tags de versão e sem GitHub Releases publicados** na consulta de 17/10/2026.  
**Objetivo futuro:** avaliar promoção de um commit homologado para `1.0.0`, sem mudar o número do produto antes do aceite.

## Evidências do código e da automação

- [Projeto Windows C#](../src/SheetsWindows.Windows/SheetsWindows.Windows.csproj): `<Version>0.9.35</Version>`.
- [Inno Setup](../installer/SheetsWindows.iss): fallback `0.9.35`; a distribuição protegida usa versão do projeto.
- [Catálogo i18n](../i18n/source.json): `productVersion = 0.9.35`, 51 idiomas.
- [Avisos de terceiros](../third-party/NOTICE.md): referência ao produto na 0.9.35; não foram reescritas as licenças.
- [CI Local core no commit HEAD 9929524](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39059144987): **concluído com sucesso** nas plataformas reportadas (Windows e Ubuntu). Checagem automatizada **não equivale a teste com Google real**.
- [Protected alpha distribution no commit 70377cb](https://github.com/zagozago/ZagoSheetsWin/actions/runs/39059101337): **concluído com sucesso**; commit imediatamente anterior à verificação visual final 9929524. Essa diferença de SHA exige confirmar novo pacote para o commit escolhido ao congelar a futura Release.
- Nenhuma evidência desta revisão autoriza afirmar que o instalador foi assinado com Authenticode ou que uma verificação formal de segurança, auditoria externa ou homologação Google foi concluída.

## Política de importação que precisa constar nos textos de 1.0

1. Se o modo **Importar como cópia** estiver ativo, o original permanece na pasta de origem.
2. Quando a substituição está autorizada e a fonte é elegível, uma **importação Google confirmada** pode substituir o original por `.url` **mesmo que valores, fórmulas ou recursos complexos não tenham equivalência comprovada**. Essa é uma decisão de produto introduzida na 0.9.33. Não prometer fidelidade perfeita nem cópia obrigatória quando a comparação falha.
3. O snapshot original é mantido em backup recuperável; o usuário deve conferir os resultados no Google Sheets e restaurar em caso de diferenças.
4. Desde 0.9.34, o aplicativo apresenta confirmação após substituir, com link para Backups, opção persistente **Não mostrar novamente** e apenas uma confirmação visível por vez. Essa janela **não é autorização prévia a cada importação**.
5. Arquivos com atributo Windows **somente leitura** podem impedir a retirada (erro nativo de acesso negado), mesmo que a importação no Google tenha acontecido; o programa **preserva o original e backup**. Depois de o usuário remover o atributo, a operação pode ser retomada com segurança.
6. Arquivos inválidos, scripts, falhas de importação, verificações estruturais críticas, falhas de hash, associação remota incerta, pasta sincronizada/rede e conflitos locais continuam sujeitos a bloqueio ou cópia conforme o caso.
7. Desde 0.9.32, os seletores de Formatos e texto não existem nas Opções avançadas, mas as preferências previamente gravadas continuam aplicáveis. Os cinco formatos seguem habilitados.

## Preparação para 1.0.0

A [lista de aceite](PRE_RELEASE_CHECKLIST.md) centraliza pendências reais. Os [guias atuais](README.md) evitam amarrar comportamentos ao número de versão em cada página, para que a futura promoção exija principalmente:

- confirmar o commit de lançamento, execução protegida correspondente, pacotes, hashes e testes;
- atualizar este estado de versão e o README raiz;
- criar a seção 1.0.0 no [CHANGELOG](../CHANGELOG.md) **somente quando existir lançamento real**;
- verificar suporte web e revisar avisos/idiomas conforme o aceite.

Os materiais de 0.9.30 são registro datado, não substituem esta referência.
