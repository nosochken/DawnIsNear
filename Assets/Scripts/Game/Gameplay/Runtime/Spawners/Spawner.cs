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

        private int _targetCount;
        private int _activeCount;

        private Coroutine _spawnRoutine;
        private WaitUntil _waitUntilHasSpace;
        
        public event Action ActiveCountDecreased;
        
        public int ActiveCount => _activeCount;

        [Inject]
        private void Construct(PlayField playField)
        {
            _playField = playField ?? throw new ArgumentNullException(nameof(playField));
        }
        
        public void Initialize(Func<T> createSpawnable, int targetCount)
        {
            _createSpawnable = createSpawnable ?? throw new ArgumentNullException(nameof(createSpawnable));
            
            if (targetCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(targetCount));
            
            _targetCount = targetCount;

            _pool = new ObjectPool<T>(
                createFunc: Create,
                actionOnGet: ActOnGet,
                actionOnRelease: ActOnRelease,
                actionOnDestroy: ActOnDestroy,
                collectionCheck: true,
                defaultCapacity: _targetCount,
                maxSize: _targetCount);
        }
        
        private void OnDisable()
        {
            StopSpawn();
        }

        public void SpawnTargetCount()
        {
            if (_pool == null)
                throw new InvalidOperationException("Spawner is not initialized.");

            for (int i = 0; i < _targetCount; i++)
                _pool.Get();
        }

        public void MaintainTargetCount()
        {
            if (_pool == null)
                throw new InvalidOperationException("Spawner is not initialized.");

            _waitUntilHasSpace = new WaitUntil(() => _activeCount < _targetCount);
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
            ActiveCountDecreased?.Invoke();
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