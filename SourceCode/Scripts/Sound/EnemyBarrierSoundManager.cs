using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//barrier��W�J�����Ƃ��Ɍ��ʉ���炷
public class EnemyBarrierSoundManager : MonoBehaviour
{
    //EnemyBarrier���A�N�e�B�u��ԂɂȂ����Ƃ��Đ������T�E���h�f�ނƂ�����Đ�����AudioSource
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;


    private void Awake()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }


    //�A�N�e�B�u��ԂɂȂ�������s
    private void OnEnable()
    {
        audioSource.clip = sound1;
        audioSource.Play();
    }
}
