using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExeManager : MonoBehaviour
{
    public int _exe;
    private bool _isCollision =false;
    private Transform _player;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DataManager.instance.playData.exe += _exe;
            _isCollision = true;
            _player = collision.transform;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

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

    }
}
