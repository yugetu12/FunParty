using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TitleObjectController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;              //Rigidbody
    [SerializeField] private float moveSpeed = 10f;     //加える力の大きさ
    [SerializeField] private float originPosY = -5f;    //Y座標の初期位置
    private float targetPosY;                           //Y座標の上限

    void Start()
    {
        //Rigidbodyが設定されていなければ取得する
        if (rb == null) rb = GetComponent<Rigidbody>();

        //重力を無効にする
        rb.useGravity = false;
        //初期位置に移動する
        targetPosY = transform.position.y;
        transform.position = new Vector3(0, originPosY, 0);
        //上方向に力を加える
        rb.linearVelocity = Vector3.up * moveSpeed;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        //targetPosYを超えたら
        if (transform.position.y > targetPosY)
        {
            //静止状態にする
            rb.linearVelocity = Vector3.zero;
        }
    }
}
