using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���̃I�u�W�F�N�g�ɋ߂��Ƃ���player���ˌ��ł��Ȃ��悤�ɂ���
public class DistinguishOtherObjInRange : MonoBehaviour
{
    //�ˌ��ł����Ԃ�
    public static bool _canShoot;
    //enemy�I�u�W�F�N�g�ɋ߂����Ȃ���
    public static bool _noEntryArea;

    //parentObj�͂��ꂪ�A�^�b�`����Ă����̃I�u�W�F�N�g�̐e�Ƃ��̃^�O(player)
    [SerializeField]
    private GameObject parentsObj;
    private string parentsTag;

    private BulletManager bm;


    private void Awake()
    {
        _canShoot = true;
        _noEntryArea = false;
        
        parentsTag = parentsObj.tag;
    }


    //�g���K�[�����I�u�W�F�N�g�̃^�O��range,parentsTag�łȂ���Ό��ĂȂ��悤�ɂ���(_canshoot = false)
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "range" && other.gameObject.tag != parentsTag)
        {
            //�G�̒e�ɂ���Č��ĂȂ��̂�h��
            bm = other.gameObject.GetComponent<BulletManager>();
            if(bm != null)
            {
                return;
            }

            _canShoot = false;

            if(other.gameObject.tag == "noEntryArea")
            {
                _noEntryArea = true;
            }
        }
    }


    //�I�u�W�F�N�g���痣�ꂽ��ēx���Ă�悤�ɂ���(_canshoot = true)
    private void OnTriggerExit(Collider other)
    {
        _canShoot = true;
        if (other.gameObject.tag == "noEntryArea")
        {
            _noEntryArea = false;
        }
    }

}
