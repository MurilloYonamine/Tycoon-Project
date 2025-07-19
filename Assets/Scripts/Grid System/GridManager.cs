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

        [Header("Debug")]
        [field: SerializeField] public GameObject[,] TileObjects { get; private set; }

        // Módulos
        public PathManager PathManager { get; private set; }
        public GridBoundsCalculator BoundsCalculator { get; private set; }
        public CellValidator CellValidator { get; private set; }
        public CoordinateConverter CoordinateConverter { get; private set; }
        public ColumnUIGenerator ColumnUIGenerator { get; private set; }

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
        public void AddTileToPathList(TILE.Tile tile) => PathManager.AddTileToPathList(tile);

        // Cell Validation
        public bool HasTileAtCell(Vector3Int cell) => CellValidator.HasTileAtCell(cell);
        public bool IsCellOccupied(Vector3Int coordinate) => CellValidator.IsCellOccupied(coordinate);
        public GameObject GetCellByCoordinate(Vector3Int coordinate) => CellValidator.GetCellByCoordinate(coordinate);

        // Coordinate Conversion
        public Vector3Int GetTileCoordinate(Vector3Int cell) => CoordinateConverter.GetTileCoordinate(cell);
        public Vector3 GetMouseWorldPosition() => CoordinateConverter.GetMouseWorldPosition();

        // Column Management
        public GameObject GetColumnObject(int col) => ColumnUIGenerator.GetColumnObject(col);
        #endregion
    }
}
