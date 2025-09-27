using UnityEngine;
using System.Collections;

public class ArmSpawn : MonoBehaviour
{
    // どこからでもこのスクリプトを参照できるようにする（シングルトンパターン）
    public static ArmSpawn Instance;

    public GameObject panel1;
    public GameObject panel2;
    public GameObject monitor1;
    public GameObject monitor2;

    private Player clear;


    [Header("設定")]
    public int SpawnMany = 5;
    private int ArmRest;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        clear = FindObjectOfType<Player>();//playerスクリプトの取得
        if (clear == null)
        {
             Debug.LogError("FATAL: シーンに Player スクリプトが見つかりません！クリア処理は実行できません。");
        }
        ArmRest = SpawnMany;
        StartCoroutine(SpawnRoutine());
    }

    //敵が破壊されたときに、外部から呼ばれるメソッド
    public void ArmDestroyed()
    {
        ArmRest--;
        Debug.Log("アームが破壊されました。残り: " + ArmRest);

        // カウンターが0以下になったらゲームクリア
        if (ArmRest <= 0)
        {
            if (clear != null)
            {
                // Player.cs にある OnClear() 関数を実行
                clear.OnClear();
            }
            else
            {
                Debug.Log("ゲームクリアの処理を実行しましたが、プレイヤーが見つかりませんでした。");
            }
        }
    }

    IEnumerator SpawnRoutine()
    {
        for (int i = 0; i < SpawnMany; i++)
        {
            int direction = Random.Range(0, 2);
            GameObject spawnedArm; // スポーンした arm を保持する変数
            if (direction == 0)
            {
                spawnedArm = Instantiate(panel1, new Vector3(5.483705f, -9.5f, -105.796f), Quaternion.Euler(-90f, 0f, 0f));
                Instantiate(monitor1, new Vector3(16.37f, 0f, -43.76f), Quaternion.Euler(-88.165f, 0f, -13.026f));
                Instantiate(monitor1, new Vector3(-21.92f, 0.01f, -41.66f), Quaternion.Euler(-88.165f, 0f, 14.664f));
                Debug.Log("panel1 " + (i + 1));
            }
            else
            {
                spawnedArm = Instantiate(panel2, new Vector3(5.483705f, -9.5f, -105.796f), Quaternion.Euler(-90f, 0f, 0f));
                Instantiate(monitor2, new Vector3(15.8f, 0f, -44.53f), Quaternion.Euler(0f, -15.838f, 0f));
                Instantiate(monitor2, new Vector3(-21.72f, 0.01f, -41.01f), Quaternion.Euler(0f, 14.222f, 0f));
                Debug.Log("panel2 " + (i + 1));
            }

            yield return new WaitUntil(() => spawnedArm == null);
        }

        Debug.Log("全てののパネルスポーンが完了しました。");
    }   
}