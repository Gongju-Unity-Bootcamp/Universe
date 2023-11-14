using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaiscMonster : MonoBehaviour
{
    public GameObject BasicMonster;
    public GameObject BasicMonster2;
    public int BasicMonsterCount = 50;
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
        GameObject newObject = Instantiate(BasicMonster, new Vector3(randomX, gameObject.transform.position.y, 0f), transform.rotation);
        GameObject newObject2 = Instantiate(BasicMonster2, new Vector3(randomX, gameObject.transform.position.y, 0f), transform.rotation);
        Destroy(newObject, 4f);
        Destroy(newObject2, 3f);
    }
}