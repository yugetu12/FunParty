using UnityEngine;

public class SendPlayerPosition : MonoBehaviour
{
    [SerializeField] private UDPReceiver receiver;  //UDP受信スクリプト
    [SerializeField] private float playerPosRange = 5.0f; // プレイヤー位置の範囲
    public TrackingData latestData;
    public int[] id;
    public float[] x;
    public float[] y;

    void Update()
    {
        // 位置情報を送信する処理をここに追加
        latestData = receiver.latestData;
        if (latestData == null || latestData.persons.Length == 0) return;
        x = new float[latestData.persons.Length];
        y = new float[latestData.persons.Length];
        id = new int[latestData.persons.Length];
        for (int i = 0; i < latestData.persons.Length; i++)
        {
            id[i] = latestData.persons[i].id;
            x[i] = (latestData.persons[i].x - 0.5f) * playerPosRange * 2;
            y[i] = (latestData.persons[i].y - 0.5f) * playerPosRange * -2.7f + 1.5f;
        }
    }
}
