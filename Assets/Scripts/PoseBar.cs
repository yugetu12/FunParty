using UnityEngine;

public class PoseBar : MonoBehaviour
{
    public Transform poseBar;//ローディングバー
    public Vector3 pos;//初期のバーを保存する場所
    public DanceGameStay currentBar;//DanceGamestayにあるholdTime(今どれだけTポーズをしたかの秒数)いれる変数
    public DanceGameStay maxBar;//DanceGamestayにあるrequiredTime(画面遷移に必要な秒数)をいれる変数
    public float BarRatio;

    void Start()
    {
        pos = poseBar.localScale;//バーの初期スケールを代入
    }
    void Update()
    {  
        BarRatio = currentBar.holdTime / maxBar.requiredTime;
        // X方向にスケールを変化
        poseBar.localScale = new Vector3(BarRatio * pos.x, pos.y, pos.z);
    }
}
