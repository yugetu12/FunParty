using UnityEngine;

public class MikanSit : MonoBehaviour
{
    public bool canMove = false;
    private Rigidbody rb;                           //Rigidbodyを宣言
    [SerializeField] private float jumpForce = 5f; //ジャンプ力
    [SerializeField] private float moveSpeed = 5f; //横移動の速さ
    private Vector3 originalScale;                  //元の大きさを記録
    private bool isCrouching = false;               //しゃがみ状態かどうか

    

    void Start()
    {
        // Rigidbodyコンポーネントを取得
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale;       // 元の大きさを保存
    }

    void Update()
    {
        if (!canMove) return;

        //Y座標が-10より下のとき
        if (transform.position.y < -10f)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().RestartGame();
        }
        //スペースキー入力をチェック
            if (Input.GetKeyDown(KeyCode.Space) && !isCrouching) //しゃがみ中ジャンプできない
            {
                Jump(); //ジャンプ
            }

        //Enterキー入力をチェック
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ToggleCrouch(); //しゃがみ切り替え
        }
        Move(); //横移動の処理
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal"); //　A=-1,D=1,無入力=0
        float moveZ = Input.GetAxis("Vertical"); //w=1,s=-1
        rb.linearVelocity  = new Vector3(moveX * moveSpeed, rb.linearVelocity.y, moveZ *moveSpeed);
    }
    private void Jump()
    {
        //y軸の速度が小さいとき
        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            //上向きに力を加える
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void ToggleCrouch()
    {
        isCrouching = !isCrouching;

        if (isCrouching)
        {
            //高さを半分にする
            transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);
        }
        else
        {
            //元の高さに戻す
            transform.localScale = originalScale;
        }
    }
}
