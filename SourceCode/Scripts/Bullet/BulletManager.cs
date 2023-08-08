using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletManager : MonoBehaviour
{
    //アタッチされているオブジェクトが与えるダメージ量
    public static int _damage;

    //アタッチされているオブジェクトのタグ
    private string tagName;


    private void Awake()
    {
        decideDamage();
    }


    //アタッチされているオブジェクトのタグでダメージ量決定
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


    //特定のオブジェクトに衝突した時にこのオブジェクトを破壊
    private void OnTriggerEnter(Collider other)
    {
        //triggerしたオブジェクトタグがrangeかnoEntryAreaであれば素通り
        //rangeは壁に接しているか,noEntryAreaはenemy敵の一定範囲内かを示す空のオブジェクト
        if (other.gameObject.tag == "range" || other.gameObject.tag == "noEntryArea")
        {
            return;
        }

        //ChildBulletのみ生成する都合上破壊はChildBulletManagerで行う
        if(this.gameObject.tag == "ChildBullet")
        {
            return;
        }

        Destroy(this.gameObject);
    }
}
