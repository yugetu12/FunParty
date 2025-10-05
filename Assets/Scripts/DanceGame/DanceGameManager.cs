using UnityEngine;

public class DanceGameManager : MonoBehaviour
{
    [SerializeField] private ChangeScene scene;     //Scene管理スクリプト
    [SerializeField] private PlayerHealth hp;       //HP管理スクリプト
    [SerializeField] private PlayerManager player;  //プレイヤー管理スクリプト
    [SerializeField] private SuccessCount count;    //成功回数計測スクリプト
    [SerializeField] private GameObject arms;       //アーム
    [SerializeField] private GameObject monitor;    //モニター
    [SerializeField] private GameObject poseSign;   //ポーズサイン
    [SerializeField] private GameObject goalSign;   //ゴールサイン
    [SerializeField] private float limitZ = 0;      //制限z座標
    [SerializeField] private Texture2D[] poseTexture;
    [SerializeField] private GameObject[] colorLights;
    private bool isPoseTrue;
    private Vector3 originPos;

    void Start()
    {
        //初期位置を保存
        originPos = arms.transform.position;
        //初期画像を与える
        arms.GetComponent<ShowTexture2D>().ChangeTexture(poseTexture[0]);
        monitor.GetComponent<ShowTexture2D>().ChangeTexture(poseTexture[0]);
    }

    void Update()
    {
        //正かを判定する
        if (player.posetrue && !isPoseTrue)
        {
            //緑の光
            poseSign.GetComponent<SignLightManager>().ChangeLight(colorLights[1]);
            isPoseTrue = true;
        }
        else if (!player.posetrue && isPoseTrue)
        {
            //赤の光
            poseSign.GetComponent<SignLightManager>().ChangeLight(colorLights[0]);
            isPoseTrue = false;
        }

        // 画面外に出たときの処理
        if (arms.transform.position.z > limitZ) Judge();
    }

    private void Judge()
    {
        //位置を戻す
        arms.transform.position = originPos;
        //パネル変更
        int index = Random.Range(0, poseTexture.Length);
        arms.GetComponent<ShowTexture2D>().ChangeTexture(poseTexture[index]);
        monitor.GetComponent<ShowTexture2D>().ChangeTexture(poseTexture[index]);

        //正誤判定
        if (player.posetrue)
        {
            //増加処理
            count.CountPlus();
            //ゲームクリア処理
            if (count.count >= count.signs.Length) scene.LoadScene("GameClearScene");
        }
        else if (!player.posetrue)
        {
            //ダメージ
            hp.TakeDamage(1);
            //ゲームオーバー処理
            if (hp.hp <= 0) scene.LoadScene("GameOverScene");
        }
    }
}
