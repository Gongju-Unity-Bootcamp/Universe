using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    //List�� �̿��ؼ� ����Ƽ �ν�����â�� �������� ���� ������ ����
    public List<GameObject> _MonsterPrefabList = new List<GameObject>();
    private float _randomX;

    private bool _isFinish = false;

    private int[] _monsterCount;
    private float _randomTime;
    // Start is called before the first frame update
    
    //ī��Ʈ �ִϸ��̼��� ���� �� ȣ��
    public void CallSpawn()
    {
        Invoke(nameof(spawn), 1f);
    }
     
    //���� ������Ʈ ����
    public void spawn()
    {
        _isFinish = false;
        _randomX = Random.Range(-2.2f, 2.2f);


        for(int i = 0; i < _MonsterPrefabList.Count; i++)
        { 
            _monsterCount = DataManager.instance.roundDataList[PlayerPrefs.GetInt("Round") - 1].monsterCount;
            int _monsterSpawnCnt = _monsterCount[i];
            if (_monsterSpawnCnt>0)
            {
                GameObject newObject = Instantiate(_MonsterPrefabList[i], new Vector3(_randomX, gameObject.transform.position.y, 0f), transform.rotation);
                _monsterCount[i]--;
                break;
            }
        }
        //���庰 ���Ͱ� ��� �����Ǿ��ٸ� ���� ����
        CheckRound();

        _randomTime = Random.Range(0.1f, 2f);

        //���ӿ��� ���¶�� �������� ����
        if (!DataManager.instance.playData.gameover)
        { 
            Invoke(nameof(spawn), _randomTime); 
        }
        else
        {
            CancelInvoke();
        }
    }

    private void CheckRound()
    {
        for (int i = 0; i < _MonsterPrefabList.Count; i++)
        {
            if (_monsterCount[i] > 0)
            {
                _isFinish = false;
                return;
            }
        }
        _isFinish = true;
        PlayerPrefs.SetInt("Round", PlayerPrefs.GetInt("Round") + 1);
    }
}
