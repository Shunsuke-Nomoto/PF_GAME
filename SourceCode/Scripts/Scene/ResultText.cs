using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultText : MonoBehaviour
{
    //TimeManagerスクリプトに保存されているクリアタイム
    private float _clearTime;

    private int _clearTimeMinute;
    private int _clearTimeSeconds;
    private float _clearTimeDecimalPoint;

    //StageLevelに保存している現在選択しているゲームのレベル
    private string gamelevel;
    private bool _selectedGameLevel;

    //結果を表示するTMPro
    [SerializeField]
    private TextMeshProUGUI resultText;
    [SerializeField]
    private TextMeshProUGUI resultTime;

    //Mainmenuに戻れるようにする
    private bool _canSkip;

    [SerializeField]
    private TextMeshProUGUI plzClick;

    //クリックを促すテキストのアニメーション
    private Animator anim;

    //ローディング中の背景パネル(初期状態は非表示)
    [SerializeField]
    private GameObject sceneFadePanel;
    //sceneFadePanelのImageコンポ―ネント
    private Image sceneFadePanelImg;
    [SerializeField]
    //"LOADING"のテキスト(初期状態は非表示)
    private TextMeshProUGUI loadingText;

    private AsyncOperation async;
    //あらかじめロードしていたシーンに遷移するかどうか
    private bool loadScene;

    //エンドロールに行くかどうか
    public static bool goEndRoll;


    //ゲーム起動時に初期化
    [RuntimeInitializeOnLoadMethod]
    private void ResetGoEndRoll()
    {
        goEndRoll = false;
    }


    private void Start()
    {
        //選択したゲームレベル
        gamelevel = StageLevel._gameLevel;

        //クリアタイム(秒数)を時間に直す
        _clearTime = TimeManager._endTime;
        _clearTimeMinute = (int)_clearTime / 60;
        _clearTimeSeconds = (int)Mathf.Floor(_clearTime % 60);
        
        _clearTimeDecimalPoint = _clearTime - (int)_clearTime;

        //小数点以下を第二位までで切り上げて自然数にする
        _clearTimeDecimalPoint = Mathf.Floor(Mathf.Ceil(1000 * _clearTimeDecimalPoint)/10);

        if(_clearTimeDecimalPoint >= 100)
        {
            _clearTimeDecimalPoint = 99;
        }
     //ここまで

        anim = plzClick.GetComponent<Animator>();
        sceneFadePanelImg = sceneFadePanel.GetComponent<Image>();

        _canSkip = false;

        //2秒経ったらシーンを変えられるようにする
        Invoke("CanSkip", 2.0f);

        DecideGameClear();

        //マウスカーソル表示
        Cursor.visible = true;
    }


    //クリアしたかどうか
    private void DecideGameClear()
    {
        //結果をStageLevelスクリプトのstatic関数に保存
        switch (gamelevel)
        {
            case "Normal":
                _selectedGameLevel = StageLevel.ClearedGameLevel[0];
                break;

            case "Hard":
                _selectedGameLevel = StageLevel.ClearedGameLevel[1];
                break;

            case "Unknown":
                _selectedGameLevel = StageLevel.ClearedGameLevel[2];
                break;
        }

        //選択した難易度をクリアしたら
        if (_selectedGameLevel == true)
        {
            resultText.text = "CLEARED : " + gamelevel;
            resultTime.text = "TIME : " + _clearTimeMinute.ToString() + " m " + _clearTimeSeconds.ToString() + " s " + _clearTimeDecimalPoint.ToString();
        }

        //ゲームオーバーだった場合
        else if ((_selectedGameLevel == false))
        {
            resultText.text = "FAILED : " + gamelevel;
            resultTime.text = "TIME : " + _clearTimeMinute.ToString() + " m " + _clearTimeSeconds.ToString() + " s " + _clearTimeDecimalPoint.ToString();
        }
    }


    //クリックでMainMenuに戻れるようにする
    private void CanSkip()
    {
        _canSkip = true;

        plzClick.enabled = true;
        anim.SetTrigger("Flashing");
    }


    private void Update()
    {
        //右クリックされたらLoad時のパネルとテキストを表示しアニメーションの再生
        if(Input.GetMouseButtonDown(0))
        {
            if(_canSkip == true)
            {
                _canSkip = false;
                plzClick.enabled = false;

                StartCoroutine("LoadingAnimationActive");

                StartCoroutine("LoadNextScene");
                StartCoroutine("ChangeScene");
            }
        }
    }


    //あらかじめ次のシーンをロード
    IEnumerator LoadNextScene()
    {
        //選択したゲームレベルがハードで初めてクリアしたならEndRollを遷移先にする
        if(goEndRoll == true)
        {
            goEndRoll = false;
            async = SceneManager.LoadSceneAsync("EndRoll");
        }
        //それ以外はメインメニュー
        else
        {
            async = SceneManager.LoadSceneAsync("MainMenu");
        }

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
        yield return new WaitForSeconds(2.0f);
        while (loadScene == false)
        {
            yield return null;
        }
        StopCoroutine("LoadingAnimationActive");
        async.allowSceneActivation = true;
    }


    //シーン遷移中の待機画面
    IEnumerator LoadingAnimationActive()
    {
        //panelとtextを表示
        sceneFadePanelImg.enabled = true;
        loadingText.enabled = true;

        for(; ; )
        {
            loadingText.text = "LOADING.";
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "LOADING..";
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "LOADING...";
            yield return new WaitForSeconds(0.5f);
        }
    }

}
