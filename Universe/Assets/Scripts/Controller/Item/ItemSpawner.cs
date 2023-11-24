using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 기능
///     1) 아이템 8종 프리팹을 리스트로 불러온다.
///     2) 랜덤으로 아이템을 생성한다.
/// 호출 : MonsterContrller에서 몬스터의 체력이 모두 소모되었을 경우 호출
/// </summary>
public class ItemSpawner : MonoBehaviour
{
    //아이템 프리팹 리스트
    private GameObject[] _itemList;

    // 아이템 오브젝트 생성
    public void CloneItem(Transform clonePos){
        //프리팹을 리스트로 생성
        _itemList = Resources.LoadAll<GameObject>("Prefabs/Items");
        int _randomIndex =Random.Range(0,_itemList.Length);
        
        //아이템 생성
        Instantiate(_itemList[_randomIndex], clonePos.position, Quaternion.identity);
    }
}
