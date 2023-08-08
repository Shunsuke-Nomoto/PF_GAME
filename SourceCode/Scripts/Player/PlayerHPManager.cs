using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHPManager : MonoBehaviour
{
    //�ő�HP
    private int _MaxHp;
    //�_���[�W�v�Z���s���ۂ�BulletManager�̂��_���[�W�l
    private int _damage;
    //���݂�HP
    private int _currentHp;

    //HP�o�[
    [SerializeField]
    private Slider slider;
    //�Փ˂����I�u�W�F�N�g������BulletManager
    private BulletManager bulletManager;

    //TimeKeeper�I�u�W�F�N�g�Ƃ��ꂪ���Q�[���̃N���A���Ԃ��Ǘ�����X�N���v�g
    [SerializeField]
    private GameObject timeKeeper;
    private TimeManager timeManager;

    //�V�[���J�ڂ��s���X�N���v�g�����I�u�W�F�N�g
    [SerializeField]
    private GameObject changeSceneManager;

    //�Q�[�����N���A�܂��̓N���A�ł��Ȃ������Ƃ��ɂ��̎|��\������e�L�X�g
    [SerializeField]
    private TextMeshProUGUI deciedClearText;
    //�Q�[���I�[�o�[�̂Ƃ��\�����鍕�̔�����ʉB��
    [SerializeField]
    private GameObject gameOverBackground;
    //������Ԃ͔�A�N�e�B�u
    private Image gameOverBackgroundImg;

    //ModeChangeManager�Ƃ��ꂪ�A�^�b�`����Ă���I�u�W�F�N�g
    [SerializeField]
    private GameObject modeChangeManager;
    private ModeChange modeChange;


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

        gameOverBackgroundImg = gameOverBackground.GetComponent<Image>();

        timeManager = timeKeeper.GetComponent<TimeManager>();

        modeChange = modeChangeManager.GetComponent<ModeChange>();

        inGameBGMManager = BGMMnager.GetComponent<InGameBGMManager>();

        soundPlayManager = this.gameObject.GetComponent<SoundPlayManager>();
    }


    //HP(_MaxHp)�̐ݒ�
    private void HPDecision()
    {
        _MaxHp = 150;
    }

 
    //��e���HP�`�F�b�N
    private void HpCheck()
    { 
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

            modeChange.enabled = false;
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


    //�N���A�ł��Ȃ�������StageLevel�Ɋi�[����Ă��邻�̓�Փx�̃N���A�����false�ɂ���
    private void DecideGameClear()
    {
        var gamelevel = StageLevel._gameLevel;

        switch (gamelevel)
        {
            case "Normal":
                StageLevel.ClearedGameLevel[0] = false;
                gameOverBackgroundImg.enabled = true;
                deciedClearText.text = gamelevel + " STAGE FAILED";
                break;

            case "Hard":
                StageLevel.ClearedGameLevel[1] = false;
                gameOverBackgroundImg.enabled = true;
                deciedClearText.text = gamelevel + " STAGE FAILED";
                break;

            case "Unknown":
                StageLevel.ClearedGameLevel[2] = false;
                gameOverBackgroundImg.enabled = true;
                deciedClearText.text = gamelevel + " STAGE FAILED";
                break;
        }
    }
}
