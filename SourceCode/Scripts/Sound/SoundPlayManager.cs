using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayManager : MonoBehaviour
{
    //�T�E���h���Đ�����ۂɐ���������̃Q�[���I�u�W�F�N�g
    [SerializeField]
    private GameObject soundObjectPrefab;
    private GameObject soundObject;
    //�e�������������ɍĐ������T�E���h
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip hitSound;

    //�T�E���h���Đ����Ă����Ԃ��ǂ���
    //�����I�u�W�F�N�g���瓯���ɕ����̃T�E���h���Đ�����Ȃ��悤��(SpereaBullet�p)
    private bool _soundPlay;


    private void Start()
    {
        _soundPlay = false;
    }


    //�e�����������ۂ̃T�E���h�̍Đ�
    IEnumerator SoundPlay()
    {
        if(_soundPlay == false)
        {
            _soundPlay = true;

            //�I�u�W�F�N�g�̐����ʒu
            var instantiatePos = this.gameObject.transform.position;
            //���𔭂���AudioSource�̐���
            soundObject = Instantiate(soundObjectPrefab, instantiatePos, Quaternion.identity);
            audioSource = soundObject.GetComponent<AudioSource>();
            audioSource.clip = hitSound;
            //�w�莞�Ԍ�ɔj��ƃT�E���h�̍Đ�
            Destroy(soundObject, 1.2f);
            audioSource.Play();
        }

        yield return new WaitForSeconds(0.1f);
        _soundPlay = false;
    }

}
