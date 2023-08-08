using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHPManager : MonoBehaviour
{
    //Enemyオブジェクトのステータス(StageLavelスクリプトで設定)
    private int _MaxHp;
    private int _damage;
    private int _currentHp;

    //Enemyオブジェクトの体力バー
    [SerializeField]
    private Slider slider;

    //Bulletオブジェクトが持つスクリプト
    private BulletManager bulletManager;

    //Unknownレベルの際に出現するEnemyオブジェクトの子オブジェクト
    [SerializeField]
    private GameObject enemyBarrier;
    //EnemyBarrierが展開されるか(UnknownレベルならTrue)
    private bool _barrierAppear;

    //TimeKeeperオブジェクトとそれが持つゲームのクリア時間を管理するスクリプト
    [SerializeField]
    private GameObject timeKeeper;
    private TimeManager timeManager;

    //シーン遷移を行うオブジェクト
    [SerializeField]
    private GameObject changeSceneManager;

    //クリアかゲームオーバーになったときにそれを示すテキスト
    [SerializeField]
    private TextMeshProUGUI deciedClearText;

    //Maingame内のBGMを管理するオブジェクトとそのスクリプト
    [SerializeField]
    private GameObject BGMMnager;
    private InGameBGMManager inGameBGMManager;

    //弾が当たった際のサウンドを再生する処理を行うスクリプト
    private SoundPlayManager soundPlayManager;


    private void Awake()
    {
        HPDecision();
    }


    // Start is called before the first frame update
    void Start()
    {
        //ステータスの反映
        slider.maxValue = _MaxHp;
        slider.value = _MaxHp;
        _currentHp = _MaxHp;

        _barrierAppear = false;

        timeManager = timeKeeper.GetComponent<TimeManager>();

        inGameBGMManager = BGMMnager.GetComponent<InGameBGMManager>();

        soundPlayManager = this.gameObject.GetComponent<SoundPlayManager>();
    }


    //HP(_MaxHp)の反映
    private void HPDecision()
    {
        _MaxHp = StageLevel._enemyMaxHP;
    }

 
    //被弾後のHPチェック
    private void HpCheck()
    {
        if(_currentHp <= _MaxHp / 2)
        {
            //難易度"unknown"のとき(StageLevel._enemyBarrier=true)、体力が半分になるとEnemyBarrier起動
            if (StageLevel._enemyBarrier == true && _barrierAppear == false)
            {
                _barrierAppear = true;
                enemyBarrier.SetActive(true);

                //BGMを最終決戦用のものに変える
                inGameBGMManager.StartCoroutine("ChangeBGM");
            }
        }
        

        //HPが0になったら
        //全てのコルーチンを止めたのち、このオブジェクトがplayerかenemyか判別しそれに応じてクリア判定を行う
        if (_currentHp <= 0)
        {
            //体力が0になったらBGMを止める
            inGameBGMManager.StartCoroutine("BGMOff");

            QuitGameManager._canPressEscape = false;

            changeSceneManager.SetActive(true);

            //TimeManagerからクリアタイムを記録
            timeManager.MeasuringClearTime();

            //クリア判定
            DecideGameClear();

            this.gameObject.SetActive(false);
        }
    }


    //オブジェクトがトリガーしたらダメージ計算
    private void OnTriggerEnter(Collider other)
    {
        DamageCheckCollider(other);
    }


    //ダメージ計算
    private void DamageCheckCollider (Collider collider)
    {
        //当たったオブジェクトのbulletManagerを取得
        bulletManager = collider.gameObject.GetComponent<BulletManager>();

        //bulletManagerが取得できなかったら(Bullet以外に当たっている場合)
        if (bulletManager == null)
        {
            return;
        }

        //弾が当たった際のサウンドの再生
        soundPlayManager.StartCoroutine("SoundPlay");

        _damage = BulletManager._damage;

        _currentHp = _currentHp - _damage;
        slider.value = _currentHp;
        HpCheck();
    }


    //クリアしたらStageLevelに格納されているその難易度のクリア判定をtrueにする
    //また、それが初めてのクリアならStageLevel.NeverCleared[i]をtrueにする
    private void DecideGameClear()
    {
        var gamelevel = StageLevel._gameLevel;

        switch (gamelevel)
        {
            case "Normal":
                StageLevel.ClearedGameLevel[0] = true;

                if(StageLevel.NeverCleared[0] == false)
                {
                    StageLevel.NeverCleared[0] = true;
                }

                deciedClearText.text = gamelevel + " STAGE CLEAR";
                break;

            case "Hard":
                StageLevel.ClearedGameLevel[1] = true;

                if (StageLevel.NeverCleared[1] == false)
                {
                    StageLevel.NeverCleared[1] = true;
                    //hardモードをクリアしてそれが初めてならEndRollに行くためのbool値をtrueに
                    ResultText.goEndRoll = true;
                }

                deciedClearText.text = gamelevel + " STAGE CLEAR";
                break;

            case "Unknown":
                StageLevel.ClearedGameLevel[2] = true;

                if (StageLevel.NeverCleared[2] == false)
                {
                    StageLevel.NeverCleared[2] = true;
                }

                deciedClearText.text = gamelevel + " STAGE CLEAR";
                break;
        }
    }
}
