using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    //実際にプレイしていた時間
    public static float _endTime;
    //計測開始したタイム
    private float _startTime;
    //ゲームをクリアかゲームオーバーになったタイム
    private float _elapsedTime;

    //ゲーム開始時のカウントダウンテキスト
    [SerializeField]
    private TextMeshProUGUI countdownText;

    //ゲームを開始syryコルーチン
    private IEnumerator gameStart;


    private void Awake()
    {
        //クリア時間の初期化
        _endTime = 0;

        gameStart = GameStart();
    }

    private void Start()
    {
        StartCoroutine(gameStart);
    }


    //ゲーム開始2秒後に3秒のカウントダウン -> ゲームスタート
    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(2.0f);

        countdownText.text = "3";
        yield return new WaitForSeconds(1.0f);

        countdownText.text = "2";
        yield return new WaitForSeconds(1.0f);

        countdownText.text = "1";
        yield return new WaitForSeconds(1.0f);

        countdownText.text = "START";
        yield return new WaitForSeconds(1.0f);

        countdownText.gameObject.SetActive(false);

        StartTimeMeasurement();
    }


    private void StartTimeMeasurement()
    {
        //計測開始時間の記録
        _startTime = Time.time;
    }


    public void MeasuringClearTime()
    {

        //終了時の時間の記録
        _elapsedTime = Time.time;

        //クリア時間 = 計測開始時間 - 終了時間

        _endTime = _elapsedTime - _startTime;
    }
}
