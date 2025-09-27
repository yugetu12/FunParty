using UnityEngine;
using UnityEngine.Events;

public class Hp : MonoBehaviour
{
    private Player human;
    public int hp_current;
    public UnityEvent<int> OnHpChange = new UnityEvent<int>();
    void Start()
    {
        human = GetComponent<Player>();
        OnHpChange.Invoke(hp_current);
    }
    void Update()
    {
    }

    // 外部から呼ばれてHPを減らすメソッド
    public void Damage(int _iDamageAmount)
    {
        hp_current -= Mathf.Abs(_iDamageAmount);
        OnHpChange.Invoke(hp_current);
        if (human != null)
        {
            if (hp_current <= 0)
            {
                human.OnDead();
            }
            else
            {
                human.OnDamage();
            }
        }

    }
}