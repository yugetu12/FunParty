using UnityEngine;

public class ArmController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;              //Rigidbody
    [SerializeField] private float armSpeed = 1.0f;     //速度
    [SerializeField] private Transform arm;             //アームの先端
    public GameObject panel;                            //パネル
    private Vector3 originPos;
    private Quaternion originRot;
    private Vector3 originScale;

    void Start()
    {
        //元の位置と回転を保存
        originPos = panel.transform.localPosition;
        originRot = panel.transform.localRotation;
        originScale = panel.transform.localScale;
        //速度を与える
        rb.linearVelocity = new Vector3(0, 0, armSpeed);
    }

    public void ChangePanel(GameObject panelObj)
    {
        //既存のアイテムを削除
        if (panel != null)
        {
            Destroy(panel);
        }
        //新しいアイテムをセット
        GameObject newPanel = Instantiate(panelObj, arm);
        newPanel.transform.localPosition = originPos;
        newPanel.transform.localRotation = originRot;
        newPanel.transform.localScale = originScale;
        panel = newPanel;
    }
}