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
        //배열을 사용하거나
        // Random.Range를 사용하여 angry 몬스터를 랜덤으로 나타나게 할 수 있다.
        randomX = Random.Range(-2, 3);
        //List의 프리팹의 인데스를 넣기 위해 랜덤으로 생성될 객체를 선언해준다.
        int RandomMonsterIndex = Random.Range(0, MonsterPrefabList.Count);
        GameObject newObject = Instantiate(MonsterPrefabList[RandomMonsterIndex], new Vector3(randomX, gameObject.transform.position.y, 0f), transform.rotation);
        Destroy(newObject, 4f);
    }
}