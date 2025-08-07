using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;   //Rigidbodyを宣言
    [SerializeField] private float jumpForce = 5f;  //ジャンプ力
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
