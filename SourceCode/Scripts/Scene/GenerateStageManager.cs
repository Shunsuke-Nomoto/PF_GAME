using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ゲーム開始時にステージ上にBlockWallオブジェクトを生成
public class GenerateStageManager : MonoBehaviour
{
    //生成するBlockWallの元となるプレハブ
    [SerializeField]
    private GameObject wallPrefab;

    //生成する際の座標の基準はenemy
    [SerializeField]
    private GameObject enemy;
    private Vector3 enemyPos;
    //基準となるBlockWallの座標
    private Vector3 centerPos;

    //扇状に生成する際の角度
    [SerializeField]
    private float _radius;
    [SerializeField]
    private float angle;
    //その倍角
    private float angle2;

    //各ラインに3つずつ生成するための座標のリスト
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


    //wallを複製するための座標の算出
    //enemyPosを基準に半径radiusの円周上(半径radius,角度angleの位置)に配置
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


    //算出した座標をもとにwallPrefabを複製
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
