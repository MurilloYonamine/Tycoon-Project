// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : ColumnUIGenerator.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 18/07/2025
// > Purpose  : Gera colunas e interface de coordenadas
// ════════════════════════════════════════════════════

using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

namespace GRID.MODULES
{
    public class ColumnUIGenerator
    {
        private readonly Grid _grid;
        private readonly Tilemap _tilemap;
        private readonly TMP_FontAsset _font;
        private readonly BoundsInt _bounds;
        private GameObject[] _columnsArray;

        public ColumnUIGenerator(Grid grid, Tilemap tilemap, TMP_FontAsset font, BoundsInt bounds)
        {
            _grid = grid;
            _tilemap = tilemap;
            _font = font;
            _bounds = bounds;
        }

        #region Public Methods
        /// <summary> Retorna a coluna correspondente baseada no índice. </summary>
        public GameObject GetColumnObject(int col)
        {
            if (_columnsArray != null && col >= 0 && col < _columnsArray.Length)
                return _columnsArray[col];
            return null;
        }

        /// <summary> Cria os objetos de coluna e os textos de coordenada dentro de cada coluna. </summary>
        public void GenerateColumnsAndCoordinates()
        {
            _columnsArray = new GameObject[_bounds.size.x];

            GameObject columnsRoot = new GameObject("Columns");
            columnsRoot.transform.SetParent(_tilemap.transform);

            GameObject leftColumns = new GameObject("Left Columns");
            leftColumns.transform.SetParent(columnsRoot.transform);

            GameObject rightColumns = new GameObject("Right Columns");
            rightColumns.transform.SetParent(columnsRoot.transform);

            int mid = _bounds.size.x / 2;

            for (int column = 0; column < _bounds.size.x; column++)
            {
                GameObject columnObject = new GameObject($"Column: {column}");
                _columnsArray[column] = columnObject;

                if (column >= mid)
                    columnObject.transform.SetParent(rightColumns.transform);
                else
                    columnObject.transform.SetParent(leftColumns.transform);

                CreateCoordinateTexts(columnObject, column);
            }
        }
        #endregion

        #region Private Methods
        /// <summary> Cria textos de coordenada dentro da coluna especificada. </summary>
        private void CreateCoordinateTexts(GameObject columnObject, int column)
        {
            GameObject textsParent = new GameObject("Coordinate Texts");
            textsParent.transform.SetParent(columnObject.transform);

            for (int row = 0; row < _bounds.size.y; row++)
            {
                Vector3Int cell = new Vector3Int(column + _bounds.xMin, _bounds.yMax - 1 - row, 0);
                if (!_tilemap.HasTile(cell)) continue;

                Vector3 cellCenter = _grid.GetCellCenterWorld(cell);

                CreateCoordinateLabel(cellCenter, textsParent.transform, column, row);
            }
        }

        /// <summary> Cria um objeto de texto para exibir as coordenadas lógicas em uma posição específica do grid. </summary>
        private void CreateCoordinateLabel(Vector3 cellPosition, Transform parent, int column, int row)
        {
            GameObject textObject = new GameObject($"Coordinate Text: ({column},{row})");
            textObject.transform.position = cellPosition;
            textObject.transform.SetParent(parent);

            TextMeshPro textMeshPro = textObject.AddComponent<TextMeshPro>();
            textMeshPro.text = $"({column},{row})";
            textMeshPro.fontSize = 3;
            textMeshPro.alignment = TextAlignmentOptions.Center;
            textMeshPro.color = Color.white;
            textMeshPro.font = _font;

            if (textObject.TryGetComponent<RectTransform>(out RectTransform rectTransform))
                rectTransform.sizeDelta = new Vector2(1f, 1f);
        }
        #endregion
    }
}
