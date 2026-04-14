using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Body))]
    [RequireComponent(typeof(Absorbable), typeof(AbsorbableOverTime))]
    [RequireComponent(typeof(Absorber),typeof(AbsorberOverTime), typeof(AbsorbableDetector))]
    public class Slime : MonoBehaviour, ISpawnable<Slime>
    {
        private Body _body;
        
        private Absorbable _absorbable;
        private AbsorbableOverTime _absorbableOverTime;
        
        private Absorber _absorber;
        private AbsorberOverTime _absorberOverTime;
        private AbsorbableDetector _absorbableDetector;

        public event Action<Slime> ReadyToSpawn;

        private void Awake()
        {
            Size size = new Size(Random.Range(1, 5));
            
            _body = GetComponent<Body>();
            _body.Initialize(size);
            
            _absorbable = GetComponent<Absorbable>();
            _absorbable.Initialize(EntityType.Slime, _body, size);
            
            _absorbableOverTime = GetComponent<AbsorbableOverTime>();
            _absorbableOverTime.Initialize(_absorbable, size, 5);
            
            _absorber = GetComponent<Absorber>();
            SlimeAbsorptionPolicy absorptionPolicy = new SlimeAbsorptionPolicy();
            _absorber.Initialize(EntityType.Slime, absorptionPolicy);

            _absorberOverTime = GetComponent<AbsorberOverTime>();
            _absorberOverTime.Initialize(_absorber, 3f);
            
            _absorbableDetector = GetComponent<AbsorbableDetector>();
        }

        private void OnEnable()
        {
            _absorbableOverTime.TurnOn();
            
            _absorbable.Absorbed += OnAbsorbed;
            _absorbableDetector.Detected += OnDetected;
            _absorbableDetector.Lost += OnLost;
        }

        private void OnDisable()
        {
            _absorbableOverTime.TurnOff();
            _absorberOverTime.StopAllAbsorb();
            
            _absorbable.Absorbed -= OnAbsorbed;
            _absorbableDetector.Detected -= OnDetected;
            _absorbableDetector.Lost -= OnLost;
        }

        private void OnDetected(IAbsorbable absorbable)
        {
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