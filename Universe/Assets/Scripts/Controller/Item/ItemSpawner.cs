using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// ���
///     1) ������ 8�� �������� ����Ʈ�� �ҷ��´�.
///     2) �������� �������� �����Ѵ�.
/// ȣ�� : MonsterContrller���� ������ ü���� ��� �Ҹ�Ǿ��� ��� ȣ��
/// </summary>
public class ItemSpawner : MonoBehaviour
{
    //������ ������ ����Ʈ
    private GameObject[] _itemList;

    // ������ ������Ʈ ����
    public void CloneItem(Transform clonePos){
        //�������� ����Ʈ�� ����
        _itemList = Resources.LoadAll<GameObject>("Prefabs/Items");
        int _randomIndex =Random.Range(0,_itemList.Length);
        
        //������ ����
        Instantiate(_itemList[_randomIndex], clonePos.position, Quaternion.identity);
    }
}
