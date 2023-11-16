using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaAddForce : MonoBehaviour
{
    public Rigidbody2D BasicMoster_Rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody2D에서 rigidbody를 가져옵니다.
        BasicMoster_Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        BasicMoster_Rigidbody.AddForce(Vector2.down * 1f, ForceMode2D.Force);

    }
}
