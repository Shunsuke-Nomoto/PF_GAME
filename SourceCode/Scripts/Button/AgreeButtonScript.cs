using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


//シーン遷移するかを尋ねた際に提示される"はい"ボタン
public class AgreeButtonScript : MonoBehaviour
{
    //このボタンが押されたときに指定のシーンに遷移する
    public void OnClick()
    {
        //QuitGameManager._canQuitGameはシーン遷移が行える状態かを示す
        if (QuitGameManager._canQuitGame == true)
        {
            QuitGameManager._canQuitGame = false;

            //一時停止解除
            Time.timeScale = 1;

            ChangeScene();
        }
    }


    //シーン遷移
    private void ChangeScene()
    {
        switch (QuitGameManager._sceneName)
        {
            case "MainMenu":
                //ゲームアプリの終了
                Application.Quit();
                break;

            case "MainGame":
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }
}
