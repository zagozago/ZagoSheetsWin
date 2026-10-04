# Preparação da internacionalização - 0.9.20

Etapa 3 implementada em 04/10/2026. A interface continua em português brasileiro.
Detecção do Windows, escolha e persistência do idioma pertencem à etapa seguinte;
os 50 idiomas alvo ainda não foram traduzidos. Suporte completo a 51 idiomas não
está anunciado. O aceite visual e funcional da 0.9.18 foi confirmado por Fernando.

## Catálogo e fontes

Uma única fonte de autoria: `i18n/source.json`. São **585 entradas**:

| Escopo | Entradas |
|---|---:|
| Aplicativo, acessibilidade, ajuda e mensagens públicas | 241 |
| Termos de ênfase seletiva | 42 |
| Mensagens próprias do instalador | 9 |
| Mensagens padrão de instalação e desinstalação do Inno Setup | 293 |

As chaves representam conceitos. Não renomear uma chave ao ajustar somente a
redação. Cada entrada inclui contexto, placeholders, invariantes e observações.
Os filtros de arquivos e parágrafos da ajuda preservam `|` como separador técnico.
Nomes de marcas, formatos, URLs, identidades do registro, nomes de arquivos/abas
existentes e textos originais de licenças não são traduzidos.

`i18n/source-usage.json` registra o inventário sintático da base 0.9.19, locais de
uso e exclusões. Inclui textos desenhados nas ilustrações, estado dinâmico e
acessibilidade; não trata apenas rótulos estáticos. A CLI de desenvolvimento e
as asserções técnicas de verificação offline não integram a interface distribuída.

`i18n/locales.json` preserva ordem, nomes nativos, códigos, variantes, escrita e
direção dos três arquivos canônicos de Tradução 51. PT usa `pt-BR`; Filipino usa
`fil`, `fil-PH` e badge `TL`. Cantonês usa `yue-Hant-HK`, sem ser confundido com
mandarim. RTL: `ar`, `ur`, `fa`, `ps`, `sd`. Estados de revisão nativa permanecem
separados de aprovação técnica.

As mensagens padrão do Inno Setup foram catalogadas da tradução PT-BR de
Cesar82 (Cesar Zanetti), revisão 6.6.1:
https://github.com/jrsoftware/issrc/blob/99c68b09f759b86ab117560ceb89e29dcdb012ca/Files/Languages/BrazilianPortuguese.isl
O texto da licença correspondente está em `i18n/reference/Inno-Setup-LICENSE.txt`.
O instalador desta etapa mantém o arquivo padrão PT-BR fornecido pelo compilador;
a integração de traduções padrão dos demais idiomas exige verificar a versão
real do compilador, IDs de mensagens e metadados `LangOptions`.

## Recursos e execução

`tools/i18n/generate.py` valida a fonte e gera:

- `i18n/generated/pt.json`: recurso offline incorporado ao aplicativo.
- `installer/i18n/pt.isl`: mensagens próprias consumidas pelo instalador.

Não editar recursos gerados diretamente. `UiText` usa chaves e placeholders
nomeados; valores podem ser reordenados pelas traduções e são interpolados uma
única vez. A formatação numérica continua seguindo a cultura regional do usuário,
separada do idioma da interface. Os destaques também vêm do catálogo.

`LocalizationCatalog` suporta pack selecionado > fallback inglês > fonte.
Nesta entrega o inglês ainda não existe: somente a fonte PT é incorporada, e
`fallbackReady=false` permanece explícito. Não expor packs incompletos no seletor.
Não usar fallback como evidência de tradução concluída.

Exceções de capacidade guardam a mensagem-fonte para os diagnósticos e um
descritor para a mensagem de interface. Comandos, caminhos, protocolos e códigos
técnicos permanecem estáveis. Os textos de registro Windows usam a fonte estável
para preservar as verificações de propriedade das associações; tradução futura
de descrições externas requer migração dos valores pertencentes ao aplicativo.

Mensagens de contagem usam rótulos completos (por exemplo, `Backups apagados:
{count}`), sem concatenar sufixos `(s)`. Se uma tradução precisar de variantes
CLDR, declarar mensagens completas e uma nova revisão do contrato.

## Validação e uso

```sh
python tools/i18n/generate.py
python tools/i18n/generate.py --check
python tools/i18n/test_catalog.py
python tools/i18n/generate.py --init-pack en > pack-en.json
python tools/i18n/generate.py --validate-pack pack-en.json
python tools/i18n/generate.py --validate-pack pack-en.json --release
dotnet run --project tools/i18n/Audit.csproj -- . audit.json
```

Inicializar um pack cria **exatamente a matriz canônica**, com `null` nas
traduções pendentes. Não equivale a tradução. O hash da fonte impede usar um
pack de uma revisão diferente, mesmo que suas chaves ainda existam.

Os validadores verificam matriz, revisão, campos variáveis e formatos, Unicode,
invariantes, valores numéricos, escrita esperada e direção. O gate de release
exige traduções preenchidas, QA editorial e QA de layout. Coincidências com a
fonte precisam de revisão explícita. Estas checagens não certificam naturalidade
nem revisão por falante nativo. CI valida recursos gerados nos dois sistemas.

## Pendência obrigatória: botões e layout dos 51 idiomas

Fernando enviou o print da 0.9.19: o botão final **Iniciar configuração** está
cortado. Pediu para não corrigir seu tamanho nesta etapa. A pendência fica aberta
e deve ser concluída antes da homologação multilíngue, junto aos demais botões.

- Medir o texto com a fonte e o DPI reais. Reservar padding, ícones e foco.
- Ajustar largura/altura e redistribuir os botões da linha quando necessário;
  não simplesmente aumentar uma largura fixa baseada em português.
- Quando não houver largura, usar quebra de linha ou reordenar a linha com
  crescimento da janela; manter todos os botões visíveis e operáveis.
- Nunca usar corte, reticências ou redução global da fonte como solução.
- Incluir o botão final do instalador, Voltar/Próximo/Cancelar, desinstalador,
  menus, Opções avançadas, autorização e ações de recuperação.
- Usar pseudolocalização com expansão de texto e validar os 51 packs nos DPIs
  100%, 125%, 150% e 200%, incluindo RTL, alto contraste, fontes, teclado e foco.
- A ajuda deve continuar sem rolagem vertical, respeitando textos traduzidos.

## Próxima etapa

Resolver preferência explícita > preferência persistida > idiomas preferidos
do Windows > inglês, com modo automático separado do fixo. Preparar o repasse
da escolha do instalador sem sobrescrever a preferência durante atualizações.
Depois, traduzir os 50 alvos, integrar recursos padrão do instalador e concluir
QA visual/editorial por pack. Tema, importação, autenticação e backups não devem
ser reiniciados ao trocar idioma. Comparação com `legal.html` permanece pendente
desde a consulta que retornou HTTP 404 na etapa anterior.
