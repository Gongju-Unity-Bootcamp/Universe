using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    public GameObject _selectPlayerUI;

    void Update()
    {
        //�ƹ�Ű�� �Է¹޾��� ��� ������Ʈ ��Ȱ��ȭ
        //���߿� �ִ� �߰�
        if(Input.anyKeyDown)
        {
            GameObject _selectUI = Instantiate(_selectPlayerUI);
            _selectUI.GetComponent<SelectPlayer>().InitScreen();
            _selectUI.GetComponent<Canvas>().worldCamera = Camera.main;
            _selectUI.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
