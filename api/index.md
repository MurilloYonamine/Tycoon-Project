# Tycoon Project - API Reference

Bem-vindo à documentação da API do **Tycoon Project**, um sistema modular de grid desenvolvido em Unity para jogos do gênero tycoon.

## 📋 Visão Geral

Este projeto implementa um sistema robusto e modular de grid com funcionalidades avançadas para posicionamento de tiles, efeitos visuais, gerenciamento de pathfinding e interface de loja. A arquitetura modular garante escalabilidade, manutenibilidade e performance otimizada.

## 🏗️ Arquitetura do Sistema

### Componentes Principais

| Namespace | Descrição | Responsabilidades |
|-----------|-----------|-------------------|
| [`GRID`](GRID.yml) | Sistema central de grid | Coordenação de módulos, gerenciamento de estado global |
| [`GRID.MODULES`](GRID.MODULES.yml) | Módulos especializados | Hover, pathfinding, validação, conversão de coordenadas |
| [`GRID.TILE`](GRID.TILE.yml) | Sistema de tiles | Comportamento, dados e pooling de tiles |
| [`SHOP`](SHOP.yml) | Sistema de loja | Interface, seleção e instanciação de tiles |

## 🎯 Funcionalidades Principais

### Sistema de Grid Modular
- **[GridManager](GRID.GridManager.yml)** - Gerenciador central com padrão Singleton
- **Arquitetura modular** - Separação clara de responsabilidades
- **Sistema de coordenadas** - Conversão automática entre sistemas de coordenadas
- **Validação de células** - Verificação de ocupação e disponibilidade

### Efeitos Visuais Avançados
- **[HoverManager](GRID.MODULES.HoverManager.yml)** - Sistema de hover responsivo
- **Feedback visual em tempo real** - Destaque de células disponíveis
- **Otimização de performance** - Atualizações apenas quando necessário
- **Gerenciamento de estado** - Preservação e restauração de cores originais

### Sistema de Tiles Inteligente
- **[Tile](GRID.TILE.Tile.yml)** - Comportamento individual com ghost mode
- **[TilePool](GRID.TILE.TilePool.yml)** - Object Pool para otimização de performance
- **[TileItemData](GRID.TILE.TileItemData.yml)** - ScriptableObject configurável
- **Estados bem definidos** - Transições controladas entre [TileState](GRID.TILE.TileState.yml)

### Interface de Loja Intuitiva
- **[ShopManager](SHOP.ShopManager.yml)** - Gerenciamento completo da loja
- **Animações suaves** - Transições coordenadas com corrotinas
- **Seleção de tiles** - Interface responsiva com preview visual
- **Integração com pool** - Reutilização eficiente de objetos

## 🚀 Começando

### Componentes Essenciais

1. **Configurar GridManager**
   ```csharp
   // O GridManager inicializa automaticamente todos os módulos
   GridManager.Instance.SetHoverEnabled(true);
   GridManager.Instance.SetHoverColor(Color.yellow);
   ```

2. **Usar Sistema de Tiles**
   ```csharp
   // Obter tile da pool
   GameObject tileObject = TilePool.Instance.GetPooledObject();
   Tile tile = tileObject.GetComponent<Tile>();
   
   // Configurar dados do tile
   tile.SetTileItemData(tileItemData);
   tile.HandleSelection(true); // Ativar ghost mode
   ```

3. **Gerenciar Loja**
   ```csharp
   // A loja é controlada automaticamente via ShopManager
   // Pressione Tab para abrir/fechar
   ```

## 📐 Sistema de Coordenadas

O projeto utiliza um sistema duplo de coordenadas:

- **Coordenadas de Célula**: Posição física no Grid do Unity
- **Coordenadas Lógicas**: Sistema lógico [coluna, linha] para organização

A conversão é realizada automaticamente pelo [CoordinateConverter](GRID.MODULES.CoordinateConverter.yml).

## 🎮 Input e Interação

### Controles Principais
- **Clique do Mouse**: Posicionar tiles selecionados
- **Tab**: Abrir/fechar loja
- **Hover**: Feedback visual automático

### Sistema de Input
O [ClickHandler](../Assets/Scripts/ClickHandler.cs) utiliza o Unity Input System para capturar eventos de forma eficiente e responsiva.

## 🔧 Módulos Especializados

### Validação e Conversão
- **[CellValidator](GRID.MODULES.CellValidator.yml)** - Validação de células e ocupação
- **[CoordinateConverter](GRID.MODULES.CoordinateConverter.yml)** - Conversão entre sistemas de coordenadas

### Interface e Organização
- **[ColumnUIGenerator](GRID.MODULES.ColumnUIGenerator.yml)** - Geração de UI para colunas
- **[GridBoundsCalculator](GRID.MODULES.GridBoundsCalculator.yml)** - Cálculo de limites do grid

### Pathfinding
- **[PathManager](GRID.MODULES.PathManager.yml)** - Gerenciamento de caminhos e conectividade

## 🏆 Padrões de Design Implementados

- **Singleton**: GridManager, TilePool
- **Object Pool**: Sistema de tiles para performance
- **Module Pattern**: Separação de responsabilidades
- **Observer Pattern**: Sistema de eventos para input
- **State Pattern**: Estados de tiles bem definidos

## 📚 Tipos e Enumerações

### [TileType](GRID.TILE.TileType.yml)
- `Default`: Tile padrão genérico
- `Building`: Construções e estruturas
- `Path`: Caminhos para pathfinding

### [TileState](GRID.TILE.TileState.yml)
- `Unplaced`: Tile não posicionado (ghost mode)
- `Placed`: Tile posicionado no grid

## 🎨 Características Visuais

- **Ghost Mode**: Preview transparente durante posicionamento
- **Hover Effects**: Destaque de células disponíveis
- **Animações Suaves**: Transições controladas por Animator
- **Organização Hierárquica**: Tiles organizados por colunas

## 🔍 Navegação da API

Explore a documentação através dos namespaces organizados:

- 🏠 **[GRID](GRID.yml)** - Sistema central
- ⚙️ **[GRID.MODULES](GRID.MODULES.yml)** - Módulos especializados  
- 🎯 **[GRID.TILE](GRID.TILE.yml)** - Sistema de tiles
- 🛒 **[SHOP](SHOP.yml)** - Sistema de loja

---

**Desenvolvido por**: Murillo Gomes Yonamine  
**Versão**: Unity 2022.3 LTS  
**Padrão de Documentação**: DocFX com XML Documentation