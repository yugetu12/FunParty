using UnityEngine;

public class FallingObjectManager : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public float fallSpeed = 10f;           // 重力加速度

    void Start()
    {

    }

    void FixedUpdate()
    {
        // 下方向に力を加え続ける
        rb.AddForce(Vector3.down * fallSpeed, ForceMode.Acceleration);
    }
}
