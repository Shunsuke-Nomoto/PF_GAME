using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultText : MonoBehaviour
{
    //TimeManager�X�N���v�g�ɕۑ�����Ă���N���A�^�C��
    private float _clearTime;

    private int _clearTimeMinute;
    private int _clearTimeSeconds;
    private float _clearTimeDecimalPoint;

    //StageLevel�ɕۑ����Ă��錻�ݑI�����Ă���Q�[���̃��x��
    private string gamelevel;
    private bool _selectedGameLevel;

    //���ʂ�\������TMPro
    [SerializeField]
    private TextMeshProUGUI resultText;
    [SerializeField]
    private TextMeshProUGUI resultTime;

    //Mainmenu�ɖ߂��悤�ɂ���
    private bool _canSkip;

    [SerializeField]
    private TextMeshProUGUI plzClick;

    //�N���b�N�𑣂��e�L�X�g�̃A�j���[�V����
    private Animator anim;

    //���[�f�B���O���̔w�i�p�l��(������Ԃ͔�\��)
    [SerializeField]
    private GameObject sceneFadePanel;
    //sceneFadePanel��Image�R���|�\�l���g
    private Image sceneFadePanelImg;
    [SerializeField]
    //"LOADING"�̃e�L�X�g(������Ԃ͔�\��)
    private TextMeshProUGUI loadingText;

    private AsyncOperation async;
    //���炩���߃��[�h���Ă����V�[���ɑJ�ڂ��邩�ǂ���
    private bool loadScene;

    //�G���h���[���ɍs�����ǂ���
    public static bool goEndRoll;


    //�Q�[���N�����ɏ�����
    [RuntimeInitializeOnLoadMethod]
    private void ResetGoEndRoll()
    {
        goEndRoll = false;
    }


    private void Start()
    {
        //�I�������Q�[�����x��
        gamelevel = StageLevel._gameLevel;

        //�N���A�^�C��(�b��)�����Ԃɒ���
        _clearTime = TimeManager._endTime;
        _clearTimeMinute = (int)_clearTime / 60;
        _clearTimeSeconds = (int)Mathf.Floor(_clearTime % 60);
        
        _clearTimeDecimalPoint = _clearTime - (int)_clearTime;

        //�����_�ȉ�����ʂ܂łŐ؂�グ�Ď��R���ɂ���
        _clearTimeDecimalPoint = Mathf.Floor(Mathf.Ceil(1000 * _clearTimeDecimalPoint)/10);

        if(_clearTimeDecimalPoint >= 100)
        {
            _clearTimeDecimalPoint = 99;
        }
     //�����܂�

        anim = plzClick.GetComponent<Animator>();
        sceneFadePanelImg = sceneFadePanel.GetComponent<Image>();

        _canSkip = false;

        //2�b�o������V�[����ς�����悤�ɂ���
        Invoke("CanSkip", 2.0f);

        DecideGameClear();

        //�}�E�X�J�[�\���\��
        Cursor.visible = true;
    }


    //�N���A�������ǂ���
    private void DecideGameClear()
    {
        //���ʂ�StageLevel�X�N���v�g��static�֐��ɕۑ�
        switch (gamelevel)
        {
            case "Normal":
                _selectedGameLevel = StageLevel.ClearedGameLevel[0];
                break;

            case "Hard":
                _selectedGameLevel = StageLevel.ClearedGameLevel[1];
                break;

            case "Unknown":
                _selectedGameLevel = StageLevel.ClearedGameLevel[2];
                break;
        }

        //�I��������Փx���N���A������
        if (_selectedGameLevel == true)
        {
            resultText.text = "CLEARED : " + gamelevel;
            resultTime.text = "TIME : " + _clearTimeMinute.ToString() + " m " + _clearTimeSeconds.ToString() + " s " + _clearTimeDecimalPoint.ToString();
        }

        //�Q�[���I�[�o�[�������ꍇ
        else if ((_selectedGameLevel == false))
        {
            resultText.text = "FAILED : " + gamelevel;
            resultTime.text = "TIME : " + _clearTimeMinute.ToString() + " m " + _clearTimeSeconds.ToString() + " s " + _clearTimeDecimalPoint.ToString();
        }
    }


    //�N���b�N��MainMenu�ɖ߂��悤�ɂ���
    private void CanSkip()
    {
        _canSkip = true;

        plzClick.enabled = true;
        anim.SetTrigger("Flashing");
    }


    private void Update()
    {
        //�E�N���b�N���ꂽ��Load���̃p�l���ƃe�L�X�g��\�����A�j���[�V�����̍Đ�
        if(Input.GetMouseButtonDown(0))
        {
            if(_canSkip == true)
            {
                _canSkip = false;
                plzClick.enabled = false;

                StartCoroutine("LoadingAnimationActive");

                StartCoroutine("LoadNextScene");
                StartCoroutine("ChangeScene");
            }
        }
    }


    //���炩���ߎ��̃V�[�������[�h
    IEnumerator LoadNextScene()
    {
        //�I�������Q�[�����x�����n�[�h�ŏ��߂ăN���A�����Ȃ�EndRoll��J�ڐ�ɂ���
        if(goEndRoll == true)
        {
            goEndRoll = false;
            async = SceneManager.LoadSceneAsync("EndRoll");
        }
        //����ȊO�̓��C�����j���[
        else
        {
            async = SceneManager.LoadSceneAsync("MainMenu");
        }

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
        yield return new WaitForSeconds(2.0f);
        while (loadScene == false)
        {
            yield return null;
        }
        StopCoroutine("LoadingAnimationActive");
        async.allowSceneActivation = true;
    }


    //�V�[���J�ڒ��̑ҋ@���
    IEnumerator LoadingAnimationActive()
    {
        //panel��text��\��
        sceneFadePanelImg.enabled = true;
        loadingText.enabled = true;

        for(; ; )
        {
            loadingText.text = "LOADING.";
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "LOADING..";
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "LOADING...";
            yield return new WaitForSeconds(0.5f);
        }
    }

}
