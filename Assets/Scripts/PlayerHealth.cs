using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int hp = 3;

    public Image[] hpIcons;
    public Sprite robotFace;
    public Sprite robotCrossed;

    public void TakeDamage(int amount)
    {
        hp -= amount;
        hp = Mathf.Clamp(hp, 0, 3);

        UpdateHPUI();

        if (hp <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

    void UpdateHPUI()
    {
        for (int i = 0; i < hpIcons.Length; i++)
        {
            if (i < hp)
            {
                hpIcons[i].sprite = robotFace;
            }
            else
            {
                hpIcons[i].sprite = robotCrossed;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallingObject"))
        {
            TakeDamage(1);
        }
    }
}