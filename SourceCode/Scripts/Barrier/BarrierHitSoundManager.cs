using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHitSoundManager : MonoBehaviour
{
    private BulletManager bulletManager;

    //弾が当たった際のサウンドを再生する処理を行うスクリプト
    private SoundPlayManager soundPlayManager;


    private void Start()
    {
        soundPlayManager = this.gameObject.GetComponent<SoundPlayManager>();
    }


    //BulletManagerを持っているBulletオブジェクトが衝突した際に衝突音を再生
    private void OnTriggerEnter(Collider other)
    {
        //当たったオブジェクトのbulletManagerを取得
        bulletManager = other.gameObject.GetComponent<BulletManager>();

        //bulletManagerが取得できなかったら(Bullet以外に当たっている場合)
        if (bulletManager == null)
        {
            return;
        }

        //弾が当たった際のサウンドの再生
        soundPlayManager.StartCoroutine("SoundPlay");
    }
}
