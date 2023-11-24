using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 기능 : 몬스터를 주기적으로 생성
/// 호출 : GamePlayUI에서 카운트가 끝날 때 호출
/// </summary>
public class MonsterSpawn : MonoBehaviour
{
    //List를 이용해서 유니티 인스펙터창에 랜덤으로 넣을 프리팹 설정
    public List<GameObject> _MonsterPrefabList = new List<GameObject>();
    //몬스터 생성 x좌표
    private float _randomX;

    //몬스터 처치해야하는 수 리스트
    private int[] _monsterCountList;
    //생성해야하는 몬스터 리스트
    private List<int> _monsterCloneList;
    //생성 간격
    private float _randomTime;
    //현재 게임 라운드
    private int _round;

    //카운트 애니메이션이 끝난 후 호출
    public void CallSpawn()
    {
        Invoke(nameof(RepeatClone), 1f);
    }
     
    private void RepeatClone()
    {
        //라운드 정보 받아오기
        _round = DataManager.instance.playData.round;
        //몬스터 생성배열 리스트로 받아오기 (추후 생성 완료된 몬스터의 원소를 지우기 위해)
        _monsterCloneList = DataManager.instance.roundDataList[_round - 1].monsterList.ToList();
        int _monsterIndex = _monsterCloneList.Count;
        //해당 라운드에 생성해야하는 몬스터리스트의 인덱스 랜덤으로 받아오기
        int _monsterRandomIndex = Random.Range(0, _monsterIndex);
        //남아있는 몬스터 처치 수를 몬스터별로 받아오기
        _monsterCountList = DataManager.instance.roundDataList[_round-1].monsterCount;

        //생성할 몬스터 처치 수가 0일 경우
        if(_monsterCountList[_monsterRandomIndex] ==0)
        {
            //만약 생성 몬스터 인덱스 리스트의 원소 개수가 1개일 경우
            //모든 몬스터의 처치가 완료되었으므로 round증가
            if(_monsterIndex == 1) { CheckRound(); return; }

            //특정 몬스터가 모두 처치 되었을 경우 생성해야하는 인덱스 리스트에서
            //해당 원소 삭제
            _monsterCloneList.Remove(_monsterRandomIndex);
            //재귀 호출 (다른 몬스터를 생성하기)
            RepeatClone();
            return;
        }

        //몬스터 생성좌표 랜덤으로 받아오기
        _randomX = Random.Range(-2.2f, 2.2f);
        //유효한 몬스터일 경우 복제
        Instantiate(_MonsterPrefabList[_monsterCloneList[_monsterRandomIndex]], new Vector3(_randomX, gameObject.transform.position.y, 0f), transform.rotation);

        float _callTime = Random.Range(0.1f, 1f);
        //게임오버 상태가 아니라면 계속해서 몬스터 생성
        if(!DataManager.instance.playData.gameover)
            Invoke(nameof(RepeatClone), _callTime);

    }
    //라운드 증가시키기
    private void CheckRound()
    {
        //라운드 증가
        DataManager.instance.playData.round++;
        //몬스터 생성 함수 호출
        Invoke(nameof(RepeatClone), 2f);
    }

}
