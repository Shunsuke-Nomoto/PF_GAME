using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class StageLevelButton : MonoBehaviour
{
    //�����ꂽ�{�^���ɂ���Č��肳���e�X�e�[�^�X
    private int _hp;
    private float _enemyShootingRate;
    private bool _enemybarrier;
    private string _gamelevel;

    //���̃I�u�W�F�N�g�̃^�O
    private string buttonTag;

    //�V�[�����[�h���ɕ\�������ʉB���ƃe�L�X�g
    [SerializeField]
    private GameObject loadingPanel;
    private Image _panel;
    [SerializeField]
    private TextMeshProUGUI loadingText;

    //����I�����ꂽ�X�e�[�W���x���Ƃ��̑O�̃��x��
    //ex.)stageLevel��hard�Ȃ�previouslevel��normal
    private string stageLevel;
    private string previouslevel;

    //�O�̃��x�����N���A���Ă��邩
    private bool�@_canClick;

    //�O�̃��x�����N���A���Ă��Ȃ��Ƃ��̃��x���͗V�ׂȂ����Ƃ������e�L�X�g
    [SerializeField]
    private TextMeshProUGUI cantPressButtonMessage;

    private AsyncOperation async;
    //���炩���߃��[�h���Ă����V�[���ɑJ�ڂ��邩�ǂ���
    private bool loadScene;

    //BGM���Đ�����X�N���v�g�ƃA�^�b�`����Ă���I�u�W�F�N�g
    [SerializeField]
    private GameObject bgmManager;
    private InGameBGMManager inGameBGMManager;


    private void Awake()
    {
        _panel = loadingPanel.GetComponent<Image>();

        HPDecision();
        CheckThisLevel();

        if (this.gameObject.tag == "unknownLevelButton" && EndRollScript._canPlayUnknown != true)
        {
            this.gameObject.SetActive(false);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        _panel.enabled = false;
        loadingText.enabled = false;

        inGameBGMManager = bgmManager.GetComponent<InGameBGMManager>();
    }


    //�I�񂾃��x���ɂ���ēG�̃X�e�[�^�X�����肵�AStageLevel�ɕۑ�
    private void HPDecision()
    {
        buttonTag = this.gameObject.tag;

        switch (buttonTag)
        {
            case "normalLevelButton":
                _hp = 200;
                _enemyShootingRate = 12f;
                _enemybarrier = false;
                _gamelevel = "Normal";
                break;

            case "hardLevelButton":
                _hp = 350;
                _enemyShootingRate = 10f;
                _enemybarrier = false;
                _gamelevel = "Hard";
                previouslevel = "Normal";
                break;

            case "unknownLevelButton":
                _hp = 600;
                _enemyShootingRate = 8f;
                _enemybarrier = true;
                _gamelevel = "Unknown";
                previouslevel = "Hard";
                break;
        }
    }


    //�O�̃��x�����N���A���Ă��Ȃ��Ƃ��̃��x����I�ׂȂ��悤�ɂ���
    private void CheckThisLevel()
    {
        switch (this.gameObject.tag)
        {
            case "normalLevelButton":

                _canClick = true;

                break;

            case "hardLevelButton":

                if(StageLevel.NeverCleared[0] == false)
                {
                    _canClick = false;
                }
                else
                {
                    _canClick = true;
                }

                break;

            case "unknownLevelButton":

                if (StageLevel.NeverCleared[1] == false)
                {
                    _canClick = false;
                }
                else
                {
                    _canClick = true;
                }

                break;
        }
    }


    public void OnClick()
    {
        QuitGameManager._canPressEscape = false;

        //���̃��x����I���\�ő��̃��x����I��łȂ���΂��̃��x����MainGame�V�[�������[�h
        if (_canClick == true)
        {
            
            if (StageLevel._gameLevelSelected == false)
            {
                cantPressButtonMessage.enabled = false;

                //�}�E�X�J�[�\����\��
                Cursor.visible = false;

                //�V�[�����[�h����escape�������Ȃ��悤�ɂ���
                QuitGameManager._canPressEscape = false;

                StageLevel._gameLevelSelected = true;
                StageLevel._enemyMaxHP = _hp;
                StageLevel._shootingRate = _enemyShootingRate;
                StageLevel._enemyBarrier = _enemybarrier;
                StageLevel._gameLevel = _gamelevel;

                inGameBGMManager.StartCoroutine("BGMOff");
                AppearLoadingPanel();

                //���̃V�[�������[�h
                StartCoroutine("LoadNextScene");
                StartCoroutine("ChangeScene");
            }
        }


        //�ЂƂO�̃��x�����N���A���Ă��Ȃ�������
        else if (_canClick == false)
        {
            if (StageLevel._gameLevelSelected == false)
            {
                StageLevel._gameLevelSelected = true;

                CantPressButtonMessage();
            }
        }
    }


    //���[�h�p�̉�ʂ��o��
    private void AppearLoadingPanel()
    {
        _panel.enabled = true;
        loadingText.enabled = true;

        LoadingText();
    }


    //���[�h���̃e�L�X�g
    private void LoadingText()
    {
        switch (buttonTag)
        {
            case "normalLevelButton":
                stageLevel = "Normal";
                break;

            case "hardLevelButton":
                stageLevel = "Hard";
                break;

            case "unknownLevelButton":
                stageLevel = "...";
                break;
        }

        loadingText.text = "Starting Game : " + stageLevel;
    }


    //�O�̃��x�����N���A���Ă��Ȃ��Ƃ��Ƀ��b�Z�[�W���o��
    private void CantPressButtonMessage()
    {
        cantPressButtonMessage.enabled = true;

        cantPressButtonMessage.text = "Need to clear the " + previouslevel + " level";

        Invoke("DisappearCantPressButtonMessage", 1.5f);
    }


    private void DisappearCantPressButtonMessage()
    {
        cantPressButtonMessage.enabled = false;
        StageLevel._gameLevelSelected = false;
        QuitGameManager._canPressEscape = true;
    }


    //���炩���ߎ��̃V�[�������[�h
    IEnumerator LoadNextScene()
    {
        async = SceneManager.LoadSceneAsync("MainGame");
        //���̎��_�ł̓V�[���J�ڂ͂��Ȃ�
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            yield return null;
        }

        loadScene = true;
    }


    //���炩���߃��[�h�����V�[���ɑJ��
    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(3.0f);
        while (loadScene == false)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        StopCoroutine("LoadingAnimationActive");
        async.allowSceneActivation = true;
    }

}
