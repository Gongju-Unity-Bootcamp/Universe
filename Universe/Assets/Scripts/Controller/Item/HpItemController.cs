using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기능
///     1) 캐릭터와 아이템의 충돌 감지
///     2) 아이템 획득 시 체력 회복 시스템
/// 호출 : 아이템이 캐릭터와 충돌할 경우 기능 호출    
/// </summary>
public class HpItemController : MonoBehaviour
{
    //캐릭터종류
    private int _playerIndex;
    //현재 캐릭터 체력
    private int _playerHp = 0;
    //해당 캐릭터의 최대 체력 + 레벨업으로 증가한 체력
    private int _maxHp = 0;
    //해당 캐릭터의 최대 체력
    private int _playerMaxHp = 0;
    //충돌 여부
    private bool _isCollision = false;
    private Transform _player;

    //트리거 사이즈
    private float _itemTriggerSize =0f;
    
    //충돌감지
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어와 충돌할 경우
        if (collision.CompareTag("Player"))
        {
            _player = collision.transform;
            //아이템 획득 메소드 호출
            GetItem();
        }
    }

    void Update()
    {
        //충돌한 경우
        if (_isCollision)
        {   
            //캐릭터의 위치로 이동
            transform.position = Vector3.MoveTowards(transform.position, _player.position, 0.05f);
            //캐릭터의 위치로 이동 완료되었다면
            if (transform.position == _player.position)
            {
                //아이템 오브젝트 삭제
                Destroy(gameObject);
            }
        }

        //자석 능력이 강화되었을 경우 아이템 획득 범위가 넓어진다.
        //아이템의 트리거 사이즈 증가시키기
        if (_itemTriggerSize != DataManager.instance.playData.bulletSize)
        {
            _itemTriggerSize = DataManager.instance.playData.bulletSize;
            GetComponent<CircleCollider2D>().radius += _itemTriggerSize;
        }

        //게임 오버라면 화면의 오브젝트 모두 삭제해야함 
        if(DataManager.instance.playData.gameover){
            Destroy(gameObject);
        }
    }

    //아이템 획득 기능
    void GetItem()
    {
        _isCollision = true;
        _playerIndex = PlayerPrefs.GetInt("PlayerIndex");
        //현재 플레이어의 체력 불러오기
        _playerHp = DataManager.instance.playerStatDataList[_playerIndex].hp;
        //현재 플레이어의 최대 체력
        _playerMaxHp = DataManager.instance.playerStatDataList[_playerIndex].maxHp;
        //현재 플레이어의 최대 체력 + 레벨업으로 증가한 체력
        _maxHp = _playerMaxHp + (int)DataManager.instance.upgradeDataList[0].upgrade
        * (DataManager.instance.upgradeDataList[0].level - 1);

        //현재 체력이 최대 체력보다 적을 경우에만 플레이어 체력 증가시킴
        if (_playerHp < _maxHp)
        {
            //아이템 회복했을 경우 최대 체력보다 크다면
            //최대 체력 상태로만 만들어준다.
            if (_playerHp + 1 > _maxHp)
            {
                DataManager.instance.playerStatDataList[_playerIndex].hp = _maxHp;
            }
            else 
            { 
                DataManager.instance.playerStatDataList[_playerIndex].hp += 1; 
            }
        }
    }
}
