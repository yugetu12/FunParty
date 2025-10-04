using UnityEngine;

public class TitleObject : MonoBehaviour
{
    [SerializeField] private float timeLimit;   //表示までの時間
    [SerializeField] private float startPosY;   //下限
    private Vector3 originPos;
    private Vector3 startPos;
    private Vector3 originScale;
    private float time;

    void Start()
    {
        //元の座標を保持
        originPos = transform.position;
        startPos = new Vector3(originPos.x, startPosY, originPos.z);
        //元の大きさを保持
        originScale = transform.localScale;
        transform.localScale = Vector3.zero;
        //開始座標に飛ぶ
        transform.position = startPos;
    }

    void Update()
    {
        //時間を計測
        time += Time.deltaTime;
        if (time > timeLimit) time = timeLimit;

        //位置を割合で計算して線形補完する
        float t = time / timeLimit;
        transform.position = Vector3.Lerp(startPos, originPos, t);
        transform.localScale = originScale * t;
    }
}
