using UnityEngine;
using Game.Input;

namespace Game.Gameplay
{
    [RequireComponent(typeof(PlayerInputController), typeof(ScreenToWorld), typeof(Movement))]

    public class Player : Absorber
    {
        private PlayerInputController _input;
        private ScreenToWorld _screenToWorld;
        private Movement _movement;
    
        protected override void ExtendConstructor(EntityConfigs config)
        {
            _movement.Construct(config.Movement, Size);
        }
        
        protected override void GetComponents()
        {
            base.GetComponents();
        
            _input = GetComponent<PlayerInputController>();
            _screenToWorld = GetComponent<ScreenToWorld>();
            _movement = GetComponent<Movement>();
        }

        private void FixedUpdate()
        {
            Vector2 targetPosition = _screenToWorld.Convert(_input.PointerScreenPosition);
            _movement.MoveTo(targetPosition, Size);
        }
    }
}