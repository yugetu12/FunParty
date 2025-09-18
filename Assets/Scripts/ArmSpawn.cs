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
                
                Instantiate(arm, new Vector3(-9.0f, 0f, 95f), Quaternion.Euler(90, 0, 0));
                Instantiate(panel1, new Vector3(-9.5f, -2.0f, 95f), Quaternion.Euler(90, -180, 0));
            }
            else
            {
                
                Instantiate(arm, new Vector3(-9.0f, 0f, 95f), Quaternion.Euler(90, 0, 0));
                Instantiate(panel2, new Vector3(-9.5f, -2.0f, 95f), Quaternion.Euler(90, -180, 0));
            }
            yield return new WaitForSeconds(SpawnSpan);
        }
    }
}
