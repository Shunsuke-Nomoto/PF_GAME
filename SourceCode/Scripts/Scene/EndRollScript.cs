using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndRollScript : MonoBehaviour
{
    //エンドロールパネルとスライドさせるアニメーター
    [SerializeField]
    private GameObject endRollPanel;
    private Animator endRollAnim;

    //画面隠し用のパネルとフェードインするアニメーター
    [SerializeField]
    private GameObject sceneFadePanel;
    private Animator sceneFadeAnim;

    //BGMを管理するオブジェクトとそのスクリプト
    [SerializeField]
    private GameObject BGMMnager;
    private InGameBGMManager inGameBGMManager;

    //UnknownLevelを解放するための条件
    public static bool _canPlayUnknown;


    //ゲーム起動時に初期化
    [RuntimeInitializeOnLoadMethod]
    private void Unknown()
    {
        _canPlayUnknown = false;
    }


    private void Start()
    {
        //UnknownLevelを解放
        _canPlayUnknown = true;

        sceneFadeAnim = sceneFadePanel.GetComponent<Animator>();
        //画面フェードイン
        sceneFadeAnim.SetTrigger("FadeIn");

        endRollAnim = endRollPanel.GetComponent<Animator>();

        inGameBGMManager = BGMMnager.GetComponent<InGameBGMManager>();

        //エンドロールスタート
        Invoke("EndRollStart", 4.0f);
        //BGM停止,画面フェードアウト
        Invoke("StopBGM", 38.0f);

        //シーン遷移
        Invoke("ChangeScene", 42.0f);
    }


    private void EndRollStart()
    {
        //エンドロールのアニメーションをスタート
        endRollAnim.SetBool("EndRollStart",true);
    }

    private void StopBGM()
    {
        //画面のフェードアウト
        sceneFadeAnim.SetTrigger("FadeOut");
        //BGMをオフ
        inGameBGMManager.StartCoroutine("BGMOff");
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
