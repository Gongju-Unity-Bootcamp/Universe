using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BulletDestroy(2f);
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void BulletDestroy(float time)
    {
        Destroy(gameObject, time);
    }
}
