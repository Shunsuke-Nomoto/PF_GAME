using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHPManager : MonoBehaviour
{
    //���̃I�u�W�F�N�g�̍ő�HP
    [SerializeField]
    private int _MaxHp;

    //�Փ˂����I�u�W�F�N�g������BulletManager
    private BulletManager bulletManager;

    //�_���[�W��
    private int _damage;

    //���݂�HP
    private int _currentHp;

    //�e�����������ۂ̃T�E���h���Đ����鏈�����s���X�N���v�g
    private SoundPlayManager soundPlayManager;


    private void Start()
    {
        //���݂̗̑͂̏�����
        _currentHp = _MaxHp;

        soundPlayManager = this.gameObject.GetComponent<SoundPlayManager>();
    }


    //���݂�HP��0�ȉ��ɂȂ��Ă��邩�𔻒�
    private void HpCheck()
    {
        //�A�^�b�`����Ă���I�u�W�F�N�g�̌��݂�HP(_currentHp)��0�ȉ��ɂȂ�����
        if (_currentHp <= 0)
        {
            Destroy(this.gameObject);
        }
    }


    //�I�u�W�F�N�g���Փ˂�����_���[�W�v�Z
    private void OnTriggerEnter(Collider other)
    {
        DamageCheckCollider(other);
    }


    //trigger�p�̃_���[�W�`�F�b�N(DamageCheckCollider)
    private void DamageCheckCollider(Collider collider)
    {
        //���������I�u�W�F�N�g��bulletManager���擾
        bulletManager = collider.gameObject.GetComponent<BulletManager>();

        //bulletManager���擾�ł��Ȃ�������(Bullet�ȊO�ɓ������Ă���ꍇ)return
        if (bulletManager == null)
        {
            return;
        }

        //�e�����������ۂ̃T�E���h�̍Đ�
        soundPlayManager.StartCoroutine("SoundPlay");

        _damage = BulletManager._damage;

        //HP�̍X�V
        _currentHp = _currentHp - _damage;

        HpCheck();
    }
}
