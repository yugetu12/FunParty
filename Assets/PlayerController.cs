using UnityEngine;

public class PlayerController : MonoBehaviour
{
<<<<<<< HEAD
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
=======
    private Rigidbody rb;   //Rigidbodyを宣言
    [SerializeField] private float jumpForce = 5f;  //ジャンプ力
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
>>>>>>> c8d726d (回るポールとジャンプするプレイヤーを追加しました。)
        }
    }

    private void Jump()
    {
<<<<<<< HEAD
        //y軸の速度が小さいとき
        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            //上向きに力を加える
=======
        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
>>>>>>> c8d726d (回るポールとジャンプするプレイヤーを追加しました。)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
