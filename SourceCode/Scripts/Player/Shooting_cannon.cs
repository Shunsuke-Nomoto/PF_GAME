using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//player�̎ˌ����s��
public class Shooting_cannon : MonoBehaviour
{
    //�C�g�ƖC�g�̊p�x(�x�N�g��)���Z�o���邽�߂ɐݒ肵���I�u�W�F�N�g,�y�т���炩��Z�o�����x�N�g��
    [SerializeField]
    private GameObject gunbarrel;
    [SerializeField]
    private GameObject bombManager;
    [SerializeField]
    private GameObject gunbarrelStandard;
    private Vector3 _standardAngle;
    private Vector3 _shootingAngle;
    private Vector3 _bulletPos;

    //gunbarrel�̃��[�J��x���̌X��
    private float _barrelAnglex;
    //������Ԃ�gunbarrel�̌X��
    private Vector3 _barrelAngleDefault;

    private float _input_Rotx;

    //��������Bullet�I�u�W�F�N�g�Ƃ��̃v���n�u
    [SerializeField]
    private GameObject bullet1Prefab;
    [SerializeField]
    private GameObject bullet2Prefab;
    [SerializeField]
    private GameObject bullet3Prefab;
    private GameObject bulletPrefab;
    private GameObject bullet;
    private Rigidbody bullet_rb;

    private int _bulletCount;

    //�����[�h�ς݂�
    private bool _reload;
    //�����[�h����
    public static bool _reloading;

    //�eBullet�������A�C�R���Ƃ����\������p�l��,�y�т���image�R���|�[�l���g
    [SerializeField]
    private Sprite sprite0;
    [SerializeField]
    private Sprite sprite1;
    [SerializeField]
    private Sprite sprite2;
    [SerializeField]
    private GameObject panel;
    private Sprite sprite;
    private Image img;

    //reload���K�v�Ȃ��Ƃ������摜
    [SerializeField]
    private Image reloadMessageImg;
    //�I�u�W�F�N�g�ɋ߂��Ƃ��ɗ����悤�����e�L�X�g
    [SerializeField]
    private TextMeshProUGUI leaveMessage;

    //�ˌ��Ԑ��̎���player����]��������͗�
    private float _inputX;
    //y����]�̃X�s�[�h
    [SerializeField]
    private float _horizontalSpeed;
    //x����]�̃X�s�[�h
    [SerializeField]
    private float _verticalSpeed;

    //player��y���̃��[�J���ȉ�]�Ƃ��̍ő�ŏ��l
    private float _currentAngle;
    private float _minAngle;
    private float _maxAngle;
    private float _angle;

    //�e�𔭎˂���ۂ̃T�E���h�Ƃ���AudioSource(BombManager�ɃA�^�b�`����Ă���)
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip shootingSound;


    private void Awake()
    {
        _barrelAngleDefault = gunbarrel.transform.localEulerAngles;
        _barrelAnglex = gunbarrel.transform.localEulerAngles.x;

        _bulletCount = 0;
        bulletPrefab = bullet1Prefab;

        _reload = false;
        _reloading = false;

        img = panel.GetComponent<Image>();
        panel.GetComponent<Image>().color = new Color(1, 1, 1, 0.70f);

        img.sprite = sprite0;

        reloadMessageImg.enabled = true;

        //AudioSource�R���|�[�l���g�̎擾�ƍĐ�����T�E���h�̐ݒ�
        audioSource = bombManager.GetComponent<AudioSource>();
        audioSource.clip = shootingSound;
    }


    //�ˌ��V�X�e��
    IEnumerator Shooting()
    {
        for (; ; )
        {

            //�ˌ��\(_reload == true)�ł���Ύˌ�
            if (Input.GetMouseButtonDown(0) && _reload == true)
            {
                if (DistinguishOtherObjInRange._canShoot == true)
                {
                    //�����[�h��Ԃ���������
                    _reload = false;

                    StopCoroutine("Reloading");

                    //Bullet�̃A�C�R���𔖂�����
                    panel.GetComponent<Image>().color = new Color(1, 1, 1, 0.70f);
                    //�����[�h�𑣂����b�Z�[�W��\������
                    reloadMessageImg.enabled = true;

                    //�e����
                    GenerateBullet();

                    //�ˌ����̍Đ�
                    audioSource.Play();

                    yield return new WaitForSeconds(1.0f);

                    StartCoroutine("Reloading");
                }

                else if (DistinguishOtherObjInRange._canShoot == false)
                {
                    //�I�u�W�F�N�g���痣���悤�������b�Z�[�W��\��
                    LeaveMessage();
                    yield return new WaitForSeconds(1.0f);
                }
               
            }

            //�ˌ��\�łȂ���΃����[�h���K�v
            else if (Input.GetMouseButtonDown(0) && _reload == false)
            {
                StartCoroutine("flashingReloadImg");

                yield return new WaitForSeconds(1.5f);
            }
            yield return null;
        }

    }


    //�����[�h�V�X�e��
    IEnumerator Reloading()
    {
        for (; ; )
        {
            if (Input.GetKeyDown("r") && _reload == false)
            {
                //�����[�h���ɂ���
                _reloading = true;

                StopCoroutine("Shooting");
                StartCoroutine("ColorAlphaChange");
                
                yield return new WaitForSeconds(1.5f);

                //�����[�h������Ԃɂ���
                _reload = true;

                //�����[�h�𑣂�Image���\���ɂ���
                reloadMessageImg.enabled = false;

                //�����[�h������������
                _reloading = false;

                yield return new WaitForSeconds(1.0f);

                StartCoroutine("Shooting");
            }

            yield return new WaitForSeconds(0.01f);
        }

    }


