using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//他のオブジェクトに近いときにplayerが射撃できないようにする
public class DistinguishOtherObjInRange : MonoBehaviour
{
    //射撃できる状態か
    public static bool _canShoot;
    //enemyオブジェクトに近すぎないか
    public static bool _noEntryArea;

    //parentObjはこれがアタッチされている空のオブジェクトの親とそのタグ(player)
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


    //トリガーしたオブジェクトのタグがrange,parentsTagでなければ撃てないようにする(_canshoot = false)
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "range" && other.gameObject.tag != parentsTag)
        {
            //敵の弾によって撃てないのを防ぐ
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


    //オブジェクトから離れたら再度撃てるようにする(_canshoot = true)
    private void OnTriggerExit(Collider other)
    {
        _canShoot = true;
        if (other.gameObject.tag == "noEntryArea")
        {
            _noEntryArea = false;
        }
    }

}
