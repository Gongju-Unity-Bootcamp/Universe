using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���: 
/// 
/// </summary>
public class MonsterController : MonoBehaviour
{
    // ������ ����
    [SerializeField]
    private int _monsterIndex;
    // ������ �̵��ӵ�
    private float _monsterMoveSpeed;
    // ������ ���� ü��
    private float _curHp;
    // �ִϸ�����
    private Animator _animator;
    // ������ ������ ���ɿ���
    private bool _canMove = true;
    // ����ġ ������
    public GameObject _exePrefab;
    // ���� �÷����ϴ� ĳ������ ����
    private int _playerIndex;

    //�Ѿ˹߻� ����
    private float _randomY;

    //���� ����
    private int _round;

    // Start is called before the first frame update
    void Start()
    {
        //DataManager�� ���� MonsterData.json�� �ش� ���� speed ���� �����´�.
        _monsterMoveSpeed = DataManager.instance.monsterDataList[_monsterIndex].speed;
        //DataManager�� ���� MonsterData.json�� �ش� ���� ü�� ���� �����´�.
        _curHp = DataManager.instance.monsterDataList[_monsterIndex].hp;
        _animator = GetComponent<Animator>();
        //PlayerPrefabs.PlayerIndex �� �������� !! ���� �ʿ�
        _playerIndex = PlayerPrefs.GetInt("PlayerIndex");

        //�Ѿ� ���� y��ǥ ��ġ (�������� ����)
        _randomY = Random.Range(2, 5);
    }

    void Update()
    {
        //���Ͱ� ������ �� �ִ� ��Ȳ�̶��
        if (_canMove)
        {
            //���� ������ ���� �̵������ �ٸ��Ƿ� �ش��ϴ� �޼ҵ� ȣ��
            if (_monsterIndex >4) { }
            else if (_monsterIndex > 1) { ShootMonsterMove(); }
            else { BasicMonsterMove(); }
        }
        //���� ���� ���¶�� ������ ���� ����
        //DataManager�� ���� PlayData.json�� gameonver �� ��������
        if (DataManager.instance.playData.gameover)
        {
            Destroy(gameObject);
        }
    }

    #region monstermMove
    //�Ѿ� ������ ������
    private void ShootMonsterMove()
    {
        transform.position += new Vector3(0, (-1) * Time.deltaTime * _monsterMoveSpeed * 2f, 0);
        //������ ��ġ�� ������ y��ġ���� �Ʒ��� ��� �̵� ����
        if (transform.position.y <= _randomY)
        {
            _canMove = false;
        }
    }

    //�⺻ ������ ������
    private void BasicMonsterMove()
    {
        transform.position += new Vector3(0, (-1) * Time.deltaTime * _monsterMoveSpeed, 0);
        //������ ��ġ�� -5f �Ʒ��� ��� ���� ����
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�÷��̾� �Ѿ˿� �浹�� ���
        if (collision.CompareTag("PlayerBullet"))
        {
            //�ߺ� �浹������ �����ϱ� ���� �Ѿ� �±� ����
            collision.gameObject.tag = "Destroy";
            //�ǰݻ��·� ��ȯ
            Attacked();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //�÷��̾� �Ѿ��� �ε��� �� �������� �Ϲ� ���� �̵����ɻ��·� ��ȯ
        if (collision.CompareTag("PlayerBullet"))
        {
            if (_monsterIndex < 2) { _canMove = true; }
        }
    }
    #endregion

    //ü�� ����, �ǰ� �ִ� ����
    private void Attacked()
    {
        //�Ϲ� ���ʹ� �ǰ� �� ���� ���� �̵�
        if (_monsterIndex < 2)
        {
            transform.position += new Vector3(0, 0.2f, 0);
        }

        //ü�°��� (������ ����ü�¿��� �÷��̾��� ���ݷ¸�ŭ ����)
        _curHp -= DataManager.instance.playerStatDataList[_playerIndex].str;

        //���ҵ� ü���� 0�̻��� ��� �ǰ� ����
        if (_curHp > 0)
        {
            //�ǰ� �ִ�
            _animator.SetTrigger("Attacked");
            Invoke(nameof(CanMove),0.5f);
        }
        //ü���� ���Ǿ����� ����
        else
        {
            //�Ϲݸ��ʹ� �̵����� �����̹Ƿ� �����ϼ� ���� ���·� �ٲ���
            if (_monsterIndex < 2) { _canMove = false; }
            _animator.SetTrigger("Die");
            gameObject.tag = "Destroy";
            //���� ���� 
            DataManager.instance.playData.score += 100 + _monsterIndex * 50;
            //������ ���θ��� �� ����
            DataManager.instance.achieveData.killMonsterCnt[_monsterIndex]++;

            _round = DataManager.instance.playData.round;
            //���� ������ óġ�ؾ��ϴ� ���� �� ����
            DataManager.instance.roundDataList[_round - 1].monsterCount[_monsterIndex]--;
            //����ġ ������Ʈ ����
            Instantiate(_exePrefab, transform.position, Quaternion.identity);
            //����Ȯ���� ������ ���
            CloneItem();
            //�ǰݾִϸ��̼� Ŭ�� ���� �ð��� ���� �� ������Ʈ �ı�
            Destroy(gameObject, 0.5f);
        }
    }

    //������ �� ���� ������ ��� ȣ��
    private void CanMove(){
        _canMove =true;
    }

    //������ ���� �޼ҵ�
    private void CloneItem(){

        int _itemDropPercent = Random.Range(0,1001);
        if(_itemDropPercent >950){
        //ItemSpawner�� _itemSpawner�� ȣ���Ͽ� ������ ����
        ItemSpawner _itemSpawner = new ItemSpawner();
        _itemSpawner.CloneItem(transform); 
        }
    }
}
