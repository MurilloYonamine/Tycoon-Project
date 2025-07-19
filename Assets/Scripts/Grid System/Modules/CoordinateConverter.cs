// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : CoordinateConverter.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 18/07/2025
// > Purpose  : Converte coordenadas entre diferentes sistemas
// ════════════════════════════════════════════════════

using UnityEngine;
using UnityEngine.InputSystem;

namespace GRID.MODULES
{
    public class CoordinateConverter
    {
        private readonly BoundsInt _bounds;

        public CoordinateConverter(BoundsInt bounds)
        {
            _bounds = bounds;
        }

        #region Public Methods
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

        /// <summary> Atualiza os bounds para recálculos de coordenadas. </summary>
        public void UpdateBounds(BoundsInt newBounds) { }
        #endregion
    }
}
