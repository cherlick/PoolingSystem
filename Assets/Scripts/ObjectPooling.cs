namespace PoolingSystem {
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class Pool {
        public GameObject prefab;
        public int size = 1;
        public bool allowtoIncrement = true;
    }
    public class ObjectPooling : MonoBehaviour {
        public static ObjectPooling Instance;

        [SerializeField] private List<Pool> _poolsOnStart;
        [SerializeField] private Transform _parentForObjects = null;
        private Dictionary<string, Queue<GameObject>> _poolDic = new Dictionary<string, Queue<GameObject>> ();

        private void Awake () {
            Instance = this;
            _parentForObjects = transform;
            CreatePools (_poolsOnStart);
        }

        public void CreatePools (List<Pool> newPoolList) {
            foreach (Pool pool in newPoolList) {
                CreatePools (pool);
            }
        }
        public void CreatePools (Pool newPool) {
            Queue<GameObject> objectPool = new Queue<GameObject> ();
            
            for (int i = 0; i < newPool.size; i++)
                objectPool.Enqueue (CreateObject (newPool.prefab));

            if (!_poolDic.ContainsKey(newPool.prefab.name))
                _poolDic.Add (newPool.prefab.name, objectPool);
            
        }

        private GameObject CreateObject (GameObject prefab) {

            GameObject obj = Instantiate (prefab);
            obj.name = prefab.name;
            obj.SetActive (false);
            obj.transform.SetParent (_parentForObjects);
            return obj;
        }
        public GameObject GetObject (Pool pool) {
            if (_poolDic.TryGetValue (pool.prefab.name, out Queue<GameObject> poolList)) {
                if (poolList.Count == 0 && pool.allowtoIncrement) {
                    return CreateObject (pool.prefab);
                } else if (poolList.Count > 0) {
                    return poolList.Dequeue ();
                } else { return null; }
            } else { return null; }
        }

        public void ReturnObject (GameObject objectToEnqueue, string tag) {
            objectToEnqueue.SetActive (false);
            if (_poolDic.TryGetValue (tag, out Queue<GameObject> poolList))
                poolList.Enqueue (objectToEnqueue);
        }
    }
}