    //���˂���e�̐���
    private void GenerateBullet()
    {
        //bombManager�͖C�g��AgunbarrelStandard�̓I�u�W�F�N�g���S
        //�����̍��W����e���ˎ��̃x�N�g�����Z�o
        _bulletPos = bombManager.transform.position;
        _standardAngle = gunbarrelStandard.transform.position;
        _shootingAngle = _bulletPos - _standardAngle;
        _shootingAngle = _shootingAngle.normalized;

        //��������_addForcePower�̗͂�������
        bullet = Instantiate(bulletPrefab, _bulletPos, Quaternion.identity);
        bullet_rb = bullet.GetComponent<Rigidbody>();
        bullet_rb.AddForce(_shootingAngle * 40.0f, ForceMode.Impulse);
    }


    //player�I�u�W�F�N�g�̖C�g�̏c�����̊p�x����
    private void RotateBarrel()
    {
        //w,s�L�[�̓���
        _input_Rotx = -1 * _verticalSpeed * Input.GetAxis("Vertical") * Time.deltaTime; 

        if (_input_Rotx != 0)
        {
            if (80 < _barrelAnglex + _input_Rotx && _barrelAnglex + _input_Rotx < 90)
            {
                gunbarrel.transform.Rotate(new Vector3(_input_Rotx, 0, 0));
                _barrelAnglex = _barrelAnglex + _input_Rotx;
            }
        }
    }



    //�e�̐؂�ւ�
    private void Bullet()
    { 
        if (Input.GetKeyDown("q") && _reload == false && _reloading == false)
        {
            _bulletCount += 1;

            if (_bulletCount > 2)
            {
                _bulletCount = 0;
            }

            switch (_bulletCount)
            {
                case 0:
                    bulletPrefab = bullet1Prefab;
                    sprite = sprite0;
                    break;

                case 1:
                    bulletPrefab = bullet2Prefab;
                    sprite = sprite1;
                    break;

                case 2:
                    bulletPrefab = bullet3Prefab;
                    sprite = sprite2;
                    break;
            }

            img.sprite = sprite;
        }    
    }


    //0.01�b�u���ɒe�̐؂�ւ��Ǝˌ��p�x�̕ύX
    IEnumerator Shooting_Coroutine()
    {
        for (; ; )
        {
            Bullet();
            RotateBarrel();
            yield return null;
        }
    }


    //�����[�h���ɂ��̎��̒e�̉摜�����X�ɖ��邭�Ȃ�(�񑕓U���͈Â��Ȃ��Ă���)
    IEnumerator ColorAlphaChange()
    {
        if (_reload == false)
        {
            for (int i = 0; i < 30; i++)
            {
                panel.GetComponent<Image>().color = new Color(1, 1, 1, img.color.a + 0.01f);

                yield return new WaitForSeconds(0.05f);
            }
        }
    }


    //�ˌ�����"���I�u�W�F�N�g�ɋ߂�"��"enemy�I�u�W�F�N�g�ɋ߂�"�Ƃ��ɂ��̎|��\��
    private void LeaveMessage()
    {
        leaveMessage.color = new Color(0, 0, 0, 1);

        if (DistinguishOtherObjInRange._noEntryArea == true)
        {
            leaveMessage.text = "Too close to the enemy";
        }
        else if((DistinguishOtherObjInRange._noEntryArea == false))
        {
            leaveMessage.text = "Move away from the object";
        }

        Invoke("LeaveMessageDelete", 0.95f);
    }


    //���ꂽ�炻�̌x�����\����
    private void LeaveMessageDelete()
    {
        leaveMessage.color = new Color(0, 0, 0, 0);
    }



    //�ˌ����[�h�̎��ɍ��E�L�[��player�����E�ɉ�]
    IEnumerator PlayerRotate()
    {
        _currentAngle = this.transform.rotation.eulerAngles.y;
        _minAngle = _currentAngle - 15;
        _maxAngle = _currentAngle + 15;

        for (; ; )
        {
            //a,d�L�[�̓���
            _inputX = _horizontalSpeed * Input.GetAxisRaw("Horizontal") * Time.deltaTime;
            _angle = _currentAngle + _inputX;

            if(_angle >= _minAngle && _angle <= _maxAngle )
            {

                this.transform.rotation *= Quaternion.AngleAxis(_inputX, Vector3.up);
                _currentAngle = _angle;
            }

            yield return new WaitForSeconds(0.01f);
        }

    }


    //reload���K�v�Ȏ��Ɏˌ��{�^�����������ꍇ��"RELOAD"�̉摜���_��
    IEnumerator flashingReloadImg()
    {

        for (int j = 1; j < 51; j++)
        {
            reloadMessageImg.GetComponent<Image>().color = new Color(1, 1, 1, 1 - 0.01f * j);
            yield return new WaitForSeconds(0.01f);
        }

        for (int j = 1; j < 51; j++)
        {
            reloadMessageImg.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f + 0.01f * j);
            yield return new WaitForSeconds(0.01f);
        }

    }


    //ModeChange�X�N���v�g�ɂ���Ă��̃X�N���v�g����A�N�e�B�u�ɂȂ�����gunbarrel�̉�]���f�t�H���g�ɖ߂�
    private void OnDisable()
    {
        gunbarrel.transform.localEulerAngles = _barrelAngleDefault;
    }

    //ModeChange�X�N���v�g�ɂ���Ă��̃X�N���v�g���A�N�e�B�u�ɂȂ�����gunbarrel�̉�]���f�t�H���g�ɖ߂�
    private void OnEnable()
    {
        _barrelAnglex = _barrelAngleDefault.x;
    }

}
