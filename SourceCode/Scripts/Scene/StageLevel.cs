using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLevel : MonoBehaviour
{
    //ゲーム難易度を選択したかどうか
    public static bool _gameLevelSelected;

    //プレイヤーの選択した難易度によって変わる敵のステータスを保存
    public static float _shootingRate;
    public static int _enemyMaxHP;
    public static bool _enemyBarrier;

    //選択したゲームの難易度
    public static string _gameLevel;

    //それぞれの難易度をクリアしたかどうか
    public static bool[] ClearedGameLevel = new bool[3];

    //それぞれの難易度を一度もクリアしていないかどうか
    public static bool[] NeverCleared = new bool[3];


    //ゲーム起動時に初期化
    [RuntimeInitializeOnLoadMethod]
    private void ResetClearedGameLevel()
    {
        for(int i = 0; i<3; i++)
        {
            ClearedGameLevel[i] = false;
            NeverCleared[i] = false;
        }
    }


    private void Awake()
    {
        //bool値の初期化
        _gameLevelSelected = false;
        _enemyBarrier = false;
    }
}
