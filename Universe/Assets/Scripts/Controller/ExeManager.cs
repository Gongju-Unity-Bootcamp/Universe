using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExeManager : MonoBehaviour
{
    public int _exe;
    private bool _isCollision =false;
    private Transform _player;

    private float _exeTriggerSize=0;
    // Start is called before the first frame update

    private void Start()
    {
        _player = GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DataManager.instance.playData.exe += _exe;
            _isCollision = true;
            _player = collision.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_isCollision)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.position, 0.05f);
            if (transform.position == _player.position)
            {
                Destroy(gameObject);
            }
        }
        if(_exeTriggerSize != DataManager.instance.playData.bulletSize)
        {
            _exeTriggerSize = DataManager.instance.playData.bulletSize;
            GetComponent<CircleCollider2D>().radius += _exeTriggerSize;
        }
        if (DataManager.instance.playData.gameover)
        {
            Destroy(gameObject) ;
        }

    }
}
