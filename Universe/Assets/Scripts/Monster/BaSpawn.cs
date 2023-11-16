using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaiscMonster : MonoBehaviour
{
    public int BasicMonsterCount = 50; // 50 -1씩 카운트 > 0가 되면 끝
    //List를 이용해서 유니티 인스펙터창에 랜덤으로 넣을 프리팹 설정
    public List<GameObject> MonsterPrefabList = new List<GameObject>();
    private float randomX;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(spawn), 0.5f, 3f);
    }


    // Update is called once per frame
    void spawn()
    {
        randomX = Random.Range(-2, 3);
        int RandomMonsterIndex = Random.Range(0, MonsterPrefabList.Count);
        GameObject newObject = Instantiate(MonsterPrefabList[RandomMonsterIndex], new Vector3(randomX, gameObject.transform.position.y, 0f), transform.rotation);
        Destroy(newObject, 4f);
    }
}