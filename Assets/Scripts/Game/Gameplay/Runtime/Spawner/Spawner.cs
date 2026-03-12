using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using System;
using Zenject;

namespace Game.Gameplay
{
    public class Spawner<T> : MonoBehaviour where T : MonoBehaviour, ISpawnable<T>
    {
        private Func<T> _createSpawnable;
        private PlayField _playField;

        private ObjectPool<T> _pool;

        private int _poolCapacity;
        private int _poolMaxSize;

        private int _activeCount;

        private Coroutine _spawnRoutine;
        private WaitUntil _waitUntilHasSpace;

        [Inject]
        private void Construct(PlayField playField)
        {
            _playField = playField ?? throw new ArgumentNullException(nameof(playField));
        }
        
        public void Initialize(Func<T> createSpawnable, int capacity)
        {
            _createSpawnable = createSpawnable ?? throw new ArgumentNullException(nameof(createSpawnable));
            
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            
            _poolCapacity = capacity;
            _poolMaxSize = _poolCapacity;

            _pool = new ObjectPool<T>(
                createFunc: Create,
                actionOnGet: ActOnGet,
                actionOnRelease: ActOnRelease,
                actionOnDestroy: ActOnDestroy,
                collectionCheck: true,
                defaultCapacity: _poolCapacity,
                maxSize: _poolMaxSize);
        }
        
        private void OnDisable()
        {
            StopSpawn();
        }

        public void Spawn(int count)
        {
            if (_pool == null)
                throw new InvalidOperationException("Spawner is not initialized.");
            
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            for (int i = 0; i < count; i++)
                _pool.Get();
        }

        public void MaintainCount(int count)
        {
            if (_pool == null)
                throw new InvalidOperationException("Spawner is not initialized.");
            
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            _waitUntilHasSpace = new WaitUntil(() => _activeCount < count);
            _spawnRoutine = StartCoroutine(SpawnLoop(_waitUntilHasSpace));
        }

        public void StopSpawn()
        {
            if (_spawnRoutine != null)
            {
                StopCoroutine(_spawnRoutine);
                _spawnRoutine = null;
            }
        }

        private T Create()
        {
            return _createSpawnable();
        }

        private void ActOnGet(T spawnable)
        {
            _activeCount++;

            spawnable.ReadyToSpawn += ReturnToPool;

            spawnable.transform.position = _playField.GetRandomPoint();
            spawnable.gameObject.SetActive(true);
        }

        private void ActOnRelease(T spawnable)
        {
            spawnable.ReadyToSpawn -= ReturnToPool;

            spawnable.gameObject.SetActive(false);
            _activeCount--;
        }

        private void ActOnDestroy(T spawnable)
        {
            Destroy(spawnable.gameObject);
        }
        
        private IEnumerator SpawnLoop(CustomYieldInstruction wait)
        {
            while (isActiveAndEnabled)
            {
                yield return wait;
                _pool.Get();
            }
        }

        private void ReturnToPool(T spawnable)
        {
            _pool.Release(spawnable);
        }
    }
}