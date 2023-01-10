using UnityEngine;

namespace App
{
    // input manager singleton
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        private PlayerControls _controls;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;

            _controls = new PlayerControls();
        }

        private void OnEnable() => _controls.Enable();


        private void OnDisable() => _controls.Disable();


        public Vector2 GetMoveInput() => _controls.Gameplay.Move.ReadValue<Vector2>();
        public Vector2 GetLookInput() => _controls.Gameplay.Look.ReadValue<Vector2>();
    }
}