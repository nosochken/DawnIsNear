using System;
using UnityEngine;
using Game.Input;
using Zenject;

namespace Game.Gameplay
{
    [RequireComponent(typeof(PlayerInputController), typeof(PlayerMovement))]

    public class Player : Unit
    {
        private PlayerInputController _input;
        private PlayerMovement _movement;
        
        [Inject]
        private void Construct(PlayerConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            
            ConstructBase(EntityType.Player, config.Size);
        }
        
        protected override void Awake()
        {
            base.Awake();
            _input = GetComponent<PlayerInputController>();
            _movement = GetComponent<PlayerMovement>();
        }

        private void FixedUpdate()
        {
            _movement.MoveTo(_input.PointerScreenPosition, Body.Size.Current);
        }
    }
}