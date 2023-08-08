using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierManager : MonoBehaviour
{
    //��]���x
    [SerializeField]
    private float _rotateSpeed;
 

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("RotateBarrierParent");
    }


    //���̃X�N���v�g���A�^�b�`����Ă���BarrierParent�I�u�W�F�N�g�����[�J�����W�ŉ�]
    private IEnumerator RotateBarrierParent()
    {
        for (; ; )
        {
            this.transform.localRotation *= Quaternion.Euler(0, _rotateSpeed, 0);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
