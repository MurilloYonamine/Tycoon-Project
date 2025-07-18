// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : GridManager.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 07/02/2025 | 20:59
// > Purpose  : Gerencia o sistema de grid com suporte a colunas e ocupação de tiles
// ════════════════════════════════════════════════════

using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using TMPro;
using GRID.TILE;

namespace GRID
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }
        [field: SerializeField] public Grid Grid { get; private set; }
        [field: SerializeField] public Tilemap Tilemap { get; private set; }
        [field: SerializeField] public ClickHandler ClickHandler { get; private set; }

        [SerializeField] private TMP_FontAsset Font;

        private GameObject[] _columnsArray;
        [field: SerializeField] public GameObject[,] TileObjects { get; private set; }

        private BoundsInt _bounds;

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
            _bounds = Tilemap.cellBounds;
            TileObjects = new GameObject[_bounds.size.x, _bounds.size.y];

            TilePool.Instance.amountToPool = _bounds.size.x * _bounds.size.y;
            TilePool.Instance.pooledObjectsContainer = Tilemap.gameObject;

            GenerateColumnsAndCoordinates();
        }
        /// <summary> Verifica se existe um tile na célula especificada no Tilemap. </summary>
        /// <param name="cell">A posição da célula a ser verificada.</param>
        /// <returns> Retorna true se houver um tile na célula; caso contrário, false. </returns>
        public bool HasTileAtCell(Vector3Int cell)
        {
            if (!Tilemap.HasTile(cell))
            {
                Debug.Log("Célula não demarcada no tilemap. Ignorando clique.");
                return false;
            }
            return true;
        }
        /// <summary> Verifica se a célula lógica (coluna, linha) já está ocupada por um objeto. </summary>
        /// <param name="coordinate"> Coordenada lógica da célula (coluna, linha). </param>
        /// <returns> Retorna true se a célula está ocupada; caso contrário, false. </returns>
        public bool IsCellOccupied(Vector3Int coordinate)
        {
            if (TileObjects[coordinate.x, coordinate.y] != null)
            {
                Debug.Log($"Célula ({coordinate.x}, {coordinate.y}) já ocupada.");
                return true;
            }
            return false;
        }

        /// <summary> Converte uma posição de célula em coordenadas lógicas (coluna, linha), com origem no canto superior esquerdo. </summary>
        public Vector3Int GetTileCoordinate(Vector3Int cell)
        {
            int col = cell.x - _bounds.xMin;
            int row = _bounds.yMax - 1 - cell.y;

            return new Vector3Int(col, row, 0);
        }

        /// <summary> Obtém a posição do mouse convertida para o mundo. </summary>
        public Vector3 GetMouseWorldPosition()
        {
            Vector2 mouseScreen = Mouse.current.position.ReadValue();
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(
                new Vector3(mouseScreen.x, mouseScreen.y, -Camera.main.transform.position.z)
            );
            return mouseWorld;
        }

        /// <summary> Retorna a coluna correspondente baseada no índice. </summary>
        public GameObject GetColumnObject(int col)
        {
            if (col >= 0 && col < _columnsArray.Length)
                return _columnsArray[col];
            return null;
        }

        /// <summary> Cria os objetos de coluna e os textos de coordenada dentro de cada coluna. </summary>
        private void GenerateColumnsAndCoordinates()
        {
            _columnsArray = new GameObject[_bounds.size.x];

            GameObject columnsRoot = new GameObject("Columns");
            columnsRoot.transform.SetParent(Tilemap.transform);

            GameObject leftColumns = new GameObject("Left Columns");
            leftColumns.transform.SetParent(columnsRoot.transform);

            GameObject rightColumns = new GameObject("Right Columns");
            rightColumns.transform.SetParent(columnsRoot.transform);

            int mid = _bounds.size.x / 2;

            for (int x = 0; x < _bounds.size.x; x++)
            {
                GameObject columnObject = new GameObject($"Column: {x}");
                _columnsArray[x] = columnObject;

                if (x >= mid)
                    columnObject.transform.SetParent(rightColumns.transform);
                else
                    columnObject.transform.SetParent(leftColumns.transform);

                CreateCoordinateTexts(columnObject, x);
            }
        }

        /// <summary> Cria textos de coordenada dentro da coluna especificada. </summary>
        private void CreateCoordinateTexts(GameObject columnObject, int x)
        {
            GameObject textsParent = new GameObject("Coordinate Texts");
            textsParent.transform.SetParent(columnObject.transform);

            for (int y = 0; y < _bounds.size.y; y++)
            {
                Vector3Int cell = new Vector3Int(x + _bounds.xMin, _bounds.yMax - 1 - y, 0);
                if (!Tilemap.HasTile(cell)) continue;

                Vector3 cellCenter = Grid.GetCellCenterWorld(cell);

                GameObject textObj = new GameObject($"Coordinate Text: ({x},{y})");
                textObj.transform.position = cellCenter;
                textObj.transform.SetParent(textsParent.transform);

                var textMeshPro = textObj.AddComponent<TextMeshPro>();
                textMeshPro.text = $"({x},{y})";
                textMeshPro.fontSize = 3;
                textMeshPro.alignment = TextAlignmentOptions.Center;
                textMeshPro.color = Color.white;
                textMeshPro.font = Font;

                if (TryGetComponent<RectTransform>(out RectTransform rectTransform))
                    rectTransform.sizeDelta = new Vector2(1f, 1f);
            }
        }
    }
}
