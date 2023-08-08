using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//playerのshootingモードとmovingモードを切り替える
public class ModeChange : MonoBehaviour
{
    //movingモードのスクリプト
    private Moving_cannon movingCannon;
    //shootingモードのスクリプト
    private Shooting_cannon shootingCannon;
    //カメラ追従のスクリプト
    private Camera_Manager cameraManager;
    //カメラをズーム状態にするスクリプト
    private CameraZoomManager cameraZoomManager;
    
    //プレイヤーのオブジェクト
    [SerializeField]
    private GameObject cannon;

    //プレイヤーの視点のカメラ
    [SerializeField]
    private GameObject mainCamera;

    //shootingモードの時のアイコンのsprite画像
    [SerializeField]
    private Sprite shootingIcon;
    //movingモードのアイコンのsprite画像
    [SerializeField]
    private Sprite movingIcon;

    //アイコンを表示するpanel
    [SerializeField]
    private GameObject panel;

    //現在のモードのアイコンを収納する変数
    private Sprite sprite;
    //panelの画像を変更する際のコンポーネント
    private Image img;


    //モード切り替え用のbool値
    public static bool _firingmode = false;


    private void Awake()
    {
        movingCannon = cannon.GetComponent<Moving_cannon>();
        shootingCannon = cannon.GetComponent<Shooting_cannon>();
        cameraManager = mainCamera.GetComponent<Camera_Manager>();
        cameraZoomManager = mainCamera.GetComponent<CameraZoomManager>();

        img = panel.GetComponent<Image>();
        //imgの初期状態はmovingIcon
        sprite = movingIcon;
        img.sprite = sprite;
    }


    private void Start()
    { 
        StopCannonScripts();

        //ゲーム開始2秒後に3秒のカウントダウン -> ゲームスタート
        Invoke("StartCannonCoroutine", 5.0f);
    }


    IEnumerator Mode()
    {
        for(; ; )
        {
            //移動モードでspaceキーが押されたら射撃モードに移行
            if (Input.GetKeyDown("space") && _firingmode == false)
            {
                //射撃モードになったらspriteをshootingIconに変更
                sprite = shootingIcon;
                img.sprite = sprite;

                _firingmode = true;

                movingCannon.enabled = false;
                shootingCannon.enabled = true;
                cameraManager.enabled = false;
                cameraZoomManager.enabled = true;

                //shootingモード中にアクティブになっているコルーチンをスタート
                shootingCannon.StartCoroutine("Shooting");
                shootingCannon.StartCoroutine("Reloading");
                shootingCannon.StartCoroutine("Shooting_Coroutine");
                shootingCannon.StartCoroutine("PlayerRotate");


                yield return new WaitForSeconds(1.0f);
            }

            //射撃モードでspaceキーが押されたら移動モードに移行
            else if (Input.GetKeyDown("space") && _firingmode == true)
            {
                //リロード中はモードチェンジしない
                if (Shooting_cannon._reloading == false)
                {

                    //移動モードになったらspriteをmovingIconに変更
                    sprite = movingIcon;
                    img.sprite = sprite;

                    _firingmode = false;

                    //shootingモード中にアクティブになっているコルーチンをストップ
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


    //ModeChangeスクリプトを起動
    //playerの初期状態はmovingモード
    private void StartCannonCoroutine()
    {
        StartCoroutine(Mode());
        movingCannon.enabled = true;
    }


    //プレイヤーについている操作可能なスクリプトを停止させる
    private void StopCannonScripts()
    {
        shootingCannon.enabled = false;
        movingCannon.enabled = false;
        cameraZoomManager.enabled = false;
    }

}
