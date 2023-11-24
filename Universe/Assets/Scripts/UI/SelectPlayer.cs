using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 작성:2023-11-22  
/// 기능:
///     게임캐릭터를 선택하는 화면
///     캐릭터 해금 : 캐릭터마다 특정 업적에 달성하면 해금
///     캐릭터 구매 : 해금된 캐릭터에 한해서 골드를 이용하여 캐릭터 구매
/// 호출     
///     1) SelectPlayerUI 오브젝트
///     2) TitleUI -> 시작버튼 클릭 시
///     3) GameOverUI -> Replay버튼 클릭 시             
/// </summary>
public class SelectPlayer : MonoBehaviour
{
    #region Create Object
    //button 클릭 시 outline 색깔지정
    private string[] _colors = { "#FF00A6FF", "#FFA300FF", "#0BFF00FF", "#00FFFFFF", "#FFFFFFFF" };
    //바뀌어야 하는 컬러
    Color _changeColor;

    //default outlime color
    const int _defaultColorIndex = 4;
    readonly Vector2 _outLineBorder = new Vector2(4,4);
    readonly Vector2 _defaultOutLineBorder = new Vector2(2, 2);
    
    //게임 플레이 버튼
    public Button _playBt;
    //게임 캐릭터 버튼리스트 (4)
    public List<Button> _selectPlayerBt;
    
    //ClonePlayer 객체
    private ClonePlayer _clonePlayer;
    //PlayUI 프리팹
    public GameObject _playUI;
    //골드
    public TMP_Text _goldTx;

    #region PurchaseUI

    //purchaseUI 프리팹
    public GameObject _purchaseUI;
    //캐릭터 해금 여부 (4)
    bool[] _canOpenPlayer = { false, false, false,false};
    //구매 캐릭터 인덱스
    private int _selectPlayerIndex;
    //캐릭터 구매 비용 
    private int _purchaseGold;
    //구매 골드 안내 텍스트
    public TMP_Text _purchasegoldTx;
    //구매하기 버튼
    public Button _purchaseBt;
    #endregion

    #endregion

    private void Start()
    {
        //생성되어있는 clonePlayer 클래스 호출
        _clonePlayer = FindObjectOfType<ClonePlayer>();
        //현재 보유 골드 출력
        _goldTx.text = PlayerPrefs.GetInt("Gold").ToString();
    }
    private void Update()
    {
        //player가 선택되었다면 플레이버튼 상호작용가등
        if (PlayerPrefs.HasKey("PlayerIndex"))
            _playBt.interactable = true;
        else
            _playBt.interactable = false;
    }

    /// <summary>
    /// 작성: 2023-11-22
    /// 기능
    ///     1) 캐릭터 구매여부 파악하여 구매되지 않은 경우 구매 팝업창 띄움
    ///     2) 선택된 버튼을 확인하여 outline 색상, 굵기 설정
    /// 호출: 캐릭터 버튼 선택 시
    /// </summary>
    /// <param name="playerIndex"></param>
    public void ClickSelectPlayerBt(int playerIndex)
    {
        //구매가 되지 않은 캐릭터의 경우 구매 팝업창 뜨도록

        if (!DataManager.instance.achieveData.getPlayer[playerIndex])
        {
            PurchaseUISetting(playerIndex);
            _purchaseUI.SetActive(true);
        }

        //캐릭터의 인덱스가 저장되어 있을 경우 이전버튼 색 outline 원복
        if (DataManager.instance.achieveData.getPlayer[playerIndex])
        {
            if (PlayerPrefs.HasKey("PlayerIndex"))
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
    }

    /// <summary>
    /// 기능 : 
    ///     1) Player화면 생성
    ///     2) 캐릭터 생성
    ///     3) SelectPlayerUI 삭제
    /// 호출 : 플레이 버튼 클릭 시
    /// </summary>
    public void ClickPlayBt()
    {
        //player화면 --> 캐릭터생성 --> 캐릭터 선택ui 비활성화
        GameObject _play = Instantiate(_playUI);
        _play.GetComponent<Canvas>().worldCamera = Camera.main;
        _clonePlayer.ClonePlayerObject();
        Destroy(gameObject);
    }


    //title에서 먼저 그리기
    public void InitScreen()
    {
        for(int i =1; i<DataManager.instance.achieveData.getPlayer.Length; i++)
        {
            if (!DataManager.instance.achieveData.getPlayer[i])
            {
                CheckOpenPlayer(i);
            }
        }
    }
    private void CheckOpenPlayer(int _playerIndex)
    {
        switch (_playerIndex)
        {
            case 1: CheckOpenMude(); break;
            case 2: CheckOpenNinja(); break;    
            case 3: CheckOpenVirtual(); break;
            default: break;
        }

        Image _playerImg = _selectPlayerBt[_playerIndex].transform.GetChild(1).GetComponent<Image>();

        if (!_canOpenPlayer[_playerIndex])
        {
            _selectPlayerBt[_playerIndex].interactable= false;
            _playerImg.color = new Color(0, 0, 0);
        }
        else
        {
            _playerImg.color = new Color(0.7f, 0.7f, 0.7f);
        }
    }

    #region initHero
    private void CheckOpenMude()
    {
        int _totalKillMonster = 0;
        for (int j = 0; j < DataManager.instance.achieveData.killMonsterCnt.Length; j++)
        { _totalKillMonster += DataManager.instance.achieveData.killMonsterCnt[j]; }

        if (_totalKillMonster > 50) {
            _canOpenPlayer[1] = true;
        }
    }
    private void CheckOpenNinja()
    {
        if(DataManager.instance.achieveData.playCnt >= 10)
        {
            _canOpenPlayer[2]= true;
        }
    }
    private void CheckOpenVirtual()
    {
        if (DataManager.instance.achieveData.maxScore >= 10000)
        {
            _canOpenPlayer[3] = true;
        }
    }
    #endregion

    #region purchaseUI

    /// <summary>
    /// 기능 : 구매 골드 출력, 구매금액보다 보유금액이 적을 경우 구매버튼 비활성화
    /// 호출 : SelectPlayerUI에서 구매되지 않는 캐릭터 선택 시
    /// </summary>
    /// <param name="_playerIndex"></param>
    public void PurchaseUISetting(int _playerIndex)
    {
        _selectPlayerIndex = _playerIndex;
        _purchaseGold = DataManager.instance.playerStatDataList[_playerIndex].gold;
        _purchasegoldTx.text = $"{_purchaseGold} GOLD";

        if (PlayerPrefs.GetInt("Gold") < _purchaseGold)
        {
            _purchaseBt.interactable = false;
        }
        else
        {
            _purchaseBt.interactable = true;
        }
    }

    //구매여부
    public void ClickYes()
    {
        StartCoroutine(SpendMoneyEffect(PlayerPrefs.GetInt("Gold") - _purchaseGold, PlayerPrefs.GetInt("Gold")));
        PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - _purchaseGold);
        DataManager.instance.achieveData.getPlayer[_selectPlayerIndex] = true;
        ClickSelectPlayerBt(_selectPlayerIndex);
        _purchaseUI.SetActive(false);

    }
    //구매여부
    public void ClickNo()
    {
        _purchaseUI.SetActive(false);
    }

    //구매시 골드 차감 효과
    IEnumerator SpendMoneyEffect(int _curGold, int befGold) 
    {
        while (befGold >_curGold)
        {
            befGold -=10;
            _goldTx.text = befGold.ToString();
            yield return new WaitForSeconds(0.01f);
        }
    }

    #endregion
}
