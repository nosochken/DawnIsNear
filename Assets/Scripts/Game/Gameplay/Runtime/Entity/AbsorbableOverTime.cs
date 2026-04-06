using System;
using System.Collections;
using UnityEngine;

namespace Game.Gameplay
{
    internal class AbsorbableOverTime : MonoBehaviour
    {
        private Size _size;
        private Absorbable _absorbable;
        
        private float _absorbOverTimeInterval;
        private Coroutine _beAbsorbedCoroutine;
        
        internal void Initialize(Absorbable absorbable, Size size, float delayInDecrease)
        {
            _absorbable = absorbable ?? throw new ArgumentNullException(nameof(absorbable));
            _size = size ?? throw new ArgumentNullException(nameof(size));
            
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
            
            while (isActiveAndEnabled)
            {
                yield return wait;

                _size.Decrease(1);

                if (_size.Current <= 0)
                {
                    _absorbable.BeAbsorbed();
                    _beAbsorbedCoroutine = null;
                    yield break;
                }
            }
        }
    }
}