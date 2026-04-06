using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Body))]
    [RequireComponent(typeof(Absorbable), typeof(AbsorbableOverTime), typeof(Absorber))]
    public abstract class Unit : MonoBehaviour
    {
        private Body _body;
        private Absorbable _absorbable;
        private AbsorbableOverTime _absorbableOverTime;
        private Absorber _absorber;
        
        public string Name { get; private set; }
        
        public IBody Body => _body;
        public IAbsorbable Absorbable => _absorbable;

        protected void InitializeBase(SizeData data)    
        {
            Size size = new Size(data.MinSize);
            
            _body = GetComponent<Body>();
            _body.Initialize(size);
            
            _absorbable = GetComponent<Absorbable>();
            _absorbable.Initialize(size);
            
            _absorbableOverTime = GetComponent<AbsorbableOverTime>();
            _absorbableOverTime.Initialize(_absorbable, size, data.DelayInDecrease);
            
            _absorber = GetComponent<Absorber>();
            _absorber.Initialize(size);
        }
        
        public void SetName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Display name cannot be empty.", nameof(newName));

            Name = newName.Trim();
        }
    }
}