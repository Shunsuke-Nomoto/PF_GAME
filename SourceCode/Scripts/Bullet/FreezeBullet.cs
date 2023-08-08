using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBullet : MonoBehaviour
{
    //enemy�I�u�W�F�N�g�̎ˌ��Ǘ��X�N���v�g
    private EnemyShootingSystem enemyShootingSystem;


    //enemy�I�u�W�F�N�g�ɓ���������,Freeze��Ԃɂł���Ȃ�EnemyShooting�X�N���v�g��Freeze_Shooting�����s
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "enemy" && EnemyShootingSystem._canFreeze == true)
        {
            EnemyShootingSystem._canFreeze = false;

            //enemy�^�O�̃I�u�W�F�N�g�Ƀg���K�[������EnemyShootingSystem���~������
            enemyShootingSystem = other.gameObject.GetComponent<EnemyShootingSystem>();
            enemyShootingSystem.StartCoroutine("Freeze_Shooting");
        }
    }
}
