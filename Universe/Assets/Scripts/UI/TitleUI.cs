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
        //�ƹ�Ű�� �Է¹޾��� ��� ������Ʈ ��Ȱ��ȭ
        //���߿� �ִ� �߰�
        if(Input.anyKeyDown)
        {
            GameObject _selectUI = Instantiate(_selectPlayerUI);
            _selectUI.GetComponent<Canvas>().worldCamera = Camera.main;
            gameObject.SetActive(false);
        }
    }
}
