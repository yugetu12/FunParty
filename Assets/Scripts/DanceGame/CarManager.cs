using UnityEngine;

public class CarManager : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;          //Rigidbody
    [SerializeField] private float speed;           //速度
    [SerializeField] private float targetPosX;      //ターゲットx座標
    private Vector3 originPos;                      //元の位置

    void Start()
    {
        //位置を保存
        originPos = transform.position;

        //移動
        rb.linearVelocity = Vector3.left * speed;
    }

    void Update()
    {
        //反転させながら移動させる
        if (transform.position.x < targetPosX)
        {
            rb.linearVelocity = Vector3.right * speed;
        }
        else if (transform.position.x > originPos.x)
        {
            rb.linearVelocity = Vector3.left * speed;
        }
    }
}
