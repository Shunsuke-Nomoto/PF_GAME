using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    //���ۂɃv���C���Ă�������
    public static float _endTime;
    //�v���J�n�����^�C��
    private float _startTime;
    //�Q�[�����N���A���Q�[���I�[�o�[�ɂȂ����^�C��
    private float _elapsedTime;

    //�Q�[���J�n���̃J�E���g�_�E���e�L�X�g
    [SerializeField]
    private TextMeshProUGUI countdownText;

    //�Q�[�����J�nsyry�R���[�`��
    private IEnumerator gameStart;


    private void Awake()
    {
        //�N���A���Ԃ̏�����
        _endTime = 0;

        gameStart = GameStart();
    }

    private void Start()
    {
        StartCoroutine(gameStart);
    }


    //�Q�[���J�n2�b���3�b�̃J�E���g�_�E�� -> �Q�[���X�^�[�g
    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(2.0f);

        countdownText.text = "3";
        yield return new WaitForSeconds(1.0f);

        countdownText.text = "2";
        yield return new WaitForSeconds(1.0f);

        countdownText.text = "1";
        yield return new WaitForSeconds(1.0f);

        countdownText.text = "START";
        yield return new WaitForSeconds(1.0f);

        countdownText.gameObject.SetActive(false);

        StartTimeMeasurement();
    }


    private void StartTimeMeasurement()
    {
        //�v���J�n���Ԃ̋L�^
        _startTime = Time.time;
    }


    public void MeasuringClearTime()
    {

        //�I�����̎��Ԃ̋L�^
        _elapsedTime = Time.time;

        //�N���A���� = �v���J�n���� - �I������

        _endTime = _elapsedTime - _startTime;
    }
}
