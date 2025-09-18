using UnityEngine;

public class ArmController : MonoBehaviour
{
    public float armSpeed = 1.0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        Vector3 force = new Vector3(0.0f, 0.0f, -armSpeed);
        rb.AddForce(force, ForceMode.VelocityChange);
    }
}
