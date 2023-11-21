using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MdAddForce : MonoBehaviour
{
    private Rigidbody2D MdRigidbody;
    //const float posY = 4;
    bool checkStop = false;
    private float randomY;
    private float randomx;
    public GameObject bullet;
    public Transform bulletPos;

    // Start is called before the first frame update
    void Start()
    {
        MdRigidbody = GetComponent<Rigidbody2D>();
        InvokeRepeating(nameof(bulletSpawn), 0.1f, 2f);

    }

    // Update is called once per frame
    void Update()
    {
        randomY = Random.Range(2.2f, 4.4f);

        if (checkStop == false)
        {
            MdRigidbody.AddForce(Vector2.down * 2f, ForceMode2D.Force); ;
        }
        if (transform.position.y <= randomY)
        {
            MdRigidbody.velocity = Vector2.zero;
            checkStop = true;
        }
    }

    void bulletSpawn()
    {
        if (checkStop == true)
        {
            // bullet 복제 및 위치
            GameObject newobject = Instantiate(bullet, bulletPos.position, bullet.transform.rotation);
            Destroy(newobject, 5f);
        }
    }
}