using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Absorbable), typeof(Absorber))]
    public abstract class Unit : MonoBehaviour
    {
        private Size _size;
        private Absorbable _absorbable;
        private Absorber _absorber;
        
        public string Name { get; private set; }

        protected void InitializeBase(SizeData data)
        {
            _size = new Size(data.MinSize);
            
            _absorbable = GetComponent<Absorbable>();
            _absorbable.Initialize(_size, data.DelayInDecrease);
            
            _absorber = GetComponent<Absorber>();
            _absorber.Initialize(_size);
        }
        
        public void SetName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Display name cannot be empty.", nameof(newName));

            Name = newName.Trim();
        }
    }
}