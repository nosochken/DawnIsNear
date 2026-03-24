using System;
using UnityEngine;
using Game.Input;
using Zenject;

namespace Game.Gameplay
{
    [RequireComponent(typeof(PlayerInputController), typeof(ScreenToWorld), typeof(PlayerMovement))]

    public class Player : Unit
    {
        private PlayerInputController _input;
        private ScreenToWorld _screenToWorld;
        private PlayerMovement _movement;
        
        [Inject]
        private void Construct(PlayerConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            
            InitializeBase(config.Size);
        }
        
        protected override void GetComponents()
        {
            base.GetComponents();
        
            _input = GetComponent<PlayerInputController>();
            _screenToWorld = GetComponent<ScreenToWorld>();
            _movement = GetComponent<PlayerMovement>();
        }

        private void FixedUpdate()
        {
            Vector2 targetPosition = _screenToWorld.Convert(_input.PointerScreenPosition);
            _movement.MoveTo(targetPosition, Size.Current);
        }
    }
}