using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("원래 데이터: "+ DataManager.instance.playerStatDataList[0].str);

        DataManager.instance.playerStatDataList[0].str = 15;
        Debug.Log("수정 데이터: "+ DataManager.instance.playerStatDataList[0].str);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
