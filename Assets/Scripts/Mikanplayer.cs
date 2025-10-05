using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mikanplayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;  // 移動速度
    private Rigidbody rb;
    private Vector3 moveVector;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // 倒れ防止
    }

    void Update()
    {
        // WASD入力
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // 移動ベクトル作成（yは保持）
        moveVector = new Vector3(moveX * moveSpeed, rb.linearVelocity.y, moveZ * moveSpeed);
    }

    void FixedUpdate()
    {
        // 移動処理
        rb.linearVelocity = moveVector;

        // 移動中なら向きを変える
        Vector3 flatMove = new Vector3(moveVector.x, 0, moveVector.z);
        if (flatMove.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(flatMove);
        }
    }
}
