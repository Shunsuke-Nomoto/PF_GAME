using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingSystem : MonoBehaviour
{
    //target��player
    [SerializeField]
    private GameObject target;

    //player�̍��W�ƌ���
    private Vector3 targetPos;
    private Vector3 targetDirection;

    //�e�𔭎˂���ۂɗ^����^����
    [SerializeField]
    private float _addForcePower;

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

    //��������e�I�u�W�F�N�g�Ƃ��̃v���n�u
    [SerializeField]
    private GameObject bulletPrefab;
    private GameObject bullet;
    private Rigidbody bullet_rb;

    //�ˌ��ł����Ԃ�
    private bool _canShoot;

    //���̃I�u�W�F�N�g(enemy)��target�I�u�W�F�N�g�̋���
    private float _distance;
    //�ő�򋗗�
    private float _maxDistance;
    //�ő�M�������Z�o���邽�߂̒e�̃X�s�[�h
    private float _bulletSpeed;

    //gunbarrel�̃��[�J���̌X��
    private float _Angle;
    //gunbarrel�̍ő�̌X���ƍŏ��̌X��
    private float _MinAngle = 0;
    private float _MaxAngle = 45;

    //���e���˂܂ł̑ҋ@����
    private float _shootingrate;

    //FreezeBullet�̌��ʂ��󂯂邩(���ʂ̏d���͂��Ȃ�)
    public static bool _canFreeze;
    //enemy�𓀂点����Ԃ������A�C�R��
    [SerializeField]
    private GameObject frozenIcon;

    //�e�𔭎˂���ۂ̃T�E���h�Ƃ���AudioSource(BombManager�ɃA�^�b�`����Ă���)
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip shootingSound;
    

    private void Awake()
    {
        _shootingrate = StageLevel._shootingRate;

        targetPos = target.transform.position;
    }


    // Start is called before the first frame update
    void Start()
    {
        _canShoot = false;
        _canFreeze = true;

        //frozenIcon�̏�����Ԃ͔�\��
        frozenIcon.SetActive(false);

        _bulletSpeed = _addForcePower / 1.5f;
        CalculateMaxRange();

        //AudioSource�R���|�[�l���g�̎擾�ƍĐ�����T�E���h�̐ݒ�
        audioSource = bombManager.GetComponent<AudioSource>();
        audioSource.clip = shootingSound;

        //�Q�[���J�n2�b���3�b�̃J�E���g�_�E��->�Q�[���X�^�[�g
        Invoke("StartShootingCoroutine", 5.0f);
    }


    //�ő�˒��̌v�Z
    private void CalculateMaxRange()
    {
        _maxDistance = Mathf.Pow(_bulletSpeed, 2) / 9.81f;
    }


    //player�̕�������������
    IEnumerator FocusTargetPos()
    {
        for(; ; )
        {
            targetPos = target.transform.position;

            if (target)

            targetDirection = targetPos - transform.position;
            targetDirection.y = 0;
            targetDirection = targetDirection.normalized;

            transform.rotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            yield return new WaitForSeconds(0.25f);
        }
    }


    //���˂���e�̐���
    private void GenerateBullet()
    {
        if (_canShoot == true)
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
            bullet_rb.AddForce(_shootingAngle * _addForcePower, ForceMode.Impulse);

            //�ˌ����̍Đ�
            audioSource.Play();

            _canShoot = false;
        }
    }


    //�e���v�Z
    private void BallisticCalculation()
    {
        //player�Ƃ̋���
        _distance = Mathf.Abs(Mathf.Sqrt(Mathf.Pow((targetPos.x - transform.position.x), 2) + Mathf.Pow((targetPos.z - transform.position.z) , 2)));
        
        //player���ő�򋗗��O�ł���Ύˌ��͍s��Ȃ�
        if(_distance >= _maxDistance)
        {
            _canShoot = false;
            return;
        }

        _canShoot = true;

        //_distance�ƒe����p�������ˊp�x�̌v�Z
        _Angle = Mathf.Asin(_distance * 9.81f / Mathf.Pow(_bulletSpeed, 2));
        _Angle = _Angle * Mathf.Rad2Deg;
        _Angle = _Angle / 2;


        _Angle = Mathf.Min(_Angle, _MaxAngle);
        _Angle = Mathf.Max(_Angle, _MinAngle);

        //_Angle�ɂȂ�悤�C�g����]
        gunbarrel.transform.localRotation = Quaternion.Euler(90 - _Angle, 0, 0);
    }


    //�e���v�Z�ƒe�̔��˂̈�A�̏�������ɂ܂Ƃ߂�_shootingrate�ɑ������e���|�Ŏˌ����s��
    IEnumerator Shooting()
    {
        for(; ; )
        {
            yield return new WaitForSeconds(_shootingrate - 1.0f);

            BallisticCalculation();

            yield return new WaitForSeconds(1.0f);

            GenerateBullet();
        }
        
    }


    //��A�̎ˌ��������J�n����
    private void StartShootingCoroutine()
    {
        StartCoroutine("FocusTargetPos");
        StartCoroutine("Shooting");
    }


    //enemy�I�u�W�F�N�g��FreezeBullet������������,��A�̎ˌ���������莞�Ԓ�~������
    IEnumerator Freeze_Shooting()
    {
        //Freeze���

        StopCoroutine("FocusTargetPos");
        StopCoroutine("Shooting");
        frozenIcon.SetActive(true);

        yield return new WaitForSeconds(3.0f);


        //Freeze��Ԃ̉���

        frozenIcon.SetActive(false);

        _canFreeze = true;

        StartShootingCoroutine();
    }
}
