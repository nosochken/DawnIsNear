using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Absorbable), typeof(Absorber))]
    public abstract class Unit : MonoBehaviour, ITargetable
    {
        private Size _size;
        private Absorbable _absorbable;
        private Absorber _absorber;
        
        public string Name { get; private set; }
        public bool IsActive => isActiveAndEnabled;
        public ISizeData Size => _size;
        public ISizeEvents Resize => _size;
        public Vector2 CurrentPosition => transform.position;
        
        public IAbsorbableEvents Absorbable => _absorbable;

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