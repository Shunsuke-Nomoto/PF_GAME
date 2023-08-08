using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadBulletManager : MonoBehaviour
{
    //�I�u�W�F�N�g�ɓ����������ɐ�������I�u�W�F�N�g�Ƃ��̃v���n�u
    [SerializeField]
    private GameObject childBulletPrefab;
    private GameObject childBullet;

    //ChildBullet��rigidbody
    private Rigidbody childBullet_rb;

    //��������ۂ̈ʒu
    private Vector3 _childBulletPoz;

    //ChildBullet�𐶐�����s�����x����������悤�ɂ���
    private bool spreadBullet_alreadyHit;


    private void Start()
    {
        spreadBullet_alreadyHit = false;
        //SpreadBullet�̂݃I�u�W�F�N�g�ɓ�����Ȃ��Ă��w��b����ChildBullet�𐶐����j��
        Invoke("GenerateChildBullet", 0.75f);
    }


    private void OnTriggerEnter(Collider other)
    {   //trigger�����I�u�W�F�N�g�^�O��range��noEntryArea�ł���Αf�ʂ�
        //range�͕ǂɐڂ��Ă��邩,noEntryArea��enemy�G�̈��͈͓�����������̃I�u�W�F�N�g
        if (other.gameObject.tag == "range" || other.gameObject.tag == "noEntryArea")
        {
            return;
        }

        //���߂ăI�u�W�F�N�g�ɓ��������Ȃ�GenerateChildBullet���s��
        if (spreadBullet_alreadyHit == false)
        {
            spreadBullet_alreadyHit = true;

            GenerateChildBullet();
        }
    }


    //GenerateChildBullet�Ŏg�p����Vector3(�͂������邽�߂̃x�N�g��)
    Vector3 SnapAngle(Vector3 vector,float angleSize)
    {
        var angle = Mathf.Atan2(vector.x,vector.z);

        var index = Mathf.RoundToInt(angle / angleSize);

        var snappedAngle = index * angleSize;

        var magnitude = vector.magnitude;

        return new Vector3(Mathf.Cos(snappedAngle) * magnitude * 0.8f, 4, Mathf.Sin(snappedAngle) * magnitude * 0.8f); 
    }


    //ChildBullet�̐���
    //������45�x����8�����̈ʒu
    private void GenerateChildBullet()
    {
        _childBulletPoz = transform.position;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i == 1 && j == 1)
                {
                    continue;
                }
                childBullet = Instantiate(childBulletPrefab, _childBulletPoz, Quaternion.identity);
                childBullet_rb = childBullet.GetComponent<Rigidbody>();
                Vector3 snapped = SnapAngle(new Vector3(i - 1, 4, j - 1), 45f * Mathf.Deg2Rad);
                childBullet_rb.AddForce(snapped, ForceMode.Impulse);
            }
        }

        Destroy(this.gameObject);
    }

}
