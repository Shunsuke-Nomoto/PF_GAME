using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadBulletManager : MonoBehaviour
{
    //オブジェクトに当たった時に生成するオブジェクトとそのプレハブ
    [SerializeField]
    private GameObject childBulletPrefab;
    private GameObject childBullet;

    //ChildBulletのrigidbody
    private Rigidbody childBullet_rb;

    //生成する際の位置
    private Vector3 _childBulletPoz;

    //ChildBulletを生成する都合上一度だけ当たるようにする
    private bool spreadBullet_alreadyHit;


    private void Start()
    {
        spreadBullet_alreadyHit = false;
        //SpreadBulletのみオブジェクトに当たらなくても指定秒数でChildBulletを生成し破壊
        Invoke("GenerateChildBullet", 0.75f);
    }


    private void OnTriggerEnter(Collider other)
    {   //triggerしたオブジェクトタグがrangeかnoEntryAreaであれば素通り
        //rangeは壁に接しているか,noEntryAreaはenemy敵の一定範囲内かを示す空のオブジェクト
        if (other.gameObject.tag == "range" || other.gameObject.tag == "noEntryArea")
        {
            return;
        }

        //初めてオブジェクトに当たったならGenerateChildBulletを行う
        if (spreadBullet_alreadyHit == false)
        {
            spreadBullet_alreadyHit = true;

            GenerateChildBullet();
        }
    }


    //GenerateChildBulletで使用するVector3(力を加えるためのベクトル)
    Vector3 SnapAngle(Vector3 vector,float angleSize)
    {
        var angle = Mathf.Atan2(vector.x,vector.z);

        var index = Mathf.RoundToInt(angle / angleSize);

        var snappedAngle = index * angleSize;

        var magnitude = vector.magnitude;

        return new Vector3(Mathf.Cos(snappedAngle) * magnitude * 0.8f, 4, Mathf.Sin(snappedAngle) * magnitude * 0.8f); 
    }


    //ChildBulletの生成
    //生成は45度刻み8方向の位置
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
