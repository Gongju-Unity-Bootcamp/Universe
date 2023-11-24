using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ��� : ���͸� �ֱ������� ����
/// ȣ�� : GamePlayUI���� ī��Ʈ�� ���� �� ȣ��
/// </summary>
public class MonsterSpawn : MonoBehaviour
{
    //List�� �̿��ؼ� ����Ƽ �ν�����â�� �������� ���� ������ ����
    public List<GameObject> _MonsterPrefabList = new List<GameObject>();
    //���� ���� x��ǥ
    private float _randomX;

    //���� óġ�ؾ��ϴ� �� ����Ʈ
    private int[] _monsterCountList;
    //�����ؾ��ϴ� ���� ����Ʈ
    private List<int> _monsterCloneList;
    //���� ����
    private float _randomTime;
    //���� ���� ����
    private int _round;

    //ī��Ʈ �ִϸ��̼��� ���� �� ȣ��
    public void CallSpawn()
    {
        Invoke(nameof(RepeatClone), 1f);
    }
     
    private void RepeatClone()
    {
        //���� ���� �޾ƿ���
        _round = DataManager.instance.playData.round;
        //���� �����迭 ����Ʈ�� �޾ƿ��� (���� ���� �Ϸ�� ������ ���Ҹ� ����� ����)
        _monsterCloneList = DataManager.instance.roundDataList[_round - 1].monsterList.ToList();
        int _monsterIndex = _monsterCloneList.Count;
        //�ش� ���忡 �����ؾ��ϴ� ���͸���Ʈ�� �ε��� �������� �޾ƿ���
        int _monsterRandomIndex = Random.Range(0, _monsterIndex);
        //�����ִ� ���� óġ ���� ���ͺ��� �޾ƿ���
        _monsterCountList = DataManager.instance.roundDataList[_round-1].monsterCount;

        //������ ���� óġ ���� 0�� ���
        if(_monsterCountList[_monsterRandomIndex] ==0)
        {
            //���� ���� ���� �ε��� ����Ʈ�� ���� ������ 1���� ���
            //��� ������ óġ�� �Ϸ�Ǿ����Ƿ� round����
            if(_monsterIndex == 1) { CheckRound(); return; }

            //Ư�� ���Ͱ� ��� óġ �Ǿ��� ��� �����ؾ��ϴ� �ε��� ����Ʈ����
            //�ش� ���� ����
            _monsterCloneList.Remove(_monsterRandomIndex);
            //��� ȣ�� (�ٸ� ���͸� �����ϱ�)
            RepeatClone();
            return;
        }

        //���� ������ǥ �������� �޾ƿ���
        _randomX = Random.Range(-2.2f, 2.2f);
        //��ȿ�� ������ ��� ����
        Instantiate(_MonsterPrefabList[_monsterCloneList[_monsterRandomIndex]], new Vector3(_randomX, gameObject.transform.position.y, 0f), transform.rotation);

        float _callTime = Random.Range(0.1f, 1f);
        //���ӿ��� ���°� �ƴ϶�� ����ؼ� ���� ����
        if(!DataManager.instance.playData.gameover)
            Invoke(nameof(RepeatClone), _callTime);

    }
    //���� ������Ű��
    private void CheckRound()
    {
        //���� ����
        DataManager.instance.playData.round++;
        //���� ���� �Լ� ȣ��
        Invoke(nameof(RepeatClone), 2f);
    }

}
