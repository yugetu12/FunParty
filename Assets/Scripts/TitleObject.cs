using UnityEngine;

public class TitleObject : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float startPosY;
    private Vector3 originPos;

    void Start()
    {
        //元の座標を保持
        originPos = transform.position;
        //y座標を下げる
        transform.position = new Vector3(originPos.x, startPosY, originPos.z);
        //上向きの速度を代入
        rb.linearVelocity = Vector3.up * speed;
    }

    void Update()
    {
        if (originPos.y < transform.position.y)
        {
            //正位置まで来たら止める
            rb.isKinematic = true;
        }
    }
}
