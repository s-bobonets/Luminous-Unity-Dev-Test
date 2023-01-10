using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App

{
    public class ColorChanger : MonoBehaviour
    {
        // OnColorSwitch action can be called from elsewhere to change the sphere color 
        public Action OnColorSwitch;

        private Color _originalColor;
        private MeshRenderer _meshRenderer;

        private void Awake() =>
            OnColorSwitch = SwitchColor; // let's store out SwitchChanger method in OnColorSwitch action

        private void Start()
        {
            // cache in mesh rendered and original color in the fields
            _meshRenderer = GetComponent<MeshRenderer>();
            _originalColor = _meshRenderer.material.color;
        }

        private void SwitchColor()
        {
            // if color equals original randomize it  - otherwise apply original back.
            var hasOriginalColor = _meshRenderer.material.color == _originalColor;
            if (hasOriginalColor)
                // randomize to a decently bright color - let's stay away from red cause
                // we don't want the new color to be too close in hue to the original
                _meshRenderer.material.color = Random.ColorHSV(.1f, .9f, 1f, 1f, 0.8f, 1f);
            else
                _meshRenderer.material.color = _originalColor;
        }
    }
}