using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMove : MonoBehaviour
{
    private int _playerIndex;

    private float _inputHorizontal;
    private float _inputVertical;

    //�̵��ӵ� ���
    const float _hSpeed =1.75f;
    const float _vSpeed = 1.2f;

    //ĳ���� �̵��ӵ�
    float _horizontalSpeed;
    float _verticalSpeed;
    float _bulletSpeed;

    //ĳ���Ͱ� ������ �� �ִ� ��Ȳ���� Ȯ��
    private bool _canMove = false;

    //ĳ���Ͱ� ������ �� �ִ� ���� ����
    readonly Vector2 _minMovePos = new Vector2(-2.3f, -4.35f);
    readonly Vector2 _maxMovePos = new Vector2(2.3f, 3.5f);


    public GameObject _bulletPrefab;
    public Transform _bulletPos;

    private int _shootSkill;
    private Animator _animator;

    //�浹 ��Ÿ��
    public GameObject _collisionEffect;
    private const float _collisionTime =2f;
    private float _currCollistionTime =0f;
    private bool _isCollision = false;

    //�ѹ߻� ����
    private float _bulletDelay;
    private float _currBulletTime =0;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _playerIndex = PlayerPrefs.GetInt("PlayerIndex");
        _collisionEffect.SetActive(false);

        //ĳ���� �ʱ� �̵� ���� �� ������ �� �ֵ���
        Invoke(nameof(initMove), 5f);
    }

    // Update is called once per frame
    void Update()
    {
        //Ű�Է�����
        _inputHorizontal = Input.GetAxisRaw("Horizontal");
        _inputVertical = Input.GetAxisRaw("Vertical");

        //���ǵ� ������ �ҷ�����
        _horizontalSpeed = DataManager.instance.playerStatDataList[_playerIndex].speed;
        _verticalSpeed = DataManager.instance.playerStatDataList[_playerIndex].speed;
        _bulletSpeed = DataManager.instance.playerStatDataList[_playerIndex].attackSpeed *200;
        _bulletDelay = 1 -(DataManager.instance.playerStatDataList[_playerIndex].attackSpeed * 0.1f);

        if (_canMove)
        {
            Move();
            Shoot();
        }

        //���Ϳ� �浹 ������ Ȯ��
        if(_isCollision)
        {
            if(_currCollistionTime >= _collisionTime)
            {
                //�浹 �����ð��� ������ ��� ���浹
                _currCollistionTime = 0f;
                PlayerAttacked(true);
            }
            else
            {
                _currCollistionTime += Time.deltaTime;
            }

        }
    }

    #region trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            PlayerAttacked(true);
        }
        if(collision.CompareTag("Exe"))
        {
            //����ġ �߰� or ������
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            PlayerAttacked(false);
        }
    }
    #endregion

    //�÷��̾ �ǰݻ��°� ���Ұ�� ȣ�� 
    private void PlayerAttacked(bool isCollision)
    {
        _isCollision = isCollision;
        if (isCollision)
        {
            _collisionEffect.SetActive(true);
            SetAniParameters("Attacked");

        }
        else
        {
            _currCollistionTime = 0f;
        }
    }

    //�ִϸ����� �Ķ���� ����
    private void SetAniParameters(string ParametersName, int integerParameter = 10 )
    {
        if (integerParameter != 10)
            _animator.SetInteger(ParametersName, integerParameter);
        else
            _animator.SetTrigger(ParametersName);
    }

    //ĳ���� �̵�
    private void Move()
    {
        transform.position += new Vector3(_inputHorizontal * Time.deltaTime * _horizontalSpeed * _hSpeed, _inputVertical * Time.deltaTime * _verticalSpeed * _vSpeed, 0);
        //transform.position = Vector2.Lerp(_minMovePos, _maxMovePos, 1f);
        //�̵� ���̶�� �ȱ� �ִϸ��̼� ���
        if (_inputHorizontal != 0 || _inputVertical != 0)
        {
            SetAniParameters("Walking", _inputHorizontal != 0 ? (int)_inputVertical : (int)_inputHorizontal);
            //�¿��̵� �� ��������Ʈ flipX�� �̿��Ͽ� ������
            if (_inputHorizontal >= 1)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (_inputHorizontal <= -1)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else
        {
            SetAniParameters("Walking", 0);
        }
    }

    //�Ѿ� �߻�
    private void Shoot()
    {
        _currBulletTime += Time.deltaTime;
        //�Ѿ� �߻�
        if (Input.GetKeyDown(KeyCode.Space)&& _currBulletTime >= _bulletDelay)
        {
            //for�ۼ� �Ѿ� ������ ���� �����ϴ� ���� ���� �ٸ���
            GameObject _bulletObject = Instantiate(_bulletPrefab, _bulletPos.position, _bulletPrefab.transform.rotation);
            _bulletObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * _bulletSpeed);
            _currBulletTime = 0;

        }
    }
    
    private void ShootTest()
    {
        _currBulletTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && _currBulletTime >= _bulletDelay)
        {
            for(int i = 0; i< 3; i++)
            {
                GameObject _bulletObject = Instantiate(_bulletPrefab, _bulletPos.position, _bulletPrefab.transform.rotation);
                _bulletObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * _bulletSpeed);
            }
            //for�ۼ� �Ѿ� ������ ���� �����ϴ� ���� ���� �ٸ���
            _currBulletTime = 0;

        }
    }
    //�ʱ� �̵��� ���� �� �̵������ϵ��� _canMove �� ����
    private void initMove()
    {
        _canMove = true;
    }

}
