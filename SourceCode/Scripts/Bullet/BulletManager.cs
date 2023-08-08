using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletManager : MonoBehaviour
{
    //�A�^�b�`����Ă���I�u�W�F�N�g���^����_���[�W��
    public static int _damage;

    //�A�^�b�`����Ă���I�u�W�F�N�g�̃^�O
    private string tagName;


    private void Awake()
    {
        decideDamage();
    }


    //�A�^�b�`����Ă���I�u�W�F�N�g�̃^�O�Ń_���[�W�ʌ���
    private void decideDamage()
    {
        tagName = gameObject.tag;

        switch (tagName)
        {
            case "Normal":
                _damage = 50;
                break;

            case "Spread":
                _damage = 30;
                break;

            case "Freeze":
                _damage = 0;
                break;

            case "ChildBullet":
                _damage = 10;
                break;

            case "EnemyBullet":
                _damage = 50;
                break;
        }
    }


    //����̃I�u�W�F�N�g�ɏՓ˂������ɂ��̃I�u�W�F�N�g��j��
    private void OnTriggerEnter(Collider other)
    {
        //trigger�����I�u�W�F�N�g�^�O��range��noEntryArea�ł���Αf�ʂ�
        //range�͕ǂɐڂ��Ă��邩,noEntryArea��enemy�G�̈��͈͓�����������̃I�u�W�F�N�g
        if (other.gameObject.tag == "range" || other.gameObject.tag == "noEntryArea")
        {
            return;
        }

        //ChildBullet�̂ݐ�������s����j���ChildBulletManager�ōs��
        if(this.gameObject.tag == "ChildBullet")
        {
            return;
        }

        Destroy(this.gameObject);
    }
}
