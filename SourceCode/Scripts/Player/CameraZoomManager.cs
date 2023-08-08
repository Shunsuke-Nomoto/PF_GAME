using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomManager : MonoBehaviour
{
    //player�I�u�W�F�N�g
    [SerializeField]
    private GameObject player;

    //�J�����̍��W
    private Vector3 cameraPos;

    //�Y�[�������ۂ�player�I�u�W�F�N�g�Ƃ̋���
    [SerializeField]
    private float _zoomDis;

    //player�I�u�W�F�N�g�������Ă������
    private Vector3 playerAngle;


    private void ZoomIn()
    {
        cameraPos = new Vector3(player.transform.position.x, 0.7f, player.transform.position.z - _zoomDis);

        this.transform.position = cameraPos;
    }

  
    //����script���L���ɂȂ����Ƃ��A�J�����̌�����player�̌����ɍ��킹��
    private void OnEnable()
    {
        ZoomIn();
        playerAngle = player.transform.rotation.eulerAngles;

        this.transform.Rotate(playerAngle);
    }

}
