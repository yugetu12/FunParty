using UnityEngine;

public class ArmController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;              //Rigidbody
    [SerializeField] private float armSpeed = 1.0f;     //速度
    [HideInInspector] public GameObject panel;          //パネル

    void Start()
    {
        //速度を与える
        rb.linearVelocity = new Vector3(0, 0, armSpeed);
    }

    public void ChangePanel(GameObject panelObj)
    {
        //パネル変更
        panel = panelObj;
    }
}