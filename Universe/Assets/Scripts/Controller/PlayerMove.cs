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
    readonly Vector2 _maxMovePos = new Vector2(2.3f, 3f);


    //�Ѿ� ������, �߻� ��ġ
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

    //�Ѿ˰���
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
        //ĳ���� �ʱ� �̵� ���� �� ������ �� �ֵ���
        Invoke(nameof(initMove), 5f);

    }

    // Update is called once per frame
    void Update()
    {
        //����� �÷��̾��� ������ ȣ��
        UpdateInfo();
        
        //�̵�, ����
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

    //�÷��̾��� �ǰݻ��°� ���Ұ�� ȣ�� 
    private void PlayerAttacked(bool isCollision)
    {
        _isCollision = isCollision;
        //�浹���� ��� 
        if (isCollision&& _collider != null)
        {
            //�浹 ����Ʈ 
            _collisionEffect.SetActive(true);
            //�浹 �ִϸ��̼�
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

        //�̵���������
        float _xpos = Mathf.Clamp(transform.position.x,_minMovePos.x, _maxMovePos.x);
        float _ypos = Mathf.Clamp(transform.position.y, _minMovePos.y, _maxMovePos.y);
        transform.position = new Vector2(_xpos, _ypos);

        //�̵� ���̶�� �ȱ� �ִϸ��̼� ���
        if (_inputHorizontal != 0 || _inputVertical != 0)
        {
            SetAniParameters("Walking", _inputHorizontal == 0 ? (int)_inputVertical : (int)_inputHorizontal);
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
    //�Ѿ˹߻�
    private void Shoot()
    {
        _currBulletTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && _currBulletTime >= _bulletDelay)
        {
            //for�ۼ� �Ѿ� ������ ���� �����ϴ� ���� ���� �ٸ���
            for (int i = 0; i< _bulletCnt; i++)
            {
                //¦���϶� i/2 == bulletCnt/2 �̸� 90
                //¦���϶� i/2 != bulletCnt/2 �̸� 90 +  (i/2+1) * _bulletAngle
                //Ȧ���϶� 90 + (-1) * (i / 2 + 1) * _bulletAngle

                //���� ����
                float _angleZ =  i %2==0 ?  i/2 == _bulletCnt / 2 ? 90 :90 + (i/2+1) * _bulletAngle 
                    : 90 + (-1) * (i / 2 + 1) * _bulletAngle;
                Vector3 _rotation = new Vector3(0, 0,_angleZ);

                //���� ��ġ ����
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
    //�ʱ� �̵��� ���� �� �̵������ϵ��� _canMove �� ����
    private void initMove()
    {
        _canMove = true;
    }

    //ĳ������ ���� ���� �ҷ�����
    private void UpdateInfo()
    {
        //Ű�Է�����
        _inputHorizontal = Input.GetAxisRaw("Horizontal");
        _inputVertical = Input.GetAxisRaw("Vertical");

        //���ǵ� ������ �ҷ�����
        _horizontalSpeed = DataManager.instance.playerStatDataList[_playerIndex].speed;
        _verticalSpeed = DataManager.instance.playerStatDataList[_playerIndex].speed;
        _bulletSpeed = DataManager.instance.playerStatDataList[_playerIndex].attackSpeed * 200;
        _bulletDelay = 1 - (DataManager.instance.playerStatDataList[_playerIndex].attackSpeed * 0.1f);
        _bulletCnt = DataManager.instance.playData.bulletCnt;
    }

    //�÷��̾� ���� ����
    private void LevelManager()
    {
        int _exe = DataManager.instance.playData.exe;
        int _level = DataManager.instance.playData.level;

        if(_exe >= Mathf.Pow(_level, 2) * 5)
        {
            DataManager.instance.playData.exe -= (int)Mathf.Pow(_level, 2) * 5;
            DataManager.instance.playData.level++;

            StartCoroutine(nameof(LevelUpAniPlay));
            //���� �� ����Ʈ ���� �ʿ�
            //���׷��̵� ui ȣ�� �ʿ�

        }
    }

    //������ effect
    IEnumerator LevelUpAniPlay()
    {
        //Time.timeScale = 0.2f;
        _levelUpEffect.SetActive(true);
        float _aniTime = _levelUpEffect.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;
        //�����
        Debug.Log(_aniTime);
        yield return new WaitForSeconds(_aniTime);
        _levelUpEffect.SetActive(false);

        _upgradeUIManager.SetActiveUpgrade();
    }
}
