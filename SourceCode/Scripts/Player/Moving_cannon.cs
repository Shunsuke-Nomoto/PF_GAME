using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//playerの移動
public class Moving_cannon : MonoBehaviour
{
    //x,z方向への入力
    private float _input_x;
    private float _input_z;

    //移動先のベクトル
    private Vector3 direction,movingDirection;
    private Vector3 destination;
    //現在のplyerの位置座標
    private Vector3 latestpos;

    //playerオブジェクトの前輪と後輪
    [SerializeField]
    private GameObject frontwheel;
    [SerializeField]
    private GameObject rearwheel;

    //移動速度
    [SerializeField]
    private float _speed;


    void Start()
    {
        latestpos = transform.position;
        ////フレームレート固定
        //Application.targetFrameRate = 60;
    }


    // Update is called once per frame
    void Update()
    {
         Moving();
    }


    private void Moving()
    {
        //a,dキーでx軸の入力
        _input_x = Input.GetAxisRaw("Horizontal");

        //w,sキーでz軸の入力
        _input_z = Input.GetAxisRaw("Vertical");

        //移動する向き(direction)とベクトル(moving)の決定
        direction = new Vector3(_input_x, 0, _input_z);

        //キーが入力されていなければreturn;
        if (Mathf.Abs(direction.magnitude) == 0)
        {
            return;
        }

        direction.Normalize();

        movingDirection = direction * _speed * Time.deltaTime;


        //移動先
        destination = transform.position + movingDirection;

        if(Mathf.Abs(destination.x) >= 30 || Mathf.Abs(destination.z) >= 30)
        {
            return;
        }

        Rotation();
        RotateWheel();

        //latestposの更新
        latestpos = destination;
    }


    //playerを移動方向へ向ける
    private void Rotation()
    {
        transform.rotation = Quaternion.LookRotation(movingDirection, Vector3.up);
        transform.position = destination;
    }


    //車輪の回転
    private void RotateWheel()
    {
        frontwheel.transform.Rotate(new Vector3(240 * Time.deltaTime, 0, 0));
        rearwheel.transform.Rotate(new Vector3(240 * Time.deltaTime, 0, 0));
    }


}
