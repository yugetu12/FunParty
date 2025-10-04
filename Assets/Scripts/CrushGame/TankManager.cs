using UnityEngine;

public class TankManager : MonoBehaviour
{
    [SerializeField] private GameTimer timer;   //時間管理スクリプト
    [SerializeField] private GameObject liquid; //液体オブジェクト
    [SerializeField] private Transform top;     //上限
    private Vector3 topPos;
    [SerializeField] private Transform buttom;  //下限
    private Vector3 buttomPos;

    void Start()
    {
        //位置の保存
        topPos = top.transform.position;
        buttomPos = buttom.transform.position;
    }

    void Update()
    {
        //位置を割合で計算して線形補完する
        float t = 1 - (timer.currentTime / timer.timeLimit);
        liquid.transform.position = Vector3.Lerp(topPos, buttomPos, t);
    }
}
