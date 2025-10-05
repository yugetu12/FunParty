using UnityEngine;

public class Balldamage : MonoBehaviour
{
    public int damage = 1;
    
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
