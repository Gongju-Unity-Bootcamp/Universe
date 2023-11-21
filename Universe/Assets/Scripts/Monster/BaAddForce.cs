using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaAddForce : MonoBehaviour
{
    [SerializeField]
    private int _monsterIndex;
    private float _monsterMoveSpeed;
    private float _curStr;
    private float _curHp;
    private Animator _animator;
    private bool _canMove=true;

    public GameObject _exePrefab;
    private int _playerIndex;
    private void Start()
    {
        _monsterMoveSpeed = DataManager.instance.monsterDataList[_monsterIndex].speed;
        _curStr = DataManager.instance.monsterDataList[_monsterIndex].hp;
        _curHp = DataManager.instance.monsterDataList[_monsterIndex].hp;
        _animator = GetComponent<Animator>();
        _playerIndex = PlayerPrefs.GetInt("PlayerIndex");
    }
    // Update is called once per frame
    void Update()
    {
        if(_canMove)
            transform.position += new Vector3(0, (-1) * Time.deltaTime * _monsterMoveSpeed,0);
        //GetComponent<Rigidbody2D>().AddForce(Vector2.down * _monsterMoveSpeed, ForceMode2D.Force);
         
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            collision.gameObject.tag = "Destroy";
            Attacked();
        }
    }

    //체력 감소, 피격 애니 실행
    private void Attacked()
    {
        if (_monsterIndex < 2)
        {
            transform.position += new Vector3(0, 0.2f, 0);
        }
        //체력감소
        _curHp -= DataManager.instance.playerStatDataList[_playerIndex].str;

        if (_curHp > 0)
        {
            //피격 애니
            _animator.SetTrigger("Attacked");
            _canMove = false;
        }
        else
        {
            _canMove = true;
            gameObject.tag = "Destroy";
             
            _animator.SetTrigger("Die");

            Instantiate(_exePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject,0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _canMove = true;
    }
}
