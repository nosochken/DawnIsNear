using System;
using UnityEngine;
using Game.Input;

namespace Game.Gameplay
{
    [RequireComponent(typeof(PlayerInputController), typeof(ScreenToWorld), typeof(PlayerMovement))]

    public class Player : Absorber
    {
        private PlayerInputController _input;
        private ScreenToWorld _screenToWorld;
        private PlayerMovement _movement;
    
        protected override void ExtendConstructor(EntityConfigs config, PlayField playField)
        {
            if (config is not PlayerConfig playerConfig)
                throw new ArgumentException("Player requires PlayerConfig");
            
            _movement.Construct(config.MovementSpeed, Size, playField, playerConfig.DistanceData);
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
            _movement.MoveTo(targetPosition, Size);
        }
    }
}