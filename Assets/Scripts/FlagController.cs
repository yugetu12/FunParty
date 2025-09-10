using UnityEngine;

public class FlagController : MonoBehaviour
{
    [SerializeField] private string raiseKey = "f";
    [SerializeField] private float raiseSpeed = 60f;
    private bool raiseFlag;
    public float rotZ;

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
        rotZ = transform.localEulerAngles.z;
        if (rotZ > 180f) rotZ -= 360f;

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
