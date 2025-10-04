using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [HideInInspector] public int hp;    //HP
    public Image[] hpIcons;             //HP画像
    public Sprite robotFace;            //画像素材
    public Sprite robotCrossed;         //画像素材

    void Start()
    {
        //画像の枚数分だけHPを設定
        hp = hpIcons.Length;
    }

    public void TakeDamage(int amount)
    {
        //HPを減らす
        hp -= amount;
        //UI更新
        UpdateHPUI();
    }

    public void UpdateHPUI()
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
}