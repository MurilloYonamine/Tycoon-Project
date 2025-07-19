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
    /// <summary> Gerencia a interface da loja, controla animações de abrir/fechar, seleção de tiles e instanciamento de tiles. </summary>
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private GameObject _tileContainer;
        [SerializeField] private List<GameObject> _tiles;

        private bool _shopIsOpen = false;

        private void Start()
        {
            _shopIsOpen = false;
            SetTestTiles();
        }

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

        /// <summary> Inicia a coroutine para lidar com as ações do tile após o fechamento da loja. </summary>
        private void HandleButton(TileItemData itemData) => StartCoroutine(HandleTileAfterClose(itemData));

        /// <summary> Fecha a loja e, após a animação, instancia e configura o tile selecionado. </summary>
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

        /// <summary> Configura os tiles da loja, atribuindo cores, nomes e eventos de clique. </summary>
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
        /// <summary> Configura visualmente e funcionalmente um tile de teste, atribuindo cor, nome e evento de clique. </summary>
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
        /// <summary> Abre a loja, acionando a animação correspondente. </summary>
        private void OpenShop()
        {
            _animator.SetBool("Close", false);
            _animator.SetBool("Open", true);
            _shopIsOpen = true;
        }

        /// <summary> Fecha a loja, acionando a animação correspondente e aguardando seu término. </summary>
        private void CloseShop()
        {
            _animator.SetBool("Open", false);
            _animator.SetBool("Close", true);
            StartCoroutine(OnCloseAnimationEnd());
        }

        /// <summary> Aguarda o término da animação de fechamento da loja. </summary>
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
    }
}