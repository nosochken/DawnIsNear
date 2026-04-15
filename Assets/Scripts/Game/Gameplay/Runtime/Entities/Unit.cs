using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Body))]
    [RequireComponent(typeof(Absorbable), typeof(AbsorbableOverTime), typeof(Absorber))]
    [RequireComponent(typeof(AbsorbableDetector))]
    public abstract class Unit : MonoBehaviour
    {
        private SizeData _sizeData;
        private EntityType _type;
        
        private Body _body;
        
        private Absorbable _absorbable;
        private AbsorbableOverTime _absorbableOverTime;
        
        private Absorber _absorber;
        private AbsorbableDetector _absorbableDetector;
        
        public string Name { get; private set; }
        
        public IBody Body => _body;
        public IAbsorbable Absorbable => _absorbable;
        public IAbsorber Absorber => _absorber;

        protected void ConstructBase(EntityType type, SizeData sizeData)    
        {
            _type = type;
            _sizeData = sizeData ?? throw new ArgumentNullException(nameof(sizeData));
        }

        protected virtual void Awake()
        {
            Size size = new Size(_sizeData.MinSize);
            
            _body = GetComponent<Body>();
            _body.Initialize(size);
            
            _absorbable = GetComponent<Absorbable>();
            _absorbable.Initialize(_type, _body, size);
            
            _absorbableOverTime = GetComponent<AbsorbableOverTime>();
            _absorbableOverTime.Initialize(_absorbable, size, _sizeData.DelayInDecrease);
            
            _absorber = GetComponent<Absorber>();
            UnitAbsorptionPolicy absorptionPolicy = new UnitAbsorptionPolicy();
            _absorber.Initialize(_type, absorptionPolicy, _body, size);
            
            _absorbableDetector = GetComponent<AbsorbableDetector>();
        }

        protected virtual void OnEnable()
        {
            _absorbableDetector.Detected += OnDetected;
            
            _absorbableOverTime.TurnOn();
        }

        protected virtual void OnDisable()
        {
            _absorbableDetector.Detected -= OnDetected;
            
            _absorbableOverTime.TurnOff();
        }
        
        public void SetName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Display name cannot be empty.", nameof(newName));

            Name = newName.Trim();
        }

        private void OnDetected(IAbsorbable absorbable)
        {
            if (_absorber.CanAbsorb(absorbable))
                _absorber.Absorb(absorbable);   
        }
    }
}