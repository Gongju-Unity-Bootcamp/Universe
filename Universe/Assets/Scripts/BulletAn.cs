using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAn : MonoBehaviour
{
    Animator ani;
    public GameObject childObject;
    private void Start()
    {
        ani =GetComponent<Animator>();
        Invoke(nameof(StartAnimation), 3f);

    }

    private void StartAnimation()
    {
        childObject.SetActive(false);
        ani.SetTrigger("Collision");
    }
}
