using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawn : MonoBehaviour
{
    public int BasicMonsterCount = 50; 
    public List<GameObject> MonsterPrefabList = new List<GameObject>();
    public GameObject bullet;
    public Transform bulletPos; 
    private float randomX;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating(nameof(spawn), 0.5f, 3f);
        InvokeRepeating(nameof(bulletSpawn), 0.1f, 2f);
    }


    // Update is called once per frame
    //void spawn()
    //{
    //    // 기본 몬스터를 랜덤 및 범위 지정
    //    randomX = Random.Range(-2, 3);
    //    int RandomMonsterIndex = Random.Range(0, MonsterPrefabList.Count);
    //    GameObject newObject = Instantiate(MonsterPrefabList[RandomMonsterIndex], new Vector3(randomX, gameObject.transform.position.y, 0f), transform.rotation);
    //    Destroy(newObject, 4f);
    //}

    void bulletSpawn()
    {
        // bullet 복제 및 위치
        GameObject newobject = Instantiate(bullet, bulletPos.position, bullet.transform.rotation);
        Destroy(newobject, 5f);
    }
}