using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectPlayer : MonoBehaviour
{
    private string[] _colors = { "#FF00A6FF", "#FFA300FF", "#0BFF00FF", "#00FFFFFF", "#FFFFFFFF" };
    Color _changeColor;
    const int _defaultColorIndex = 4;

    readonly Vector2 _outLineBorder = new Vector2(4,4);
    readonly Vector2 _defaultOutLineBorder = new Vector2(2, 2);
    
    public Button _playBt;
    public List<Button> _selectPlayerBt;

    private ClonePlayer _clonePlayer;
    public GameObject _playUI;   

    private void Start()
    {
        _clonePlayer = FindObjectOfType<ClonePlayer>();
    }
    private void Update()
    {
        //player�� ���õǾ����� Ȯ��
        if(PlayerPrefs.HasKey("PlayerIndex"))
            _playBt.interactable = true;
        else
            _playBt.interactable = false;
    }
    //ĳ���� ���� ��ư Ŭ�� �� ȣ�� 
    public void ClickSelectPlayerBt(int playerIndex)
    {
        //ĳ������ �ε����� ����Ǿ� ���� ��� ������ư �� outline ����
        if(PlayerPrefs.HasKey("PlayerIndex"))
        {
            int beforePlayerIndex = PlayerPrefs.GetInt("PlayerIndex");
            ColorUtility.TryParseHtmlString(_colors[_defaultColorIndex], out _changeColor);
            _selectPlayerBt[beforePlayerIndex].GetComponent<Outline>().effectColor = _changeColor;
            _selectPlayerBt[beforePlayerIndex].GetComponent<Outline>().effectDistance = _defaultOutLineBorder;
        }
        ColorUtility.TryParseHtmlString(_colors[playerIndex], out _changeColor);
        _selectPlayerBt[playerIndex].GetComponent<Outline>().effectColor = _changeColor;
        _selectPlayerBt[playerIndex].GetComponent<Outline>().effectDistance = _outLineBorder;

        PlayerPrefs.SetInt("PlayerIndex", playerIndex);
    }
    //�÷��� ��ư Ŭ�� �� ȣ��
    public void ClickPlayBt()
    {
        //playerȭ�� --> ĳ���ͻ��� --> ĳ���� ����ui ��Ȱ��ȭ
        GameObject _playui = Instantiate(_playUI);
        _playui.GetComponent<Canvas>().worldCamera = Camera.main;
        _clonePlayer.ClonePlayerObject();
        gameObject.SetActive(false);
    }

}
