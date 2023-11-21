using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    const float _destroyDefaultTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        BulletDestroy(_destroyDefaultTime);
    }

    public void BulletDestroy(float time)
    {
        Destroy(gameObject, time);
    }
}
