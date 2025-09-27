using UnityEngine;
using System.Collections;

public class TrolleyManager : MonoBehaviour
{
    [SerializeField] private GameObject trolleyPrefab; // トロッコの元
    [SerializeField] private float destroyX = 10f;     // 消す位置
    private GameObject spawnedTrolley;                 // 出てるトロッコ
    private Rigidbody rb;

    void Start()
    {
        SpawnTrolley();
    }

    void Update()
    {
         if (spawnedTrolley != null && spawnedTrolley.transform.position.x > destroyX)
        {
            Destroy(spawnedTrolley);
            SpawnTrolley();
        }
    }

    void SpawnTrolley()
    {
        spawnedTrolley = Instantiate(trolleyPrefab,new Vector3(-42.82f, -17.6f, -57.01f),Quaternion.Euler(-90f, 0f, 0f) );
        rb = spawnedTrolley.GetComponent<Rigidbody>();
        StartCoroutine(TrolleyMove());
    }

    IEnumerator TrolleyMove()
    {
        if (spawnedTrolley != null)
        {
            rb.AddForce(Vector3.right * 5f, ForceMode.Impulse);
            yield return new WaitForSeconds(7f);
        }
    }
}
