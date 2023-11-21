using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpItemController : MonoBehaviour
{
    private int _playerIndex;
    private bool _isCollision = false;
    private Transform _player;

    private float _itemTriggerSize;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerIndex = PlayerPrefs.GetInt("PlayerIndex");
            DataManager.instance.playerStatDataList[_playerIndex].hp += 1;
            _isCollision = true;
            _player = collision.transform;
        }
    }

    void Update()
    {
        if (_isCollision)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.position, 0.05f);
            if (transform.position == _player.position)
            {
                Destroy(gameObject);
            }
        }

        if (_itemTriggerSize != DataManager.instance.playData.bulletSize)
        {
            _itemTriggerSize = DataManager.instance.playData.bulletSize;
            GetComponent<CircleCollider2D>().radius += _itemTriggerSize;
        }

    }
}
