# Getting Started - Guia Prático

Este guia irá levá-lo desde a configuração inicial até a criação de seu primeiro jogo usando o **Tycoon Project**. Em poucos minutos você terá um sistema de grid funcional com todas as funcionalidades principais.

## 📋 Pré-requisitos

Antes de começar, certifique-se de ter:

- **Unity 2022.3 LTS** ou superior
- **Visual Studio** ou **VS Code** para edição de código
- **Conhecimento básico de C#** e Unity
- **Git** (opcional, para versionamento)

## 🚀 Configuração Rápida

### Passo 1: Setup do Projeto

1. **Clone ou baixe o projeto**
   ```bash
   git clone https://github.com/MurilloYonamine/Tycoon-Project.git
   ```

2. **Abra no Unity**
   - Abra o Unity Hub
   - Clique em "Open" e selecione a pasta do projeto
   - Aguarde o Unity importar os assets

3. **Verifique as dependências**
   - Unity Input System deve estar instalado
   - TextMeshPro deve estar configurado

### Passo 2: Configuração da Cena

1. **Abra a cena principal**
   ```
   Assets/Scenes/MainScene.unity
   ```

2. **Verifique os componentes essenciais**
   - **GridManager** deve estar presente na cena
   - **Tilemap** e **Grid** devem estar configurados
   - **TilePool** deve estar vinculado

3. **Configure o Input System**
   ```csharp
   // O ClickHandler já está configurado para capturar:
   // - Cliques do mouse (posicionamento)
   // - Tecla Tab (abrir/fechar loja)
   ```

## 🎯 Primeiro Teste

### Execute o Projeto

1. **Pressione Play** no Unity
2. **Pressione Tab** para abrir a loja
3. **Clique em um tile** na loja para selecioná-lo
4. **Mova o mouse** sobre o grid (observe o hover effect)
5. **Clique no grid** para posicionar o tile

🎉 **Parabéns!** Seu sistema básico está funcionando!

## 🔧 Configuração Básica

### Configurando o GridManager

O **GridManager** é o coração do sistema. Aqui está como configurá-lo:

```csharp
public class MyGameManager : MonoBehaviour
{
    void Start()
    {
        // Configurar hover effects
        GridManager.Instance.SetHoverEnabled(true);
        GridManager.Instance.SetHoverColor(Color.yellow);
        
        // Verificar se o sistema está inicializado
        if (GridManager.Instance != null)
        {
            Debug.Log("Grid System inicializado com sucesso!");
        }
    }
}
```

### Configurando a Loja

A **ShopManager** controla a interface de seleção:

```csharp
public class CustomShopSetup : MonoBehaviour
{
    void Start()
    {
        // A loja é configurada automaticamente
        // Controles:
        // - Tab: Abrir/Fechar
        // - Clique: Selecionar tile
        
        // Para customizar tiles programaticamente:
        CreateCustomTile();
    }
    
    void CreateCustomTile()
    {
        // Criar dados customizados para tile
        TileItemData customTile = ScriptableObject.CreateInstance<TileItemData>();
        customTile.SetName("Meu Tile Customizado");
        customTile.SetColor(Color.blue);
        customTile.SetTileType(TileType.Building);
    }
}
```

### Configurando Object Pool

O **TilePool** otimiza performance:

```csharp
void ConfigureTilePool()
{
    // O pool é configurado automaticamente baseado no tamanho do grid
    // Mas você pode ajustar manualmente:
    
    TilePool.Instance.amountToPool = 200; // Aumentar para grids maiores
    
    // Verificar tiles disponíveis
    GameObject availableTile = TilePool.Instance.GetPooledObject();
    if (availableTile != null)
    {
        Debug.Log("Tile disponível no pool!");
    }
}
```

## 🎮 Exemplos Práticos

### Exemplo 1: Jogo de Construção de Cidade

