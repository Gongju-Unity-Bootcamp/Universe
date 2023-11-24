using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���
///     1) ĳ���Ϳ� �������� �浹 ����
///     2) ������ ȹ�� �� ü�� ȸ�� �ý���
/// ȣ�� : �������� ĳ���Ϳ� �浹�� ��� ��� ȣ��    
/// </summary>
public class HpItemController : MonoBehaviour
{
    //ĳ��������
    private int _playerIndex;
    //���� ĳ���� ü��
    private int _playerHp = 0;
    //�ش� ĳ������ �ִ� ü�� + ���������� ������ ü��
    private int _maxHp = 0;
    //�ش� ĳ������ �ִ� ü��
    private int _playerMaxHp = 0;
    //�浹 ����
    private bool _isCollision = false;
    private Transform _player;

    //Ʈ���� ������
    private float _itemTriggerSize =0f;
    
    //�浹����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�÷��̾�� �浹�� ���
        if (collision.CompareTag("Player"))
        {
            _player = collision.transform;
            //������ ȹ�� �޼ҵ� ȣ��
            GetItem();
        }
    }

    void Update()
    {
        //�浹�� ���
        if (_isCollision)
        {   
            //ĳ������ ��ġ�� �̵�
            transform.position = Vector3.MoveTowards(transform.position, _player.position, 0.05f);
            //ĳ������ ��ġ�� �̵� �Ϸ�Ǿ��ٸ�
            if (transform.position == _player.position)
            {
                //������ ������Ʈ ����
                Destroy(gameObject);
            }
        }

        //�ڼ� �ɷ��� ��ȭ�Ǿ��� ��� ������ ȹ�� ������ �о�����.
        //�������� Ʈ���� ������ ������Ű��
        if (_itemTriggerSize != DataManager.instance.playData.bulletSize)
        {
            _itemTriggerSize = DataManager.instance.playData.bulletSize;
            GetComponent<CircleCollider2D>().radius += _itemTriggerSize;
        }

        //���� ������� ȭ���� ������Ʈ ��� �����ؾ��� 
        if(DataManager.instance.playData.gameover){
            Destroy(gameObject);
        }
    }

    //������ ȹ�� ���
    void GetItem()
    {
        _isCollision = true;
        _playerIndex = PlayerPrefs.GetInt("PlayerIndex");
        //���� �÷��̾��� ü�� �ҷ�����
        _playerHp = DataManager.instance.playerStatDataList[_playerIndex].hp;
        //���� �÷��̾��� �ִ� ü��
        _playerMaxHp = DataManager.instance.playerStatDataList[_playerIndex].maxHp;
        //���� �÷��̾��� �ִ� ü�� + ���������� ������ ü��
        _maxHp = _playerMaxHp + (int)DataManager.instance.upgradeDataList[0].upgrade
        * (DataManager.instance.upgradeDataList[0].level - 1);

        //���� ü���� �ִ� ü�º��� ���� ��쿡�� �÷��̾� ü�� ������Ŵ
        if (_playerHp < _maxHp)
        {
            //������ ȸ������ ��� �ִ� ü�º��� ũ�ٸ�
            //�ִ� ü�� ���·θ� ������ش�.
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
