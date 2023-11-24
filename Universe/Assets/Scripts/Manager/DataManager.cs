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

        //리스트 할당
        playerStatDataList = new List<PlayerStatData>();
        playData = new PlayData();
        achieveData = new AchieveData();
        monsterDataList = new List<MonsterData>();
        upgradeDataList = new List<UpgradeData>();
        roundDataList = new List<RoundData>();


        PlayerPrefs.SetInt("Gold",30000);
        //1일 경우 플레이 기록이 존재 , 0일 경우 처음 플레이
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
         * PlayerIndex: 선택한 캐릭터 (저장불필요)
         */
        //골드와 점수는 프리팹으로 저장
        if (!PlayerPrefs.HasKey("Gold"))
        {
            PlayerPrefs.SetInt("Gold", 0);
        }

        //캐릭터 스탯 데이터
        TextAsset _playerdata = Resources.Load("PlayerStatData", typeof(TextAsset)) as TextAsset;
        playerStatDataList = JsonConvert.DeserializeObject<List<PlayerStatData>>(_playerdata.ToString());

        //총알 데이터
        TextAsset _playdata = Resources.Load("PlayData", typeof(TextAsset)) as TextAsset;
        playData = JsonConvert.DeserializeObject<PlayData>(_playdata.ToString());

        //몬스터 데이터
        TextAsset _monsterdata = Resources.Load("MonsterData", typeof(TextAsset)) as TextAsset;
        monsterDataList = JsonConvert.DeserializeObject<List<MonsterData>>(_monsterdata.ToString());


        //업그레이드 데이터
        TextAsset _upgradedata = Resources.Load("UpgradeData", typeof(TextAsset)) as TextAsset;
        upgradeDataList = JsonConvert.DeserializeObject<List<UpgradeData>>(_upgradedata.ToString());


        //라운드 데이터
        TextAsset _roundData = Resources.Load("RoundData", typeof(TextAsset)) as TextAsset;
        roundDataList = JsonConvert.DeserializeObject<List<RoundData>>(_roundData.ToString());
        
        //1일 경우 플레이 기록이 존재 , 0일 경우 처음 플레이
        if (PlayerPrefs.HasKey("Save"))
        {
            string _achieveData = File.ReadAllText(Application.persistentDataPath + "/AchieveData.json");
            achieveData = JsonConvert.DeserializeObject<AchieveData>(_achieveData.ToString());

        }
        else
        {
            //업적 데이터
            TextAsset _achieveData = Resources.Load("AchieveData", typeof(TextAsset)) as TextAsset;
            achieveData = JsonConvert.DeserializeObject<AchieveData>(_achieveData.ToString());
        }
    }

    //게임 재시작 시 데이터 새로 세팅
    public void ReplayGame()
    {
        PlayerPrefs.DeleteKey("PlayerIndex");
        //캐릭터 스탯 데이터
        TextAsset _playerdata = Resources.Load("PlayerStatData", typeof(TextAsset)) as TextAsset;
        playerStatDataList = JsonConvert.DeserializeObject<List<PlayerStatData>>(_playerdata.ToString());

        //총알 데이터
        TextAsset _playdata = Resources.Load("PlayData", typeof(TextAsset)) as TextAsset;
        playData = JsonConvert.DeserializeObject<PlayData>(_playdata.ToString());

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

        PlayerPrefs.DeleteKey("PlayerIndex");
    }

    //게임 강제 종료 시 데이터 저장
    private void OnApplicationPause(bool pause)
    {
        if (pause) {
            if (!PlayerPrefs.HasKey("Save"))
                PlayerPrefs.SetInt("Save", 1);
            SaveData();
        }
    }

    //게임 종료 시 데이터 저장
    private void OnApplicationQuit()
    {
        if (!PlayerPrefs.HasKey("Save"))
            PlayerPrefs.SetInt("Save", 1);        
        SaveData();
    }

}

#region PlayerStatData
//캐릭터 스탯 데이터
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
//총알 데이터
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
//업적 달성 데이터
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
//몬스터 정보
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
//라운드 정보
public class RoundData {
    public RoundData(int _round, int[] _monsterCount, int[] _monsterList)
    {
        round = _round; monsterCount = _monsterCount; monsterList =_monsterList;
    }
    public int round;
    public int[] monsterCount, monsterList;
}
#endregion