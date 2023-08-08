using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierControlSystem : MonoBehaviour
{
    //生成するオブジェクトとその元となるプレハブ
    [SerializeField]
    private GameObject barrierPrefab;
    private GameObject barrier;

    //barrierを束ねるオブジェクト
    [SerializeField]
    private GameObject barrierRotateCenterObj;

    //barrierRotateCenterObjが回転する際の基準となる軸
    private Vector3 axis;
    //barrierRotateCenterObjの座標
    private Vector3 centerPos;
    //axisを決定する際に用いられる点の座標(範囲内から抽選)
    private Vector3 lotteryPos;

    //lottryPosを抽選する際に用いられる範囲(球体)の半径
    [SerializeField]
    private float _radius;
    //axisを新たに決定する待ち時間
    [SerializeField]
    private float _rotateAxisChangeTime;
    //複製されたbarrierの親オブジェクト
    [SerializeField]
    private GameObject parentObj;
    //軸を回転させるスピード
    [SerializeField]
    private float _rotateSpeed;
    //回転を与えるためのQuaternion
    private Quaternion angleAxis;


    // Start is called before the first frame update
    void Start()
    {
        GenerateBarrier();
        StartCoroutine("ChangeRotateAxis");
        StartCoroutine("RotateAroundEnemy");
    }


    //axisを変更
    IEnumerator ChangeRotateAxis()
    {
        for (; ; )
        {
            LotteryAxisPos();
            yield return new WaitForSeconds(_rotateAxisChangeTime);
        }
    }

    //lotteryPosを抽選してcenterPosとlotteryPosを結んだランダムな軸の作成
    private void LotteryAxisPos()
    {
        centerPos = barrierRotateCenterObj.transform.position;
        //抽選
        lotteryPos = _radius * Random.insideUnitSphere;
        lotteryPos = lotteryPos + centerPos;
        //軸の決定
        axis = lotteryPos - centerPos;
        axis = axis.normalized;
    }


    //parentObj下に8方位8個のbarrierを生成
    private void GenerateBarrier()
    {
        centerPos = barrierRotateCenterObj.transform.position;

        for(int i = 0; i<3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Vector3 snapped = SnapAngle(new Vector3(i - 1, 0, j - 1), 45f * Mathf.Deg2Rad);
                snapped = snapped + centerPos;
                
                barrier = Instantiate(barrierPrefab, snapped, Quaternion.identity);
                barrier.transform.parent = parentObj.transform;
            }
        }
        
    }

    //任意の角度刻みの座標
    Vector3 SnapAngle(Vector3 vector, float angleSize)
    {
        var angle = Mathf.Atan2(vector.x, vector.z);

        var index = Mathf.RoundToInt(angle / angleSize);

        var snappedAngle = index * angleSize;

        int scale = 4;

        return new Vector3(Mathf.Cos(snappedAngle) * scale, 0, Mathf.Sin(snappedAngle)*scale);
    }


    //axisを軸にこのオブジェクトを回転
    private IEnumerator RotateAroundEnemy()
    {
        for (; ; )
        {
            var rot = this.transform.localRotation;

            angleAxis = Quaternion.AngleAxis(360 / _rotateSpeed, axis);

            this.transform.localRotation = angleAxis * rot;

            yield return new WaitForSeconds(0.1f);
        }
    }
}
