using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletAddForce : MonoBehaviour
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

        // �Ʒ��� �������� �Ѿ�
        bulletRigidbody.AddForce(Vector2.down * speed, ForceMode2D.Impulse);
        Destroy(bulletRigidbody, lifeTime);


    }
}