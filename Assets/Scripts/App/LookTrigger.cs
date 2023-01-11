using UnityEngine;

namespace App
{
    public class LookTrigger : MonoBehaviour
    {
        private ColorChanger _colorChanger;
        private Transform _playerCameraTransform;

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
            // cache to avoid repeated properties access
            var trans = transform;
            var pos = trans.position;
            var pCamPos = _playerCameraTransform.position;

            // let's assume that spheres are uniformly scaled so we can use any scale axis
            var targetScale = .5f * trans.localScale.x;
            // gets vector from player cam to the target sphere
            var pCamToTarget = pos - pCamPos;
            // makes it a direction by normalizing it
            var pCamToTargetDir = pCamToTarget.normalized;
            // gets a perpendicular direction by crossing it with a downwards pointing vector and normalizing it
            var parallelDir = Vector3.Cross(pCamToTarget, -Vector3.up).normalized;
            // scales it with the sphere local scale
            var parallelScaled = parallelDir * targetScale;
            // moves parallel vector to the target sphere position touching it's surface on the right relative to the player cam;
            var targetRight = parallelScaled + pos;
            // gets a direction from player cam to targetRight
            var pToRight = (targetRight - pCamPos).normalized;
            // forward direction of player cam
            var camFaceDir = _playerCameraTransform.forward;

            var dotTgtEdge = Vector3.Dot(pCamToTargetDir, pToRight);
            var dotTgtFace = Vector3.Dot(pCamToTargetDir, camFaceDir);
            // if dot product of vector from player cam to camera forward facing direction is larger than
            // dot product of vector from player cam to the right edge of the target sphere, than player looks at the target
            var looksAt = dotTgtFace > dotTgtEdge;

            // turned towards target, invoking OnColorSwitch once
            if (looksAt && !_switched)
            {
                _colorChanger.OnColorSwitch?.Invoke();
                _switched = true;
            }
            // turning away, invoking OnColorSwitch once
            else if (!looksAt && _switched)
            {
                _colorChanger.OnColorSwitch?.Invoke();
                _switched = false;
            }
        }
    }
}