```csharp
public class CityBuilder : MonoBehaviour
{
    [SerializeField] private TileItemData houseTile;
    [SerializeField] private TileItemData roadTile;
    [SerializeField] private TileItemData parkTile;
    
    void Start()
    {
        SetupCityTiles();
    }
    
    void SetupCityTiles()
    {
        // Configurar tile de casa
        houseTile.SetName("Casa");
        houseTile.SetColor(Color.blue);
        houseTile.SetTileType(TileType.Building);
        
        // Configurar tile de estrada
        roadTile.SetName("Estrada");
        roadTile.SetColor(Color.gray);
        roadTile.SetTileType(TileType.Path);
        
        // Configurar tile de parque
        parkTile.SetName("Parque");
        parkTile.SetColor(Color.green);
        parkTile.SetTileType(TileType.Building);
    }
    
    void Update()
    {
        // Teclas de atalho para seleção rápida
        if (Input.GetKeyDown(KeyCode.H))
            SelectTile(houseTile);
        if (Input.GetKeyDown(KeyCode.R))
            SelectTile(roadTile);
        if (Input.GetKeyDown(KeyCode.P))
            SelectTile(parkTile);
    }
    
    void SelectTile(TileItemData tileData)
    {
        GameObject newTile = TilePool.Instance.GetPooledObject();
        if (newTile != null)
        {
            newTile.SetActive(true);
            Tile tileComponent = newTile.GetComponent<Tile>();
            tileComponent.SetTileItemData(tileData);
            tileComponent.HandleSelection(true); // Ativar ghost mode
        }
    }
}
```

### Exemplo 2: Sistema de Fazenda

```csharp
public class FarmSystem : MonoBehaviour
{
    [Header("Farm Tiles")]
    public TileItemData wheatTile;
    public TileItemData cornTile;
    public TileItemData waterTile;
    
    void Start()
    {
        ConfigureFarmTiles();
        SetupFarmControls();
    }
    
    void ConfigureFarmTiles()
    {
        // Trigo
        wheatTile.SetName("Trigo");
        wheatTile.SetColor(Color.yellow);
        wheatTile.SetTileType(TileType.Building);
        
        // Milho
        cornTile.SetName("Milho");
        cornTile.SetColor(new Color(1f, 0.8f, 0f)); // Dourado
        cornTile.SetTileType(TileType.Building);
        
        // Irrigação
        waterTile.SetName("Irrigação");
        waterTile.SetColor(Color.cyan);
        waterTile.SetTileType(TileType.Path);
    }
    
    void SetupFarmControls()
    {
        // Configurar hover especial para fazenda
        GridManager.Instance.SetHoverColor(Color.green);
        
        // Sistema automático de irrigação
        StartCoroutine(AutoIrrigationSystem());
    }
    
    IEnumerator AutoIrrigationSystem()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            
            // Verificar tiles que precisam de água
            CheckIrrigationNeeds();
        }
    }
    
    void CheckIrrigationNeeds()
    {
        // Lógica para verificar tiles de plantio próximos à água
        Debug.Log("Verificando necessidades de irrigação...");
    }
}
```

### Exemplo 3: Sistema de Defesa Tower Defense

```csharp
public class TowerDefense : MonoBehaviour
{
    [Header("Defense Tiles")]
    public TileItemData towerTile;
    public TileItemData wallTile;
    public TileItemData pathTile;
    
    void Start()
    {
        SetupDefenseTiles();
        ConfigureTowerDefenseGrid();
    }
    
    void SetupDefenseTiles()
    {
        // Torre de defesa
        towerTile.SetName("Torre");
        towerTile.SetColor(Color.red);
        towerTile.SetTileType(TileType.Building);
        
        // Muralha
        wallTile.SetName("Muralha");
        wallTile.SetColor(Color.gray);
        wallTile.SetTileType(TileType.Building);
        
        // Caminho dos inimigos
        pathTile.SetName("Caminho");
        pathTile.SetColor(Color.brown);
        pathTile.SetTileType(TileType.Path);
    }
    
    void ConfigureTowerDefenseGrid()
    {
        // Hover especial para áreas de construção
        GridManager.Instance.SetHoverColor(Color.red);
        
        // Detectar quando uma torre é posicionada
        // (Integrar com sistema de eventos)
    }
    
    void OnTowerPlaced(Vector3Int position)
    {
        Debug.Log($"Torre posicionada em: {position}");
        
        // Lógica de range de ataque
        ShowTowerRange(position);
    }
    
    void ShowTowerRange(Vector3Int towerPosition)
    {
        // Destacar células dentro do alcance da torre
        int range = 3;
        
        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                Vector3Int checkPosition = towerPosition + new Vector3Int(x, y, 0);
                // Destacar posição se estiver no alcance
            }
        }
    }
}
```

