using System.Collections;
using UnityEngine;

public class PistonManager : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float limitY;
    private Vector3 originPos;

    void Start()
    {
        //位置の保存
        originPos = transform.position;

        //下向きに速度を与える
        rb.linearVelocity = new Vector3(0, speed, 0);
    }

    void Update()
    {

    }
}
