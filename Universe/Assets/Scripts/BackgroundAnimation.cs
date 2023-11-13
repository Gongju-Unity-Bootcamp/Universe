using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnimation : MonoBehaviour
{
    readonly float _minYpos = -10f;
    readonly float _maxYpos = 9.9f;
    readonly float _backgroundMoveSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= _minYpos)
        {
            transform.position = new Vector3(0, _maxYpos, 0);
        }
        else
        {
            transform.position -= new Vector3(0, Time.deltaTime * _backgroundMoveSpeed, 0);
        }
    }
}
