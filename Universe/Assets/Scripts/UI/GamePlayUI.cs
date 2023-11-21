using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{
    public GameObject _countFrame;
    public TMP_Text _countTx;
    public TMP_Text _scoreTx;
    public GameObject _gameoverUI;

    //경험치
    public Slider _exeSlider;
    public TMP_Text _exeTx;
    private MonsterSpawn _monsterSpawn;
    private int _playerLevel;
    private int _currExe;
    private int _playerIndex;
    private bool _callGameover = false;


    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Round", 1);
        _scoreTx.text = DataManager.instance.playData.score.ToString();
        _monsterSpawn = FindAnyObjectByType<MonsterSpawn>();
        _playerIndex = PlayerPrefs.GetInt("PlayerIndex");

        StartCoroutine(nameof(CountAni));
    }

    private void Update()
    {
        if (!_callGameover)
        {
            UpdateScore();
            SliderManager();
            CheckGameOver();
        }
    }

    IEnumerator CountAni()
    {
        yield return new WaitForSeconds(1.5f);
        _countFrame.SetActive(true);
        for(int i=3; i>=1; i--)
        {
            _countTx.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        _countTx.text = "GO!";
        yield return new WaitForSeconds(1f);
        _countFrame.SetActive(false);
        _monsterSpawn.CallSpawn();
    }

    //점수 업데이트
    private void UpdateScore()
    {
        _scoreTx.text = DataManager.instance.playData.score.ToString();
    }

    //현재 경험치 슬라이드 업데이트
    private void SliderManager()
    {
        _playerLevel = DataManager.instance.playData.level;
        _currExe = DataManager.instance.playData.exe;

        //경험치 슬라이드 조정
        int _levelUPExe = (int)Mathf.Pow(_playerLevel, 2) * 5;
        _exeTx.text = $"{_currExe}/{_levelUPExe}";
        if (_currExe != 0)
        {
            _exeSlider.value = (float)_currExe / _levelUPExe;
        }
        else
        {
            _exeSlider.value = 0;
        }
    }

    //게임 상태 확인
    private void CheckGameOver()
    {
        if (DataManager.instance.playerStatDataList[_playerIndex].hp <= 0)
        {
            Invoke(nameof(ActiveGameOver), 1f);
            _callGameover = true;
        }
    }

    private void ActiveGameOver()
    {
        GameObject _cloneGameoverUI = Instantiate(_gameoverUI);
        _cloneGameoverUI.GetComponent<Canvas>().worldCamera = Camera.main;
        Destroy(gameObject);
    }
}
