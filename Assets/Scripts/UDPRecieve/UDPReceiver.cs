using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class PersonData
{
    public int id;
    public float x;
    public float y;
    public int foot_x_pixel;
    public int foot_y_pixel;
    public float confidence;
}

[System.Serializable]
public class TrackingData
{
    public double timestamp;
    public int frame_width;
    public int frame_height;
    public PersonData[] persons;
}

public class UDPReceiver : MonoBehaviour
{
    [Header("UDP Settings")]
    public int port = 12345;
    
    [Header("Debug")]
    public bool showDebugLog = true;
    
    private UdpClient udpClient;
    private Thread receiveThread;
    private bool isReceiving = false;
    
    // 最新の人物データ
    public TrackingData latestData;
    
    // イベント
    public System.Action<TrackingData> OnDataReceived;

    void Start()
    {
        StartReceiving();
    }

    void StartReceiving()
    {
        try
        {
            udpClient = new UdpClient(port);
            isReceiving = true;
            
            receiveThread = new Thread(ReceiveData);
            receiveThread.IsBackground = true;
            receiveThread.Start();
            
            if (showDebugLog)
                Debug.Log($"UDP受信開始: ポート {port}");
        }
        catch (Exception e)
        {
            Debug.LogError($"UDP受信開始エラー: {e.Message}");
        }
    }

    void ReceiveData()
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        
        while (isReceiving)
        {
            try
            {
                byte[] data = udpClient.Receive(ref remoteEndPoint);
                string jsonString = Encoding.UTF8.GetString(data);
                
                // メインスレッドで処理するためにキューに追加
                lock (this)
                {
                    latestData = JsonConvert.DeserializeObject<TrackingData>(jsonString);
                }
                
                if (showDebugLog)
                {
                    Debug.Log($"受信データ: {latestData.persons.Length}人検出");
                }
            }
            catch (Exception e)
            {
                if (isReceiving)
                    Debug.LogError($"UDP受信エラー: {e.Message}");
            }
        }
    }

    void Update()
    {
        // 最新データがある場合はイベントを発火
        if (latestData != null)
        {
            OnDataReceived?.Invoke(latestData);
            
            // 人物データの処理例
            foreach (var person in latestData.persons)
            {
                // Unity座標系に変換（必要に応じて調整）
                Vector2 unityPosition = new Vector2(person.x, 1.0f - person.y); // Y軸反転
                
                if (showDebugLog)
                {
                    Debug.Log($"人物ID:{person.id} 座標:({unityPosition.x:F3}, {unityPosition.y:F3}) 信頼度:{person.confidence:F2}");
                }
                
                // ここに人物の座標に基づく処理を追加
                // 例: GameObject の移動、UI の更新など
            }
            
            latestData = null; // 処理済みフラグ
        }
    }

    void OnApplicationQuit()
    {
        StopReceiving();
    }

    void OnDestroy()
    {
        StopReceiving();
    }

    void StopReceiving()
    {
        isReceiving = false;
        
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Abort();
        }
        
        if (udpClient != null)
        {
            udpClient.Close();
        }
        
        if (showDebugLog)
            Debug.Log("UDP受信停止");
    }
}
