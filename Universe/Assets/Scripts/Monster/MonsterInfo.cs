using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기능: 몬스터 인덱스 반환
/// 호출: 플레이어 피격 시 몬스터의 공격력만큼 체력 차감하기 위해 몬스터의 인덱스 호출
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
