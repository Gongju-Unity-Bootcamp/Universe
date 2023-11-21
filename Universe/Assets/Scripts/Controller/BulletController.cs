using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Animator _bulletAnimator;
    private Rigidbody2D _bulletRigidBody;
    public GameObject _bulletPos;
    private float _bulletSpeed;
    private bool _isCollision=false;

    private void Start()
    {
        _bulletAnimator = GetComponent<Animator>();
        _bulletRigidBody = GetComponent<Rigidbody2D>();
    }

    //몬스터와 충돌하면 tag 변경
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            _isCollision= true;
            _bulletRigidBody.velocity = Vector3.zero;
            _bulletAnimator.SetTrigger("Collision");

            float _animationTime = _bulletAnimator.GetCurrentAnimatorStateInfo(0).length;
            Destroy (gameObject, _animationTime);
        }
    }
    private void Update()
    {
        if (!_isCollision)
        {
            Vector3 _angle = new Vector3(_bulletPos.transform.position.x - transform.position.x, _bulletPos.transform.position.y - transform.position.y,0);

            _bulletSpeed = DataManager.instance.playerStatDataList[PlayerPrefs.GetInt("PlayerIndex")].attackSpeed * 0.05f;
            transform.position += _angle.normalized *Time.deltaTime * _bulletSpeed *80 ;
            
            //_bulletRigidBody.AddForce(_angle.normalized * _bulletSpeed, ForceMode2D.Impulse);
        }

        if(transform.position.y >= 2.8f)
        {
            Destroy(gameObject);
        }
    }

}

