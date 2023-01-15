using UnityEngine;

namespace App
{
    // I would normally use CharacterController component for this
    // but since we don't requite gravity or jumping mechanic let's simplify
    // let's not use physics altogether for this project 
    public class PlayerController : MonoBehaviour
    {
        // player controller options:
        [SerializeField] [Range(0f, 1f)] private float _mouseSensitivity = .1f; // set mouse sensitivity
        [SerializeField] [Range(1f, 12f)] private float _playerSpeed = 1; //set player movement speed
        [SerializeField] private bool _lockCursor = true;

        private InputManager _inputManager;
        private Camera _playerCamera;
        private float _camPitch;

        private void Start()

        {
            // grab components
            _inputManager = InputManager.Instance;
            _playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

            LockCursor();
        }

        private void LockCursor()
        {
            // lock cursor if needs be
            if (!_lockCursor) return;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            Look();
            Move();
        }

        // implements mouse look
        private void Look()
        {
            var lookInput = _inputManager.GetLookInput(); // get look input 
            var lookAmount = lookInput * _mouseSensitivity; // set look sensitivity

            _camPitch -= lookAmount.y; // set camera pitch value
            _camPitch = Mathf.Clamp(_camPitch, -90f, 90f); // clamp camera pitch value

            _playerCamera.transform.localEulerAngles = Vector3.right * _camPitch; // apply pitch to the camera

            transform.Rotate(transform.up *
                             lookAmount.x); // rotate player body around up axis (player camera inherits this rotation)
        }

        // implements player movement
        private void Move()
        {
            var moveInput = _inputManager.GetMoveInput(); // get movement input 
            var trans = transform; // caching transformation to avoid repeated property access
            // remaps input to players lateral plane
            var moveAmount = trans.forward * moveInput.y + trans.right * moveInput.x;
            //apply moveAmount
            trans.Translate(moveAmount * (_playerSpeed * Time.deltaTime), Space.World);
        }
    }
}