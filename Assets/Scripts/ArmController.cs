using UnityEngine;

public class ArmController : MonoBehaviour
{
    public float armSpeed = 1.0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearVelocity = new Vector3(0, 0, armSpeed);
    }
}
