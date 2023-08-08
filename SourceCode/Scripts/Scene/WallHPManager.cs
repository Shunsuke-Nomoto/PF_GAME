using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHPManager : MonoBehaviour
{
    //このオブジェクトの最大HP
    [SerializeField]
    private int _MaxHp;

    //衝突したオブジェクトが持つBulletManager
    private BulletManager bulletManager;

    //ダメージ量
    private int _damage;

    //現在のHP
    private int _currentHp;

    //弾が当たった際のサウンドを再生する処理を行うスクリプト
    private SoundPlayManager soundPlayManager;


    private void Start()
    {
        //現在の体力の初期化
        _currentHp = _MaxHp;

        soundPlayManager = this.gameObject.GetComponent<SoundPlayManager>();
    }


    //現在のHPが0以下になっているかを判定
    private void HpCheck()
    {
        //アタッチされているオブジェクトの現在のHP(_currentHp)が0以下になったら
        if (_currentHp <= 0)
        {
            Destroy(this.gameObject);
        }
    }


    //オブジェクトが衝突したらダメージ計算
    private void OnTriggerEnter(Collider other)
    {
        DamageCheckCollider(other);
    }


    //trigger用のダメージチェック(DamageCheckCollider)
    private void DamageCheckCollider(Collider collider)
    {
        //当たったオブジェクトのbulletManagerを取得
        bulletManager = collider.gameObject.GetComponent<BulletManager>();

        //bulletManagerが取得できなかったら(Bullet以外に当たっている場合)return
        if (bulletManager == null)
        {
            return;
        }

        //弾が当たった際のサウンドの再生
        soundPlayManager.StartCoroutine("SoundPlay");

        _damage = BulletManager._damage;

        //HPの更新
        _currentHp = _currentHp - _damage;

        HpCheck();
    }
}
