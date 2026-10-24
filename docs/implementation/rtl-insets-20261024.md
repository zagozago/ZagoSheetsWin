# 0.9.48 — recuos dos botões em RTL

Correção solicitada após o teste visual do modal principal e de Configurações: as colunas verticais espelhadas posicionavam os controles sem o recuo esquerdo.

`InsetFlowLayoutPanel` preserva o recuo horizontal após o layout nativo, usando o padding e as margens já escalados pelo Windows. Aplica-se às colunas verticais sem quebra somente em `LauncherForm` e `SetupForm`. Os controles mantêm o sentido RTL, a ordem, os textos e os handlers existentes. As demais telas continuam usando o posicionamento anterior.

## Validação

- Código: `08dbced61ed6395534ed6daeff6cf23342c5a4a9`.
- [Local core 40126683024](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40126683024): aprovado no Windows e Linux, incluindo testes, matriz nativa e ciclo do instalador.
- A verificação de localização agora exige recuo físico dos dois lados para os botões, cartões e linhas de ações nessas duas telas em RTL. Cobre os cinco idiomas RTL do catálogo: árabe, urdu, persa, pashto e sindi. A matriz completa dos 51 idiomas foi preservada.
- Capturas: artefato `ZagoSheetsWin-branding-preview` 12572884317. Inspeção visual do principal em árabe e urdu e de Configurações em árabe, pashto e persa confirmou o afastamento da borda.
- Compilação local: zero erros e avisos. Os 14 contratos do catálogo passaram; textos e recursos traduzidos não mudaram.
- Escopo de produção: apresentação das duas telas. Núcleo de importação, infraestrutura e fluxo do primeiro uso não foram modificados.

[Distribuição alpha corrigida](https://github.com/zagozago/ZagoSheetsWin/actions/runs/40127989020): mesma implementação validada, preparada no workflow protegido. A liberação exige a aprovação habitual do ambiente `zagosheets-distribution`.
