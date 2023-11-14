using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Animator _bulletAnimator;
    private Rigidbody2D _bulletRigidBody;

    private void Start()
    {
        _bulletAnimator = GetComponent<Animator>();
        _bulletRigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            _bulletRigidBody.velocity = Vector3.zero;
            _bulletAnimator.SetTrigger("Collision");
            
        }
    }

}

