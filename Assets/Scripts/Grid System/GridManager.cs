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
    public class GridManager : MonoBehaviour
    {
        #region Properties and Fields
        public static GridManager Instance { get; private set; }

        [Header("Core Components")]
        [field: SerializeField] public Grid Grid { get; private set; }
        [field: SerializeField] public Tilemap Tilemap { get; private set; }
        [field: SerializeField] public ClickHandler ClickHandler { get; private set; }

        [Header("UI")]
        [SerializeField] private TMP_FontAsset Font;
        [SerializeField] private Button StartButton;

        [Header("Hover Settings")]
        [SerializeField] private bool enableHoverEffect = true;
        [SerializeField] private Color hoverColor = Color.yellow;

        [Header("Debug")]
        [field: SerializeField] public GameObject[,] TileObjects { get; private set; }

        // Módulos
        public PathManager PathManager { get; private set; }
        public GridBoundsCalculator BoundsCalculator { get; private set; }
        public CellValidator CellValidator { get; private set; }
        public CoordinateConverter CoordinateConverter { get; private set; }
        public ColumnUIGenerator ColumnUIGenerator { get; private set; }
        public HoverManager HoverManager { get; private set; }

        // Dados compartilhados
        public BoundsInt Bounds { get; private set; }
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            InitializeModules();
            InitializeGrid();
        }

        private void Update()
        {
            HoverManager?.UpdateHover();
        }

        private void OnDisable()
        {
            HoverManager?.ClearHoverEffects();
        }
        #endregion

        #region Initialization
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

        /// <summary> Adiciona um tile à lista de caminhos, reorganiza a lista e verifica por gaps. </summary>
        public void AddTileToPathList(TILE.Tile tile) => PathManager.AddTileToPathList(tile);

        // Cell Validation

        /// <summary> Verifica se existe um tile na célula especificada no Tilemap. </summary>
        public bool HasTileAtCell(Vector3Int cell) => CellValidator.HasTileAtCell(cell);
        
        /// <summary> Verifica se a célula lógica (coluna, linha) já está ocupada por um objeto. </summary>
        public bool IsCellOccupied(Vector3Int coordinate) => CellValidator.IsCellOccupied(coordinate);
        
        /// <summary> Retorna o GameObject da célula especificada pela coordenada lógica (coluna, linha). </summary>
        public GameObject GetCellByCoordinate(Vector3Int coordinate) => CellValidator.GetCellByCoordinate(coordinate);

        // Coordinate Conversion

        /// <summary> Converte uma posição de célula em coordenadas lógicas (coluna, linha), com origem no canto superior esquerdo. </summary>
        public Vector3Int GetTileCoordinate(Vector3Int cell) => CoordinateConverter.GetTileCoordinate(cell);
        
        /// <summary> Obtém a posição do mouse convertida para o mundo. </summary>
        public Vector3 GetMouseWorldPosition() => CoordinateConverter.GetMouseWorldPosition();

        // Column Management
        
        /// <summary> Retorna a coluna correspondente baseada no índice. </summary>
        public GameObject GetColumnObject(int col) => ColumnUIGenerator.GetColumnObject(col);

        // Hover Management

        /// <summary> Remove todos os efeitos de hover. </summary>
        public void ClearHoverEffects() => HoverManager?.ClearHoverEffects();
        
        /// <summary> Define a cor do efeito de hover. </summary>
        public void SetHoverColor(Color color) => HoverManager?.SetHoverColor(color);
        
        /// <summary> Ativa ou desativa o efeito de hover. </summary>
        public void SetHoverEnabled(bool enabled) => HoverManager?.SetHoverEnabled(enabled);
        
        /// <summary> Verifica se está fazendo hover sobre um tile. </summary>
        public bool IsHovering => HoverManager?.IsHovering ?? false;
        
        /// <summary> Obtém a célula atualmente sob hover. </summary>
        public Vector3Int CurrentHoveredCell => HoverManager?.CurrentHoveredCell ?? Vector3Int.zero;
        #endregion
    }
}
