// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : GridManager.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 07/02/2025 | 20:59
// > Purpose  : Gerencia o sistema de grid com suporte a colunas e ocupação de tiles
// ════════════════════════════════════════════════════

using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using GRID.TILE;
using GRID.MODULES;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace GRID
{
    /// <summary>
    /// Gerenciador central do sistema de grid responsável pela coordenação de todos os módulos e funcionalidades do grid.
    /// Implementa o padrão Singleton e atua como ponto de entrada para operações relacionadas ao grid.
    /// </summary>
    /// <remarks>
    /// Este gerenciador utiliza uma arquitetura modular onde cada funcionalidade específica é delegada
    /// para módulos especializados (PathManager, HoverManager, CellValidator, etc.). Isso permite
    /// manutenção mais fácil, testabilidade melhorada e clara separação de responsabilidades.
    /// 
    /// O GridManager coordena:
    /// - Inicialização e configuração de todos os módulos
    /// - Comunicação entre módulos através de uma API pública
    /// - Gerenciamento do ciclo de vida do sistema de grid
    /// - Delegação de operações específicas para módulos apropriados
    /// </remarks>
    public class GridManager : MonoBehaviour
    {
        #region Properties and Fields
        /// <summary>
        /// Instância singleton do GridManager para acesso global.
        /// </summary>
        public static GridManager Instance { get; private set; }

        [Header("Core Components")]
        /// <summary>
        /// Componente Grid principal do Unity para conversões de coordenadas.
        /// </summary>
        [field: SerializeField] public Grid Grid { get; private set; }
        
        /// <summary>
        /// Tilemap onde os tiles base estão posicionados e que define a área jogável.
        /// </summary>
        [field: SerializeField] public Tilemap Tilemap { get; private set; }
        
        /// <summary>
        /// Manipulador de cliques responsável por capturar e processar input do usuário.
        /// </summary>
        [field: SerializeField] public ClickHandler ClickHandler { get; private set; }

        [Header("UI")]
        /// <summary>
        /// Fonte utilizada para texto da interface do usuário do grid.
        /// </summary>
        [SerializeField] private TMP_FontAsset Font;
        
        /// <summary>
        /// Botão para iniciar funcionalidades específicas do grid.
        /// </summary>
        [SerializeField] private Button StartButton;

        [Header("Hover Settings")]
        /// <summary>
        /// Flag para ativar/desativar efeitos de hover no grid.
        /// </summary>
        [SerializeField] private bool enableHoverEffect = true;
        
        /// <summary>
        /// Cor aplicada aos tiles durante o efeito de hover.
        /// </summary>
        [SerializeField] private Color hoverColor = Color.yellow;

        [Header("Debug")]
        /// <summary>
        /// Array bidimensional contendo referências para todos os GameObjects de tiles posicionados no grid.
        /// Organizado por [coluna, linha] para acesso direto baseado em coordenadas lógicas.
        /// </summary>
        [field: SerializeField] public GameObject[,] TileObjects { get; private set; }

        // Módulos especializados
        /// <summary>
        /// Módulo responsável pelo gerenciamento de caminhos e pathfinding.
        /// </summary>
        public PathManager PathManager { get; private set; }
        
        /// <summary>
        /// Módulo responsável pelo cálculo de limites e dimensões do grid.
        /// </summary>
        public GridBoundsCalculator BoundsCalculator { get; private set; }
        
        /// <summary>
        /// Módulo responsável pela validação de células e ocupação de tiles.
        /// </summary>
        public CellValidator CellValidator { get; private set; }
        
        /// <summary>
        /// Módulo responsável por conversões entre diferentes sistemas de coordenadas.
        /// </summary>
        public CoordinateConverter CoordinateConverter { get; private set; }
        
        /// <summary>
        /// Módulo responsável pela geração de UI de colunas e coordenadas.
        /// </summary>
        public ColumnUIGenerator ColumnUIGenerator { get; private set; }
        
        /// <summary>
        /// Módulo responsável pelos efeitos visuais de hover sobre tiles.
        /// </summary>
        public HoverManager HoverManager { get; private set; }

        /// <summary>
        /// Limites calculados do grid baseados nos tiles marcados no Tilemap.
        /// Define a área utilizável do grid para posicionamento de tiles.
        /// </summary>
        public BoundsInt Bounds { get; private set; }
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// Inicializa o singleton garantindo que apenas uma instância exista.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        /// <summary>
        /// Inicializa todos os módulos e configura o sistema de grid.
        /// </summary>
        private void Start()
        {
            InitializeModules();
            InitializeGrid();
        }

        /// <summary>
        /// Atualiza efeitos de hover a cada frame.
        /// </summary>
        private void Update()
        {
            HoverManager?.UpdateHover();
        }

        /// <summary>
        /// Limpa efeitos visuais quando o objeto é desativado.
        /// </summary>
        private void OnDisable()
        {
            HoverManager?.ClearHoverEffects();
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Inicializa todos os módulos especializados do sistema de grid na ordem correta.
        /// </summary>
        /// <remarks>
        /// A ordem de inicialização é importante pois alguns módulos dependem de outros:
        /// 1. BoundsCalculator - calcula limites do grid
        /// 2. TileObjects array - inicializado baseado nos bounds
        /// 3. Outros módulos - inicializados com as dependências necessárias
        /// </remarks>
        private void InitializeModules()
        {
            // Inicializar calculador de bounds primeiro
            BoundsCalculator = new GridBoundsCalculator(Tilemap);
            Bounds = BoundsCalculator.GetTrimmedBounds();

            // Inicializar array de tiles
            TileObjects = new GameObject[Bounds.size.x, Bounds.size.y];

            // Inicializar outros módulos
            PathManager = new PathManager(this);
            CellValidator = new CellValidator(Tilemap, this);
            CoordinateConverter = new CoordinateConverter(Bounds);
            ColumnUIGenerator = new ColumnUIGenerator(Grid, Tilemap, Font, Bounds);
            HoverManager = new HoverManager(this, enableHoverEffect, hoverColor);
        }

        /// <summary>
        /// Configura o grid após a inicialização dos módulos.
        /// </summary>
        /// <remarks>
        /// Configura o sistema de Object Pool e gera a interface do usuário das colunas.
        /// Esta configuração final garante que todos os módulos estejam prontos antes
        /// da geração de elementos visuais.
        /// </remarks>
        private void InitializeGrid()
        {
            // Configurar TilePool
            TilePool.Instance.amountToPool = BoundsCalculator.GetAllMarkedTiles(Bounds);
            TilePool.Instance.pooledObjectsContainer = Tilemap.gameObject;

            // Gerar UI
            ColumnUIGenerator.GenerateColumnsAndCoordinates();
        }
        #endregion

        #region Public API - Delegação para Módulos
        // Path Management
        /// <summary>
        /// Adiciona um tile à lista de caminhos, reorganiza a lista e verifica por gaps.
        /// </summary>
        /// <param name="tile">Tile a ser adicionado ao sistema de pathfinding.</param>
        /// <remarks>
        /// Delega a operação para o PathManager que gerencia a lógica de pathfinding
        /// e conectividade entre tiles de caminho.
        /// </remarks>
        public void AddTileToPathList(TILE.Tile tile) => PathManager.AddTileToPathList(tile);

        // Cell Validation
        /// <summary>
        /// Verifica se existe um tile na célula especificada no Tilemap.
        /// </summary>
        /// <param name="cell">Coordenada da célula no grid a ser verificada.</param>
        /// <returns>True se existe um tile na célula, false caso contrário.</returns>
        /// <remarks>
        /// Delega para o CellValidator que mantém a lógica de validação de células.
        /// </remarks>
        public bool HasTileAtCell(Vector3Int cell) => CellValidator.HasTileAtCell(cell);
        
        /// <summary>
        /// Verifica se a célula lógica (coluna, linha) já está ocupada por um objeto.
        /// </summary>
        /// <param name="coordinate">Coordenada lógica [coluna, linha] a ser verificada.</param>
        /// <returns>True se a célula está ocupada, false se está livre.</returns>
        /// <remarks>
        /// Utiliza o array TileObjects para verificação rápida de ocupação.
        /// </remarks>
        public bool IsCellOccupied(Vector3Int coordinate) => CellValidator.IsCellOccupied(coordinate);
        
        /// <summary>
        /// Retorna o GameObject da célula especificada pela coordenada lógica (coluna, linha).
        /// </summary>
        /// <param name="coordinate">Coordenada lógica [coluna, linha].</param>
        /// <returns>GameObject na coordenada especificada, ou null se vazia.</returns>
        public GameObject GetCellByCoordinate(Vector3Int coordinate) => CellValidator.GetCellByCoordinate(coordinate);

        // Coordinate Conversion
        /// <summary>
        /// Converte uma posição de célula em coordenadas lógicas (coluna, linha), com origem no canto superior esquerdo.
        /// </summary>
        /// <param name="cell">Célula do grid a ser convertida.</param>
        /// <returns>Coordenada lógica correspondente à célula.</returns>
        /// <remarks>
        /// Delega para o CoordinateConverter que mantém a lógica de conversão entre sistemas.
        /// </remarks>
        public Vector3Int GetTileCoordinate(Vector3Int cell) => CoordinateConverter.GetTileCoordinate(cell);
        
        /// <summary>
        /// Obtém a posição do mouse convertida para o mundo.
        /// </summary>
        /// <returns>Posição mundial do cursor do mouse.</returns>
        public Vector3 GetMouseWorldPosition() => CoordinateConverter.GetMouseWorldPosition();

        // Column Management
        /// <summary>
        /// Retorna a coluna correspondente baseada no índice.
        /// </summary>
        /// <param name="col">Índice da coluna desejada.</param>
        /// <returns>GameObject da coluna especificada.</returns>
        /// <remarks>
        /// As colunas são utilizadas para organização hierárquica dos tiles na interface.
        /// </remarks>
        public GameObject GetColumnObject(int col) => ColumnUIGenerator.GetColumnObject(col);

        // Hover Management
        /// <summary>
        /// Remove todos os efeitos de hover.
        /// </summary>
        public void ClearHoverEffects() => HoverManager?.ClearHoverEffects();
        
        /// <summary>
        /// Define a cor do efeito de hover.
        /// </summary>
        /// <param name="color">Nova cor para o efeito de hover.</param>
        public void SetHoverColor(Color color) => HoverManager?.SetHoverColor(color);
        
        /// <summary>
        /// Ativa ou desativa o efeito de hover.
        /// </summary>
        /// <param name="enabled">True para ativar, false para desativar.</param>
        public void SetHoverEnabled(bool enabled) => HoverManager?.SetHoverEnabled(enabled);
        
        /// <summary>
        /// Verifica se está fazendo hover sobre um tile.
        /// </summary>
        /// <value>True se um tile está sendo destacado por hover.</value>
        public bool IsHovering => HoverManager?.IsHovering ?? false;
        
        /// <summary>
        /// Obtém a célula atualmente sob hover.
        /// </summary>
        /// <value>Coordenada da célula sendo destacada, ou Vector3Int.zero se nenhuma.</value>
        public Vector3Int CurrentHoveredCell => HoverManager?.CurrentHoveredCell ?? Vector3Int.zero;
        #endregion
    }
}
