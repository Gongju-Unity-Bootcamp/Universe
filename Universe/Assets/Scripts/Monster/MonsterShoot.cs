using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���: ���Ͱ� �����ϰ� �Ѿ� �߻�
/// </summary>
public class MonsterShoot : MonoBehaviour
{
    public GameObject bullet;

    private void Start()
    {
        //�Ѿ� �߻� �޼ҵ� ȣ��
        Invoke(nameof(bulletSpawn), 2f);
    }
    void bulletSpawn()
    {
        // bullet ���� �� ��ġ
        GameObject newobject = Instantiate(bullet, transform.position, bullet.transform.rotation);

        //�Ѿ� �߻� ����
        float _shootTime = Random.Range(0.1f, 2f);
        //�Ѿ� �߻� �޼ҵ� ȣ��
        Invoke(nameof(bulletSpawn), _shootTime);
    }
}
