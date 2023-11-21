using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MdAddForce : MonoBehaviour
{

    public GameObject bullet;
    public Transform bulletPos;


    [SerializeField] 
    private int _monsterIndex;
    private float _monsterMoveSpeed;
    private float _curHp;
    private float _curStr;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private float _randomY;
    private bool _checkStop = false;
    private bool _isDie = false;
    public GameObject _exePrefab;

    private int _playerIndex;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _monsterMoveSpeed = DataManager.instance.monsterDataList[_monsterIndex].speed;
        _curStr = DataManager.instance.monsterDataList[_monsterIndex].hp;
        _curHp = DataManager.instance.monsterDataList[_monsterIndex].hp;
        _animator = GetComponent<Animator>();
        _playerIndex = PlayerPrefs.GetInt("PlayerIndex");

        Invoke(nameof(bulletSpawn), 2f);
    }

    // Update is called once per frame
    void Update()
    {
        _randomY = Random.Range(2, 5);

        if (_checkStop == false)
        {
            transform.position += new Vector3(0, (-1) * Time.deltaTime * _monsterMoveSpeed * 2f, 0);
        }
        else
        {
             if(_isDie) CancelInvoke();
        }

        if (transform.position.y <= _randomY)
        {
            _rigidbody.velocity = Vector2.zero;
            _checkStop = true;
        }

        if (DataManager.instance.playData.gameover)
        {
            Destroy(gameObject);
        }
    }

    void bulletSpawn()
    {
        // bullet ���� �� ��ġ
        GameObject newobject = Instantiate(bullet, bulletPos.position, bullet.transform.rotation);

        float _shootTime = Random.Range(0.1f, 2f);
        Invoke(nameof(bulletSpawn), _shootTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerBullet"))
        {
            Attacked();
        }
    }
    //ü�� ����, �ǰ� �ִ� ����
    private void Attacked()
    {
        if (_monsterIndex < 2)
        {
            transform.position += new Vector3(0, 0.2f, 0);
        }

        //ü�°���
        _curHp -= DataManager.instance.playerStatDataList[_playerIndex].str;

        if (_curHp > 0)
        {
            //�ǰ� �ִ�
            _animator.SetTrigger("Attacked");
        }
        else
        {
            _isDie = true;
            gameObject.tag = "Destroy";
            DataManager.instance.playData.score += 100 + _monsterIndex * 50;

            _animator.SetTrigger("Die");
            Instantiate(_exePrefab, transform.position, Quaternion.identity);
            //�ǰݾִϸ��̼� Ŭ�� ���� �ð��� ���� �� ������Ʈ �ı�
            Destroy(gameObject, 0.5f);
        }
    }
}                                   