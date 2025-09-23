using UnityEngine;
using System.Collections;

public class spawn : MonoBehaviour
{
    public GameObject arm;
    public GameObject spawnPoint;
    public GameObject panel1;
    public GameObject panel2;
    public float SpawnSpan;
    public int SpawnMany = 5;
    private int SpawnCount;

    void Start()
    {
        SpawnCount = SpawnMany; // スポーン数に合わせる
        StartCoroutine(Spawn());
    }

    void Update()
    {
        if (SpawnCount == 0)
        {
            GameClear();
        }
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < SpawnMany; i++)
        {
            Debug.Log("Spawn: " + i);

            int direction = Random.Range(0, 2);
            if (direction == 0)
            {
                Instantiate(arm, new Vector3(-74.62f, 0f, -46.49f), Quaternion.Euler(-90, 0, 0));
                Instantiate(panel1, spawnPoint.transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(arm, new Vector3(-74.62f, 0f, -46.49f), Quaternion.Euler(-90, 0, 0));
                Instantiate(panel2, spawnPoint.transform.position, Quaternion.identity);
            }

            SpawnCount--; // 毎回減らす
            yield return new WaitForSeconds(SpawnSpan); // 毎回待機
        }
    }

    void GameClear()
    {
        Debug.Log("ゲームクリア！");
    }
}
