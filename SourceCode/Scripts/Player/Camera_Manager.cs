
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
    //playerオブジェクト
    [SerializeField]
    private GameObject player;

    //z軸の任意の距離
    [SerializeField]
    private float _cameraDistance;

    //playerオブジェクトの座標と_cameraDistanceをとった座標
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


    //_cameraDistanceを保ってplayerを追従
    private void Cameramoving()
    {
        camerapos = new Vector3(player.transform.position.x, 0.7f, player.transform.position.z - _cameraDistance);

        //動いていなかったらreturn
        if (Mathf.Abs(player.transform.position.z - _cameraDistance) == 0f)
        {
            return;
        }

        transform.position = camerapos;
    }


    //メインカメラはズーム時状態を解除したとき常にz軸正方向を向く
    private void OnEnable()
    {
        this.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
    }

}
