using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input
{
    [RequireComponent((typeof(PlayerInput)))]
    public class PlayerInputController : MonoBehaviour
    {
        private PlayerInput _playerInput;
   
        public Vector2 PointerScreenPosition {get; private set;}
        public bool IsPointerPressed { get; private set; }

        private void Awake()
        {
            _playerInput = new PlayerInput();
        }

        private void OnEnable() 
        {
            _playerInput.Enable();
      
            _playerInput.Player.Move.performed += OnPointerPosition;
            _playerInput.Player.IsMove.performed += OnPointerPress;
        }

        private void OnDisable()
        {
            _playerInput.Disable();
      
            _playerInput.Player.Move.performed -= OnPointerPosition;
            _playerInput.Player.IsMove.performed -= OnPointerPress;
        }

        private void OnPointerPosition(InputAction.CallbackContext context)
        {
            PointerScreenPosition = context.ReadValue<Vector2>();
        }

        private void OnPointerPress(InputAction.CallbackContext context)
        {
            IsPointerPressed = context.ReadValueAsButton();
        }
    }
}