## ⚙️ Customização Avançada

### Criando Novos Tipos de Tile

```csharp
// 1. Estenda o enum TileType
public enum TileType
{
    Default,
    Building,
    Path,
    Water,      // Novo tipo
    Forest,     // Novo tipo
    Mountain    // Novo tipo
}

// 2. Crie comportamentos customizados
public class WaterTile : MonoBehaviour
{
    void Start()
    {
        // Comportamento específico para tiles de água
        AddRippleEffect();
    }
    
    void AddRippleEffect()
    {
        // Adicionar efeito visual de ondulação
    }
}
```

### Customizando Efeitos Visuais

```csharp
public class CustomVisualEffects : MonoBehaviour
{
    void Start()
    {
        CustomizeHoverEffects();
        AddParticleEffects();
    }
    
    void CustomizeHoverEffects()
    {
        // Hover customizado com gradiente
        Color[] hoverGradient = { Color.yellow, Color.orange, Color.red };
        
        // Aplicar baseado no tipo de tile
        GridManager.Instance.SetHoverColor(hoverGradient[0]);
    }
    
    void AddParticleEffects()
    {
        // Adicionar partículas quando tiles são posicionados
        // Integrar com sistema de eventos do GridManager
    }
}
```

### Sistema de Save/Load

```csharp
[System.Serializable]
public class GridSaveData
{
    public List<TileSaveData> placedTiles = new List<TileSaveData>();
}

[System.Serializable]
public class TileSaveData
{
    public Vector3Int position;
    public TileType tileType;
    public string tileName;
    public Color tileColor;
}

public class SaveSystem : MonoBehaviour
{
    public void SaveGrid()
    {
        GridSaveData saveData = new GridSaveData();
        
        // Coletar todos os tiles posicionados
        for (int x = 0; x < GridManager.Instance.Bounds.size.x; x++)
        {
            for (int y = 0; y < GridManager.Instance.Bounds.size.y; y++)
            {
                GameObject tileObject = GridManager.Instance.TileObjects[x, y];
                if (tileObject != null)
                {
                    Tile tile = tileObject.GetComponent<Tile>();
                    TileSaveData tileData = new TileSaveData
                    {
                        position = new Vector3Int(x, y, 0),
                        tileType = tile.TileItemData.TileType,
                        tileName = tile.TileItemData.Name,
                        tileColor = tile.TileItemData.Color
                    };
                    saveData.placedTiles.Add(tileData);
                }
            }
        }
        
        // Salvar em JSON
        string json = JsonUtility.ToJson(saveData, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/grid_save.json", json);
        
        Debug.Log("Grid salvo com sucesso!");
    }
    
    public void LoadGrid()
    {
        string filePath = Application.persistentDataPath + "/grid_save.json";
        
        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            GridSaveData saveData = JsonUtility.FromJson<GridSaveData>(json);
            
            // Limpar grid atual
            ClearGrid();
            
            // Recriar tiles salvos
            foreach (TileSaveData tileData in saveData.placedTiles)
            {
                RecreateFile(tileData);
            }
            
            Debug.Log("Grid carregado com sucesso!");
        }
    }
    
    void ClearGrid()
    {
        // Retornar todos os tiles para o pool
        for (int x = 0; x < GridManager.Instance.Bounds.size.x; x++)
        {
            for (int y = 0; y < GridManager.Instance.Bounds.size.y; y++)
            {
                GameObject tileObject = GridManager.Instance.TileObjects[x, y];
                if (tileObject != null)
                {
                    TilePool.Instance.ReturnPooledObject(tileObject);
                    GridManager.Instance.TileObjects[x, y] = null;
                }
            }
        }
    }
    
    void RecreateFile(TileSaveData tileData)
    {
        // Obter tile do pool
        GameObject newTile = TilePool.Instance.GetPooledObject();
        
        if (newTile != null)
        {
            // Configurar tile
            Tile tileComponent = newTile.GetComponent<Tile>();
            TileItemData itemData = ScriptableObject.CreateInstance<TileItemData>();
            
            itemData.SetName(tileData.tileName);
            itemData.SetColor(tileData.tileColor);
            itemData.SetTileType(tileData.tileType);
            itemData.SetCoordinate(tileData.position.x, tileData.position.y);
            
            tileComponent.SetTileItemData(itemData);
            
            // Posicionar no grid
            Vector3 worldPosition = GridManager.Instance.Grid.GetCellCenterWorld(tileData.position);
            newTile.transform.position = worldPosition;
            newTile.SetActive(true);
            
            // Registrar no GridManager
            GridManager.Instance.TileObjects[tileData.position.x, tileData.position.y] = newTile;
        }
    }
}
```

