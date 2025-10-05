using UnityEngine;

public class FallingObjectManager : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;  //Rigidbody
    public float fallSpeed = 10f;           // 重力加速度
    public int damage = 1;                 //ダメージ

    void FixedUpdate()
    {
        // 下方向に力を加え続ける
        rb.AddForce(Vector3.down * fallSpeed, ForceMode.Acceleration);
    }

    void OnTriggerEnter(Collider collision)
    {
        //プレイヤー
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth hP = collision.gameObject.GetComponent<PlayerHealth>();
            if (hP != null)
            {
                hP.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        //足場
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
