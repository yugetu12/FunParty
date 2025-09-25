using UnityEngine;
using System.Collections;

public class ArmSpawn : MonoBehaviour
{
    // どこからでもこのスクリプトを参照できるようにする（シングルトンパターン）
    public static ArmSpawn Instance;

    public GameObject arm1;
    public GameObject arm2;
    public GameObject panel1;
    public GameObject panel2;

    private Player clear;


    [Header("設定")]
    public int SpawnMany = 5;

    // ★修正: 倒すべき残り敵数として使用
    private int EnemiesRemaining;

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
        // 倒すべき総敵数に設定
        clear = FindObjectOfType<Player>();
        if (clear == null)
        {
             Debug.LogError("FATAL: シーンに Player スクリプトが見つかりません！クリア処理は実行できません。");
        }
        EnemiesRemaining = SpawnMany;
        StartCoroutine(SpawnRoutine());
    }

    // ★追加: 敵が破壊されたときに、外部から呼ばれるメソッド
    public void EnemyDestroyed()
    {
        EnemiesRemaining--;
        Debug.Log("アームが破壊されました。残り: " + EnemiesRemaining);

        // カウンターが0以下になったらゲームクリア
        if (EnemiesRemaining <= 0)
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

            // 2. panel をスポーン
            if (direction == 0)
            {
                spawnedArm = Instantiate(arm1, new Vector3(-74.34321f, -1.57f, -56.9f), Quaternion.Euler(-90f, 0f, 0f));
                Instantiate(panel1, new Vector3(16.85f, -0.3f, -17.26f), Quaternion.Euler(0f, -12.39f, 0f));
                Instantiate(panel1, new Vector3(-15.37f, 0.01f, -17.73f), Quaternion.Euler(0f, 15.033f, 0f));
                Debug.Log("panel1 " + (i + 1));
            }
            else
            {
                spawnedArm = Instantiate(arm2, new Vector3(-74.34321f, -1.57f, -56.9f), Quaternion.Euler(-90f, 0f, 0f));
                Instantiate(panel2, new Vector3(16.85f, -0.3f, -17.26f), Quaternion.Euler(0f, -12.39f, 0f));
                Instantiate(panel2, new Vector3(-15.37f, 0.01f, -17.73f), Quaternion.Euler(0f, 15.033f, 0f));
                Debug.Log("panel2 " + (i + 1));
            }

            yield return new WaitUntil(() => spawnedArm == null);
        }

        Debug.Log("全てののパネルスポーンが完了しました。");
    }   
}