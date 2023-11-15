using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(bulletSpawn), 0.2f, 2f);
    }

    // Update is called once per frame
    void bulletSpawn()
    {
        GameObject newobject = Instantiate(bullet, transform.position, transform.rotation);
        Destroy(newobject, 5f);
    }
}