## 🐛 Troubleshooting

### Problemas Comuns

#### **1. Tiles não aparecem no hover**
```csharp
// Verificar se o Tilemap tem tiles marcados
Debug.Log($"Tiles no Tilemap: {GridManager.Instance.BoundsCalculator.GetAllMarkedTiles(GridManager.Instance.Bounds)}");

// Verificar se hover está habilitado
Debug.Log($"Hover habilitado: {GridManager.Instance.IsHovering}");
```

#### **2. Pool de tiles vazio**
```csharp
// Verificar configuração do pool
Debug.Log($"Tiles no pool: {TilePool.Instance.amountToPool}");

// Reconfigurar se necessário
TilePool.Instance.amountToPool = 100;
```

#### **3. Cliques não funcionam**
```csharp
// Verificar se ClickHandler está ativo
Debug.Log($"ClickHandler ativo: {GridManager.Instance.ClickHandler != null}");

// Verificar se Input System está configurado
// Window > Analysis > Input Debugger
```

### Dicas de Performance

#### **Otimização para Grids Grandes**
```csharp
void OptimizeForLargeGrids()
{
    // Aumentar pool baseado no tamanho do grid
    int gridSize = GridManager.Instance.Bounds.size.x * GridManager.Instance.Bounds.size.y;
    TilePool.Instance.amountToPool = Mathf.Min(gridSize / 4, 500); // Máximo 500 tiles
    
    // Desabilitar hover se performance for crítica
    if (gridSize > 10000)
    {
        GridManager.Instance.SetHoverEnabled(false);
    }
}
```

#### **Otimização de Renderização**
```csharp
void OptimizeRendering()
{
    // Usar LOD para tiles distantes
    // Implementar frustum culling
    // Agrupar tiles similares em batches
}
```

## 📚 Próximos Passos

### Aprofunde seus Conhecimentos

1. **[API Reference Completa](../api/index.md)** - Explore todas as funcionalidades
2. **[Módulos Avançados](../api/index.md#grid-modules)** - PathManager, HoverManager, etc.
3. **[Casos de Uso Específicos](introduction.md#casos-de-uso)** - Exemplos por gênero de jogo

### Expanda o Sistema

1. **Adicione novos tipos de tile** - Customize para seu jogo
2. **Implemente save/load** - Persistência de dados
3. **Crie efeitos visuais** - Partículas e animações
4. **Integre com outros sistemas** - Audio, UI, Networking

## 💡 Dicas Finais

### Melhores Práticas

- **Use Object Pooling** sempre que possível
- **Mantenha módulos independentes** para facilitar testes
- **Documente suas customizações** para facilitar manutenção
- **Teste em diferentes tamanhos de grid** para garantir performance

### Recursos Adicionais

- **[Documentação Unity](https://docs.unity3d.com/)** - Referência oficial
- **[Input System Guide](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/index.html)** - Sistema de input moderno
- **[Performance Optimization](https://unity.com/how-to/optimize-game-performance)** - Guias de otimização

Unity 6 | Sistema Modular | Performance Otimizada