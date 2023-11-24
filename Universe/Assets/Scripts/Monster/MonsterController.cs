using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 기능: 
/// 
/// </summary>
public class MonsterController : MonoBehaviour
{
    // 몬스터의 종류
    [SerializeField]
    private int _monsterIndex;
    // 몬스터의 이동속도
    private float _monsterMoveSpeed;
    // 몬스터의 현재 체력
    private float _curHp;
    // 애니메이터
    private Animator _animator;
    // 몬스터의 움직임 가능여부
    private bool _canMove = true;
    // 경험치 프리팹
    public GameObject _exePrefab;
    // 현재 플레이하는 캐릭터의 정보
    private int _playerIndex;

    //총알발사 몬스터
    private float _randomY;

    //라운드 정보
    private int _round;

    // Start is called before the first frame update
    void Start()
    {
        //DataManager를 통해 MonsterData.json의 해당 몬스터 speed 값을 가져온다.
        _monsterMoveSpeed = DataManager.instance.monsterDataList[_monsterIndex].speed;
        //DataManager를 통해 MonsterData.json의 해당 몬스터 체력 값을 가져온다.
        _curHp = DataManager.instance.monsterDataList[_monsterIndex].hp;
        _animator = GetComponent<Animator>();
        //PlayerPrefabs.PlayerIndex 값 가져오기 !! 수정 필요
        _playerIndex = PlayerPrefs.GetInt("PlayerIndex");

        //총알 몬스터 y좌표 위치 (랜덤으로 설정)
        _randomY = Random.Range(2, 5);
    }

    void Update()
    {
        //몬스터가 움직일 수 있는 상황이라면
        if (_canMove)
        {
            //몬스터 종류에 따라서 이동방식이 다르므로 해당하는 메소드 호출
            if (_monsterIndex >4) { }
            else if (_monsterIndex > 1) { ShootMonsterMove(); }
            else { BasicMonsterMove(); }
        }
        //게임 오버 상태라면 생성된 몬스터 삭제
        //DataManager를 통해 PlayData.json의 gameonver 값 가져오기
        if (DataManager.instance.playData.gameover)
        {
            Destroy(gameObject);
        }
    }

    #region monstermMove
    //총알 몬스터의 움직임
    private void ShootMonsterMove()
    {
        transform.position += new Vector3(0, (-1) * Time.deltaTime * _monsterMoveSpeed * 2f, 0);
        //몬스터의 위치가 정해진 y위치보다 아래일 경우 이동 멈춤
        if (transform.position.y <= _randomY)
        {
            _canMove = false;
        }
    }

    //기본 몬스터의 움직임
    private void BasicMonsterMove()
    {
        transform.position += new Vector3(0, (-1) * Time.deltaTime * _monsterMoveSpeed, 0);
        //몬스터의 위치가 -5f 아래일 경우 몬스터 삭제
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어 총알에 충돌할 경우
        if (collision.CompareTag("PlayerBullet"))
        {
            //중복 충돌감지를 방지하기 위해 총알 태그 변경
            collision.gameObject.tag = "Destroy";
            //피격상태로 변환
            Attacked();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //플레이어 총알이 부딪힌 후 사라진경우 일반 몬스터 이동가능상태로 변환
        if (collision.CompareTag("PlayerBullet"))
        {
            if (_monsterIndex < 2) { _canMove = true; }
        }
    }
    #endregion

    //체력 감소, 피격 애니 실행
    private void Attacked()
    {
        //일반 몬스터는 피격 시 위로 조금 이동
        if (_monsterIndex < 2)
        {
            transform.position += new Vector3(0, 0.2f, 0);
        }

        //체력감소 (몬스터의 현재체력에서 플레이어의 공격력만큼 차감)
        _curHp -= DataManager.instance.playerStatDataList[_playerIndex].str;

        //감소된 체력이 0이상이 경우 피격 상태
        if (_curHp > 0)
        {
            //피격 애니
            _animator.SetTrigger("Attacked");
            Invoke(nameof(CanMove),0.5f);
        }
        //체력이 고갈되었으면 죽음
        else
        {
            //일반몬스터는 이동중인 상태이므로 움직일수 없는 상태로 바꿔줌
            if (_monsterIndex < 2) { _canMove = false; }
            _animator.SetTrigger("Die");
            gameObject.tag = "Destroy";
            //점수 가산 
            DataManager.instance.playData.score += 100 + _monsterIndex * 50;
            //업적에 죽인몬스터 수 가산
            DataManager.instance.achieveData.killMonsterCnt[_monsterIndex]++;

            _round = DataManager.instance.playData.round;
            //라운드 정보에 처치해야하는 몬스터 수 차감
            DataManager.instance.roundDataList[_round - 1].monsterCount[_monsterIndex]--;
            //경험치 오브젝트 생성
            Instantiate(_exePrefab, transform.position, Quaternion.identity);
            //일정확률로 아이템 드랍
            CloneItem();
            //피격애니메이션 클립 실행 시간이 끝난 후 오브젝트 파괴
            Destroy(gameObject, 0.5f);
        }
    }

    //움직일 수 없는 상태일 경우 호출
    private void CanMove(){
        _canMove =true;
    }

    //아이템 생성 메소드
    private void CloneItem(){

        int _itemDropPercent = Random.Range(0,1001);
        if(_itemDropPercent >950){
        //ItemSpawner의 _itemSpawner를 호출하여 아이템 생성
        ItemSpawner _itemSpawner = new ItemSpawner();
        _itemSpawner.CloneItem(transform); 
        }
    }
}
