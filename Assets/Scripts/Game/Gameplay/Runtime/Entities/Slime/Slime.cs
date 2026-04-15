using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Body))]
    [RequireComponent(typeof(Absorbable), typeof(AbsorbableOverTime))]
    [RequireComponent(typeof(Absorber),typeof(AbsorberOverTime), typeof(AbsorbableDetector))]
    public class Slime : MonoBehaviour, ISpawnable<Slime>
    {
        private SlimeConfig _config;
        private Size _size;
        private Body _body;
        
        private Absorbable _absorbable;
        private AbsorbableOverTime _absorbableOverTime;
        
        private Absorber _absorber;
        private AbsorberOverTime _absorberOverTime;
        private AbsorbableDetector _absorbableDetector;

        public event Action<Slime> ReadyToSpawn;
        
        [Inject]
        private void Construct(SlimeConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        private void Awake()
        {
            _size = new Size(_config.Size.MinSize);
            
            _body = GetComponent<Body>();
            _body.Initialize(_size);
            
            _absorbable = GetComponent<Absorbable>();
            _absorbable.Initialize(EntityType.Slime, _body, _size);
            
            _absorbableOverTime = GetComponent<AbsorbableOverTime>();
            _absorbableOverTime.Initialize(_absorbable, _size, _config.Size.DelayInDecrease);
            
            _absorber = GetComponent<Absorber>();
            SlimeAbsorptionPolicy absorptionPolicy = new SlimeAbsorptionPolicy();
            _absorber.Initialize(EntityType.Slime, absorptionPolicy);

            _absorberOverTime = GetComponent<AbsorberOverTime>();
            _absorberOverTime.Initialize(_absorber, _config.DelayInAbsorb);
            
            _absorbableDetector = GetComponent<AbsorbableDetector>();
        }

        private void OnEnable()
        {
            _size.SetCurrent(Random.Range(_config.Size.MinSize, _config.Size.MaxSize + 1));
            
            _absorbable.Absorbed += OnAbsorbed;
            _absorbableDetector.Detected += OnDetected;
            _absorbableDetector.Lost += OnLost;
        }

        private void OnDisable()
        {
            _absorberOverTime.StopAllAbsorb();
            
            _absorbable.Absorbed -= OnAbsorbed;
            _absorbableDetector.Detected -= OnDetected;
            _absorbableDetector.Lost -= OnLost;
        }

        private void OnDetected(IAbsorbable absorbable)
        {
            if (_absorber.CanAbsorb(absorbable))
                _absorberOverTime.StartAbsorbGradually(absorbable);
        }

        private void OnLost(IAbsorbable absorbable)
        {
            _absorberOverTime.StopAbsorbGradually(absorbable);
        }

        private void OnAbsorbed(IAbsorbable absorbable)
        {
            ReadyToSpawn?.Invoke(this);
        }
    }
}