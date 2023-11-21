using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpController : MonoBehaviour
{
    public GameObject _heartImg;
    public GameObject _halfHeartImg;
    private int _playerHp =0;
    private int _prePlayerHp =0;

    private void Update()
    {
        _playerHp = DataManager.instance.playerStatDataList[PlayerPrefs.GetInt("PlayerIndex")].hp;

        if(_prePlayerHp != _playerHp)
        {
            ReloadHp();
        }
    }

    private void ReloadHp()
    {
        _prePlayerHp = _playerHp;
        int _heartCnt = _playerHp / 2;
        int _halfHeart = _playerHp % 2;
        int _createHeart =0;

        Transform[] _childObject = GetComponentsInChildren<Transform>(true);
        if(_childObject != null )
        {
            for (int i = 1; i < _childObject.Length; i++)
            {
                Destroy(_childObject[i].gameObject);
            }
        }

        while (_createHeart < _heartCnt)
        {
            GameObject _heartObject = Instantiate(_heartImg);
            _heartObject.transform.SetParent(gameObject.transform);
            _heartObject.transform.localScale = Vector3.one;
            ++_createHeart;
        }

        if(_halfHeart != 0)
        {
            GameObject _heartObject = Instantiate(_halfHeartImg);
            _heartObject.transform.SetParent(gameObject.transform);
            _heartObject.transform.localScale = Vector3.one;
        }
    }
}
