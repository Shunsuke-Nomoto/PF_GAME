using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//����͎g�p���Ȃ�����,���[���h���HP�o�[��player�Ɍ�������X�N���v�g
public class HPSliderLookMe : MonoBehaviour
{
    private GameObject player;

    //player�̍��W��player�Ɍ������x�N�g��
    private Vector3 targetPos;
    private Vector3 targetDirection;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("player");
        StartCoroutine(LookAtPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LookAtPlayer()
    {
        for(; ; )
        {
            targetPos = player.transform.position;

            targetDirection = transform.position - targetPos;
            targetDirection.y = 0;
            targetDirection = targetDirection.normalized;
           
            transform.rotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            yield return new WaitForSeconds(0.25f);
        }
        
    }
}
