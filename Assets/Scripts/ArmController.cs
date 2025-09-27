using UnityEngine;

public class ArmController : MonoBehaviour
{
    public float armSpeed = 1.0f;
    private Rigidbody rb;

    public float destroyZ;

    // ★ spawnwait は不要なので削除

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearVelocity = new Vector3(0, 0, armSpeed);
    }
    
    void Update()
    {
        // 画面外に出たときの処理
        if (transform.position.z > destroyZ)
        {
            HandleDestruction();
        }
    }
    
    // 破壊処理をまとめたメソッド
    void HandleDestruction()
    {
        // 1. monitor タグのオブジェクトを破壊
        DestroyGameObjectsWithTag();

        // 2. スポナーに敵が破壊されたことを通知し、カウンターを減らす
        if (ArmSpawn.Instance != null)
        {
            ArmSpawn.Instance.ArmDestroyed();
        }

        // 3. 敵自身を破壊 (これで spawn.cs の WaitUntil が解除される)
        Destroy(gameObject);
    }

    // "monitor"タグのオブジェクトを破壊する処理
    void DestroyGameObjectsWithTag()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("monitor");
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }
}