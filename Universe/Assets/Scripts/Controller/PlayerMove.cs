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
    readonly Vector2 _maxMovePos = new Vector2(2.3f, 3f);


    //총알 프리팹, 발사 위치
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

    //총알개수
    [SerializeField]
    private int _bulletCnt;
    private float _bulletAngle =8f;

    public GameObject _levelUpEffect;

    private GameObject _collider;
    
    private UpgradeUIManager _upgradeUIManager;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _playerIndex = PlayerPrefs.GetInt("PlayerIndex");
        _collisionEffect.SetActive(false);
        _upgradeUIManager = FindAnyObjectByType<UpgradeUIManager>();
        //캐릭터 초기 이동 끝난 후 움직일 수 있도록
        Invoke(nameof(initMove), 5f);

    }

    // Update is called once per frame
    void Update()
    {
        //변경된 플레이어의 정보를 호출
        UpdateInfo();
        
        //이동, 공격
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
            _collider = collision.gameObject;
            PlayerAttacked(true);
        }
        if(collision.CompareTag("Exe"))
        {
            LevelManager();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            PlayerAttacked(false);
        }
        _collider = null;
    }
    #endregion

    //플레이어의 피격상태가 변할경우 호출 
    private void PlayerAttacked(bool isCollision)
    {
        _isCollision = isCollision;
        //충돌했을 경우 
        if (isCollision&& _collider != null)
        {
            //충돌 이펙트 
            _collisionEffect.SetActive(true);
            //충돌 애니메이션
            SetAniParameters("Attacked");

            int _monsterIndex = _collider.GetComponent<MonsterInfo>().GetMonsterIndex();
            DataManager.instance.playerStatDataList[_playerIndex].hp
                -= DataManager.instance.monsterDataList[_monsterIndex].str;
            if (DataManager.instance.playerStatDataList[_playerIndex].hp <= 0)
            {
                _canMove = false;
                DataManager.instance.playData.gameover = true;

                SetAniParameters("Jumping");
                Time.timeScale = 0.3f;

                float _aniTime = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
                Destroy(gameObject, _aniTime);
            }

        }
        else
        {
            _isCollision =false;
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

        //이동범위제한
        float _xpos = Mathf.Clamp(transform.position.x,_minMovePos.x, _maxMovePos.x);
        float _ypos = Mathf.Clamp(transform.position.y, _minMovePos.y, _maxMovePos.y);
        transform.position = new Vector2(_xpos, _ypos);

        //이동 중이라면 걷기 애니메이션 출력
        if (_inputHorizontal != 0 || _inputVertical != 0)
        {
            SetAniParameters("Walking", _inputHorizontal == 0 ? (int)_inputVertical : (int)_inputHorizontal);
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
    //총알발사
    private void Shoot()
    {
        _currBulletTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && _currBulletTime >= _bulletDelay)
        {
            //for작성 총알 갯수에 따라 복제하는 갯수 각도 다르게
            for (int i = 0; i< _bulletCnt; i++)
            {
                //짝수일때 i/2 == bulletCnt/2 이면 90
                //짝수일때 i/2 != bulletCnt/2 이면 90 +  (i/2+1) * _bulletAngle
                //홀수일때 90 + (-1) * (i / 2 + 1) * _bulletAngle

                //각도 조절
                float _angleZ =  i %2==0 ?  i/2 == _bulletCnt / 2 ? 90 :90 + (i/2+1) * _bulletAngle 
                    : 90 + (-1) * (i / 2 + 1) * _bulletAngle;
                Vector3 _rotation = new Vector3(0, 0,_angleZ);

                //생성 위치 조절
                float _bulletPosX = _bulletPos.position.x;
                float _bulletPosY = _bulletPos.position.y;
                float _posX = i % 2 == 0 ? i / 2 == _bulletCnt / 2 ? _bulletPosX : _bulletPosX + (-1) * (i / 2 + 1) * 0.1f
                    : _bulletPosX + (i / 2 + 1) * 0.1f;

                Vector2 _clonePos = new Vector2(_posX, _bulletPosY);
                Instantiate(_bulletPrefab, _clonePos, Quaternion.Euler(_rotation));
            }
            _currBulletTime = 0;

        }
    }
    //초기 이동이 끝난 후 이동가능하도록 _canMove 값 수정
    private void initMove()
    {
        _canMove = true;
    }

    //캐릭터의 고유 스탯 불러오기
    private void UpdateInfo()
    {
        //키입력정보
        _inputHorizontal = Input.GetAxisRaw("Horizontal");
        _inputVertical = Input.GetAxisRaw("Vertical");

        //스피드 데이터 불러오기
        _horizontalSpeed = DataManager.instance.playerStatDataList[_playerIndex].speed;
        _verticalSpeed = DataManager.instance.playerStatDataList[_playerIndex].speed;
        _bulletSpeed = DataManager.instance.playerStatDataList[_playerIndex].attackSpeed * 200;
        _bulletDelay = 1 - (DataManager.instance.playerStatDataList[_playerIndex].attackSpeed * 0.1f);
        _bulletCnt = DataManager.instance.playData.bulletCnt;
    }

    //플레이어 레벨 관리
    private void LevelManager()
    {
        int _exe = DataManager.instance.playData.exe;
        int _level = DataManager.instance.playData.level;

        if(_exe >= Mathf.Pow(_level, 2) * 5)
        {
            DataManager.instance.playData.exe -= (int)Mathf.Pow(_level, 2) * 5;
            DataManager.instance.playData.level++;

            StartCoroutine(nameof(LevelUpAniPlay));
            //레벨 업 이펙트 실행 필요
            //업그레이드 ui 호출 필요

        }
    }

    //레벨업 effect
    IEnumerator LevelUpAniPlay()
    {
        //Time.timeScale = 0.2f;
        _levelUpEffect.SetActive(true);
        float _aniTime = _levelUpEffect.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;
        //디버그
        Debug.Log(_aniTime);
        yield return new WaitForSeconds(_aniTime);
        _levelUpEffect.SetActive(false);

        _upgradeUIManager.SetActiveUpgrade();
    }
}
