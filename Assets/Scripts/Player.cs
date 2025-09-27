using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Hp playerHp;

    public bool posetrue = false;
    void Start()
    {
        playerHp = GetComponent<Hp>();
        if (playerHp == null)
        {
            Debug.LogError("Player GameObjectに Hp スクリプトがアタッチされていません！");
        }

    }

    // Update is called once per frame
    void Update()
    {
        PoseJudge();
    }

    public void OnClear()
    {
        Debug.Log("ゲームクリア");
        SceneManager.LoadScene("FlagClear");
    }

    public void OnDead()
    {
        Debug.Log("あなたは死んだ");
        SceneManager.LoadScene("FlagOver");
    }

    public void OnDamage()
    {
        Debug.Log("ダメージを受けた");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("panel") && !posetrue)
        {
            if (playerHp != null)
            {
                playerHp.Damage(1);
                Debug.Log("一ダメージ");
            }
        }
    }

    public void PoseJudge()
    {
        posetrue = Input.GetKey(KeyCode.F);
    }

}
