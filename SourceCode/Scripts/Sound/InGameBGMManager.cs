using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameBGMManager : MonoBehaviour
{
    //�V�[����
    private string _sceneName;
    //BGM���Đ�����AudioSource
    private AudioSource audioSource;
    private AudioClip bgm;
    //BGM�f��
    [SerializeField]
    private AudioClip mainMenu;
    [SerializeField]
    private AudioClip mainGame1;
    //Unknown���x���œG�̗̑͂������ȉ��ɂȂ�����Đ�����BGM
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
        //�ŏ��͉���0
        audioSource.volume = 0;

        //�V�[�����ōĐ�����BGM��ς���
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

    
    //MainGame�J�n��5�b���BGM���Đ����n�߂�
    IEnumerator BGMStart()
    {
        switch (_sceneName)
        {
            case "MainMenu":
                break;

            //MainGame�V�[����������J�n5�b�҂�
            case "MainGame":
                yield return new WaitForSeconds(5.0f);
                break;

            case "Result":
                break;

            //MainGame�V�[����������J�n3�b�҂�
            case "EndRoll":
                yield return new WaitForSeconds(3.0f);
                break;
        }

        audioSource.clip = bgm;
        audioSource.Play();

        //�i�X�ƃ{�����[�����グ��
        for(int i = 1; i < 26; i++)
        {
            audioSource.volume += 0.004f;
            yield return new WaitForSeconds(0.1f);
        }
    }


    //BGM�����X�ɉ�����~����
    IEnumerator BGMOff()
    {
        //�i�X�ƃ{�����[����������
        for (int i = 1; i < 26; i++)
        {
            audioSource.volume -= 0.04f;
            yield return new WaitForSeconds(0.1f);
        }
        audioSource.enabled = false;
    }


    //Unknown���x���̎���Enemy�̗̑͂�������؂��EnemyHPManager�ɂ���ČĂ΂��
    //MainGame���ŗ���Ă���BGM��ς���
    IEnumerator ChangeBGM()
    {
        StartCoroutine("BGMOff");
        //BGMOff�J�n��2.5f��audioSource.enabled = falsen�ɂȂ邽�߂���܂ŏ������~�߂Ă���
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
