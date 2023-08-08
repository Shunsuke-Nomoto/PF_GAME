using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLevel : MonoBehaviour
{
    //�Q�[����Փx��I���������ǂ���
    public static bool _gameLevelSelected;

    //�v���C���[�̑I��������Փx�ɂ���ĕς��G�̃X�e�[�^�X��ۑ�
    public static float _shootingRate;
    public static int _enemyMaxHP;
    public static bool _enemyBarrier;

    //�I�������Q�[���̓�Փx
    public static string _gameLevel;

    //���ꂼ��̓�Փx���N���A�������ǂ���
    public static bool[] ClearedGameLevel = new bool[3];

    //���ꂼ��̓�Փx����x���N���A���Ă��Ȃ����ǂ���
    public static bool[] NeverCleared = new bool[3];


    //�Q�[���N�����ɏ�����
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
        //bool�l�̏�����
        _gameLevelSelected = false;
        _enemyBarrier = false;
    }
}
