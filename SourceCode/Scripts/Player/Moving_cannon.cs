using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//player�̈ړ�
public class Moving_cannon : MonoBehaviour
{
    //x,z�����ւ̓���
    private float _input_x;
    private float _input_z;

    //�ړ���̃x�N�g��
    private Vector3 direction,movingDirection;
    private Vector3 destination;
    //���݂�plyer�̈ʒu���W
    private Vector3 latestpos;

    //player�I�u�W�F�N�g�̑O�ւƌ��
    [SerializeField]
    private GameObject frontwheel;
    [SerializeField]
    private GameObject rearwheel;

    //�ړ����x
    [SerializeField]
    private float _speed;


    void Start()
    {
        latestpos = transform.position;
        ////�t���[�����[�g�Œ�
        //Application.targetFrameRate = 60;
    }


    // Update is called once per frame
    void Update()
    {
         Moving();
    }


    private void Moving()
    {
        //a,d�L�[��x���̓���
        _input_x = Input.GetAxisRaw("Horizontal");

        //w,s�L�[��z���̓���
        _input_z = Input.GetAxisRaw("Vertical");

        //�ړ��������(direction)�ƃx�N�g��(moving)�̌���
        direction = new Vector3(_input_x, 0, _input_z);

        //�L�[�����͂���Ă��Ȃ����return;
        if (Mathf.Abs(direction.magnitude) == 0)
        {
            return;
        }

        direction.Normalize();

        movingDirection = direction * _speed * Time.deltaTime;


        //�ړ���
        destination = transform.position + movingDirection;

        if(Mathf.Abs(destination.x) >= 30 || Mathf.Abs(destination.z) >= 30)
        {
            return;
        }

        Rotation();
        RotateWheel();

        //latestpos�̍X�V
        latestpos = destination;
    }


    //player���ړ������֌�����
    private void Rotation()
    {
        transform.rotation = Quaternion.LookRotation(movingDirection, Vector3.up);
        transform.position = destination;
    }


    //�ԗւ̉�]
    private void RotateWheel()
    {
        frontwheel.transform.Rotate(new Vector3(240 * Time.deltaTime, 0, 0));
        rearwheel.transform.Rotate(new Vector3(240 * Time.deltaTime, 0, 0));
    }


}
