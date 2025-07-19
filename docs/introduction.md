# Introdução ao Tycoon Project

Bem-vindo ao **Tycoon Project**, um sistema modular e robusto de grid desenvolvido em Unity para criação de jogos do gênero tycoon. Este projeto implementa uma arquitetura escalável que facilita o desenvolvimento de jogos de construção e gerenciamento.

## 🎯 O que é o Tycoon Project?

O **Tycoon Project** é uma solução completa que fornece:

- **Sistema de Grid Inteligente** - Gerenciamento avançado de posicionamento de objetos
- **Arquitetura Modular** - Componentes independentes e reutilizáveis
- **Performance Otimizada** - Object pooling e otimizações de rendering
- **Interface Intuitiva** - Sistema de loja e seleção responsivo
- **Efeitos Visuais** - Hover effects e feedback visual em tempo real

## 🏗️ Arquitetura do Sistema

### Princípios Fundamentais

O projeto foi construído seguindo princípios sólidos de engenharia de software:

#### **1. Separação de Responsabilidades**
Cada módulo tem uma responsabilidade específica e bem definida:

- **GridManager** - Coordenação central e orquestração
- **HoverManager** - Efeitos visuais de hover
- **PathManager** - Sistema de pathfinding
- **TilePool** - Gerenciamento de performance via object pooling
- **ShopManager** - Interface de seleção e compra

#### **2. Modularidade**
```csharp
// Cada módulo é independente e pode ser usado separadamente
HoverManager hoverModule = new HoverManager(gridManager, true, Color.yellow);
PathManager pathModule = new PathManager(gridManager);
```

#### **3. Performance First**
```csharp
// Object pooling para reutilização eficiente
GameObject tile = TilePool.Instance.GetPooledObject();
// Atualizações otimizadas apenas quando necessário
if (currentCell != previousCell) ProcessHover();
```

## 🎮 Funcionalidades Principais

### Sistema de Grid Avançado

O coração do sistema é o **GridManager**, que coordena todas as operações:

```csharp
// Inicialização automática de todos os módulos
GridManager.Instance.InitializeModules();

// API unificada para operações de grid
bool isOccupied = GridManager.Instance.IsCellOccupied(coordinate);
Vector3Int logicalCoord = GridManager.Instance.GetTileCoordinate(cellPosition);
```

### Efeitos Visuais Responsivos

O **HoverManager** fornece feedback visual imediato:

```csharp
// Configuração simples de hover
GridManager.Instance.SetHoverEnabled(true);
GridManager.Instance.SetHoverColor(Color.yellow);

// Sistema automático de detecção
// Destaca células livres quando o mouse passa sobre elas
```

### Sistema de Tiles Inteligente

Cada tile possui comportamento rico e estados bem definidos:

```csharp
// Estados do ciclo de vida
public enum TileState
{
    Unplaced, // Modo fantasma durante posicionamento
    Placed    // Definitivamente posicionado no grid
}

// Comportamento de ghost mode
tile.HandleSelection(true);  // Ativa preview transparente
tile.FollowMouse();          // Segue cursor do mouse
```

### Object Pool para Performance

O **TilePool** otimiza performance reutilizando objetos:

```csharp
// Singleton pattern para acesso global
TilePool.Instance.amountToPool = 100;

// Reutilização eficiente
GameObject reusedTile = TilePool.Instance.GetPooledObject();
TilePool.Instance.ReturnPooledObject(tileToReturn);
```

## 🛠️ Tecnologias e Padrões

### Tecnologias Utilizadas

- **Unity 2022.3 LTS** - Engine principal
- **C# 9.0** - Linguagem de programação
- **Unity Input System** - Sistema moderno de input
- **ScriptableObjects** - Configuração flexível de dados
- **Tilemap System** - Renderização eficiente de tiles

### Padrões de Design Implementados

#### **Singleton Pattern**
```csharp
public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
}
```

#### **Object Pool Pattern**
```csharp
public class TilePool : MonoBehaviour
{
    private List<GameObject> pooledObjects;
    
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
                return pooledObjects[i];
        }
        return null;
    }
}
```

#### **Module Pattern**
```csharp
// Cada funcionalidade como módulo independente
public class HoverManager { /* Lógica de hover */ }
public class PathManager { /* Lógica de pathfinding */ }
public class CellValidator { /* Lógica de validação */ }
```

## 🎯 Casos de Uso

### Jogos de Construção de Cidades
- Posicionamento de edifícios
- Sistema de ruas e pathfinding
- Zonas de construção definidas

### Jogos de Fazenda/Agricultura
- Plantio em grid organizado
- Sistema de irrigação
- Expansão de terrenos

### Jogos de Estratégia
- Posicionamento de unidades
- Construção de bases
- Sistema de recursos por tile

### Jogos de Puzzle
- Posicionamento preciso de peças
- Validação de movimentos
- Sistema de snap-to-grid

## 🚀 Vantagens do Sistema

### Para Desenvolvedores

1. **Produtividade** - Sistema pronto para uso imediato
2. **Flexibilidade** - Módulos intercambiáveis e configuráveis
3. **Documentação** - API completamente documentada
4. **Exemplos** - Casos de uso práticos incluídos
5. **Performance** - Otimizações já implementadas

### Para Jogadores

1. **Responsividade** - Feedback visual imediato
2. **Intuitividade** - Controles naturais e previsíveis
3. **Fluidez** - Animações suaves e transições
4. **Precisão** - Sistema de snap automático
5. **Visual** - Efeitos de hover e preview

## 📈 Próximos Passos

### Configuração Inicial

1. **[Setup do Projeto](getting-started.md)** - Configure seu ambiente
2. **[Configuração Básica](getting-started.md#configuracao-basica)** - Primeiros passos
3. **[Exemplos Práticos](getting-started.md#exemplos)** - Casos de uso reais

### Exploração Avançada

1. **[API Reference](../api/index.md)** - Documentação completa
2. **[Módulos Especializados](../api/index.md#grid-modules)** - Funcionalidades avançadas
3. **[Customização](getting-started.md#customizacao)** - Adaptação para suas necessidades

## 🎨 Filosofia de Design

### Simplicidade sem Comprometer Poder

O projeto foi desenhado com a filosofia de **"fácil de usar, difícil de quebrar"**:

- **Interface Simples** - API intuitiva para operações comuns
- **Poder Avançado** - Funcionalidades sofisticadas quando necessário
- **Documentação Clara** - Cada funcionalidade bem explicada
- **Exemplos Práticos** - Código pronto para adaptar

### Escalabilidade Incorporada

- **Arquitetura Modular** - Adicione novos módulos facilmente
- **Performance Otimizada** - Suporta grids grandes sem impacto
- **Configuração Flexível** - Adapte-se a diferentes necessidades
- **Extensibilidade** - Herde classes base para funcionalidades customizadas

---

## 🏁 Conclusão

O **Tycoon Project** não é apenas um sistema de grid - é uma **plataforma completa** para desenvolvimento de jogos que requerem posicionamento preciso, interface intuitiva e performance otimizada.

Com sua arquitetura modular, documentação completa e foco em performance, este projeto oferece uma base sólida para criar experiências de jogo envolventes e profissionais.

**Pronto para começar?** Explore a [documentação da API](../api/index.md) ou siga o [guia de configuração](getting-started.md) para dar seus primeiros passos!

---

**Desenvolvido por Murillo Gomes Yonamine**  
Unity 2022.3 LTS | C# 9.0 | Arquitetura Modular