using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.CompilerServices;

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

        //리스트 할당
        playerStatDataList = new List<PlayerStatData>();
        playData = new PlayData();
        achieveData = new AchieveData();
        monsterDataList = new List<MonsterData>();
        upgradeDataList = new List<UpgradeData>();
        roundDataList = new List<RoundData>();

        LoadFromResourcesData();
    }
    
    //dataManager 인스턴스
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

    //데이터 세팅
    private void LoadFromResourcesData()
    {
        /* 프리팹 목록
         * Gold : 골드 (저장필요)
         * Score: 점수 (저장불필요) 
         * PlayerIndex: 선택한 캐릭터 (저장불필요)
         * Round : 라운드정보 (저장불필요)
         */
        //골드와 점수는 프리팹으로 저장
        if (!PlayerPrefs.HasKey("Gold"))
        {
            PlayerPrefs.SetInt("Gold", 0);
        }
        PlayerPrefs.SetInt("Score", 0);

        //캐릭터 스탯 데이터
        TextAsset _playerdata = Resources.Load("PlayerStatData", typeof(TextAsset)) as TextAsset;
        playerStatDataList = JsonConvert.DeserializeObject<List<PlayerStatData>>(_playerdata.ToString());

        //총알 데이터
        TextAsset _playdata = Resources.Load("PlayData", typeof(TextAsset)) as TextAsset;
        playData = JsonConvert.DeserializeObject<PlayData>(_playdata.ToString());

        //업적 데이터
        TextAsset _achievedata = Resources.Load("AchieveData", typeof(TextAsset)) as TextAsset;
        achieveData = JsonConvert.DeserializeObject<AchieveData>(_achievedata.ToString());

        //몬스터 데이터
        TextAsset _monsterdata = Resources.Load("MonsterData", typeof(TextAsset)) as TextAsset;
        monsterDataList = JsonConvert.DeserializeObject<List<MonsterData>>(_monsterdata.ToString());


        //업그레이드 데이터
        TextAsset _upgradedata = Resources.Load("UpgradeData", typeof(TextAsset)) as TextAsset;
        upgradeDataList = JsonConvert.DeserializeObject<List<UpgradeData>>(_upgradedata.ToString());


        //라운드 데이터
        TextAsset _roundData = Resources.Load("RoundData", typeof(TextAsset)) as TextAsset;
        roundDataList = JsonConvert.DeserializeObject<List<RoundData>>(_roundData.ToString());

    }

    //데이터 저장
    private void SaveData()
    {
        //업적 데이터 저장
        string _achieveData = JsonConvert.SerializeObject(achieveData);
        File.WriteAllText(Application.persistentDataPath + "/AchieveData.json", _achieveData);

        PlayerPrefs.DeleteKey("Score");
        PlayerPrefs.DeleteKey("PlayerIndex");
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}

#region PlayerStatData
//캐릭터 스탯 데이터
public class PlayerStatData
{
    public PlayerStatData(string _name, int _level, float _hp, float _speed, float _str, float _attackSpeed, float _bulletIndex) {
        name = _name; level = _level; hp = _hp; speed = _speed; attackSpeed = _attackSpeed; bulletIndex = _bulletIndex;
    }
    public string name;
    public int level;
    public float hp, speed, str, attackSpeed, bulletIndex;
}
#endregion

#region PlayData
//총알 데이터
public class PlayData
{
    public PlayData()
    {

    }
    public PlayData(int _level,int _exe,int _bulletCnt, float _bulletSize)
    {
        level = _level; exe = _exe; bulletCnt = _bulletCnt; bulletSize = _bulletSize;
    }
    public int level,exe,bulletCnt;
    public float bulletSize;
}
#endregion

#region AchieveData
//업적 달성 데이터
public class AchieveData
{
    public int maxScore, playCnt;
    public bool[] getPlayer;
    public int[] killMonsterCnt;
}
#endregion

#region MonsterData
//몬스터 정보
public class MonsterData
{
    public MonsterData(string _name, float _hp, float _str, float _speed, float _attackSpeed)
    {
        name = _name; hp = _hp; str = _str; speed =_speed; attackSpeed = _attackSpeed;
    }
    public string name;
    public float hp, str, speed,attackSpeed;
}
#endregion

#region UpgradeData
//업그레이드 정보
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
public class RoundData {
    public RoundData(int _round, int[] _monsterCount)
    {
        round = _round; monsterCount = _monsterCount;
    }
    public int round;
    public int[] monsterCount;
}
#endregion