using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


//シーン遷移するかを尋ねた際に提示される"いいえ"ボタン
public class DisagreeButtonScript : MonoBehaviour
{
    //シーン遷移するか尋ねた際の画面を隠す背景のパネルとそのimageコンポーネント
    [SerializeField]
    private Image sceneFadePanel;
    private Image _panel;

    //シーン遷移するか尋ねるテキスト
    [SerializeField]
    private TextMeshProUGUI confirmText;

    //"はい"ボタン
    [SerializeField]
    private GameObject agreeButton;
    //"いいえ"ボタン(このオブジェクト)
    [SerializeField]
    private GameObject disagreeButton;


    private void Awake()
    {
        _panel = sceneFadePanel.GetComponent<Image>();
    }


    //このボタンが押されたときに一時停止状態を解除し元の画面を再開する
    public void OnClick()
    {
        if (QuitGameManager._canQuitGame == true)
        {
            //一時停止解除
            Time.timeScale = 1;

            QuitGameManager._canQuitGame = false;

            DisappearConfirm();
        }
    }


    //このボタンが非アクティブになったらQuitGameManager._canPressEscapeを1秒間押せなくする
    private void OnDisable()
    {
        Invoke("CanPressEscape", 1.0f);
    }
        

    private void CanPressEscape()
    {
        QuitGameManager._canPressEscape = true;
    }


    //シーン遷移するかを尋ねる一連のパネル,ボタンを非アクティブにする
    private void DisappearConfirm()
    {
        if(QuitGameManager._sceneName == "MainGame")
        {
            //マウスカーソル非表示
            Cursor.visible = false;
        }

        _panel.enabled = false;
        confirmText.enabled = false;

        agreeButton.SetActive(false);
        disagreeButton.SetActive(false);
    }
}
