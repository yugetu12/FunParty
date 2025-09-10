using UnityEngine;

public class FlagController : MonoBehaviour
{
    [SerializeField] private string raiseKey = "f";
    [SerializeField] private float raiseSpeed = 60f;
    private bool raiseFlag;

    void Start()
    {

    }

    void Update()
    {
        //指定キー入力判定
        if (Input.GetKeyDown(raiseKey))
        {
            raiseFlag = !raiseFlag;
        }
    }

    void FixedUpdate()
    {
        //角度調整
        float rotZ = transform.localEulerAngles.z;
        if (rotZ > 180f) rotZ -= 360f;
        Debug.Log(rotZ);

        //旗操作
        if (rotZ < 30f && raiseFlag)
        {
            transform.Rotate(0, 0, raiseSpeed * Time.fixedDeltaTime);
        }
        else if (rotZ > -30f && !raiseFlag)
        {
            transform.Rotate(0, 0, -raiseSpeed * Time.fixedDeltaTime);
        }
    }
}
