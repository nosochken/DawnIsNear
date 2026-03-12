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
        
        protected void InitializeBase(SizeData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            
            MinSize = data.MinSize;
            Size = MinSize;
            //_maxSize = config.Size.MaxSize;
            
            _absorbOverTimeInterval = data.DelayInDecrease;
        }

        private void Awake()
        {
            _absorbableDetector = GetComponent<AbsorbableDetector>();
            GetComponents();
        }
        
        protected virtual void GetComponents() { }

        private void OnEnable()
        {
            if (MinSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(MinSize));
            
            _absorbableDetector.Detected +=  OnDetected;
            Subscribe();
            
            Size = MinSize;
            SizeChanged?.Invoke(Size);
            
            if (_beAbsorbedCoroutine == null) 
                _beAbsorbedCoroutine = StartCoroutine(BeAbsorbedOverTime());
        }
        
        protected virtual void Subscribe() { }

        private void OnDisable()
        {
            _absorbableDetector.Detected -=  OnDetected;
            
            Unsubscribe();
            
            if (_beAbsorbedCoroutine != null)
            {
                StopCoroutine(_beAbsorbedCoroutine);
                _beAbsorbedCoroutine = null;
            }
        }
        
        protected virtual void Unsubscribe() { }

        public void BeAbsorbed()
        {
            SizeChanged?.Invoke(Size);
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
                {
                    BeAbsorbed();
                    _beAbsorbedCoroutine = null;
                    yield break;
                }
            
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