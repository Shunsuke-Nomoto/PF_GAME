using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class StageLevelButton : MonoBehaviour
{
    //押されたボタンによって決定される各ステータス
    private int _hp;
    private float _enemyShootingRate;
    private bool _enemybarrier;
    private string _gamelevel;

    //このオブジェクトのタグ
    private string buttonTag;

    //シーンロード中に表示する画面隠しとテキスト
    [SerializeField]
    private GameObject loadingPanel;
    private Image _panel;
    [SerializeField]
    private TextMeshProUGUI loadingText;

    //今回選択されたステージレベルとその前のレベル
    //ex.)stageLevelがhardならpreviouslevelはnormal
    private string stageLevel;
    private string previouslevel;

    //前のレベルをクリアしているか
    private bool　_canClick;

    //前のレベルをクリアしていないとこのレベルは遊べないことを示すテキスト
    [SerializeField]
    private TextMeshProUGUI cantPressButtonMessage;

    private AsyncOperation async;
    //あらかじめロードしていたシーンに遷移するかどうか
    private bool loadScene;

    //BGMを再生するスクリプトとアタッチされているオブジェクト
    [SerializeField]
    private GameObject bgmManager;
    private InGameBGMManager inGameBGMManager;


    private void Awake()
    {
        _panel = loadingPanel.GetComponent<Image>();

        HPDecision();
        CheckThisLevel();

        if (this.gameObject.tag == "unknownLevelButton" && EndRollScript._canPlayUnknown != true)
        {
            this.gameObject.SetActive(false);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        _panel.enabled = false;
        loadingText.enabled = false;

        inGameBGMManager = bgmManager.GetComponent<InGameBGMManager>();
    }


    //選んだレベルによって敵のステータスを決定し、StageLevelに保存
    private void HPDecision()
    {
        buttonTag = this.gameObject.tag;

        switch (buttonTag)
        {
            case "normalLevelButton":
                _hp = 200;
                _enemyShootingRate = 12f;
                _enemybarrier = false;
                _gamelevel = "Normal";
                break;

            case "hardLevelButton":
                _hp = 350;
                _enemyShootingRate = 10f;
                _enemybarrier = false;
                _gamelevel = "Hard";
                previouslevel = "Normal";
                break;

            case "unknownLevelButton":
                _hp = 600;
                _enemyShootingRate = 8f;
                _enemybarrier = true;
                _gamelevel = "Unknown";
                previouslevel = "Hard";
                break;
        }
    }


    //前のレベルをクリアしていないとそのレベルを選べないようにする
    private void CheckThisLevel()
    {
        switch (this.gameObject.tag)
        {
            case "normalLevelButton":

                _canClick = true;

                break;

            case "hardLevelButton":

                if(StageLevel.NeverCleared[0] == false)
                {
                    _canClick = false;
                }
                else
                {
                    _canClick = true;
                }

                break;

            case "unknownLevelButton":

                if (StageLevel.NeverCleared[1] == false)
                {
                    _canClick = false;
                }
                else
                {
                    _canClick = true;
                }

                break;
        }
    }


    public void OnClick()
    {
        QuitGameManager._canPressEscape = false;

        //そのレベルを選択可能で他のレベルを選んでなければそのレベルのMainGameシーンをロード
        if (_canClick == true)
        {
            
            if (StageLevel._gameLevelSelected == false)
            {
                cantPressButtonMessage.enabled = false;

                //マウスカーソル非表示
                Cursor.visible = false;

                //シーンロード中はescapeを押せないようにする
                QuitGameManager._canPressEscape = false;

                StageLevel._gameLevelSelected = true;
                StageLevel._enemyMaxHP = _hp;
                StageLevel._shootingRate = _enemyShootingRate;
                StageLevel._enemyBarrier = _enemybarrier;
                StageLevel._gameLevel = _gamelevel;

                inGameBGMManager.StartCoroutine("BGMOff");
                AppearLoadingPanel();

                //次のシーンをロード
                StartCoroutine("LoadNextScene");
                StartCoroutine("ChangeScene");
            }
        }


        //ひとつ前のレベルをクリアしていなかったら
        else if (_canClick == false)
        {
            if (StageLevel._gameLevelSelected == false)
            {
                StageLevel._gameLevelSelected = true;

                CantPressButtonMessage();
            }
        }
    }


    //ロード用の画面を出現
    private void AppearLoadingPanel()
    {
        _panel.enabled = true;
        loadingText.enabled = true;

        LoadingText();
    }


    //ロード時のテキスト
    private void LoadingText()
    {
        switch (buttonTag)
        {
            case "normalLevelButton":
                stageLevel = "Normal";
                break;

            case "hardLevelButton":
                stageLevel = "Hard";
                break;

            case "unknownLevelButton":
                stageLevel = "...";
                break;
        }

        loadingText.text = "Starting Game : " + stageLevel;
    }


    //前のレベルをクリアしていないときにメッセージを出す
    private void CantPressButtonMessage()
    {
        cantPressButtonMessage.enabled = true;

        cantPressButtonMessage.text = "Need to clear the " + previouslevel + " level";

        Invoke("DisappearCantPressButtonMessage", 1.5f);
    }


    private void DisappearCantPressButtonMessage()
    {
        cantPressButtonMessage.enabled = false;
        StageLevel._gameLevelSelected = false;
        QuitGameManager._canPressEscape = true;
    }


    //あらかじめ次のシーンをロード
    IEnumerator LoadNextScene()
    {
        async = SceneManager.LoadSceneAsync("MainGame");
        //この時点ではシーン遷移はしない
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            yield return null;
        }

        loadScene = true;
    }


    //あらかじめロードしたシーンに遷移
    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(3.0f);
        while (loadScene == false)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        StopCoroutine("LoadingAnimationActive");
        async.allowSceneActivation = true;
    }

}
