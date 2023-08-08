using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


//�V�[���J�ڂ��邩��q�˂��ۂɒ񎦂����"������"�{�^��
public class DisagreeButtonScript : MonoBehaviour
{
    //�V�[���J�ڂ��邩�q�˂��ۂ̉�ʂ��B���w�i�̃p�l���Ƃ���image�R���|�[�l���g
    [SerializeField]
    private Image sceneFadePanel;
    private Image _panel;

    //�V�[���J�ڂ��邩�q�˂�e�L�X�g
    [SerializeField]
    private TextMeshProUGUI confirmText;

    //"�͂�"�{�^��
    [SerializeField]
    private GameObject agreeButton;
    //"������"�{�^��(���̃I�u�W�F�N�g)
    [SerializeField]
    private GameObject disagreeButton;


    private void Awake()
    {
        _panel = sceneFadePanel.GetComponent<Image>();
    }


    //���̃{�^���������ꂽ�Ƃ��Ɉꎞ��~��Ԃ����������̉�ʂ��ĊJ����
    public void OnClick()
    {
        if (QuitGameManager._canQuitGame == true)
        {
            //�ꎞ��~����
            Time.timeScale = 1;

            QuitGameManager._canQuitGame = false;

            DisappearConfirm();
        }
    }


    //���̃{�^������A�N�e�B�u�ɂȂ�����QuitGameManager._canPressEscape��1�b�ԉ����Ȃ�����
    private void OnDisable()
    {
        Invoke("CanPressEscape", 1.0f);
    }
        

    private void CanPressEscape()
    {
        QuitGameManager._canPressEscape = true;
    }


    //�V�[���J�ڂ��邩��q�˂��A�̃p�l��,�{�^�����A�N�e�B�u�ɂ���
    private void DisappearConfirm()
    {
        if(QuitGameManager._sceneName == "MainGame")
        {
            //�}�E�X�J�[�\����\��
            Cursor.visible = false;
        }

        _panel.enabled = false;
        confirmText.enabled = false;

        agreeButton.SetActive(false);
        disagreeButton.SetActive(false);
    }
}
