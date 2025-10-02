using UnityEngine;

public class DanceGameManager : MonoBehaviour
{
    [SerializeField] private GameObject arms;       //アーム
    [SerializeField] private GameObject monitor;    //モニター
    [SerializeField] private GameObject players;    //プレイヤー
    [SerializeField] private float limitZ = 0;      //制限z座標
    [SerializeField] private GameObject[] panelPrefabs;
    [SerializeField] private Texture2D[] monitorTexture;
    private Vector3 originPos;

    void Start()
    {
        //初期位置を保存
        originPos = arms.transform.position;
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
        int index = Random.Range(0, panelPrefabs.Length);
        arms.GetComponent<ArmController>().ChangePanel(panelPrefabs[index]);

        //正誤判定
        PlayerManager player = players.GetComponent<PlayerManager>();
        if (player.posetrue)
        {
            //判定が正なら実行する
            Debug.Log("Success");
        }
    }
}
