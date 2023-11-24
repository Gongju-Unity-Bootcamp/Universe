using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSkillUI : MonoBehaviour
{
    private int[] _skillIndex;
    private int _skillCnt;
    private int _clickBt;

    //스킬 아이콘 이미지 리스트
    public List<Image> _skillIconList;
    public List<TMP_Text> _skillTitleList;
    public List<TMP_Text> _skillLevelTextList;
    public List<Button> _skillFrame;

    public List<Sprite> _skillIconSprite;
    public List<Sprite> _skillFrameSprite;

    public Button _upgradeBt;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        _skillIndex = new int[] {-1,-1,-1};
        _skillCnt = DataManager.Instance.upgradeDataList.Count;

        RandomSKillSet();

        _clickBt = -1;
    }

    public void RandomSKillSet()
    {
        for(int i =0; i< _skillIndex.Length; i++)
        {
            int _random = Random.Range(0,_skillCnt);

            //업그레이드 리스트에 스킬이 중복되어 나오지 않도록 설정
            //스킬의 현재 레벨이 맥스레벨보다 작아야만 함
            if (_skillIndex.Contains(_random)&&
                DataManager.instance.upgradeDataList[_random].level < DataManager.instance.upgradeDataList[_random].maxLevel)
            {
                i--;
            }
            else
            {
                _skillIndex[i] = _random;

                //스킬 타이틀 세팅
                _skillTitleList[i].text = DataManager.instance.upgradeDataList[_skillIndex[i]].category;
                //스킬 프레임 색상 스킬 레벨에 따라 세팅
                _skillFrame[i].GetComponent<Image>().sprite = _skillFrameSprite[DataManager.instance.upgradeDataList[_skillIndex[i]].level - 1];
                //스킬 아이콘
                _skillIconList[i].sprite = _skillIconSprite[_skillIndex[i]];
                //스킬 레벨
                int _nextLevel = DataManager.instance.upgradeDataList[_skillIndex[i]].level + 1;
                _skillLevelTextList[i].text = $"Lv.{_nextLevel}";

            }
        }
    }

    public void ClickSkillBt(int index)
    {
        _clickBt = index;
        _upgradeBt.interactable = true;
    }

    public void ClickUpgradeBt()
    {
        ++DataManager.instance.upgradeDataList[_skillIndex[_clickBt]].level;
        switch (_skillIndex[_clickBt])
        {
            case 0:
                DataManager.instance.playerStatDataList[PlayerPrefs.GetInt("PlayerIndex")].hp
                    += (int)DataManager.instance.upgradeDataList[0].upgrade;
                break;
            case 1:
                DataManager.instance.playerStatDataList[PlayerPrefs.GetInt("PlayerIndex")].str
                    += DataManager.instance.upgradeDataList[1].upgrade;
                break;
            case 2:
                DataManager.instance.playData.bulletCnt += (int)DataManager.instance.upgradeDataList[2].upgrade;
                break;
            case 3: 
                DataManager.instance.playerStatDataList[PlayerPrefs.GetInt("PlayerIndex")].speed
                    += DataManager.instance.upgradeDataList[3].upgrade;
                break;
            case 4:
                DataManager.instance.playerStatDataList[PlayerPrefs.GetInt("PlayerIndex")].attackSpeed
                    += DataManager.instance.upgradeDataList[4].upgrade;
                break;
            case 5:
                DataManager.instance.playData.bulletSize += DataManager.instance.upgradeDataList[5].upgrade;
                break;

            default: break;
        }


        Time.timeScale = 1;
        Destroy(gameObject);
    }

}
