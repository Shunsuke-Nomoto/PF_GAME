using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{

    //PlayerHPManager��EnemyHPManager�ɂ���Ă��̃X�N���v�g���A�N�e�B�u�ɂȂ�
    //�A�N�e�B�u�ɂȂ�����w��b���ŃV�[���J��
    private void OnEnable()
    {
        Invoke("ChangeScene", 3.0f);
    }


    private void ChangeScene()
    {
        SceneManager.LoadScene("Result"); 
    }
}
