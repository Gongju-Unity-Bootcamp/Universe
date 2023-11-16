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

    //이동속도 상수
    const float _hSpeed =1.75f;
    const float _vSpeed = 1.2f;

    //캐릭터 이동속도
    float _horizontalSpeed;
    float _verticalSpeed;
    float _bulletSpeed;

    //캐릭터가 움직일 수 있는 상황인지 확인
    private bool _canMove = false;

    //캐릭터가 움직일 수 있는 범위 지정
    readonly Vector2 _minMovePos = new Vector2(-2.3f, -4.35f);
    readonly Vector2 _maxMovePos = new Vector2(2.3f, 3.5f);


    public GameObject _bulletPrefab;
    public Transform _bulletPos;

    private int _shootSkill;
    private Animator _animator;

    //충돌 쿨타임
    public GameObject _collisionEffect;
    private const float _collisionTime =2f;
    private float _currCollistionTime =0f;
    private bool _isCollision = false;

    //총발사 간격
    private float _bulletDelay;
    private float _currBulletTime =0;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _playerIndex = PlayerPrefs.GetInt("PlayerIndex");
        _collisionEffect.SetActive(false);

        //캐릭터 초기 이동 끝난 후 움직일 수 있도록
        Invoke(nameof(initMove), 5f);
    }

    // Update is called once per frame
    void Update()
    {
        //키입력정보
        _inputHorizontal = Input.GetAxisRaw("Horizontal");
        _inputVertical = Input.GetAxisRaw("Vertical");

        //스피드 데이터 불러오기
        _horizontalSpeed = DataManager.instance.playerStatDataList[_playerIndex].speed;
        _verticalSpeed = DataManager.instance.playerStatDataList[_playerIndex].speed;
        _bulletSpeed = DataManager.instance.playerStatDataList[_playerIndex].attackSpeed *200;
        _bulletDelay = 1 -(DataManager.instance.playerStatDataList[_playerIndex].attackSpeed * 0.1f);

        if (_canMove)
        {
            Move();
            Shoot();
        }

        //몬스터와 충돌 중인지 확인
        if(_isCollision)
        {
            if(_currCollistionTime >= _collisionTime)
            {
                //충돌 무적시간이 지났을 경우 재충돌
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
            //경험치 추가 or 레벨업
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

    //플레이어가 피격상태가 변할경우 호출 
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

    //애니메이터 파라미터 수정
    private void SetAniParameters(string ParametersName, int integerParameter = 10 )
    {
        if (integerParameter != 10)
            _animator.SetInteger(ParametersName, integerParameter);
        else
            _animator.SetTrigger(ParametersName);
    }

    //캐릭터 이동
    private void Move()
    {
        transform.position += new Vector3(_inputHorizontal * Time.deltaTime * _horizontalSpeed * _hSpeed, _inputVertical * Time.deltaTime * _verticalSpeed * _vSpeed, 0);
        //transform.position = Vector2.Lerp(_minMovePos, _maxMovePos, 1f);
        //이동 중이라면 걷기 애니메이션 출력
        if (_inputHorizontal != 0 || _inputVertical != 0)
        {
            SetAniParameters("Walking", _inputHorizontal != 0 ? (int)_inputVertical : (int)_inputHorizontal);
            //좌우이동 시 스프라이트 flipX를 이용하여 뒤집기
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

    //총알 발사
    private void Shoot()
    {
        _currBulletTime += Time.deltaTime;
        //총알 발사
        if (Input.GetKeyDown(KeyCode.Space)&& _currBulletTime >= _bulletDelay)
        {
            //for작성 총알 갯수에 따라 복제하는 갯수 각도 다르게
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
            //for작성 총알 갯수에 따라 복제하는 갯수 각도 다르게
            _currBulletTime = 0;

        }
    }
    //초기 이동이 끝난 후 이동가능하도록 _canMove 값 수정
    private void initMove()
    {
        _canMove = true;
    }

}
