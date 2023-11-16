using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    public GameObject _selectPlayerUI;
    // Update is called once per frame
    void Update()
    {
        //아무키나 입력받았을 경우 오브젝트 비활성화
        //나중에 애니 추가
        if(Input.anyKeyDown)
        {
            GameObject _selectUI = Instantiate(_selectPlayerUI);
            _selectUI.GetComponent<Canvas>().worldCamera = Camera.main;
            gameObject.SetActive(false);
        }
    }
}
