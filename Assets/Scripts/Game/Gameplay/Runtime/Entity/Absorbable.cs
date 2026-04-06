using System;
using UnityEngine;

namespace Game.Gameplay
{
    public class Absorbable : MonoBehaviour, IAbsorbable
    {
        private Size _size;
        
        public event Action<IAbsorbable> Absorbed;
        
        public ISize Size => _size;

        internal void Initialize(Size size)
        {
            _size = size ?? throw new ArgumentNullException(nameof(size));
        }

        public void BeAbsorbed()
        {
            _size.DecreaseToZero();
            Absorbed?.Invoke(this);
        }
    }
}