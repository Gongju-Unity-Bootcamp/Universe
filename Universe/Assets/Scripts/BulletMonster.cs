using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    public GameObject bullet;
    public Transform bullPos;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(bulletSpawn), 4f, 3f);
    }

    // Update is called once per frame
    void bulletSpawn()
    {
        Instantiate(bullet, bullPos.position, transform.rotation);
    }
}




//public class BulletAn : MonoBehaviour
//{
//    Animator ani;
//    public GameObject childObject;
//    private void Start()
//    {
//        ani = GetComponent<Animator>();
//        Invoke(nameof(StartAnimation), 3f);

//    }

//    private void StartAnimation()
//    {
//        childObject.SetActive(false);
//        ani.SetTrigger("Collision");
//    }
//}

