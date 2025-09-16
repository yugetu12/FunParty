using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;                           //Rigidbodyを宣言
    private Vector3 originScale;                    //元の大きさ
    [SerializeField] private float jumpForce = 5f;  //ジャンプ力
    [SerializeField] private float moveSpeed = 5f;  //移動速度
    [SerializeField] private float gravity = 5f;    //重力
    private float moveX;                            //X方向の移動量
    private float moveZ;                            //Z方向の移動量
    private bool isEnableControll;                  //操作可能判定
    private bool isJumping;                         //ジャンプ判定
    private bool isStooping;                        //しゃがみ判定

    void Start()
    {
        //元の大きさを保存
        originScale = transform.localScale;
        // Rigidbodyコンポーネントを取得
        rb = GetComponent<Rigidbody>();
        //重力設定
        Physics.gravity = new Vector3(0, gravity * -1f, 0);
    }

    void Update()
    {
        //Space入力判定
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }

        //WASD入力判定
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        //Shift入力判定
    }

    void FixedUpdate()
    {
        //操作制限
        if (Mathf.Abs(rb.linearVelocity.y) > 0.01f) isEnableControll = false;

        if (!isEnableControll) return;

        //ジャンプ処理
        if (isJumping)
        {
            isJumping = false;
            //y速度が静止状態ならジャンプ
            if (Mathf.Abs(rb.linearVelocity.y) < 0.01f) Jump();
        }

        //移動処理
        rb.linearVelocity = new Vector3(moveX * moveSpeed, rb.linearVelocity.y, moveZ * moveSpeed);

        //しゃがみ処理
    }

    private void Jump()
    {
        //上向きに力を加える
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void OnCollisionStay(Collision collision)
    {
        //地面に接触したら操作可能にする
        if (collision.gameObject.CompareTag("Ground") && !isEnableControll) isEnableControll = true;

        //ポールに接触したら操作不可能にする
        if (collision.gameObject.CompareTag("Pole") && isEnableControll) isEnableControll = false;
    }
}
