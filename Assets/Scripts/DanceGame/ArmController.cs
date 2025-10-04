using UnityEngine;

public class ArmController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;              //Rigidbody
    [SerializeField] private float armSpeed = 1.0f;     //速度

    void Start()
    {
        rb.linearVelocity = new Vector3(0, 0, armSpeed);
    }
}