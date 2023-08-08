using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHPManager : MonoBehaviour
{
    //最大HP
    private int _MaxHp;
    //ダメージ計算を行う際のBulletManagerのもつダメージ値
    private int _damage;
    //現在のHP
    private int _currentHp;

    //HPバー
    [SerializeField]
    private Slider slider;
    //衝突したオブジェクトが持つBulletManager
    private BulletManager bulletManager;

    //TimeKeeperオブジェクトとそれが持つゲームのクリア時間を管理するスクリプト
    [SerializeField]
    private GameObject timeKeeper;
    private TimeManager timeManager;

    //シーン遷移を行うスクリプトを持つオブジェクト
    [SerializeField]
    private GameObject changeSceneManager;

    //ゲームをクリアまたはクリアできなかったときにその旨を表示するテキスト
    [SerializeField]
    private TextMeshProUGUI deciedClearText;
    //ゲームオーバーのとき表示する黒の薄い画面隠し
    [SerializeField]
    private GameObject gameOverBackground;
    //初期状態は非アクティブ
    private Image gameOverBackgroundImg;

    //ModeChangeManagerとそれがアタッチされているオブジェクト
    [SerializeField]
    private GameObject modeChangeManager;
    private ModeChange modeChange;


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

        gameOverBackgroundImg = gameOverBackground.GetComponent<Image>();

        timeManager = timeKeeper.GetComponent<TimeManager>();

        modeChange = modeChangeManager.GetComponent<ModeChange>();

        inGameBGMManager = BGMMnager.GetComponent<InGameBGMManager>();

        soundPlayManager = this.gameObject.GetComponent<SoundPlayManager>();
    }


    //HP(_MaxHp)の設定
    private void HPDecision()
    {
        _MaxHp = 150;
    }

 
    //被弾後のHPチェック
    private void HpCheck()
    { 
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

            modeChange.enabled = false;
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


    //クリアできなかったらStageLevelに格納されているその難易度のクリア判定をfalseにする
    private void DecideGameClear()
    {
        var gamelevel = StageLevel._gameLevel;

        switch (gamelevel)
        {
            case "Normal":
                StageLevel.ClearedGameLevel[0] = false;
                gameOverBackgroundImg.enabled = true;
                deciedClearText.text = gamelevel + " STAGE FAILED";
                break;

            case "Hard":
                StageLevel.ClearedGameLevel[1] = false;
                gameOverBackgroundImg.enabled = true;
                deciedClearText.text = gamelevel + " STAGE FAILED";
                break;

            case "Unknown":
                StageLevel.ClearedGameLevel[2] = false;
                gameOverBackgroundImg.enabled = true;
                deciedClearText.text = gamelevel + " STAGE FAILED";
                break;
        }
    }
}
