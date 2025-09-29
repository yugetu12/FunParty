using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatingObject : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float range;
    private Vector3 originPos;

    void Start()
    {
        //元の座標を保持
        originPos = transform.position;
        //上向きの速度を代入
        rb.linearVelocity = Vector3.up * speed;
    }

    void Update()
    {
        if (originPos.y + range < transform.position.y)
        {
            //下向きの速度を代入
            rb.linearVelocity = Vector3.down * speed;
        }
        else if (originPos.y - range > transform.position.y)
        {
            //上向きの速度を代入
            rb.linearVelocity = Vector3.up * speed;
        }
    }
}
