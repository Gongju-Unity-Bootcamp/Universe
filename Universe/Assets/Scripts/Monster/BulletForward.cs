using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletForward : MonoBehaviour
{
    public GameObject playerPos;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, gameObject.transform.position, 2f);
  
    }
}
