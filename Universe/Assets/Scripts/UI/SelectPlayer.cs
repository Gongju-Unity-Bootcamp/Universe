using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// �ۼ�:2023-11-22  
/// ���:
///     ����ĳ���͸� �����ϴ� ȭ��
///     ĳ���� �ر� : ĳ���͸��� Ư�� ������ �޼��ϸ� �ر�
///     ĳ���� ���� : �رݵ� ĳ���Ϳ� ���ؼ� ��带 �̿��Ͽ� ĳ���� ����
/// ȣ��     
///     1) SelectPlayerUI ������Ʈ
///     2) TitleUI -> ���۹�ư Ŭ�� ��
///     3) GameOverUI -> Replay��ư Ŭ�� ��             
/// </summary>
public class SelectPlayer : MonoBehaviour
{
    #region Create Object
    //button Ŭ�� �� outline ��������
    private string[] _colors = { "#FF00A6FF", "#FFA300FF", "#0BFF00FF", "#00FFFFFF", "#FFFFFFFF" };
    //�ٲ��� �ϴ� �÷�
    Color _changeColor;

    //default outlime color
    const int _defaultColorIndex = 4;
    readonly Vector2 _outLineBorder = new Vector2(4,4);
    readonly Vector2 _defaultOutLineBorder = new Vector2(2, 2);
    
    //���� �÷��� ��ư
    public Button _playBt;
    //���� ĳ���� ��ư����Ʈ (4)
    public List<Button> _selectPlayerBt;
    
    //ClonePlayer ��ü
    private ClonePlayer _clonePlayer;
    //PlayUI ������
    public GameObject _playUI;
    //���
    public TMP_Text _goldTx;

    #region PurchaseUI

    //purchaseUI ������
    public GameObject _purchaseUI;
    //ĳ���� �ر� ���� (4)
    bool[] _canOpenPlayer = { false, false, false,false};
    //���� ĳ���� �ε���
    private int _selectPlayerIndex;
    //ĳ���� ���� ��� 
    private int _purchaseGold;
    //���� ��� �ȳ� �ؽ�Ʈ
    public TMP_Text _purchasegoldTx;
    //�����ϱ� ��ư
    public Button _purchaseBt;
    #endregion

    #endregion

    private void Start()
    {
        //�����Ǿ��ִ� clonePlayer Ŭ���� ȣ��
        _clonePlayer = FindObjectOfType<ClonePlayer>();
        //���� ���� ��� ���
        _goldTx.text = PlayerPrefs.GetInt("Gold").ToString();
    }
    private void Update()
    {
        //player�� ���õǾ��ٸ� �÷��̹�ư ��ȣ�ۿ밡��
        if (PlayerPrefs.HasKey("PlayerIndex"))
            _playBt.interactable = true;
        else
            _playBt.interactable = false;
    }

    /// <summary>
    /// �ۼ�: 2023-11-22
    /// ���
    ///     1) ĳ���� ���ſ��� �ľ��Ͽ� ���ŵ��� ���� ��� ���� �˾�â ���
    ///     2) ���õ� ��ư�� Ȯ���Ͽ� outline ����, ���� ����
    /// ȣ��: ĳ���� ��ư ���� ��
    /// </summary>
    /// <param name="playerIndex"></param>
    public void ClickSelectPlayerBt(int playerIndex)
    {
        //���Ű� ���� ���� ĳ������ ��� ���� �˾�â �ߵ���

        if (!DataManager.instance.achieveData.getPlayer[playerIndex])
        {
            PurchaseUISetting(playerIndex);
            _purchaseUI.SetActive(true);
        }

        //ĳ������ �ε����� ����Ǿ� ���� ��� ������ư �� outline ����
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
    /// ��� : 
    ///     1) Playerȭ�� ����
    ///     2) ĳ���� ����
    ///     3) SelectPlayerUI ����
    /// ȣ�� : �÷��� ��ư Ŭ�� ��
    /// </summary>
    public void ClickPlayBt()
    {
        //playerȭ�� --> ĳ���ͻ��� --> ĳ���� ����ui ��Ȱ��ȭ
        GameObject _play = Instantiate(_playUI);
        _play.GetComponent<Canvas>().worldCamera = Camera.main;
        _clonePlayer.ClonePlayerObject();
        Destroy(gameObject);
    }


    //title���� ���� �׸���
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
    /// ��� : ���� ��� ���, ���űݾ׺��� �����ݾ��� ���� ��� ���Ź�ư ��Ȱ��ȭ
    /// ȣ�� : SelectPlayerUI���� ���ŵ��� �ʴ� ĳ���� ���� ��
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

    //���ſ���
    public void ClickYes()
    {
        StartCoroutine(SpendMoneyEffect(PlayerPrefs.GetInt("Gold") - _purchaseGold, PlayerPrefs.GetInt("Gold")));
        PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - _purchaseGold);
        DataManager.instance.achieveData.getPlayer[_selectPlayerIndex] = true;
        ClickSelectPlayerBt(_selectPlayerIndex);
        _purchaseUI.SetActive(false);

    }
    //���ſ���
    public void ClickNo()
    {
        _purchaseUI.SetActive(false);
    }

    //���Ž� ��� ���� ȿ��
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
