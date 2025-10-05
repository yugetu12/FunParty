using UnityEngine;

public class DanceGameManager : MonoBehaviour
{
    [SerializeField] private GameObject arms;       //アーム
    [SerializeField] private GameObject monitor;    //モニター
    [SerializeField] private GameObject players;    //プレイヤー
    [SerializeField] private GameObject[] signs;    //サインライト
    [SerializeField] private float limitZ = 0;      //制限z座標
    [SerializeField] private Texture2D[] poseTexture;
    [SerializeField] private GameObject[] colorLights;
    private Vector3 originPos;
    private int countSuccess;

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
        PlayerManager player = players.GetComponent<PlayerManager>();
        if (player.posetrue)
        {
            //判定が正なら実行する
            Debug.Log("Success");
        }
    }
}
