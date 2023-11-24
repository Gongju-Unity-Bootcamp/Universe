using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기능: 몬스터가 랜덤하게 총알 발사
/// </summary>
public class MonsterShoot : MonoBehaviour
{
    public GameObject bullet;

    private void Start()
    {
        //총알 발사 메소드 호출
        Invoke(nameof(bulletSpawn), 2f);
    }
    void bulletSpawn()
    {
        // bullet 복제 및 위치
        GameObject newobject = Instantiate(bullet, transform.position, bullet.transform.rotation);

        //총알 발사 간격
        float _shootTime = Random.Range(0.1f, 2f);
        //총알 발사 메소드 호출
        Invoke(nameof(bulletSpawn), _shootTime);
    }
}
