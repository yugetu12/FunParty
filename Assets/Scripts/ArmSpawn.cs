using UnityEngine;
using System.Collections;
public class spawn : MonoBehaviour
{
    public GameObject arm;
    public GameObject panel1;
    public GameObject panel2;
    public bool GameState = true;
    public float SpawnSpan;
    public int SpawnMany = 5;
    private int SpawnCount;
    void Start()
    {
        StartCoroutine(Spawn());
        SpawnCount = SpawnMany;
    }

    void Update()
    {
        if (SpawnCount == 0)
        {
            GameClear();
        }
    }
    // Update is called once per frame

    IEnumerator Spawn()
    {
        for (int i = 0; i < SpawnMany; i++)  // ← 5回だけ実行
        {
            Debug.Log("Spawn: " + i);
            int direction = Random.Range(0, 2);
            if (direction == 0)
            {

                Instantiate(arm, new Vector3(-9.0f, 0f, 95f), Quaternion.Euler(90, 0, 0));
                Instantiate(panel1, new Vector3(-9.5f, -2.0f, 95f), Quaternion.Euler(90, -180, 0));
            }
            else
            {

                Instantiate(arm, new Vector3(-9.0f, 0f, 95f), Quaternion.Euler(90, 0, 0));
                Instantiate(panel2, new Vector3(-9.5f, -2.0f, 95f), Quaternion.Euler(90, -180, 0));
            }
            SpawnCount --;
            yield return new WaitForSeconds(SpawnSpan);
        }
        yield return new WaitForSeconds(SpawnSpan);
    }

    void GameClear()
    {
      Debug.Log("ゲームクリア！");
    }
    
}
