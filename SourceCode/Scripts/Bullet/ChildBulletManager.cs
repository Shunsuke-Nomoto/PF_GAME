using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBulletManager: MonoBehaviour
{
    //SpreadBullet�ɂ���Đ������ꂽ���̃I�u�W�F�N�g(ChildBullet)��BulletManager�ł͂Ȃ��w��b���Ŕj��
    //������W�ŕ�����ChildBullet�𐶐����邽��BulletManager�ł̔j�󂪍���
    void Start()
    {
        Destroy(this.gameObject,1.0f);
    }

}
