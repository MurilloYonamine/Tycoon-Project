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
    [CreateAssetMenu(fileName = "TileItemData", menuName = "ScriptableObjects/TileItemData", order = 1)]
    public class TileItemData : ScriptableObject
    {
        [field: SerializeField] public string _name { get; private set; } = string.Empty;
        [field: SerializeField] public Color _color { get; private set; } = Color.white;
        [field: SerializeField] public Sprite _sprite { get; private set; } = null;

        public void SetColor(Color color) => this._color = color;
        public void SetName(string name) => this._name = name;

    }
}