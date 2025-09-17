using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatingObjectController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;              //Rigidbody
    [SerializeField] private float moveSpeed = 10f;     //加える力の大きさ
    [SerializeField] private float floatHeight = 2f;    //浮遊する幅
    private Vector3 originPos;                          //初期位置

    void Start()
    {
        //Rigidbodyが設定されていなければ取得する
        if (rb == null) rb = GetComponent<Rigidbody>();

        //重力を無効にする
        rb.useGravity = false;
        originPos = transform.position;
        //上向きに力を加える
        rb.linearVelocity = Vector3.up * moveSpeed;
    }

    void FixedUpdate()
    {
        //上下移動
        if (transform.position.y > originPos.y + floatHeight)
        {
            //下向きに力を加える
            rb.linearVelocity = Vector3.down * moveSpeed;
        }
        else if (transform.position.y < originPos.y - floatHeight)
        {
            //上向きに力を加える
            rb.linearVelocity = Vector3.up * moveSpeed;
        }
    }
}
