using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MdForce : MonoBehaviour
{
    public Rigidbody2D MdRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        MdRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MdRigidbody.AddForce(Vector2.down * 0.4f, ForceMode2D.Force);
    }
}
