using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

//ゲームを終了するスクリプト
public class QuitGameManager : MonoBehaviour
{
    //ゲームを終了できる(メインメニューに戻れる)か
    public static bool _canQuitGame;
    //escapeボタンを押せるか
    public static bool _canPressEscape;
    //現在のシーン名
    public static string _sceneName;

    //ロード用の画面隠し
    [SerializeField]
    private Image sceneFadePanel;
    //scenepanelのimgコンポーネント
    private Image _panel;

    //終了するかどうかの確認テキスト
    [SerializeField]
    private TextMeshProUGUI confirmText;

    //yesボタン
    [SerializeField]
    private GameObject agreeButton;
    //noボタン
    [SerializeField]
    private GameObject disagreeButton;


    private void Awake()
    {
        _panel = sceneFadePanel.GetComponent<Image>();

        agreeButton.SetActive(false);
        disagreeButton.SetActive(false);

        _sceneName = SceneManager.GetActiveScene().name;
    }


    private void Start()
    { 
        //MainMenuならゲームを終了するかどうか,MainGameならMainMenuに帰るかどうか
        _canQuitGame = false;

        //Escapeを押せるかどうか
        _canPressEscape = true;

        _panel.enabled = false;
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && _canPressEscape == true)
        {
            //ゲームの一時停止(一時停止解除はDisagreeButtonScriptとAgreeButtonScriptで行う)
            
            Time.timeScale = 0;

            //マウスカーソル表示
            Cursor.visible = true;

            _canPressEscape = false;
            Confirm();
            _canQuitGame = true;
        }
    }


    private void Confirm()
    {
        //確認用の画面を出現
        _panel.enabled = true;
        confirmText.enabled = true;

        //今のシーン名によって確認用のテキストを出す
        switch(_sceneName)
        {
            case "MainMenu":
                confirmText.text = "Quit the game?";
                break;

            case "MainGame":
                confirmText.text = "Return to the main menu?";
                break;
        }

        AppearConfirmButton();
    }


    private void AppearConfirmButton()
    {
        //マウスカーソル表示
        Cursor.visible = true;

        agreeButton.SetActive(true);
        disagreeButton.SetActive(true);
    }

}
