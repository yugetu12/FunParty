using UnityEngine;

public class SignLightManager : MonoBehaviour
{
    public GameObject lightObj;            //光のオブジェクト
    private Transform originTransform;  //初期transform

    void Start()
    {
        //transformを保存
        originTransform = lightObj.transform;
    }

    public void ChangeLight(GameObject prefab)
    {
        //削除と生成
        Destroy(lightObj);
        GameObject newLightObj = Instantiate(prefab, transform);
        //保存していたtransformから元の状態を代入
        newLightObj.transform.position = originTransform.position;
        newLightObj.transform.rotation = originTransform.rotation;
        newLightObj.transform.localScale = originTransform.localScale;
        //新たなオブジェクトとして保存
        lightObj = newLightObj;
    }
}
