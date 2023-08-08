using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHPManager : MonoBehaviour
{
    //Enemy�I�u�W�F�N�g�̃X�e�[�^�X(StageLavel�X�N���v�g�Őݒ�)
    private int _MaxHp;
    private int _damage;
    private int _currentHp;

    //Enemy�I�u�W�F�N�g�̗̑̓o�[
    [SerializeField]
    private Slider slider;

    //Bullet�I�u�W�F�N�g�����X�N���v�g
    private BulletManager bulletManager;

    //Unknown���x���̍ۂɏo������Enemy�I�u�W�F�N�g�̎q�I�u�W�F�N�g
    [SerializeField]
    private GameObject enemyBarrier;
    //EnemyBarrier���W�J����邩(Unknown���x���Ȃ�True)
    private bool _barrierAppear;

    //TimeKeeper�I�u�W�F�N�g�Ƃ��ꂪ���Q�[���̃N���A���Ԃ��Ǘ�����X�N���v�g
    [SerializeField]
    private GameObject timeKeeper;
    private TimeManager timeManager;

    //�V�[���J�ڂ��s���I�u�W�F�N�g
    [SerializeField]
    private GameObject changeSceneManager;

    //�N���A���Q�[���I�[�o�[�ɂȂ����Ƃ��ɂ���������e�L�X�g
    [SerializeField]
    private TextMeshProUGUI deciedClearText;

    //Maingame����BGM���Ǘ�����I�u�W�F�N�g�Ƃ��̃X�N���v�g
    [SerializeField]
    private GameObject BGMMnager;
    private InGameBGMManager inGameBGMManager;

    //�e�����������ۂ̃T�E���h���Đ����鏈�����s���X�N���v�g
    private SoundPlayManager soundPlayManager;


    private void Awake()
    {
        HPDecision();
    }


    // Start is called before the first frame update
    void Start()
    {
        //�X�e�[�^�X�̔��f
        slider.maxValue = _MaxHp;
        slider.value = _MaxHp;
        _currentHp = _MaxHp;

        _barrierAppear = false;

        timeManager = timeKeeper.GetComponent<TimeManager>();

        inGameBGMManager = BGMMnager.GetComponent<InGameBGMManager>();

        soundPlayManager = this.gameObject.GetComponent<SoundPlayManager>();
    }


    //HP(_MaxHp)�̔��f
    private void HPDecision()
    {
        _MaxHp = StageLevel._enemyMaxHP;
    }

 
    //��e���HP�`�F�b�N
    private void HpCheck()
    {
        if(_currentHp <= _MaxHp / 2)
        {
            //��Փx"unknown"�̂Ƃ�(StageLevel._enemyBarrier=true)�A�̗͂������ɂȂ��EnemyBarrier�N��
            if (StageLevel._enemyBarrier == true && _barrierAppear == false)
            {
                _barrierAppear = true;
                enemyBarrier.SetActive(true);

                //BGM���ŏI����p�̂��̂ɕς���
                inGameBGMManager.StartCoroutine("ChangeBGM");
            }
        }
        

        //HP��0�ɂȂ�����
        //�S�ẴR���[�`�����~�߂��̂��A���̃I�u�W�F�N�g��player��enemy�����ʂ�����ɉ����ăN���A������s��
        if (_currentHp <= 0)
        {
            //�̗͂�0�ɂȂ�����BGM���~�߂�
            inGameBGMManager.StartCoroutine("BGMOff");

            QuitGameManager._canPressEscape = false;

            changeSceneManager.SetActive(true);

            //TimeManager����N���A�^�C�����L�^
            timeManager.MeasuringClearTime();

            //�N���A����
            DecideGameClear();

            this.gameObject.SetActive(false);
        }
    }


    //�I�u�W�F�N�g���g���K�[������_���[�W�v�Z
    private void OnTriggerEnter(Collider other)
    {
        DamageCheckCollider(other);
    }


    //�_���[�W�v�Z
    private void DamageCheckCollider (Collider collider)
    {
        //���������I�u�W�F�N�g��bulletManager���擾
        bulletManager = collider.gameObject.GetComponent<BulletManager>();

        //bulletManager���擾�ł��Ȃ�������(Bullet�ȊO�ɓ������Ă���ꍇ)
        if (bulletManager == null)
        {
            return;
        }

        //�e�����������ۂ̃T�E���h�̍Đ�
        soundPlayManager.StartCoroutine("SoundPlay");

        _damage = BulletManager._damage;

        _currentHp = _currentHp - _damage;
        slider.value = _currentHp;
        HpCheck();
    }


    //�N���A������StageLevel�Ɋi�[����Ă��邻�̓�Փx�̃N���A�����true�ɂ���
    //�܂��A���ꂪ���߂ẴN���A�Ȃ�StageLevel.NeverCleared[i]��true�ɂ���
    private void DecideGameClear()
    {
        var gamelevel = StageLevel._gameLevel;

        switch (gamelevel)
        {
            case "Normal":
                StageLevel.ClearedGameLevel[0] = true;

                if(StageLevel.NeverCleared[0] == false)
                {
                    StageLevel.NeverCleared[0] = true;
                }

                deciedClearText.text = gamelevel + " STAGE CLEAR";
                break;

            case "Hard":
                StageLevel.ClearedGameLevel[1] = true;

                if (StageLevel.NeverCleared[1] == false)
                {
                    StageLevel.NeverCleared[1] = true;
                    //hard���[�h���N���A���Ă��ꂪ���߂ĂȂ�EndRoll�ɍs�����߂�bool�l��true��
                    ResultText.goEndRoll = true;
                }

                deciedClearText.text = gamelevel + " STAGE CLEAR";
                break;

            case "Unknown":
                StageLevel.ClearedGameLevel[2] = true;

                if (StageLevel.NeverCleared[2] == false)
                {
                    StageLevel.NeverCleared[2] = true;
                }

                deciedClearText.text = gamelevel + " STAGE CLEAR";
                break;
        }
    }
}
