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
    //객체
    //컴포넌트
    //프로퍼티

    // Update is called once per frame
    void Update()
    {
        //이동..
        _inputHorizontal = Input.GetAxisRaw("Horizontal");
        _inputVertical = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(_inputHorizontal * Time.deltaTime * _horizonSpeed, _inputVertical * Time.deltaTime * _vertiSpeed, 0);

        //총알 발사
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject _bulletObject = Instantiate(_bulletPrefab, _bulletPos.position, _bulletPrefab.transform.rotation);

            _bulletObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * _bulletSpeed);

        }
    }
}
