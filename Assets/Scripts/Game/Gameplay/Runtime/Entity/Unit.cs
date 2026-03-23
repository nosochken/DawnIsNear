using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(AbsorbableDetector))]
    public abstract class Unit : MonoBehaviour, ITargetable
    {
        private Size _size;
        private Absorbable _absorbable;
        private Absorber _absorber;
        private AbsorbableDetector _absorbableDetector;
        private Coroutine _beAbsorbedCoroutine;
        
        public Size Size => _size;
        public Absorbable Absorbable => _absorbable;
        internal AbsorbableDetector AbsorbableDetector => _absorbableDetector;
        
        public string Name { get; private set; }
        public bool IsActive => isActiveAndEnabled;
        public int CurrentSize => _size.Current;
        public Vector2 CurrentPosition => transform.position;

        protected void Initialize(SizeData data)
        {
            _size = new Size(data.MinSize);
            _absorbable = new Absorbable(this, data.DelayInDecrease);
            _absorber = new Absorber(this);
        }

        private void Awake()
        {
            _absorbableDetector = GetComponent<AbsorbableDetector>();
            GetComponents();
        }

        protected virtual void GetComponents() { }

        private void OnEnable()
        {
            _size.SetMin();
            _absorber.Subscribe();
            
            if (_beAbsorbedCoroutine == null) 
                _beAbsorbedCoroutine = StartCoroutine(_absorbable.BeAbsorbedOverTime());

            Subscribe();
        }
        
        protected virtual void Subscribe() { }

        private void OnDisable()
        {
            _absorber.Unsubscribe();
            
            if (_beAbsorbedCoroutine != null)
            {
                StopCoroutine(_beAbsorbedCoroutine);
                _beAbsorbedCoroutine = null;
            }
            
            Unsubscribe();
        }
        
        protected virtual void Unsubscribe() { }

        public void SetName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Display name cannot be empty.", nameof(newName));

            Name = newName.Trim();
        }
    }
}
