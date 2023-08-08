using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


//�V�[���J�ڂ��邩��q�˂��ۂɒ񎦂����"�͂�"�{�^��
public class AgreeButtonScript : MonoBehaviour
{
    //���̃{�^���������ꂽ�Ƃ��Ɏw��̃V�[���ɑJ�ڂ���
    public void OnClick()
    {
        //QuitGameManager._canQuitGame�̓V�[���J�ڂ��s�����Ԃ�������
        if (QuitGameManager._canQuitGame == true)
        {
            QuitGameManager._canQuitGame = false;

            //�ꎞ��~����
            Time.timeScale = 1;

            ChangeScene();
        }
    }


    //�V�[���J��
    private void ChangeScene()
    {
        switch (QuitGameManager._sceneName)
        {
            case "MainMenu":
                //�Q�[���A�v���̏I��
                Application.Quit();
                break;

            case "MainGame":
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }
}
