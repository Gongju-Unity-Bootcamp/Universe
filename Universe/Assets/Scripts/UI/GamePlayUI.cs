using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class GamePlayUI : MonoBehaviour
{
    public GameObject _countFrame;
    public TMP_Text _countTx;
    public TMP_Text _scoreTx;
    // Start is called before the first frame update
    void Start()
    {
        _scoreTx.text = PlayerPrefs.GetInt("Score").ToString();
        StartCoroutine(nameof(CountAni));
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
    }
}
