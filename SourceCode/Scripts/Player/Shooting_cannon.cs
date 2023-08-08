using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//playerの射撃を行う
public class Shooting_cannon : MonoBehaviour
{
    //砲身と砲身の角度(ベクトル)を算出するために設定したオブジェクト,及びそれらから算出したベクトル
    [SerializeField]
    private GameObject gunbarrel;
    [SerializeField]
    private GameObject bombManager;
    [SerializeField]
    private GameObject gunbarrelStandard;
    private Vector3 _standardAngle;
    private Vector3 _shootingAngle;
    private Vector3 _bulletPos;

    //gunbarrelのローカルx軸の傾き
    private float _barrelAnglex;
    //初期状態のgunbarrelの傾き
    private Vector3 _barrelAngleDefault;

    private float _input_Rotx;

    //生成するBulletオブジェクトとそのプレハブ
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

    //リロード済みか
    private bool _reload;
    //リロード中か
    public static bool _reloading;

    //各Bulletを示すアイコンとそれを表示するパネル,及びそのimageコンポーネント
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

    //reloadが必要なことを示す画像
    [SerializeField]
    private Image reloadMessageImg;
    //オブジェクトに近いときに離れるよう促すテキスト
    [SerializeField]
    private TextMeshProUGUI leaveMessage;

    //射撃態勢の時にplayerを回転させる入力量
    private float _inputX;
    //y軸回転のスピード
    [SerializeField]
    private float _horizontalSpeed;
    //x軸回転のスピード
    [SerializeField]
    private float _verticalSpeed;

    //playerのy軸のローカルな回転とその最大最小値
    private float _currentAngle;
    private float _minAngle;
    private float _maxAngle;
    private float _angle;

    //弾を発射する際のサウンドとそのAudioSource(BombManagerにアタッチされている)
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

        //AudioSourceコンポーネントの取得と再生するサウンドの設定
        audioSource = bombManager.GetComponent<AudioSource>();
        audioSource.clip = shootingSound;
    }


    //射撃システム
    IEnumerator Shooting()
    {
        for (; ; )
        {

            //射撃可能(_reload == true)であれば射撃
            if (Input.GetMouseButtonDown(0) && _reload == true)
            {
                if (DistinguishOtherObjInRange._canShoot == true)
                {
                    //リロード状態を解除する
                    _reload = false;

                    StopCoroutine("Reloading");

                    //Bulletのアイコンを薄くする
                    panel.GetComponent<Image>().color = new Color(1, 1, 1, 0.70f);
                    //リロードを促すメッセージを表示する
                    reloadMessageImg.enabled = true;

                    //弾発射
                    GenerateBullet();

                    //射撃音の再生
                    audioSource.Play();

                    yield return new WaitForSeconds(1.0f);

                    StartCoroutine("Reloading");
                }

                else if (DistinguishOtherObjInRange._canShoot == false)
                {
                    //オブジェクトから離れるよう促すメッセージを表示
                    LeaveMessage();
                    yield return new WaitForSeconds(1.0f);
                }
               
            }

            //射撃可能でなければリロードが必要
            else if (Input.GetMouseButtonDown(0) && _reload == false)
            {
                StartCoroutine("flashingReloadImg");

                yield return new WaitForSeconds(1.5f);
            }
            yield return null;
        }

    }


    //リロードシステム
    IEnumerator Reloading()
    {
        for (; ; )
        {
            if (Input.GetKeyDown("r") && _reload == false)
            {
                //リロード中にする
                _reloading = true;

                StopCoroutine("Shooting");
                StartCoroutine("ColorAlphaChange");
                
                yield return new WaitForSeconds(1.5f);

                //リロード完了状態にする
                _reload = true;

                //リロードを促すImageを非表示にする
                reloadMessageImg.enabled = false;

                //リロード中を解除する
                _reloading = false;

                yield return new WaitForSeconds(1.0f);

                StartCoroutine("Shooting");
            }

            yield return new WaitForSeconds(0.01f);
        }

    }


    //発射する弾の生成
    private void GenerateBullet()
    {
        //bombManagerは砲身先、gunbarrelStandardはオブジェクト中心
        //これらの座標から弾発射時のベクトルを算出
        _bulletPos = bombManager.transform.position;
        _standardAngle = gunbarrelStandard.transform.position;
        _shootingAngle = _bulletPos - _standardAngle;
        _shootingAngle = _shootingAngle.normalized;

        //生成時に_addForcePowerの力を加える
        bullet = Instantiate(bulletPrefab, _bulletPos, Quaternion.identity);
        bullet_rb = bullet.GetComponent<Rigidbody>();
        bullet_rb.AddForce(_shootingAngle * 40.0f, ForceMode.Impulse);
    }


    //playerオブジェクトの砲身の縦方向の角度調整
    private void RotateBarrel()
    {
        //w,sキーの入力
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



    //弾の切り替え
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


    //0.01秒置きに弾の切り替えと射撃角度の変更
    IEnumerator Shooting_Coroutine()
    {
        for (; ; )
        {
            Bullet();
            RotateBarrel();
            yield return null;
        }
    }


    //リロード時にその時の弾の画像が徐々に明るくなる(非装填時は暗くなっている)
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


    //射撃時に"他オブジェクトに近い"か"enemyオブジェクトに近い"ときにその旨を表示
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


    //離れたらその警告を非表示に
    private void LeaveMessageDelete()
    {
        leaveMessage.color = new Color(0, 0, 0, 0);
    }



    //射撃モードの時に左右キーでplayerを左右に回転
    IEnumerator PlayerRotate()
    {
        _currentAngle = this.transform.rotation.eulerAngles.y;
        _minAngle = _currentAngle - 15;
        _maxAngle = _currentAngle + 15;

        for (; ; )
        {
            //a,dキーの入力
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


    //reloadが必要な時に射撃ボタンを押した場合に"RELOAD"の画像が点滅
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


    //ModeChangeスクリプトによってこのスクリプトが非アクティブになったらgunbarrelの回転をデフォルトに戻す
    private void OnDisable()
    {
        gunbarrel.transform.localEulerAngles = _barrelAngleDefault;
    }

    //ModeChangeスクリプトによってこのスクリプトがアクティブになったらgunbarrelの回転をデフォルトに戻す
    private void OnEnable()
    {
        _barrelAnglex = _barrelAngleDefault.x;
    }

}
