using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���: ���� �ε��� ��ȯ
/// ȣ��: �÷��̾� �ǰ� �� ������ ���ݷ¸�ŭ ü�� �����ϱ� ���� ������ �ε��� ȣ��
/// </summary>
public class MonsterInfo : MonoBehaviour
{
    [SerializeField]
    private int _monsterIndex;

    public int GetMonsterIndex()
    {
        return _monsterIndex;
    }
}
