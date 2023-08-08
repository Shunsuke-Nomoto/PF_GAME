using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{

    //PlayerHPManagerとEnemyHPManagerによってこのスクリプトがアクティブになる
    //アクティブになったら指定秒数でシーン遷移
    private void OnEnable()
    {
        Invoke("ChangeScene", 3.0f);
    }


    private void ChangeScene()
    {
        SceneManager.LoadScene("Result"); 
    }
}
