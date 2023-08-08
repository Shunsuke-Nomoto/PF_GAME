using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayManager : MonoBehaviour
{
    //サウンドを再生する際に生成される空のゲームオブジェクト
    [SerializeField]
    private GameObject soundObjectPrefab;
    private GameObject soundObject;
    //弾が当たった時に再生されるサウンド
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip hitSound;

    //サウンドを再生している状態かどうか
    //同じオブジェクトから同時に複数のサウンドが再生されないように(SpereaBullet用)
    private bool _soundPlay;


    private void Start()
    {
        _soundPlay = false;
    }


    //弾が当たった際のサウンドの再生
    IEnumerator SoundPlay()
    {
        if(_soundPlay == false)
        {
            _soundPlay = true;

            //オブジェクトの生成位置
            var instantiatePos = this.gameObject.transform.position;
            //音を発するAudioSourceの生成
            soundObject = Instantiate(soundObjectPrefab, instantiatePos, Quaternion.identity);
            audioSource = soundObject.GetComponent<AudioSource>();
            audioSource.clip = hitSound;
            //指定時間後に破壊とサウンドの再生
            Destroy(soundObject, 1.2f);
            audioSource.Play();
        }

        yield return new WaitForSeconds(0.1f);
        _soundPlay = false;
    }

}
