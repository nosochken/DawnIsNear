using System;
using System.Collections;
using UnityEngine;

namespace Game.Gameplay
{
    public class Absorbable : IAbsorbable
    {
        private Unit _unit;
        
        private float _absorbOverTimeInterval;
        
        internal Absorbable(Unit unit, float absorbOverTimeInterval)
        {
            _unit = unit;
            _absorbOverTimeInterval = absorbOverTimeInterval;
        }
        
        public event Action<IAbsorbable> Absorbed;
        
        public ITargetable Owner => _unit;

        public void BeAbsorbed()
        {
            Absorbed?.Invoke(this);
        }
        
        internal IEnumerator BeAbsorbedOverTime()
        {
            WaitForSeconds wait = new WaitForSeconds(_absorbOverTimeInterval);
            
            while (_unit.IsActive)
            {
                yield return wait;

                _unit.Size.Decrease(1);

                if (_unit.Size.Current <= 0)
                {
                    BeAbsorbed();
                    yield break;
                }
            }
        }
    }
}