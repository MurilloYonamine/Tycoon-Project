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

namespace GRID
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private ClickHandler _clickHandler;

        [SerializeField] private TMP_FontAsset _font;

        private GameObject[] _columnsArray;
        private GameObject[,] _tileObjects;

        private BoundsInt _bounds;

        private void Awake()
        {
            _bounds = _tilemap.cellBounds;
            _tileObjects = new GameObject[_bounds.size.x, _bounds.size.y];

            TilePool.Instance.amountToPool = _bounds.size.x * _bounds.size.y;
            TilePool.Instance.pooledObjectsContainer = _tilemap.gameObject;

            GenerateColumnsAndCoordinates();
        }
        private void OnEnable() => _clickHandler.OnClickPerformed += HandleClick;
        private void OnDisable() => _clickHandler.OnClickPerformed -= HandleClick;

        private void HandleClick(InputAction.CallbackContext context)
        {
            Vector3 worldPosition = GetMouseWorldPosition();
            Vector3Int cell = _grid.WorldToCell(worldPosition);

            if (!_tilemap.HasTile(cell))
            {
                Debug.Log("Célula não demarcada no tilemap. Ignorando clique.");
                return;
            }

            Vector3Int coordinate = GetTileCoordinate(cell);

            if (_tileObjects[coordinate.x, coordinate.y] != null)
            {
                Debug.Log($"Célula ({coordinate.x}, {coordinate.y}) já ocupada.");
                return;
            }

            Vector3 cellCenter = _grid.GetCellCenterWorld(cell);
            //GameObject tileObject = Instantiate(_tilePrefab, cellCenter, Quaternion.identity);
            
            GameObject tileObject = TilePool.Instance.GetPooledObject();
            tileObject.transform.position = cellCenter;
            tileObject.name = $"Tile: ({coordinate.x}, {coordinate.y})";

            GameObject columnObject = GetColumnObject(coordinate.x);
            tileObject.transform.SetParent(columnObject.transform);

            _tileObjects[coordinate.x, coordinate.y] = tileObject;

            Debug.Log($"Coordenada: ({coordinate.x}, {coordinate.y})");
        }


        /// <summary> Converte uma posição de célula em coordenadas lógicas (coluna, linha), com origem no canto superior esquerdo. </summary>
        private Vector3Int GetTileCoordinate(Vector3Int cell)
        {
            int col = cell.x - _bounds.xMin;
            int row = _bounds.yMax - 1 - cell.y;

            return new Vector3Int(col, row, 0);
        }

        /// <summary> Obtém a posição do mouse convertida para o mundo. </summary>
        private Vector3 GetMouseWorldPosition()
        {
            Vector2 mouseScreen = Mouse.current.position.ReadValue();
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(
                new Vector3(mouseScreen.x, mouseScreen.y, -Camera.main.transform.position.z)
            );
            return mouseWorld;
        }

        /// <summary> Retorna a coluna correspondente baseada no índice. </summary>
        private GameObject GetColumnObject(int col)
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
            columnsRoot.transform.SetParent(_tilemap.transform);

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
                if (!_tilemap.HasTile(cell)) continue;

                Vector3 cellCenter = _grid.GetCellCenterWorld(cell);

                GameObject textObj = new GameObject($"Coordinate Text: ({x},{y})");
                textObj.transform.position = cellCenter;
                textObj.transform.SetParent(textsParent.transform);

                var textMeshPro = textObj.AddComponent<TextMeshPro>();
                textMeshPro.text = $"({x},{y})";
                textMeshPro.fontSize = 3;
                textMeshPro.alignment = TextAlignmentOptions.Center;
                textMeshPro.color = Color.white;
                textMeshPro.font = _font;
            }
        }
    }
}
