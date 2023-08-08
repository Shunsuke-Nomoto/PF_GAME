using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndRollScript : MonoBehaviour
{
    //�G���h���[���p�l���ƃX���C�h������A�j���[�^�[
    [SerializeField]
    private GameObject endRollPanel;
    private Animator endRollAnim;

    //��ʉB���p�̃p�l���ƃt�F�[�h�C������A�j���[�^�[
    [SerializeField]
    private GameObject sceneFadePanel;
    private Animator sceneFadeAnim;

    //BGM���Ǘ�����I�u�W�F�N�g�Ƃ��̃X�N���v�g
    [SerializeField]
    private GameObject BGMMnager;
    private InGameBGMManager inGameBGMManager;

    //UnknownLevel��������邽�߂̏���
    public static bool _canPlayUnknown;


    //�Q�[���N�����ɏ�����
    [RuntimeInitializeOnLoadMethod]
    private void Unknown()
    {
        _canPlayUnknown = false;
    }


    private void Start()
    {
        //UnknownLevel�����
        _canPlayUnknown = true;

        sceneFadeAnim = sceneFadePanel.GetComponent<Animator>();
        //��ʃt�F�[�h�C��
        sceneFadeAnim.SetTrigger("FadeIn");

        endRollAnim = endRollPanel.GetComponent<Animator>();

        inGameBGMManager = BGMMnager.GetComponent<InGameBGMManager>();

        //�G���h���[���X�^�[�g
        Invoke("EndRollStart", 4.0f);
        //BGM��~,��ʃt�F�[�h�A�E�g
        Invoke("StopBGM", 38.0f);

        //�V�[���J��
        Invoke("ChangeScene", 42.0f);
    }


    private void EndRollStart()
    {
        //�G���h���[���̃A�j���[�V�������X�^�[�g
        endRollAnim.SetBool("EndRollStart",true);
    }

    private void StopBGM()
    {
        //��ʂ̃t�F�[�h�A�E�g
        sceneFadeAnim.SetTrigger("FadeOut");
        //BGM���I�t
        inGameBGMManager.StartCoroutine("BGMOff");
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
