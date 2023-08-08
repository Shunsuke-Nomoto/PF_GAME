using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingSystem : MonoBehaviour
{
    //targetはplayer
    [SerializeField]
    private GameObject target;

    //playerの座標と向き
    private Vector3 targetPos;
    private Vector3 targetDirection;

    //弾を発射する際に与える運動量
    [SerializeField]
    private float _addForcePower;

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

    //生成する弾オブジェクトとそのプレハブ
    [SerializeField]
    private GameObject bulletPrefab;
    private GameObject bullet;
    private Rigidbody bullet_rb;

    //射撃できる状態か
    private bool _canShoot;

    //このオブジェクト(enemy)とtargetオブジェクトの距離
    private float _distance;
    //最大飛距離
    private float _maxDistance;
    //最大貴距離を算出するための弾のスピード
    private float _bulletSpeed;

    //gunbarrelのローカルの傾き
    private float _Angle;
    //gunbarrelの最大の傾きと最小の傾き
    private float _MinAngle = 0;
    private float _MaxAngle = 45;

    //次弾発射までの待機時間
    private float _shootingrate;

    //FreezeBulletの効果を受けるか(効果の重複はしない)
    public static bool _canFreeze;
    //enemyを凍らせた状態を示すアイコン
    [SerializeField]
    private GameObject frozenIcon;

    //弾を発射する際のサウンドとそのAudioSource(BombManagerにアタッチされている)
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

        //frozenIconの初期状態は非表示
        frozenIcon.SetActive(false);

        _bulletSpeed = _addForcePower / 1.5f;
        CalculateMaxRange();

        //AudioSourceコンポーネントの取得と再生するサウンドの設定
        audioSource = bombManager.GetComponent<AudioSource>();
        audioSource.clip = shootingSound;

        //ゲーム開始2秒後に3秒のカウントダウン->ゲームスタート
        Invoke("StartShootingCoroutine", 5.0f);
    }


    //最大射程の計算
    private void CalculateMaxRange()
    {
        _maxDistance = Mathf.Pow(_bulletSpeed, 2) / 9.81f;
    }


    //playerの方向を向く処理
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


    //発射する弾の生成
    private void GenerateBullet()
    {
        if (_canShoot == true)
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
            bullet_rb.AddForce(_shootingAngle * _addForcePower, ForceMode.Impulse);

            //射撃音の再生
            audioSource.Play();

            _canShoot = false;
        }
    }


    //弾道計算
    private void BallisticCalculation()
    {
        //playerとの距離
        _distance = Mathf.Abs(Mathf.Sqrt(Mathf.Pow((targetPos.x - transform.position.x), 2) + Mathf.Pow((targetPos.z - transform.position.z) , 2)));
        
        //playerが最大飛距離外であれば射撃は行わない
        if(_distance >= _maxDistance)
        {
            _canShoot = false;
            return;
        }

        _canShoot = true;

        //_distanceと弾速を用いた発射角度の計算
        _Angle = Mathf.Asin(_distance * 9.81f / Mathf.Pow(_bulletSpeed, 2));
        _Angle = _Angle * Mathf.Rad2Deg;
        _Angle = _Angle / 2;


        _Angle = Mathf.Min(_Angle, _MaxAngle);
        _Angle = Mathf.Max(_Angle, _MinAngle);

        //_Angleになるよう砲身を回転
        gunbarrel.transform.localRotation = Quaternion.Euler(90 - _Angle, 0, 0);
    }


    //弾道計算と弾の発射の一連の処理を一つにまとめて_shootingrateに則したテンポで射撃を行う
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


    //一連の射撃挙動を開始する
    private void StartShootingCoroutine()
    {
        StartCoroutine("FocusTargetPos");
        StartCoroutine("Shooting");
    }


    //enemyオブジェクトにFreezeBulletが当たった際,一連の射撃挙動を一定時間停止させる
    IEnumerator Freeze_Shooting()
    {
        //Freeze状態

        StopCoroutine("FocusTargetPos");
        StopCoroutine("Shooting");
        frozenIcon.SetActive(true);

        yield return new WaitForSeconds(3.0f);


        //Freeze状態の解除

        frozenIcon.SetActive(false);

        _canFreeze = true;

        StartShootingCoroutine();
    }
}
