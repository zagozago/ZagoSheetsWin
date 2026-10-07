# Integração de idiomas - piloto 0.9.22

O aplicativo e o instalador incluem os 51 idiomas da ordem canônica.
Os recursos são gerados a partir dos catálogos completos recebidos, com fallback
em inglês. Escolha explícita prevalece sobre os idiomas de interface do Windows;
atualizações preservam a preferência existente. A seleção não altera a cultura
usada para formatar dados numéricos do usuário.

O seletor oferece busca por código, nome nativo e nome em inglês. Filipino usa
o código `fil` e badge `TL`. Mandarim simplificado (`zh-CN`) e cantonês tradicional
(`yue-Hant-HK`) são recursos distintos. Não se adota Mandarim simplificado para
uma preferência automática de Mandarim tradicional.

As cinco variantes RTL (`ar`, `ur`, `fa`, `ps`, `sd`) usam as propriedades nativas
de leitura e espelhamento dos formulários e listas. Caminhos e originais legais
mantêm leitura LTR. Labels nativos substituem o desenho manual das frases para
usar composição, quebra de linha e fallback de fontes do Windows. Famílias por
escrita são selecionadas quando instaladas; Segoe UI e o fallback do Windows são
a alternativa. Não se baixa uma fonte ou pacote de idioma silenciosamente.

Cada idioma do instalador tem mensagens próprias, nome nativo, ID do Windows
quando disponível, UTF-8 e direção de leitura. Idiomas sem ID Windows usam zero,
conforme a documentação do Inno Setup; permanecem selecionáveis manualmente.
O botão final adapta a largura ao texto e a escolha é enviada ao aplicativo.

## Verificação

`LocaleVerification` exercita formulários nativos de início, configuração,
avançadas, backups, idiomas, processamento com erro e as quatro páginas de ajuda
para os 51 idiomas, sem Google nem persistência de preferências. Verifica textos
de botões, limites dos contêineres, direção RTL e ausência de rolagem na ajuda.
Gera capturas por idioma e um relatório em `locales/verification.json`.

O teste de tamanho de fonte dos botões cobre 100%, 125%, 150% e 200%. Isso é uma
verificação de dimensionamento, não substitui o aceite em monitores físicos com
cada DPI e pacotes opcionais de fontes do Windows. As capturas permitem revisão
visual dos alfabetos e das telas. Compilação do instalador e ciclo de instalação,
atualização e desinstalação são executados separadamente no Windows.

Os packs mantêm metadados de revisão nativa separados. A geração deste piloto
inclui catálogos `translated` quando a integração multilíngue está habilitada;
isso não converte automaticamente a revisão editorial em aceite visual/nativo.
O gate estrito de release dos packs continua disponível e não é ignorado.
