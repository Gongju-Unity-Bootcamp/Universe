using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterBulletController : MonoBehaviour
{
    private Rigidbody2D bulletRigidbody;
    public float speed = 0.7f;
    public float lifeTime = 3f;


    // Start is called before the first frame update
    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        // ¾Æ·¡·Î ¶³¾îÁö´Â ÃÑ¾Ë
        bulletRigidbody.AddForce(Vector2.down * speed, ForceMode2D.Impulse);
        Destroy(bulletRigidbody, lifeTime);
    }
}