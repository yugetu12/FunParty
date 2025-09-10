using UnityEngine;

public class Balldamage : MonoBehaviour
{
    public int damage = 1;
    public GameObject explosionEffect;
    void OnCollisionEnter(Collision collision)
    {
        //プレイヤー
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth hP = collision.gameObject.GetComponent<PlayerHealth>();
            if (hP != null)
            {
                hP.TakeDamage(damage);
            }

            TriggerExplosion();
            Destroy(gameObject);
        }
        //足場
        else if (collision.gameObject.CompareTag("Ground"))
        {
            TriggerExplosion();
            Destroy(gameObject);
        }
    }

    void TriggerExplosion()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
    }           
}
