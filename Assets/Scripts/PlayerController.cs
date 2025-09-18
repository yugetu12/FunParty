using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;          //Rigidbody
    [SerializeField] private float moveSpeed = 5f;  //移動速度
    [SerializeField] private bool isAutoMove;       //自動移動の切り替え
    private bool isEnableControll;                  //操作可能判定
    private float moveX;                            //X方向の移動量
    private float moveZ;                            //Z方向の移動量
    private Vector3 moveVector;                     //移動ベクトル
    private Vector3 targetPos;                      //目的地
    private float targetPosX = 0;
    private float targetPosZ = 0;

    void Start()
    {
        //移動不可
        isEnableControll = false;
    }

    void Update()
    {
        //カメラ情報判定(未完成)
        if (isAutoMove)
        {
            //距離を計測
            float distance = Vector3.Distance(transform.position, targetPos);
            if (distance < 0.1f)
            {
                //目的地更新
                targetPosX = Random.Range(-3.5f, 3.5f);//<-ここの値をカメラ情報で置き換え
                targetPosZ = Random.Range(-3.5f, 3.5f);//<-ここの値をカメラ情報で置き換え
            }
            //位置ベクトルの作成
            targetPos = new Vector3(targetPosX, transform.position.y, targetPosZ);
            Debug.Log(targetPos);
        }
        //WASD入力判定
        else
        {
            //移動ベクトルのx成分とz成分を取得
            moveX = Input.GetAxis("Horizontal");
            moveZ = Input.GetAxis("Vertical");
            //移動ベクトルを作成
            moveVector = new Vector3(moveX * moveSpeed, rb.linearVelocity.y, moveZ * moveSpeed);
        }
    }

    void FixedUpdate()
    {
        if (!isEnableControll) return;

        //移動処理
        if (isAutoMove)
        {
            //移動
            rb.MovePosition(Vector3.MoveTowards(rb.position, targetPos, moveSpeed * Time.fixedDeltaTime));
            //回転
            transform.LookAt(targetPos);
        }
        else
        {
            //移動
            rb.linearVelocity = moveVector;
            //回転
            if (Mathf.Abs(moveVector.magnitude) < 0.1) return;
            transform.rotation = Quaternion.LookRotation(moveVector);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //衝突オブジェクトのタグがGroundかを判定
        if (collision.gameObject.CompareTag("Ground"))
        {
            isEnableControll = true;
        }
    }
}
