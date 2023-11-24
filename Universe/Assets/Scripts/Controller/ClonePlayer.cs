using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기능 : 선택한 플레이어 생성 (등장 연출)
/// 호출 : SelectPlayerUI에서 호출
/// </summary>
public class ClonePlayer : MonoBehaviour
{
    public List<GameObject> _playerList;
    private GameObject _player;
    
    const float _maxY=  -4f;

    void Update()
    {
        //생성한 플레이어가 있을 경우 
        if (_player != null)
        {
            //플레이어 등장 애니메이션
            _player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 1f);
            if (_player.transform.position.y >= _maxY)
            {
                _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                _player = null;
            }
        }
    }

    //플레이어 생성
    public void ClonePlayerObject()
    {
        _player = Instantiate(_playerList[PlayerPrefs.GetInt("PlayerIndex")], transform.position,Quaternion.identity);
    }
}
