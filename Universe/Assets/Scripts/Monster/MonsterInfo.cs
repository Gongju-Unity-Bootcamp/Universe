using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInfo : MonoBehaviour
{
    [SerializeField]
    private int _monsterIndex;

    public int GetMonsterIndex()
    {
        return _monsterIndex;
    }
}
