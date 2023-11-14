using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMonster_upgrade : MonoBehaviour
{
    public SpriteRenderer BM_upgrade;

    // Start is called before the first frame update
    void Start()
    {
        BM_upgrade = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        BM_upgrade.material.color = Color.red;
    }
}