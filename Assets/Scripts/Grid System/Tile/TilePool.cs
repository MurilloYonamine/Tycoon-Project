// ════════════════ [ SCRIPT HEADER ] ════════════════
// > Script   : TilePool.cs
// > Author   : Murillo Gomes Yonamine
// > Date     : 15/07/2025 | 04:55
// > Purpose  : Gerenciar uma pool de tiles para otimizar a criação e reutilização de GameObjects no Unity,
// evitando instanciamentos/destruições frequentes e melhorando a performance do jogo.
// ════════════════════════════════════════════════════

using System.Collections.Generic;
using UnityEngine;

namespace GRID.TILE
{
    /// <summary>
    /// Gerencia um pool de tiles para otimizar a criação e reutilização de GameObjects.
    /// Implementa o padrão Object Pool para evitar instanciamentos e destruições frequentes.
    /// </summary>
    /// <remarks>
    /// O sistema de pooling:
    /// - Pré-instancia uma quantidade definida de tiles
    /// - Reutiliza objetos inativos em vez de criar novos
    /// - Organiza tiles em grupos de 10 para melhor hierarquia
    /// - Melhora significativamente a performance do jogo
    /// - Reduz o garbage collection
    /// </remarks>
    public class TilePool : MonoBehaviour
    {
        #region Singleton
        /// <summary>
        /// Instância singleton do TilePool.
        /// </summary>
        public static TilePool Instance { get; private set; }
        #endregion

        #region Public Fields
        /// <summary>
        /// Container onde os objetos da pool serão organizados na hierarquia.
        /// </summary>
        [Tooltip("GameObject pai que conterá todos os tiles da pool")]
        public GameObject pooledObjectsContainer;
        
        /// <summary>
        /// Prefab do tile que será usado para criar os objetos da pool.
        /// </summary>
        [Tooltip("Prefab do tile a ser instanciado na pool")]
        public GameObject objectToPool;
        
        /// <summary>
        /// Lista de todos os tiles disponíveis na pool.
        /// </summary>
        [HideInInspector] 
        public List<GameObject> pooledObjects;
        
        /// <summary>
        /// Quantidade total de tiles para pré-instanciar na pool.
        /// </summary>
        [Tooltip("Número de tiles a serem criados na inicialização")]
        public int amountToPool;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// Inicializa o singleton e garante que apenas uma instância existe.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
        }

        /// <summary>
        /// Inicializa a pool criando todos os tiles e organizando-os em containers.
        /// </summary>
        private void Start()
        {
            pooledObjects = new List<GameObject>();
            GameObject temp;

            GameObject tilePool = new GameObject("Tile Pool");
            tilePool.transform.SetParent(pooledObjectsContainer.transform);

            int parentCount = Mathf.CeilToInt(amountToPool / 10f);
            GameObject[] subContainers = new GameObject[parentCount];
            for (int i = 0; i < parentCount; i++)
            {
                subContainers[i] = new GameObject($"Tile Pool {i * 10}-{(i + 1) * 10 - 1}");
                subContainers[i].transform.SetParent(tilePool.transform);
            }

            for (int i = 0; i < amountToPool; i++)
            {
                temp = Instantiate(objectToPool);
                temp.SetActive(false);
                pooledObjects.Add(temp);
                int parentIndex = i / 10;
                temp.transform.SetParent(subContainers[parentIndex].transform);
            }
        }
        #endregion

        #region Pool Management Methods
        /// <summary>
        /// Obtém um tile inativo da pool para reutilização.
        /// </summary>
        /// <returns>
        /// Um GameObject inativo da pool, ou null se nenhum estiver disponível.
        /// O tile retornado deve ser ativado pelo chamador antes do uso.
        /// </returns>
        /// <remarks>
        /// Este método implementa o padrão Object Pool para otimização de performance,
        /// evitando a criação e destruição constante de GameObjects durante o runtime.
        /// Itera através da pool procurando por objetos inativos disponíveis para reutilização.
        /// </remarks>
        public GameObject GetPooledObject()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                if (!pooledObjects[i].activeInHierarchy) return pooledObjects[i];
            }
            return null;
        }
        #endregion
    }
}