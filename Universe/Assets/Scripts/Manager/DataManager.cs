using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Collections;

public class DataManager : MonoBehaviour
{
    public static DataManager instance = null;

    public List<PlayerStatData> playerStatDataList;
    public PlayData playData;
    public AchieveData achieveData;
    public List<MonsterData> monsterDataList;
    public List<UpgradeData> upgradeDataList;
    public List<RoundData> roundDataList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this.gameObject)
            Destroy(gameObject);

        //����Ʈ �Ҵ�
        playerStatDataList = new List<PlayerStatData>();
        playData = new PlayData();
        achieveData = new AchieveData();
        monsterDataList = new List<MonsterData>();
        upgradeDataList = new List<UpgradeData>();
        roundDataList = new List<RoundData>();


        PlayerPrefs.SetInt("Gold",30000);
        //1�� ��� �÷��� ����� ���� , 0�� ��� ó�� �÷���
        LoadFromResourcesData();
    }
    
    //dataManager �ν��Ͻ�
    public static DataManager Instance
    {
        get
        {
            if(null==instance)
            {
                return null;
            }
            return instance;
        }
    }

    //������ ����
    private void LoadFromResourcesData()
    {
        /* ������ ���
         * Gold : ��� (�����ʿ�)
         * PlayerIndex: ������ ĳ���� (������ʿ�)
         */
        //���� ������ ���������� ����
        if (!PlayerPrefs.HasKey("Gold"))
        {
            PlayerPrefs.SetInt("Gold", 0);
        }

        //ĳ���� ���� ������
        TextAsset _playerdata = Resources.Load("PlayerStatData", typeof(TextAsset)) as TextAsset;
        playerStatDataList = JsonConvert.DeserializeObject<List<PlayerStatData>>(_playerdata.ToString());

        //�Ѿ� ������
        TextAsset _playdata = Resources.Load("PlayData", typeof(TextAsset)) as TextAsset;
        playData = JsonConvert.DeserializeObject<PlayData>(_playdata.ToString());

        //���� ������
        TextAsset _monsterdata = Resources.Load("MonsterData", typeof(TextAsset)) as TextAsset;
        monsterDataList = JsonConvert.DeserializeObject<List<MonsterData>>(_monsterdata.ToString());


        //���׷��̵� ������
        TextAsset _upgradedata = Resources.Load("UpgradeData", typeof(TextAsset)) as TextAsset;
        upgradeDataList = JsonConvert.DeserializeObject<List<UpgradeData>>(_upgradedata.ToString());


        //���� ������
        TextAsset _roundData = Resources.Load("RoundData", typeof(TextAsset)) as TextAsset;
        roundDataList = JsonConvert.DeserializeObject<List<RoundData>>(_roundData.ToString());
        
        //1�� ��� �÷��� ����� ���� , 0�� ��� ó�� �÷���
        if (PlayerPrefs.HasKey("Save"))
        {
            string _achieveData = File.ReadAllText(Application.persistentDataPath + "/AchieveData.json");
            achieveData = JsonConvert.DeserializeObject<AchieveData>(_achieveData.ToString());

        }
        else
        {
            //���� ������
            TextAsset _achieveData = Resources.Load("AchieveData", typeof(TextAsset)) as TextAsset;
            achieveData = JsonConvert.DeserializeObject<AchieveData>(_achieveData.ToString());
        }
    }

    //���� ����� �� ������ ���� ����
    public void ReplayGame()
    {
        PlayerPrefs.DeleteKey("PlayerIndex");
        //ĳ���� ���� ������
        TextAsset _playerdata = Resources.Load("PlayerStatData", typeof(TextAsset)) as TextAsset;
        playerStatDataList = JsonConvert.DeserializeObject<List<PlayerStatData>>(_playerdata.ToString());

        //�Ѿ� ������
        TextAsset _playdata = Resources.Load("PlayData", typeof(TextAsset)) as TextAsset;
        playData = JsonConvert.DeserializeObject<PlayData>(_playdata.ToString());

        //���� ������
        TextAsset _monsterdata = Resources.Load("MonsterData", typeof(TextAsset)) as TextAsset;
        monsterDataList = JsonConvert.DeserializeObject<List<MonsterData>>(_monsterdata.ToString());


        //���׷��̵� ������
        TextAsset _upgradedata = Resources.Load("UpgradeData", typeof(TextAsset)) as TextAsset;
        upgradeDataList = JsonConvert.DeserializeObject<List<UpgradeData>>(_upgradedata.ToString());


        //���� ������
        TextAsset _roundData = Resources.Load("RoundData", typeof(TextAsset)) as TextAsset;
        roundDataList = JsonConvert.DeserializeObject<List<RoundData>>(_roundData.ToString());
    }
    //������ ����
    private void SaveData()
    {
        //���� ������ ����
        string _achieveData = JsonConvert.SerializeObject(achieveData);
        File.WriteAllText(Application.persistentDataPath + "/AchieveData.json", _achieveData);

        PlayerPrefs.DeleteKey("PlayerIndex");
    }

    //���� ���� ���� �� ������ ����
    private void OnApplicationPause(bool pause)
    {
        if (pause) {
            if (!PlayerPrefs.HasKey("Save"))
                PlayerPrefs.SetInt("Save", 1);
            SaveData();
        }
    }

    //���� ���� �� ������ ����
    private void OnApplicationQuit()
    {
        if (!PlayerPrefs.HasKey("Save"))
            PlayerPrefs.SetInt("Save", 1);        
        SaveData();
    }

}

