using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBulletManager: MonoBehaviour
{
    //SpreadBulletによって生成されたこのオブジェクト(ChildBullet)はBulletManagerではなく指定秒数で破壊
    //ある座標で複数のChildBulletを生成するためBulletManagerでの破壊が困難
    void Start()
    {
        Destroy(this.gameObject,1.0f);
    }

}
