using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonePlayer : MonoBehaviour
{
    public List<GameObject> _playerList;
    private GameObject _player;
    
    const float _maxY=  -4f;

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            _player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 1f);
            if (_player.transform.position.y >= _maxY)
            {
                _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                _player = null;
            }
        }
    }

    public void ClonePlayerObject()
    {
        _player = Instantiate(_playerList[PlayerPrefs.GetInt("PlayerIndex")], transform.position,Quaternion.identity);
    }
}