#region PlayerStatData
//ĳ���� ���� ������
public class PlayerStatData
{
    public PlayerStatData(string _name, int _level, int _hp, int _maxHp, float _speed, float _str, float _attackSpeed, float _bulletIndex , int _gold) {
        name = _name; level = _level; hp = _hp; maxHp = _maxHp; gold = _gold;
        speed = _speed; attackSpeed = _attackSpeed; bulletIndex = _bulletIndex; 
    }
    public string name;
    public int level, hp, maxHp, gold;
    public float speed, str, attackSpeed, bulletIndex;
}
#endregion

#region PlayData
//�Ѿ� ������
public class PlayData
{
    public PlayData(){}
    public PlayData(int _level, int _exe, int _score, bool _gameover, int _round, int _bulletCnt, float _bulletSize)
    {
        level = _level; exe = _exe; score = _score; round = _round;
        bulletCnt = _bulletCnt; bulletSize = _bulletSize;
        gameover = _gameover;
    }
    public int level,exe,score, round,bulletCnt;
    public float bulletSize;
    public bool gameover;
}
#endregion

#region AchieveData
//���� �޼� ������
public class AchieveData
{
    public AchieveData(){ }
    public AchieveData(int _maxScore, int _playCnt, bool[] _getPlayer, int[] _killMonsterCnt) {
        maxScore = _maxScore; playCnt = _playCnt; getPlayer = _getPlayer; killMonsterCnt = _killMonsterCnt; 
    }
    public int maxScore, playCnt;
    public bool[] getPlayer;
    public int[] killMonsterCnt;
}
#endregion

#region MonsterData
//���� ����
public class MonsterData
{
    public MonsterData(string _name, float _hp, int _str, float _speed, float _attackSpeed)
    {
        name = _name; hp = _hp; str = _str; speed =_speed; attackSpeed = _attackSpeed;
    }
    public string name;
    public float hp, speed,attackSpeed;
    public int str;
}
#endregion

#region UpgradeData
//���׷��̵� ����
public class UpgradeData
{
    public UpgradeData(string _category, int _level, float _upgrade, int _maxLevel) {
        category= _category; level = _level; upgrade = _upgrade; maxLevel = _maxLevel;
    }
    public string category;
    public int level, maxLevel;
    public float upgrade;
}
#endregion

#region RoundData
//���� ����
public class RoundData {
    public RoundData(int _round, int[] _monsterCount, int[] _monsterList)
    {
        round = _round; monsterCount = _monsterCount; monsterList =_monsterList;
    }
    public int round;
    public int[] monsterCount, monsterList;
}
#endregion