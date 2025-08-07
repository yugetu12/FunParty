using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;                           //Rigidbodyを宣言
    [SerializeField] private float jumpForce = 5f;  //ジャンプ力

    void Start()
    {
        // Rigidbodyコンポーネントを取得
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //スペースキー入力をチェック
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump(); //ジャンプ
        }
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
}
