using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float _inputHorizontal;
    float _inputVertical;

    [SerializeField] private float _horizonSpeed;
    [SerializeField] private float _vertiSpeed;
    [SerializeField] private float _bulletSpeed;

    public GameObject _bulletPrefab;
    public Transform _bulletPos;

    private int _shootSkill;
    private Animator _animator;
    //��ü
    //������Ʈ
    //������Ƽ
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        //�̵�..
        _inputHorizontal = Input.GetAxisRaw("Horizontal");
        _inputVertical = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(_inputHorizontal * Time.deltaTime * _horizonSpeed, _inputVertical * Time.deltaTime * _vertiSpeed, 0);

        //�Ѿ� �߻�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject _bulletObject = Instantiate(_bulletPrefab, _bulletPos.position, _bulletPrefab.transform.rotation);

            _bulletObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * _bulletSpeed);

        }

        if(_inputHorizontal != 0 || _inputVertical != 0)
        {
            int _randomJump = Random.Range(0, 101);
            
            if(_randomJump >95)
            {
                _animator.SetTrigger("Jumping");
                return;
            }
            _animator.SetBool("Walking", true);
        }
        else
        {
            _animator.SetBool("Walking", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            //ü�� ���� �ڵ� �ۼ� �ʿ�
            _animator.SetTrigger("Attacked");
        }
    }
}
