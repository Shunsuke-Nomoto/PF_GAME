using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//player��shooting���[�h��moving���[�h��؂�ւ���
public class ModeChange : MonoBehaviour
{
    //moving���[�h�̃X�N���v�g
    private Moving_cannon movingCannon;
    //shooting���[�h�̃X�N���v�g
    private Shooting_cannon shootingCannon;
    //�J�����Ǐ]�̃X�N���v�g
    private Camera_Manager cameraManager;
    //�J�������Y�[����Ԃɂ���X�N���v�g
    private CameraZoomManager cameraZoomManager;
    
    //�v���C���[�̃I�u�W�F�N�g
    [SerializeField]
    private GameObject cannon;

    //�v���C���[�̎��_�̃J����
    [SerializeField]
    private GameObject mainCamera;

    //shooting���[�h�̎��̃A�C�R����sprite�摜
    [SerializeField]
    private Sprite shootingIcon;
    //moving���[�h�̃A�C�R����sprite�摜
    [SerializeField]
    private Sprite movingIcon;

    //�A�C�R����\������panel
    [SerializeField]
    private GameObject panel;

    //���݂̃��[�h�̃A�C�R�������[����ϐ�
    private Sprite sprite;
    //panel�̉摜��ύX����ۂ̃R���|�[�l���g
    private Image img;


    //���[�h�؂�ւ��p��bool�l
    public static bool _firingmode = false;


    private void Awake()
    {
        movingCannon = cannon.GetComponent<Moving_cannon>();
        shootingCannon = cannon.GetComponent<Shooting_cannon>();
        cameraManager = mainCamera.GetComponent<Camera_Manager>();
        cameraZoomManager = mainCamera.GetComponent<CameraZoomManager>();

        img = panel.GetComponent<Image>();
        //img�̏�����Ԃ�movingIcon
        sprite = movingIcon;
        img.sprite = sprite;
    }


    private void Start()
    { 
        StopCannonScripts();

        //�Q�[���J�n2�b���3�b�̃J�E���g�_�E�� -> �Q�[���X�^�[�g
        Invoke("StartCannonCoroutine", 5.0f);
    }


    IEnumerator Mode()
    {
        for(; ; )
        {
            //�ړ����[�h��space�L�[�������ꂽ��ˌ����[�h�Ɉڍs
            if (Input.GetKeyDown("space") && _firingmode == false)
            {
                //�ˌ����[�h�ɂȂ�����sprite��shootingIcon�ɕύX
                sprite = shootingIcon;
                img.sprite = sprite;

                _firingmode = true;

                movingCannon.enabled = false;
                shootingCannon.enabled = true;
                cameraManager.enabled = false;
                cameraZoomManager.enabled = true;

                //shooting���[�h���ɃA�N�e�B�u�ɂȂ��Ă���R���[�`�����X�^�[�g
                shootingCannon.StartCoroutine("Shooting");
                shootingCannon.StartCoroutine("Reloading");
                shootingCannon.StartCoroutine("Shooting_Coroutine");
                shootingCannon.StartCoroutine("PlayerRotate");


                yield return new WaitForSeconds(1.0f);
            }

            //�ˌ����[�h��space�L�[�������ꂽ��ړ����[�h�Ɉڍs
            else if (Input.GetKeyDown("space") && _firingmode == true)
            {
                //�����[�h���̓��[�h�`�F���W���Ȃ�
                if (Shooting_cannon._reloading == false)
                {

                    //�ړ����[�h�ɂȂ�����sprite��movingIcon�ɕύX
                    sprite = movingIcon;
                    img.sprite = sprite;

                    _firingmode = false;

                    //shooting���[�h���ɃA�N�e�B�u�ɂȂ��Ă���R���[�`�����X�g�b�v
                    shootingCannon.StopCoroutine("Shooting");
                    shootingCannon.StopCoroutine("Reloading");
                    shootingCannon.StopCoroutine("Shooting_Coroutine");
                    shootingCannon.StopCoroutine("PlayerRotate");

                    cameraZoomManager.enabled = false;
                    cameraManager.enabled = true;
                    shootingCannon.enabled = false;
                    movingCannon.enabled = true;

                    yield return new WaitForSeconds(1.0f);
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
       
    }


    //ModeChange�X�N���v�g���N��
    //player�̏�����Ԃ�moving���[�h
    private void StartCannonCoroutine()
    {
        StartCoroutine(Mode());
        movingCannon.enabled = true;
    }


    //�v���C���[�ɂ��Ă��鑀��\�ȃX�N���v�g���~������
    private void StopCannonScripts()
    {
        shootingCannon.enabled = false;
        movingCannon.enabled = false;
        cameraZoomManager.enabled = false;
    }

}
