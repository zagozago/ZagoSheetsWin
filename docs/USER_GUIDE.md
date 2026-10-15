# Manual do usuário — ZagoSheetsWin

**Status:** aplicativo Windows x64 em fase alfa (candidato 0.9.30). Procedimentos em Português Técnico Controlado, inspirados no ASD-STE100. A aparência e os nomes de botões podem variar conforme o idioma escolhido.

## 1. O que o aplicativo faz

O ZagoSheetsWin abre planilhas locais no Google Sheets. O programa importa um arquivo, cria um documento Google nativo e abre o navegador. Se o arquivo for elegível à substituição e o usuário tiver autorizado esse comportamento, o aplicativo guarda uma **cópia de segurança dos bytes originais** e substitui o arquivo local por um **atalho `.url`** na mesma pasta.

**O atalho não é a planilha.** Ele contém um endereço para o documento Google. O backup é uma cópia local recuperável da importação inicial. **Não existe sincronização bidirecional:** alterações no Google Sheets não voltam ao arquivo original.

## 2. Pré-requisitos

- Windows compatível em computador x64, navegador padrão e conexão com a Internet.
- Uma conta Google que possa autorizar o aplicativo e utilizar Google Drive.
- Espaço suficiente no disco para backup e arquivos temporários.
- Arquivo local de um destes tipos: `.xlsx`, `.xls`, `.ods`, `.csv` ou `.tsv`.

Use cópias de teste durante a fase alfa. Não presuma que fórmulas, gráficos, formatação ou macros terão correspondência integral no Google Sheets.

## 3. Instalar

