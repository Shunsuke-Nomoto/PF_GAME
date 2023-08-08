using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierManager : MonoBehaviour
{
    //回転速度
    [SerializeField]
    private float _rotateSpeed;
 

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("RotateBarrierParent");
    }


    //このスクリプトがアタッチされているBarrierParentオブジェクトをローカル座標で回転
    private IEnumerator RotateBarrierParent()
    {
        for (; ; )
        {
            this.transform.localRotation *= Quaternion.Euler(0, _rotateSpeed, 0);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
