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
        //player가 선택되었는지 확인
        if(PlayerPrefs.HasKey("PlayerIndex"))
            _playBt.interactable = true;
        else
            _playBt.interactable = false;
    }
    //캐릭터 선택 버튼 클릭 시 호출 
    public void ClickSelectPlayerBt(int playerIndex)
    {
        //캐릭터의 인덱스가 저장되어 있을 경우 이전버튼 색 outline 원복
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
    //플레이 버튼 클릭 시 호출
    public void ClickPlayBt()
    {
        //player화면 --> 캐릭터생성 --> 캐릭터 선택ui 비활성화
        GameObject _playui = Instantiate(_playUI);
        _playui.GetComponent<Canvas>().worldCamera = Camera.main;
        _clonePlayer.ClonePlayerObject();
        gameObject.SetActive(false);
    }

}
