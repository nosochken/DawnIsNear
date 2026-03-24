using System;
using System.Collections;
using UnityEngine;

namespace Game.Gameplay
{
    public class Absorbable : MonoBehaviour, IAbsorbable
    {
        private Size _size;
        
        private float _absorbOverTimeInterval;
        private Coroutine _beAbsorbedCoroutine;
        
        public event Action<IAbsorbable> Absorbed;
        
        public bool IsActive => isActiveAndEnabled;
        public int CurrentSize => _size.Current;
        public Vector2 CurrentPosition => transform.position;

        internal void Initialize(Size size, float delayInDecrease)
        {
            if (minSize <= 0f)
                throw new ArgumentOutOfRangeException(nameof(minSize));
            
            Body = new Body(minSize, transform, this);
            
            if (delayInDecrease <= 0f)
                throw new ArgumentOutOfRangeException(nameof(delayInDecrease));
            
            _absorbOverTimeInterval = delayInDecrease;
        }

        private void OnEnable()
        {
            if (_beAbsorbedCoroutine == null) 
                _beAbsorbedCoroutine = StartCoroutine(BeAbsorbedOverTime());
        }

        private void OnDisable()
        {
            if (_beAbsorbedCoroutine != null)
            {
                StopCoroutine(_beAbsorbedCoroutine);
                _beAbsorbedCoroutine = null;
            }
        }
        
        private IEnumerator BeAbsorbedOverTime()
        {
            WaitForSeconds wait = new WaitForSeconds(_absorbOverTimeInterval);
            
            while (Body.IsActive)
            {
                yield return wait;

                Body.Size.Decrease(1);

                if (Body.Size.Current <= 0)
                {
                    BeAbsorbed();
                    _beAbsorbedCoroutine = null;
                    yield break;
                }
            }
        }

        public void BeAbsorbed()
        {
            Body.Size.DecreaseToZero();
            Absorbed?.Invoke(this);
        }
    }
}