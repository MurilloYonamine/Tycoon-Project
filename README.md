# Unity Grid & Shop System

Este projeto em Unity implementa um sistema modular de **loja** e **grade (grid)**, permitindo ao jogador selecionar tiles em uma interface animada e posicioná-los em um tilemap via drag-and-drop com pré-visualização (ghost).

---

## 📋 Sumário

* [Descrição](#descrição)
* [Funcionalidades](#funcionalidades)
* [Arquitetura](#arquitetura)
* [Pré-requisitos](#pré-requisitos)
* [Instalação](#instalação)
* [Uso](#uso)
* [Scripts Principais](#scripts-principais)
* [Como Funciona o Fluxo](#como-funciona-o-fluxo)
* [Futuras Melhorias](#futuras-melhorias)
* [Licença](#licença)

---

## Descrição

Este sistema possibilita ao jogador:

1. Abrir/fechar uma **loja** via tecla `Tab` (animações controladas por `Animator`).
2. Selecionar um **tile** na loja, que ganha um efeito de pré-visualização transparente (ghost) seguindo o cursor.
3. Posicionar o tile em uma **grade** (`Tilemap` + `Grid`), respeitando células demarcadas e ocupação.
4. Reutilizar objetos usando um **Object Pooling** (`TilePool`), garantindo performance.

O design modular e documentado facilita futuras expansões, como estados adicionais (upgrades, remoção) e interação multiplayer.

---

## Funcionalidades

* **Loja Animada** (`ShopManager`)
* **Preview Ghost** para posicionamento (`Tile.GhostEffect`)
* **Verificação de Células** demarcadas e ocupadas (`GridManager`)
* **Clique Ignora UI** para evitar interação indevida (`EventSystem`)
* **Pooling de Tiles** para performance otimizada
* **ScriptableObject** (`TileItemData`) para dados de tiles

---

## Arquitetura

```
 ┌─────────────┐      ┌─────────────┐      ┌────────────┐
 │ ShopManager ├─────▶│ TilePool    ├─────▶│ Tile       │
 └─────────────┘      └─────────────┘      └────────────┘
        │                                        ▲
        │                                        │
        ▼                                        │
  UI (Buttons)                                   │
                                                 │
                                          ┌─────────────┐
                                          │ GridManager │
                                          └─────────────┘
                                                 ▲
                                                 │
                                            ClickHandler
```

* **ShopManager**: controla UI e animações da loja.
* **TilePool**: gerencia o pool de objetos Tile.
* **Tile**: lógica de preview, posicionamento e estados.
* **GridManager**: trata coordenação com `Tilemap` e ocupação.
* **ClickHandler**: dispara eventos de clique amigáveis ao novo Input System.

---

## Pré-requisitos

* Unity 6 ou superior (LTS recomendado)
* Pacote **TextMeshPro**
* Pacote **Input System**

---

## Scripts Principais

| Script            | Descrição                                            |
| ----------------- | ---------------------------------------------------- |
| `ShopManager.cs`  | Gerencia UI, animações e seleção de tiles na loja.   |
| `Tile.cs`         | Lógica de preview, posicionamento e estados do tile. |
| `GridManager.cs`  | Singleton que controla grid, tilemap e ocupação.     |
| `ClickHandler.cs` | Trata clique do mouse com o novo Input System.       |
| `TileItemData.cs` | `ScriptableObject` contendo dados de cada tile.      |

---

## Como Funciona o Fluxo

1. **Loja Abre** (`ShopManager.OpenShop`)
2. Usuário **clica em botão** → detona `HandleButton`, coleta `TileItemData`.
3. **Loja Fecha** → coroutine espera animação.
4. **Pool** fornece um `Tile`; chama `SetTileItemData` + `HandleSelection(true)`.
5. **Tile** entra em modo ghost (segue o mouse).
6. **Tile.HandleClick** dispara ao clicar fora de UI e em célula válida → chama `PlaceTile`.
7. Tile finaliza em posição e estado `Placed`.

---

## Futuras Melhorias

* Migrar `TileStateType` para **FSM** com múltiplos estados (Selected, Upgrading).
* Remoção de tiles e **undo** de ação.
* Integração de **custos** e **inventário**.
* Dados via **Addressables** para facilitar conteúdos dinâmicos.

---

## Licença

MIT © Murillo Gomes Yonamine