1. Abra a [execução de distribuição alfa](https://github.com/zagozago/ZagoSheetsWin/actions/workflows/distribution.yml) aprovada para o commit que deseja testar.
2. Obtenha o artefato `ZagoSheetsWin-Setup-win-x64`; verifique o checksum e metadados disponibilizados para o pacote. O artefato pode exigir login e pode expirar.
3. Extraia o ZIP e execute `ZagoSheetsWin-Setup-win-x64.exe`.
4. Escolha o idioma no seletor do instalador. O aplicativo conserva essa escolha explícita.
5. Complete o assistente de primeira utilização de **três páginas**, incluindo a conexão da conta Google.
6. No Windows, abra **Configurações → Aplicativos → Aplicativos padrão** e escolha ZagoSheetsWin para as extensões desejadas.

O instalador **registra o aplicativo como opção**, mas o Windows mantém o controle do programa padrão de cada tipo de arquivo. A instalação é por usuário, no perfil local do Windows.

## 4. Primeiro uso e autorização

1. Leia a explicação do fluxo de importação, backup e substituição.
2. Selecione **Conectar Google** e autorize o acesso no navegador do sistema. O escopo solicitado é `drive.file`.
3. Confira o estado da conexão e prossiga.
4. Defina os aplicativos padrão do Windows, se desejar abrir por duplo clique.
5. Finalize. O programa pode mostrar a tela inicial após o assistente.

O assistente de primeira utilização tem **três páginas**. O tutorial de Ajuda dentro do aplicativo é outro recurso e possui **quatro páginas**. No uso padrão com pacote protegido, o aplicativo já inclui a identificação OAuth desktop; o usuário não precisa configurar Google Cloud. Um JSON próprio é um recurso avançado, disponível antes da vinculação do cliente. Não altere o cliente OAuth existente apagando arquivos de configuração.

## 5. Abrir uma planilha

1. Se o ZagoSheetsWin for o aplicativo padrão da extensão, dê dois cliques no arquivo. Alternativamente, abra o programa e selecione **Abrir planilha**.
2. Acompanhe a janela de processamento e aguarde o retorno do aplicativo.
3. Verifique o documento aberto no Google Sheets.
4. Confira se foi criado um atalho `.url` na origem **somente quando o modo de substituição for elegível e estiver habilitado**.
5. Para próximas aberturas, use o atalho para ir diretamente ao documento existente.

O aplicativo não converte um atalho em arquivo editável offline. Se o arquivo local mudar depois de uma importação conservada, o sistema impede reenvio silencioso da versão divergente. Não tente forçar a criação de outro documento eliminando o histórico.

## 6. Importar como cópia

Use o modo **Importar como cópia** quando desejar manter o arquivo na pasta original. O Google Sheets é criado; a fonte local permanece. Se a criação de atalhos estiver habilitada, o atalho de cópia fica na pasta privada do aplicativo, acessível por **Abrir pasta de atalhos das cópias** quando disponível.

O programa também pode usar o modo de cópia automaticamente ao reconhecer origem sincronizada, como OneDrive, ou rede. Isso evita que a retirada local se propague a outros dispositivos. Arquivos somente na nuvem devem estar disponíveis localmente antes de serem importados. A detecção não cobre todos os provedores, configurações de sincronização ou compartilhamentos: confira sua pasta.

## 7. Configurações

Na tela **Configurações**, o usuário pode conferir/conectar/verificar/desconectar a conta Google, abrir os aplicativos padrão do Windows, habilitar atalhos e selecionar importação como cópia.

Em **Opções avançadas**, estão as políticas de pasta, consentimento de substituição, codificação e delimitador para texto e comportamento de substituição de XLS. Algumas escolhas de interpretação de CSV/TSV são persistentes e não podem ser alteradas diretamente depois de fixadas, para proteger a reinterpretação de operações antigas. Preferências da interface são salvas automaticamente.

O tema inicial é **claro**. O usuário pode escolher o tema escuro e selecionar qualquer um dos **51 idiomas**. Uma seleção explícita prevalece sobre a preferência automática do Windows.

## 8. Backups e restauração

**Valores padrão:** 30 dias de retenção, quota de 200 MB e limpeza automática **desligada**. É possível configurar 1 a 365 dias e 1 a 1.000 MB. A retenção define elegibilidade para limpeza, não a exclusão obrigatória na data exata. Ao ativar a limpeza automática, o programa solicita confirmação; ela é aplicada durante futuras operações, sem serviço residente.

Para restaurar:

1. Abra **Backups/Recuperação**.
2. Selecione a operação desejada.
3. Clique em **Restaurar em…**.
4. Escolha um **novo destino**, sem sobrescrever um arquivo existente.
5. Verifique a extensão e o conteúdo restaurados.

A restauração recupera **o original importado**, não edições feitas depois no Google Sheets. Excluir um backup concluído remove a possibilidade de recuperar aqueles bytes locais. Operações pendentes/ambíguas recebem proteção adicional; uma exclusão excepcional de operação pendente requer consentimento explícito e deve ser tratada como descarte da recuperação.

Outras ações: **Retomar como cópia**, **Concluir substituição** quando elegível, **Limpar vencidos/excesso**, **Apagar selecionados** e **Exportar diagnóstico**. Não exclua manualmente o diretório de dados do aplicativo.

## 9. Interrupções e erros

A interface oferece uma tentativa automática **limitada** para falhas transitórias, com contagem regressiva. Na janela de erro, **Tentar novamente** permite nova tentativa explícita; o histórico evita recriar cegamente uma importação que possa ter chegado ao Google. Em dúvida, escolha **Recuperar operação/backup** e mantenha o original. Falhas de conexão, autorização, formato, limite ou conferência exigem correções diferentes; use [Solução de problemas](TROUBLESHOOTING.md).

## 10. Atualização e desinstalação

Atualize instalando uma versão posterior aprovada. **Não apague configuração, registros SQLite, backups ou autorizações como forma de atualizar.** Versões antigas podem ser bloqueadas por incompatibilidade de estado. Na desinstalação, o aplicativo remove seus arquivos e registros de integração próprios, preservando por padrão o estado local, os backups, atalhos do usuário e documentos Google. Não há atualização automática assinada confirmada nesta fase.

## 11. Privacidade, direitos e suporte

As planilhas são enviadas diretamente às APIs Google; não passam por servidor Zagotools. Tokens do usuário são protegidos com DPAPI. Backups locais são acessíveis no contexto autorizado do Windows e **não têm criptografia própria de conteúdo**. Relatórios de diagnóstico excluem dados de células e tokens, mas o **nome sugerido do arquivo exportado** pode revelar o nome do documento: revise-o antes de enviar.

Links: [Privacidade](https://zagotools.top/legal.html#privacidade), [Termos](https://zagotools.top/legal.html#termos), [Licenças](https://zagotools.top/legal.html#licencas), [GitHub Issues](https://github.com/zagozago/ZagoSheetsWin/issues). A futura central de ajuda web ainda depende de publicação e verificação; consulte [plano de links](HELP_CENTER_PLAN.md).

Licença MIT. Projeto derivado de [Open in Google](https://github.com/SwatiK425/open-in-google), de Swati K; atribuição preservada em `LICENSE` e `ATTRIBUTION.md`.
