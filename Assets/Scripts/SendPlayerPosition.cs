using UnityEngine;

public class SendPlayerPosition : MonoBehaviour
{
    [SerializeField] private UDPReceiver receiver;  //UDP受信スクリプト
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
            x[i] = latestData.persons[i].x * 1;
            y[i] = latestData.persons[i].y * 1;
        }
    }
}
