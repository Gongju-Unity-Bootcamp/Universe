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
        InvokeRepeating(nameof(bulletSpawn), 0.1f, 2f);
    }

    void bulletSpawn()
    {
        // bullet 복제 및 위치
        GameObject newobject = Instantiate(bullet, bulletPos.position, bullet.transform.rotation);
        Destroy(newobject, 5f);
    }
}