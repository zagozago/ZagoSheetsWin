# Reformulação de interface — alpha 0.9.10

Implementação em Windows Forms com Segoe UI, marca Z isolada, ações principais verdes, separadores e textos revisados.

- Tela principal compacta; importação como cópia em opções avançadas.
- Tutorial de quatro páginas, corpo rolável e rodapé estável; preferência de ocultar exige escolha explícita.
- Configurações em três seções; opções avançadas após o fluxo principal.
- Backups em tabela; ações secundárias no menu Mais ações. IDs continuam associados aos registros, sem exposição na lista.
- Ilustrações locais desenhadas em código, sem dependências externas.
- Nenhuma mudança de escopos OAuth, política de backups ou semântica de importação.

## Validação

Compilação e testes devem passar antes do aceite. O pipeline Windows executa verificações nativas e gera prévias em claro/escuro. Conferência visual em 100%, 125%, 150% e 200%, teclado, atualização, importação e restauração compõem a fase 2. Não considerar a imagem gerada como evidência da implementação.

## Evidência local

- Build da solução e do aplicativo Windows: aprovado, sem avisos ou erros.
- Suíte: 185 aprovados; 31 específicos do Windows não executados neste ambiente.
- Verificações nativas de UI atualizadas, ainda pendentes de execução no Windows.
- Candidata 0.9.10 autorizada para publicação na branch de desenvolvimento e validação no Windows. O aceite visual e funcional continua pendente.


## Compatibilidade de importação — candidata 0.9.12

- XLSX: resolver o tipo de conteúdo por Override e, na ausência dele, Default. Rejeitar declarações ambíguas e tipos incompatíveis.
- XLS: quando a exportação Google contém fórmulas não verificáveis, abrir a mesma importação como cópia, preservar original e backup e mostrar um aviso. Divergências de valores continuam bloqueando a substituição.
- Diagnóstico: categorias de falha sem mensagens de exceção, caminhos, contas ou conteúdo.
- Arquivos reais enviados pelo usuário: leitura/preparação local de XLS e XLSX aprovada. Arquivos e dados privados não incluídos no repositório.
- Testes sintéticos cobrem Default, precedência de Override, duplicidade, categorias do diagnóstico e reutilização do upload XLS com fórmulas. Validação integrada Google/Windows pendente.

## 0.9.13 — XLS local e licenças

Novas importações XLS enviam XLSX convertido com NPOI 2.7.6, preservando o backup binário e a verificação com ExcelDataReader. O link passa a Sobre / Licenças; as licenças próprias das dependências são incluídas. Detalhes e limites em XLS_CONVERSION.md.

## 0.9.14 — Painéis de backups adaptáveis

Os painéis de regras e ações calculam a altura pelo conteúdo, em vez de cortar controles quando a fonte ou a largura provocam quebra de linha. A verificação nativa mantém a checagem de limites e cobre a janela mínima, com o nome e as dimensões do botão em caso de falha. O cabeçalho reserva espaço para Sobre / Licenças e o seletor de tema.

## 0.9.15 — XLS de relatórios HTML e ícone ampliado

A importação detecta HTML dentro de XLS e gera XLSX localmente, com filtros em Informações e verificação dos valores. O ícone do aplicativo/instalador usa Z ampliado sobre fundo transparente; o ícone dos atalhos de planilha permanece separado.

## 0.9.16 — Tutorial visual, conexão e seleção de backups

- Ilustrações locais maiores, com fluxo de três etapas, cinco formatos visíveis e legendas curtas. Ações diretas para Aplicativos padrão e Backups.
- Ordem: 1. Conta Google; 2. Abrir com dois cliques; 3. Pastas sincronizadas (opcional). A declaração de não sincronizar vem marcada em instalação nova e recolhe a escolha de pasta. A preferência anterior é preservada em atualização.
- Política de abertura persistida em opening-policy.json, sem exigir pasta no modo geral. replacement-root.txt continua sendo lido para migração. Proteções de arquivo local, handles, backups e verificação de conversão permanecem. Raízes OneDrive e provedores registrados no SyncRootManager são reconhecidos; origens sincronizadas identificadas/rede abrem como cópia. A declaração do usuário é necessária porque a detecção não cobre todas as configurações de sincronização.
- Autorização Google independente da pasta; indicador de autorização salva, verificação online com refresh + identidade da conta e indicação de nova autorização. Falha de rede não é apresentada como revogação.
- Tabela permite múltiplas marcas e seleção; Selecionar todos inclui apenas backups concluídos elegíveis. A confirmação informa a quantidade e mantém protegidos/pendentes. Restaurar exige uma única linha selecionada.
- Testes de migração, consentimento, política inválida e limpeza de múltiplos backups, mais verificações nativas Windows de seleção, seção recolhida e modal avançado. Prévias e testes nativos são executados no pipeline Windows.

## 0.9.17 — Correção do teste de autorização e botão de tema

O teste Windows de revogação agora compara os bytes protegidos do armazenamento antes/depois, sem comparar a identidade de objetos reconstituídos. A 0.9.16 foi bloqueada por essa comparação incorreta; não houve mudança no comportamento do OAuth.

O seletor de tema mostra um único ícone vetorial: sol no modo claro, lua no escuro. Um clique alterna o tema e salva a preferência; as janelas abertas são atualizadas juntas. O botão mantém foco por teclado, descrição acessível e contraste do tema, sem trilha ou marcador deslizante.
