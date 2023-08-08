using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//barrierを展開したときに効果音を鳴らす
public class EnemyBarrierSoundManager : MonoBehaviour
{
    //EnemyBarrierがアクティブ状態になったとき再生されるサウンド素材とそれを再生するAudioSource
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;


    private void Awake()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }


    //アクティブ状態になったら実行
    private void OnEnable()
    {
        audioSource.clip = sound1;
        audioSource.Play();
    }
}
