using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameBGMManager : MonoBehaviour
{
    //シーン名
    private string _sceneName;
    //BGMを再生するAudioSource
    private AudioSource audioSource;
    private AudioClip bgm;
    //BGM素材
    [SerializeField]
    private AudioClip mainMenu;
    [SerializeField]
    private AudioClip mainGame1;
    //Unknownレベルで敵の体力が半分以下になったら再生するBGM
    [SerializeField]
    private AudioClip mainGame2;
    [SerializeField]
    private AudioClip result;
    [SerializeField]
    private AudioClip endRoll;


    // Start is called before the first frame update
    void Start()
    {
        _sceneName = SceneManager.GetActiveScene().name;

        audioSource = this.gameObject.GetComponent<AudioSource>();
        //最初は音量0
        audioSource.volume = 0;

        //シーン名で再生するBGMを変える
        switch (_sceneName)
        {
            case "MainMenu":
                bgm = mainMenu;
                break;

            case "MainGame":
                bgm = mainGame1;
                break;

            case "Result":
                bgm = result;
                break;

            case "EndRoll":
                bgm = endRoll;
                break;
        }
        StartCoroutine("BGMStart"); 
    }

    
    //MainGame開始後5秒後にBGMを再生し始める
    IEnumerator BGMStart()
    {
        switch (_sceneName)
        {
            case "MainMenu":
                break;

            //MainGameシーンだったら開始5秒待つ
            case "MainGame":
                yield return new WaitForSeconds(5.0f);
                break;

            case "Result":
                break;

            //MainGameシーンだったら開始3秒待つ
            case "EndRoll":
                yield return new WaitForSeconds(3.0f);
                break;
        }

        audioSource.clip = bgm;
        audioSource.Play();

        //段々とボリュームを上げる
        for(int i = 1; i < 26; i++)
        {
            audioSource.volume += 0.004f;
            yield return new WaitForSeconds(0.1f);
        }
    }


    //BGMを徐々に下げ停止する
    IEnumerator BGMOff()
    {
        //段々とボリュームを下げる
        for (int i = 1; i < 26; i++)
        {
            audioSource.volume -= 0.04f;
            yield return new WaitForSeconds(0.1f);
        }
        audioSource.enabled = false;
    }


    //Unknownレベルの時にEnemyの体力が半分を切るとEnemyHPManagerによって呼ばれる
    //MainGame内で流れているBGMを変える
    IEnumerator ChangeBGM()
    {
        StartCoroutine("BGMOff");
        //BGMOff開始後2.5fでaudioSource.enabled = falsenになるためそれまで処理を止めておく
        while (audioSource.enabled == true)
        {
            yield return null;
        }
        audioSource.volume = 0.1f;
        audioSource.clip = mainGame2;
        audioSource.enabled = true;
        audioSource.Play();
    }
}
