using System.Collections;
using UnityEngine;

public class PistonManager : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float limitZ;
    private Vector3 originPos;

    void Start()
    {
        //位置の保存
        originPos = transform.localPosition;

        //上向きに速度を与える
        rb.linearVelocity = Vector3.up * speed;
    }

    void Update()
    {
        if (transform.localPosition.z > limitZ)
        {
            //下向きに速度を与える
            rb.linearVelocity = Vector3.down * speed;
        }
        else if (transform.localPosition.z < originPos.z)
        {
            //上向きに速度を与える
            rb.linearVelocity = Vector3.up * speed;
        }
    }
}
