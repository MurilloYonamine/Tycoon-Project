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
    /// <summary>
    /// ScriptableObject responsável por armazenar dados configuráveis de tiles para o sistema de grid.
    /// Permite criação de diferentes tipos de tiles através do Unity Editor com propriedades visuais e funcionais.
    /// </summary>
    /// <remarks>
    /// Este ScriptableObject facilita a criação e configuração de tiles de forma modular e reutilizável.
    /// Cada tile pode ter suas próprias propriedades visuais (sprite, cor) e funcionais (tipo, nome).
    /// A configuração via Inspector permite designers criarem novos tipos de tiles sem programação.
    /// </remarks>
    [CreateAssetMenu(fileName = "TileItemData", menuName = "Tiles/TileItemData", order = 1)]
    public class TileItemData : ScriptableObject
    {
        #region Serialized Properties
        /// <summary>
        /// Tipo funcional do tile que determina seu comportamento no grid.
        /// </summary>
        /// <value>Valor padrão é <see cref="TileType.Default"/>.</value>
        [field: SerializeField] public TileType TileType { get; private set; } = TileType.Default;
        
        /// <summary>
        /// Nome identificador do tile exibido na interface do usuário.
        /// </summary>
        /// <value>String vazia por padrão.</value>
        [field: SerializeField] public string Name { get; private set; } = string.Empty;
        
        /// <summary>
        /// Cor visual aplicada ao tile para diferenciação visual no grid.
        /// </summary>
        /// <value>Cor branca por padrão.</value>
        [field: SerializeField] public Color Color { get; private set; } = Color.white;
        
        /// <summary>
        /// Sprite visual do tile renderizado no grid.
        /// </summary>
        /// <value>Null por padrão, deve ser configurado no Inspector.</value>
        [field: SerializeField] public Sprite Sprite { get; private set; } = null;
        
        /// <summary>
        /// Coordenada atual do tile no sistema de grid 2D.
        /// </summary>
        /// <value>Vector2.zero por padrão (posição 0,0).</value>
        [field: SerializeField] public Vector2 Coordinate { get; private set; } = Vector2.zero;
        #endregion

        #region Configuration Methods
        /// <summary>
        /// Define a cor visual do tile.
        /// </summary>
        /// <param name="color">Nova cor a ser aplicada ao tile.</param>
        public void SetColor(Color color) => this.Color = color;
        
        /// <summary>
        /// Define o nome identificador do tile.
        /// </summary>
        /// <param name="name">Novo nome para o tile.</param>
        public void SetName(string name) => this.Name = name;
        
        /// <summary>
        /// Define o tipo funcional do tile.
        /// </summary>
        /// <param name="tileType">Novo tipo que determina o comportamento do tile.</param>
        public void SetTileType(TileType tileType) => this.TileType = tileType;
        
        /// <summary>
        /// Define a coordenada do tile no grid usando valores inteiros.
        /// </summary>
        /// <param name="column">Coordenada X (coluna) no grid.</param>
        /// <param name="row">Coordenada Y (linha) no grid.</param>
        public void SetCoordinate(int column, int row) => Coordinate = new Vector2(column, row);
        #endregion
    }
}