// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : ShopManager.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 15/07/2025 | 17:16
// > Purpose: Gerencia a interface da loja, seleção de tiles e animações da loja.
// ════════════════════════════════════════════════════

using System.Collections;
using System.Collections.Generic;
using GRID;
using GRID.TILE;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SHOP
{
    /// <summary>
    /// Gerenciador principal do sistema de loja responsável pela interface, animações e seleção de tiles.
    /// Controla a abertura/fechamento da loja, configuração de tiles disponíveis e instanciação de tiles selecionados.
    /// </summary>
    /// <remarks>
    /// Este gerenciador implementa um sistema completo de loja para o jogo de tycoon, permitindo
    /// que jogadores selecionem diferentes tipos de tiles para posicionamento no grid. Inclui
    /// animações suaves de abertura/fechamento, configuração dinâmica de tiles e integração
    /// com o sistema de Object Pool para otimização de performance.
    /// </remarks>
    public class ShopManager : MonoBehaviour
    {
        #region Serialized Fields
        /// <summary>
        /// Componente Animator responsável pelas animações de abertura e fechamento da loja.
        /// </summary>
        [SerializeField] private Animator _animator;

        /// <summary>
        /// Prefab do tile que será instanciado quando selecionado na loja.
        /// </summary>
        [SerializeField] private GameObject _tilePrefab;
        
        /// <summary>
        /// Container onde os tiles selecionados serão organizados na hierarquia.
        /// </summary>
        [SerializeField] private GameObject _tileContainer;
        
        /// <summary>
        /// Lista dos GameObjects de tiles disponíveis na interface da loja.
        /// </summary>
        [SerializeField] private List<GameObject> _tiles;
        #endregion

        #region Private Fields
        /// <summary>
        /// Flag indicando se a loja está atualmente aberta.
        /// </summary>
        private bool _shopIsOpen = false;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// Inicializa o estado da loja e configura os tiles de teste.
        /// </summary>
        private void Start()
        {
            _shopIsOpen = false;
            SetTestTiles();
        }

        /// <summary>
        /// Verifica input do usuário para abrir/fechar a loja usando a tecla Tab.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (!_shopIsOpen)
                    OpenShop();
                else
                    CloseShop();
            }
        }
        #endregion

        #region Tile Selection Handling
        /// <summary>
        /// Inicia a coroutine para lidar com as ações do tile após o fechamento da loja.
        /// </summary>
        /// <param name="itemData">Dados do tile selecionado pelo jogador.</param>
        /// <remarks>
        /// Este método coordena o processo de seleção de tile, iniciando a sequência
        /// de fechamento da loja seguido pela instanciação do tile selecionado.
        /// </remarks>
        private void HandleButton(TileItemData itemData) => StartCoroutine(HandleTileAfterClose(itemData));

        /// <summary>
        /// Fecha a loja e, após a animação, instancia e configura o tile selecionado.
        /// </summary>
        /// <param name="itemData">Dados do tile a ser instanciado.</param>
        /// <returns>Enumerator para execução como corrotina.</returns>
        /// <remarks>
        /// Esta corrotina garante que o tile só seja instanciado após o fechamento completo
        /// da loja. Utiliza o sistema de Object Pool para reutilização eficiente de objetos
        /// e cria uma cópia dos dados do tile para evitar compartilhamento de instâncias.
        /// O tile instanciado é automaticamente colocado em modo fantasma para posicionamento.
        /// </remarks>
        private IEnumerator HandleTileAfterClose(TileItemData itemData)
        {
            CloseShop();

            while (_shopIsOpen) yield return null;

            GameObject tilePool = TilePool.Instance.GetPooledObject();
            tilePool.transform.SetParent(null);
            tilePool.SetActive(true);

            Tile tile = tilePool.GetComponent<Tile>();

            // Fazemos uma cópia para evitar compartilhamento de instância
            TileItemData copiedData = ScriptableObject.CreateInstance<TileItemData>();
            copiedData.SetName(itemData.Name);
            copiedData.SetColor(itemData.Color);
            copiedData.SetTileType(itemData.TileType);

            tile.SetTileItemData(copiedData);
            tile.HandleSelection(true);
        }
        #endregion

        #region Tile Configuration Methods
        /// <summary>
        /// Configura os tiles da loja, atribuindo cores, nomes e eventos de clique.
        /// </summary>
        /// <remarks>
        /// Este método configura tiles de teste para demonstração, incluindo um tile de caminho
        /// (verde) e múltiples tiles de construção com cores variadas. Em uma implementação
        /// completa, esta configuração seria baseada em dados externos ou configurações de jogo.
        /// </remarks>
        private void SetTestTiles()
        {
            // Caminho
            Color pathColor = Color.green;
            string pathName = "Caminho";
            ConfigureTile(_tiles[0], pathColor, pathName, TileType.Path);

            // Contruções
            Color[] colors = { Color.yellow, Color.red, Color.blue, Color.magenta, Color.gray };
            string[] colorNames = { "Amarelo", "Vermelho", "Azul", "Magenta", "Cinza" };

            for (int i = 1; i < _tiles.Count; i++)
            {
                Color color = colors[i % colors.Length];
                string colorName = colorNames[i % colorNames.Length];
                ConfigureTile(_tiles[i], color, colorName, TileType.Building);
            }
        }
        
        /// <summary>
        /// Configura visualmente e funcionalmente um tile de teste, atribuindo cor, nome e evento de clique.
        /// </summary>
        /// <param name="tileObject">GameObject do tile a ser configurado na interface da loja.</param>
        /// <param name="color">Cor visual do tile.</param>
        /// <param name="tileName">Nome do tile exibido na interface.</param>
        /// <param name="tileType">Tipo funcional do tile.</param>
        /// <remarks>
        /// Este método configura completamente um tile da loja, incluindo aparência visual,
        /// texto informativo e evento de clique. Cria uma instância única de TileItemData
        /// para cada tile da loja, garantindo que cada seleção seja independente.
        /// </remarks>
        private void ConfigureTile(GameObject tileObject, Color color, string tileName, TileType tileType)
        {
            Image squareImage = tileObject.GetComponentInChildren<Image>();
            TextMeshProUGUI tileTMP = tileObject.GetComponentInChildren<TextMeshProUGUI>();
            Button button = tileObject.GetComponentInChildren<Button>();
            Tile tile = tileObject.GetComponent<Tile>();

            squareImage.color = color;
            tileTMP.text = tileName;

            TileItemData itemData = ScriptableObject.CreateInstance<TileItemData>();
            itemData.SetName(tileName);
            itemData.SetColor(color);
            itemData.SetTileType(tileType);

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                HandleButton(itemData);
            });

        }
        #endregion

        #region Shop Animation Methods
        /// <summary>
        /// Abre a loja, acionando a animação correspondente.
        /// </summary>
        /// <remarks>
        /// Configura os parâmetros do Animator para iniciar a animação de abertura
        /// e atualiza o estado interno da loja.
        /// </remarks>
        private void OpenShop()
        {
            _animator.SetBool("Close", false);
            _animator.SetBool("Open", true);
            _shopIsOpen = true;
        }

        /// <summary>
        /// Fecha a loja, acionando a animação correspondente e aguardando seu término.
        /// </summary>
        /// <remarks>
        /// Inicia a animação de fechamento e coordena com uma corrotina para aguardar
        /// o término da animação antes de atualizar o estado da loja.
        /// </remarks>
        private void CloseShop()
        {
            _animator.SetBool("Open", false);
            _animator.SetBool("Close", true);
            StartCoroutine(OnCloseAnimationEnd());
        }

        /// <summary>
        /// Aguarda o término da animação de fechamento da loja.
        /// </summary>
        /// <returns>Enumerator para execução como corrotina.</returns>
        /// <remarks>
        /// Esta corrotina determina dinamicamente a duração da animação de fechamento
        /// buscando pelo clip "Close" no AnimatorController. Isso garante sincronização
        /// precisa entre a animação visual e o estado lógico da loja.
        /// </remarks>
        private IEnumerator OnCloseAnimationEnd()
        {
            float closeAnimLength = 0f;
            if (_animator.runtimeAnimatorController != null)
            {
                foreach (var clip in _animator.runtimeAnimatorController.animationClips)
                {
                    if (clip.name == "Close")
                    {
                        closeAnimLength = clip.length;
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(closeAnimLength);

            _shopIsOpen = false;
            _animator.SetBool("Close", false);
            _animator.SetBool("Open", false);
        }
        #endregion
    }
}