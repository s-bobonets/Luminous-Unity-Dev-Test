// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The following class should move this Transform to the right until it reaches
/// (3, 0, 0) where it resets to (-3, 0, 0) the moves to the right again.
/// It should move at 1m per second, but does not.
/// Please correct the code to move smoothly at 1m/s.
/// </summary>
public class Move : MonoBehaviour
{
    // I assume we want to start with x at -3
    private float _delta = -3f;

    private void Update()
    {
        // _delta += Time.fixedDeltaTime;
        // fixedDeltaTime uses physics tick(FixedUpdate) and shouldn't be used in Update function
        _delta += Time.deltaTime;
        transform.position = new Vector3(_delta, 0f, 0f);

        if (_delta > 3f)
            _delta = -3f;
    }
}