using UnityEngine;

public class Raise : MonoBehaviour
{
    public Transform leftFlag;
    public Transform rightFlag;

    public Vector3 upPositionR = new Vector3(0.8f, 0.5f, 0);
    public Vector3 downPositionR = new Vector3(1.0f, 0, 0);
    public Vector3 upRotationR = new Vector3(0, 0, 135);
    public Vector3 downRotationR = new Vector3(0, 0, 90);
    public Vector3 upPositionL = new Vector3(-0.8f, 0.5f, 0);
    public Vector3 downPositionL = new Vector3(-1.0f, 0, 0);
    public Vector3 upRotationL = new Vector3(0, 0, 45);
    public Vector3 downRotationL = new Vector3(0, 0, 90);
    public bool isRaisedR;
    public bool isRaisedL;


    void Start()
    {
        isRaisedR = false;
        isRaisedL = false;

    }

    // Update is called once per frame
    void Update()
    {
        // 右手操作
        if (Input.GetKeyDown(KeyCode.J))
        {
            isRaisedR = !isRaisedR;
            rightFlag.localPosition = isRaisedR ? upPositionR : downPositionR;
            rightFlag.localEulerAngles = isRaisedR ? upRotationR : downRotationR;
        }
        // 左手操作
        if (Input.GetKeyDown(KeyCode.F))
        {
            isRaisedL = !isRaisedL;
            leftFlag.localPosition = isRaisedL ? upPositionL : downPositionL;
            leftFlag.localEulerAngles = isRaisedL ? upRotationL : downRotationL;
        }

    }
}

 