using UnityEngine;

public class SignLightManager : MonoBehaviour
{
    public GameObject lightObj;     //光のオブジェクト
    private Vector3 originPos;
    private Quaternion originRot;
    private Vector3 originScale;

    void Start()
    {
        //初期のtransform情報を保存
        originPos = lightObj.transform.position;
        originRot = lightObj.transform.rotation;
        originScale = lightObj.transform.localScale;
    }

    public void ChangeLight(GameObject prefab)
    {
        //削除と生成
        Destroy(lightObj);
        GameObject newLightObj = Instantiate(prefab, transform);
        //保存していたtransformから元の状態を代入
        newLightObj.transform.position = originPos;
        newLightObj.transform.rotation = originRot;
        newLightObj.transform.localScale = originScale;
        //新たなオブジェクトとして保存
        lightObj = newLightObj;
    }
}
