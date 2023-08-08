using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierControlSystem : MonoBehaviour
{
    //��������I�u�W�F�N�g�Ƃ��̌��ƂȂ�v���n�u
    [SerializeField]
    private GameObject barrierPrefab;
    private GameObject barrier;

    //barrier�𑩂˂�I�u�W�F�N�g
    [SerializeField]
    private GameObject barrierRotateCenterObj;

    //barrierRotateCenterObj����]����ۂ̊�ƂȂ鎲
    private Vector3 axis;
    //barrierRotateCenterObj�̍��W
    private Vector3 centerPos;
    //axis�����肷��ۂɗp������_�̍��W(�͈͓����璊�I)
    private Vector3 lotteryPos;

    //lottryPos�𒊑I����ۂɗp������͈�(����)�̔��a
    [SerializeField]
    private float _radius;
    //axis��V���Ɍ��肷��҂�����
    [SerializeField]
    private float _rotateAxisChangeTime;
    //�������ꂽbarrier�̐e�I�u�W�F�N�g
    [SerializeField]
    private GameObject parentObj;
    //������]������X�s�[�h
    [SerializeField]
    private float _rotateSpeed;
    //��]��^���邽�߂�Quaternion
    private Quaternion angleAxis;


    // Start is called before the first frame update
    void Start()
    {
        GenerateBarrier();
        StartCoroutine("ChangeRotateAxis");
        StartCoroutine("RotateAroundEnemy");
    }


    //axis��ύX
    IEnumerator ChangeRotateAxis()
    {
        for (; ; )
        {
            LotteryAxisPos();
            yield return new WaitForSeconds(_rotateAxisChangeTime);
        }
    }

    //lotteryPos�𒊑I����centerPos��lotteryPos�����񂾃����_���Ȏ��̍쐬
    private void LotteryAxisPos()
    {
        centerPos = barrierRotateCenterObj.transform.position;
        //���I
        lotteryPos = _radius * Random.insideUnitSphere;
        lotteryPos = lotteryPos + centerPos;
        //���̌���
        axis = lotteryPos - centerPos;
        axis = axis.normalized;
    }


    //parentObj����8����8��barrier�𐶐�
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

    //�C�ӂ̊p�x���݂̍��W
    Vector3 SnapAngle(Vector3 vector, float angleSize)
    {
        var angle = Mathf.Atan2(vector.x, vector.z);

        var index = Mathf.RoundToInt(angle / angleSize);

        var snappedAngle = index * angleSize;

        int scale = 4;

        return new Vector3(Mathf.Cos(snappedAngle) * scale, 0, Mathf.Sin(snappedAngle)*scale);
    }


    //axis�����ɂ��̃I�u�W�F�N�g����]
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
