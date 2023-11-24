using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public TMP_Text _gameOverText;
    public TMP_Text _scoreText;
    public TMP_Text _goldText;
    public Button _replayBt;

    public GameObject _selectPlayerUI;

    private int _score;
    private int _gold;
    private bool _speedUp = false;
    private readonly Vector3 _textScale = new Vector3(1.1f, 1.1f, 1.1f);

    private void Start()
    {
        Time.timeScale = 1;

        _scoreText.text = 0.ToString();
        _goldText.text = 0.ToString();

        DataManager.instance.achieveData.playCnt++;
        StartCoroutine(nameof(GameOverTextEffect));
        StartCoroutine(nameof(ScoreToGold));
    }

    IEnumerator GameOverTextEffect()
    {
        while(true)
        {
            _gameOverText.transform.localScale = _textScale;
            yield return new WaitForSeconds(0.5f);
            _gameOverText.transform.localScale = Vector3.one;
            yield return new WaitForSeconds(0.5f);
        }

    }

    public void ClickReplayBt()
    {
        if(_score> 0) { _speedUp  = true; }

        GameObject _clone = Instantiate(_selectPlayerUI);
        _clone.GetComponent<Canvas>().worldCamera = Camera.main;
        _clone.GetComponent<SelectPlayer>().InitScreen();
        _clone.SetActive(true);
        DataManager.instance.ReplayGame();
        Destroy(gameObject);

    }

    IEnumerator ScoreToGold()
    {
        _score = DataManager.instance.playData.score;
        PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + _score / 50);
        while (_score > 0)
        {
            _score -= 50;
            _gold += 1;
            _scoreText.text = _score.ToString();
            yield return new WaitForSeconds(0.01f);
            _goldText.text = _gold.ToString();
            yield return new WaitForSeconds(0.01f);
        }

        if(DataManager.instance.playData.score > DataManager.instance.achieveData.maxScore)
        {
            DataManager.instance.achieveData.maxScore = DataManager.instance.playData.score;
        }
        _replayBt.gameObject.SetActive(true);
    }

    private void Reset()
    {
        PlayerPrefs.DeleteKey("PlayerIndex");
    }
}
