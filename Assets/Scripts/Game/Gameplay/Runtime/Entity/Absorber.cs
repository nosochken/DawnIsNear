using System;
using System.Collections;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(AbsorbableDetector))]
    public abstract class Absorber : MonoBehaviour, IAbsorber, IAbsorbable
    {
        //private  int _maxSize;
        private AbsorbableDetector _absorbableDetector;
        
        private float _absorbOverTimeInterval;
        private Coroutine _beAbsorbedCoroutine;
        
        public event Action<int> SizeChanged;
        public event Action<IAbsorbable> Absorbed;
        
        public int MinSize { get; private set; }
        public int Size { get; private set; }
        
        public bool IsActive => isActiveAndEnabled;
        public Vector2 CurrentPosition => transform.position;
        
        public void Construct(EntityConfigs config, PlayField playField)
        {
            MinSize = config.Size.MinSize;
            //_maxSize = config.Size.MaxSize;
            Size = config.Size.MinSize;
            SizeChanged?.Invoke(Size);
            
            _absorbOverTimeInterval = config.Size.DelayInDecrease;
            
            ExtendConstructor(config, playField);
            
            _beAbsorbedCoroutine = StartCoroutine(BeAbsorbedOverTime());
        }

        protected abstract void ExtendConstructor(EntityConfigs config, PlayField playField);

        private void Awake()
        {
            _absorbableDetector = GetComponent<AbsorbableDetector>();
            GetComponents();
        }
        
        protected virtual void GetComponents() { }

        private void OnEnable()
        {
            _absorbableDetector.Detected +=  OnDetected;
            Subscribe();
        }
        
        protected virtual void Subscribe() { }

        private void OnDisable()
        {
            _absorbableDetector.Detected -=  OnDetected;
            
            Unsubscribe();
            
            if(_beAbsorbedCoroutine != null)
                StopCoroutine(_beAbsorbedCoroutine);
        }
        
        protected virtual void Unsubscribe() { }

        public void BeAbsorbed()
        {
            Absorbed?.Invoke(this);
        }
        
        private IEnumerator BeAbsorbedOverTime()
        {
            WaitForSeconds wait = new WaitForSeconds(_absorbOverTimeInterval);
            
            while (isActiveAndEnabled)
            {
                yield return wait;

                Size--;

                if (Size <= 0)
                    BeAbsorbed();
                else
                    SizeChanged?.Invoke(Size);
            }
        }
        
        private void Absorb(IAbsorbable absorbable)
        {
            Size += absorbable.Size;
            SizeChanged?.Invoke(Size);
            
            absorbable.BeAbsorbed();
        }
    
        private void OnDetected(IAbsorbable absorbable)
        {
            if (absorbable.Size <= Size)
                Absorb(absorbable);
        }
    }
}