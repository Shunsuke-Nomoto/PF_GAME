using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

//�Q�[�����I������X�N���v�g
public class QuitGameManager : MonoBehaviour
{
    //�Q�[�����I���ł���(���C�����j���[�ɖ߂��)��
    public static bool _canQuitGame;
    //escape�{�^���������邩
    public static bool _canPressEscape;
    //���݂̃V�[����
    public static string _sceneName;

    //���[�h�p�̉�ʉB��
    [SerializeField]
    private Image sceneFadePanel;
    //scenepanel��img�R���|�[�l���g
    private Image _panel;

    //�I�����邩�ǂ����̊m�F�e�L�X�g
    [SerializeField]
    private TextMeshProUGUI confirmText;

    //yes�{�^��
    [SerializeField]
    private GameObject agreeButton;
    //no�{�^��
    [SerializeField]
    private GameObject disagreeButton;


    private void Awake()
    {
        _panel = sceneFadePanel.GetComponent<Image>();

        agreeButton.SetActive(false);
        disagreeButton.SetActive(false);

        _sceneName = SceneManager.GetActiveScene().name;
    }


    private void Start()
    { 
        //MainMenu�Ȃ�Q�[�����I�����邩�ǂ���,MainGame�Ȃ�MainMenu�ɋA�邩�ǂ���
        _canQuitGame = false;

        //Escape�������邩�ǂ���
        _canPressEscape = true;

        _panel.enabled = false;
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && _canPressEscape == true)
        {
            //�Q�[���̈ꎞ��~(�ꎞ��~������DisagreeButtonScript��AgreeButtonScript�ōs��)
            
            Time.timeScale = 0;

            //�}�E�X�J�[�\���\��
            Cursor.visible = true;

            _canPressEscape = false;
            Confirm();
            _canQuitGame = true;
        }
    }


    private void Confirm()
    {
        //�m�F�p�̉�ʂ��o��
        _panel.enabled = true;
        confirmText.enabled = true;

        //���̃V�[�����ɂ���Ċm�F�p�̃e�L�X�g���o��
        switch(_sceneName)
        {
            case "MainMenu":
                confirmText.text = "Quit the game?";
                break;

            case "MainGame":
                confirmText.text = "Return to the main menu?";
                break;
        }

        AppearConfirmButton();
    }


    private void AppearConfirmButton()
    {
        //�}�E�X�J�[�\���\��
        Cursor.visible = true;

        agreeButton.SetActive(true);
        disagreeButton.SetActive(true);
    }

}
