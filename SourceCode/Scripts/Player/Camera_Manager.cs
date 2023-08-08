
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
    //player�I�u�W�F�N�g
    [SerializeField]
    private GameObject player;

    //z���̔C�ӂ̋���
    [SerializeField]
    private float _cameraDistance;

    //player�I�u�W�F�N�g�̍��W��_cameraDistance���Ƃ������W
    private Vector3 camerapos;


    // Start is called before the first frame update
    void Start()
    {
        camerapos = new Vector3(player.transform.position.x, 0.8f, player.transform.position.z - _cameraDistance);
    }


    private void LateUpdate()
    {
        Cameramoving();
    }


    //_cameraDistance��ۂ���player��Ǐ]
    private void Cameramoving()
    {
        camerapos = new Vector3(player.transform.position.x, 0.7f, player.transform.position.z - _cameraDistance);

        //�����Ă��Ȃ�������return
        if (Mathf.Abs(player.transform.position.z - _cameraDistance) == 0f)
        {
            return;
        }

        transform.position = camerapos;
    }


    //���C���J�����̓Y�[������Ԃ����������Ƃ����z��������������
    private void OnEnable()
    {
        this.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
    }

}
