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
                GameObject.FindWithTag("MainCamera").GetComponent<Transform>(); // grab player camera transform
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

            // let's assume that spheres are uniformly scaled so we can use the scale of any axis
            var targetScale = .5f * trans.localScale.x;
            // gets vector from player cam to the target sphere
            var pCamToTarget = pos - pCamPos;
            // gets a perpendicular direction by crossing it with a downwards pointing vector and normalizing it
            var parallelDir = Vector3.Cross(pCamToTarget, -Vector3.up).normalized;
            // applies scale to it
            var parallelScaled = parallelDir * targetScale;
            // moves parallel vector to the target sphere position touching it's surface on the right relative to the player cam;
            var targetRight = parallelScaled + pos;
            // gets vector from player cam to targetRight
            var pToRight = (targetRight - pCamPos);
            // forward direction of player cam
            var camFaceDir = _playerCameraTransform.forward;

            var dotTgtFace = Vector3.Dot(pCamToTarget, camFaceDir * pToRight.magnitude);
            var dotTgtEdge = Vector3.Dot(pCamToTarget, pToRight);
            // if dot product of dotTgtFace is larger than dotTgtEdge than player is looking at the sphere
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