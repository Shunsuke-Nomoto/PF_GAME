using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBullet : MonoBehaviour
{
    //enemyオブジェクトの射撃管理スクリプト
    private EnemyShootingSystem enemyShootingSystem;


    //enemyオブジェクトに当たった時,Freeze状態にできるならEnemyShootingスクリプトのFreeze_Shootingを実行
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "enemy" && EnemyShootingSystem._canFreeze == true)
        {
            EnemyShootingSystem._canFreeze = false;

            //enemyタグのオブジェクトにトリガーしたらEnemyShootingSystemを停止させる
            enemyShootingSystem = other.gameObject.GetComponent<EnemyShootingSystem>();
            enemyShootingSystem.StartCoroutine("Freeze_Shooting");
        }
    }
}
