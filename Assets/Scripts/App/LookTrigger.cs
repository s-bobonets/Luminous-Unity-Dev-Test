using UnityEngine;

namespace App
{
    public class LookTrigger : MonoBehaviour
    {
        [SerializeField] private float _triggerLookSensitivity = .9f; // trigger sensitivity value
        [SerializeField] private float _rangeOfInteraction = 4f; // distance at which triggers became interactable

        private ColorChanger _colorChanger;
        private Transform _playerCameraTransform;

        private bool _canInteract; // true when the sphere is triggered
        private bool _switched; // controls repetitive invocation of ColorChanger

        private void Start()
        {
            _playerCameraTransform =
                GameObject.FindWithTag("MainCamera").GetComponent<Transform>(); // grab player camera
            _colorChanger = GetComponent<ColorChanger>(); // grab ColorChanger script
        }

        private void Update()
        {
            Trigger();
        }

        private void Trigger()
        {
            // get vector from player to the target sphere
            var playerToTarget = transform.position - _playerCameraTransform.position;
            // make it a direction by normalizing it
            var playerToTargetDir = playerToTarget.normalized;
            // get dot product of playerToTargetDir and forward camera vector direction 
            var lookAmount = Vector3.Dot(playerToTargetDir, _playerCameraTransform.forward);

            // figure if it's close enough to be able to trigger the sphere
            var isClose = playerToTarget.magnitude < _rangeOfInteraction;
            // figure if player sufficiently looks in the direction of the sphere
            var canSee = lookAmount > _triggerLookSensitivity;
            _canInteract = isClose && canSee;

            // turned towards target, invoking OnColorSwitch once
            if (_canInteract && !_switched)
            {
                _colorChanger.OnColorSwitch?.Invoke();
                _switched = true;
            }
            // turning away, invoking OnColorSwitch once
            else if (!_canInteract && _switched)
            {
                _colorChanger.OnColorSwitch?.Invoke();
                _switched = false;
            }
        }
    }
}