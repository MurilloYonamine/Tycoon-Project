// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : TileItemData.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 15/07/2025 | 17:56
// > Purpose  : Ele serve para armazenar dados de um item de tile em um grid, como nome, cor e sprite associado.
//   Isso facilita a criação e configuração de diferentes tipos de tiles diretamente pelo editor do Unity, sem necessidade de codificação adicional.
// ════════════════════════════════════════════════════

using UnityEngine;

namespace GRID.TILE
{
    [CreateAssetMenu(fileName = "TileItemData", menuName = "Tiles/TileItemData", order = 1)]
    public class TileItemData : ScriptableObject
    {
        [field: SerializeField] public TileType TileType { get; private set; } = TileType.Default;
        [field: SerializeField] public string Name { get; private set; } = string.Empty;
        [field: SerializeField] public Color Color { get; private set; } = Color.white;
        [field: SerializeField] public Sprite Sprite { get; private set; } = null;
        [field: SerializeField] public Vector2 Coordinate { get; private set; } = Vector2.zero;

        public void SetColor(Color color) => this.Color = color;
        public void SetName(string name) => this.Name = name;
        public void SetTileType(TileType tileType) => this.TileType = tileType;
        public void SetCoordinate(int column, int row) => Coordinate = new Vector2(column, row);
    }
}