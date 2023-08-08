using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHitSoundManager : MonoBehaviour
{
    private BulletManager bulletManager;

    //�e�����������ۂ̃T�E���h���Đ����鏈�����s���X�N���v�g
    private SoundPlayManager soundPlayManager;


    private void Start()
    {
        soundPlayManager = this.gameObject.GetComponent<SoundPlayManager>();
    }


    //BulletManager�������Ă���Bullet�I�u�W�F�N�g���Փ˂����ۂɏՓˉ����Đ�
    private void OnTriggerEnter(Collider other)
    {
        //���������I�u�W�F�N�g��bulletManager���擾
        bulletManager = other.gameObject.GetComponent<BulletManager>();

        //bulletManager���擾�ł��Ȃ�������(Bullet�ȊO�ɓ������Ă���ꍇ)
        if (bulletManager == null)
        {
            return;
        }

        //�e�����������ۂ̃T�E���h�̍Đ�
        soundPlayManager.StartCoroutine("SoundPlay");
    }
}
