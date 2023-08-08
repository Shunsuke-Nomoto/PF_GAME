using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Q�[���J�n���ɃX�e�[�W���BlockWall�I�u�W�F�N�g�𐶐�
public class GenerateStageManager : MonoBehaviour
{
    //��������BlockWall�̌��ƂȂ�v���n�u
    [SerializeField]
    private GameObject wallPrefab;

    //��������ۂ̍��W�̊��enemy
    [SerializeField]
    private GameObject enemy;
    private Vector3 enemyPos;
    //��ƂȂ�BlockWall�̍��W
    private Vector3 centerPos;

    //���ɐ�������ۂ̊p�x
    [SerializeField]
    private float _radius;
    [SerializeField]
    private float angle;
    //���̔{�p
    private float angle2;

    //�e���C����3���������邽�߂̍��W�̃��X�g
    private Vector3[] basisPos = new Vector3[3];
    private Vector3[] pos1radius = new Vector3[3];
    private Vector3[] pos2radius = new Vector3[3];
    private Vector3[] pos_1radius = new Vector3[3];
    private Vector3[] pos_2radius = new Vector3[3];


    // Start is called before the first frame update
    private void Awake()
    {
        GenerateStage();
    }


    //wall�𕡐����邽�߂̍��W�̎Z�o
    //enemyPos����ɔ��aradius�̉~����(���aradius,�p�xangle�̈ʒu)�ɔz�u
    private void CalculateWallPos()
    {
        enemyPos = enemy.transform.position;

        centerPos = new Vector3(enemyPos.x, 0.5f, enemyPos.z);

        angle = angle * Mathf.Deg2Rad;
        angle2 = 2 * angle * Mathf.Deg2Rad;

        for (int i = 0; i < 3; i++)
        {
            basisPos[i] = new Vector3(0, 0, -1 * (i + 1) * _radius) + centerPos; 

            pos1radius[i] = new Vector3((i + 1) * _radius * Mathf.Cos(angle),0, -1 * (i + 1) * _radius * Mathf.Sin(angle)) + centerPos;
            pos2radius[i] = new Vector3((i + 1) * _radius * Mathf.Cos(angle2),0, -1 * (i + 1) * _radius * Mathf.Sin(angle2)) + centerPos; 

            pos_1radius[i] = new Vector3(-1 * (i + 1) * _radius * Mathf.Cos(angle),0, -1 * (i + 1) * _radius * Mathf.Sin(angle)) + centerPos;
            pos_2radius[i] = new Vector3(-1 * (i + 1) * _radius * Mathf.Cos(angle2),0, -1 * (i + 1) * _radius * Mathf.Sin(angle2)) + centerPos;
        }
    }


    //�Z�o�������W�����Ƃ�wallPrefab�𕡐�
    private void GenerateStage()
    {
        CalculateWallPos();

        for (int i = 0; i < 3; i++)
        {
            Instantiate(wallPrefab, basisPos[i], Quaternion.identity);

            Instantiate(wallPrefab, pos1radius[i], Quaternion.identity);
            Instantiate(wallPrefab, pos2radius[i], Quaternion.identity);

            Instantiate(wallPrefab, pos_1radius[i], Quaternion.identity);
            Instantiate(wallPrefab, pos_2radius[i], Quaternion.identity);
        }
    }

}
