using UnityEngine;
using System.Collections;
public class spawn : MonoBehaviour
{
    public GameObject arm;
    public GameObject panel1;
    public GameObject panel2;
    public bool GameState = true;
    public float SpawnSpan;
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Spawn()
    {
        while (GameState)
        {
            int direction = Random.Range(0, 2);
            if (direction == 0)
            {
                // 例: 左側
                Instantiate(arm, new Vector3(0f, 0f, 0f), Quaternion.identity);
                Instantiate(panel1, new Vector3(0f, 0f, 0f), Quaternion.identity);
            }
            else
            {
                // 例: 右側
                Instantiate(arm, new Vector3(0f, 0f, 0f), Quaternion.identity);
                Instantiate(panel2, new Vector3(0f, 0f, 0f), Quaternion.identity);
            }
            yield return new WaitForSeconds(SpawnSpan);
        }
    }
}
