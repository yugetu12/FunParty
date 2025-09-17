using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;          //Rigidbodyを宣言
    [SerializeField] private float moveSpeed = 5f;  //移動速度
    private float moveX;                            //X方向の移動量
    private float moveZ;                            //Z方向の移動量
    private Vector3 moveVector;                     //移動ベクトル
    private bool isEnableControll;                  //操作可能判定

    void Start()
    {
        isEnableControll = true;
    }

    void Update()
    {
        //WASD入力判定
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        moveVector = new Vector3(moveX * moveSpeed, rb.linearVelocity.y, moveZ * moveSpeed);

        if (Mathf.Abs(moveX) > 0 || Mathf.Abs(moveZ) > 0)
        {
            //回転させる
            transform.rotation = Quaternion.LookRotation(moveVector);
        }
    }

    void FixedUpdate()
    {
        if (!isEnableControll) return;

        //移動処理
        rb.linearVelocity = moveVector;
    }
}
