using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomManager : MonoBehaviour
{
    //playerオブジェクト
    [SerializeField]
    private GameObject player;

    //カメラの座標
    private Vector3 cameraPos;

    //ズームした際のplayerオブジェクトとの距離
    [SerializeField]
    private float _zoomDis;

    //playerオブジェクトが向いている方向
    private Vector3 playerAngle;


    private void ZoomIn()
    {
        cameraPos = new Vector3(player.transform.position.x, 0.7f, player.transform.position.z - _zoomDis);

        this.transform.position = cameraPos;
    }

  
    //このscriptが有効になったとき、カメラの向きをplayerの向きに合わせる
    private void OnEnable()
    {
        ZoomIn();
        playerAngle = player.transform.rotation.eulerAngles;

        this.transform.Rotate(playerAngle);
    }

}
