namespace PoolingSystem
{
    using UnityEngine;
    using System.Collections.Generic;
    using System;

    public class SpawnPooling : MonoBehaviour
    {
        private enum SpawnerType { Timer, RandomTime, EventTrigger, KeyInput };
        private ObjectPooling _pooling;
        [Header("Pools List")]
        [SerializeField] private Pool _localPool;

        [Header("Spawner Settings")]
        [SerializeField] private SpawnerType _spawnerType;
        [SerializeField] private bool _resetPosition = true;
        [SerializeField] private bool _resetRotation = true;

        [Header("Spawner Type Timer Settings")]
        [SerializeField] private float _spawnerTimeInervals = 0;
        private float _timer;
        [Header("Spawner Type Random Settings")]

        [Range(0,100)]
        [SerializeField] private float _betweenMinNumber;
        [Range(0,100)]
        [SerializeField] private float _betweenMaxNumber;
        [Header("Spawner Type KeyInput Settings")]
        [SerializeField] private KeyCode interactKey;

        public event Action OnEventSpawn;
        public void EventSpawn(){
            if (OnEventSpawn != null) OnEventSpawn();
        }

        private void Start() {
            _pooling = ObjectPooling.Instance;
            _pooling.CreatePools(_localPool);
            _timer = _spawnerTimeInervals;
        }

        private void Update() {
            switch (_spawnerType)
            {
                case SpawnerType.Timer:
                    TimerSpawn();
                    break;
                case SpawnerType.RandomTime:
                    RandomSpawn();
                    break;
                case SpawnerType.EventTrigger:
                    break;
                case SpawnerType.KeyInput:
                    KeyInputSpawn();
                    break;
            }
            
        }

        private void TimerSpawn()
        {
            _timer -= Time.deltaTime;
            if (_timer<0)
            {
                SpawnObject();
                _timer = _spawnerTimeInervals;
            }
        }

        private void KeyInputSpawn()
        {
            if(Input.GetKeyDown(interactKey))
                SpawnObject();
        }

        private void RandomSpawn()
        {
            float rnd =  UnityEngine.Random.Range(_betweenMinNumber, _betweenMaxNumber);
            _timer -= Time.deltaTime;
            if(_timer<0)
            {
                SpawnObject();
                _timer = rnd;
            }
                

        }

        private void SpawnObject(){
            GameObject obj = _pooling.GetObject(_localPool);
            if (_resetRotation)
                obj.transform.rotation = transform.rotation;
            if (_resetPosition)
                obj.transform.position = transform.position;
            obj.SetActive(true);
        }
    }
}

