using UnityEngine;

public class mikanblock : MonoBehaviour
{
    public float fallSpeed = 10f; // 好きな重力加速度
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Unity標準の重力を無効化
        rb.useGravity = false;
        //横移動をしないように
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        // 毎フレーム、下方向に力を加える（落ちる速度を自分で決める）
        rb.AddForce(Vector3.down * fallSpeed, ForceMode.Acceleration);
    }
}
