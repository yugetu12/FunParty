using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Newtonsoft.Json;

public class PoseResultUDPReceiver : MonoBehaviour
{
    [Header("UDP通信設定")]
    public int udpPort = 5006;
    
    [Header("ポーズ判定結果")]
    public bool isPoseSuccess = false;
    public float angleError = 0f;
    public string lastUpdateTime = "";
    
    [Header("UI表示")]
    public UnityEngine.UI.Text statusText;
    public UnityEngine.UI.Text errorText;
    public UnityEngine.UI.Text connectionText;
    
    [Header("イベント")]
    public UnityEngine.Events.UnityEvent OnPoseSuccess;
    public UnityEngine.Events.UnityEvent OnPoseFail;
    
    [Header("デバッグ")]
    public bool showDebugGUI = true;
    
    private UdpClient udpClient;
    private Thread udpThread;
    private bool isReceiving = false;
    private bool hasRecentData = false;
    private float lastDataTime = 0f;
    
    [System.Serializable]
    public class PoseMessage
    {
        public string type;
        public bool success;
        public float angle_error;
        public double timestamp;
        public System.Collections.Generic.Dictionary<string, float> angles;
    }
    
    private readonly System.Collections.Generic.Queue<PoseMessage> messageQueue = 
        new System.Collections.Generic.Queue<PoseMessage>();
    
    void Start()
    {
        StartUDPListener();
    }
    
    void StartUDPListener()
    {
        try
        {
            udpClient = new UdpClient(udpPort);
            isReceiving = true;
            
            udpThread = new Thread(new ThreadStart(UDPListener));
            udpThread.IsBackground = true;
            udpThread.Start();
            
            Debug.Log($"UDP受信を開始しました。ポート: {udpPort}");
        }
        catch (Exception e)
        {
            Debug.LogError($"UDP受信開始エラー: {e.Message}");
        }
    }
    
    void UDPListener()
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, udpPort);
        
        while (isReceiving)
        {
            try
            {
                byte[] data = udpClient.Receive(ref remoteEndPoint);
                string message = Encoding.UTF8.GetString(data);
                
                ProcessMessage(message);
                hasRecentData = true;
                lastDataTime = Time.time;
            }
            catch (Exception e)
            {
                if (isReceiving)
                {
                    Debug.LogError($"UDP受信エラー: {e.Message}");
                }
            }
        }
    }
    
    void ProcessMessage(string jsonMessage)
    {
        try
        {
            PoseMessage poseMsg = JsonConvert.DeserializeObject<PoseMessage>(jsonMessage);
            
            if (poseMsg.type == "pose_result")
            {
                // メインスレッドで実行するためキューに追加
                lock (messageQueue)
                {
                    messageQueue.Enqueue(poseMsg);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"メッセージ処理エラー: {e.Message}");
            Debug.LogError($"受信メッセージ: {jsonMessage}");
        }
    }
    
    void Update()
    {
        // キューからメッセージを処理（メインスレッド）
        lock (messageQueue)
        {
            while (messageQueue.Count > 0)
            {
                PoseMessage msg = messageQueue.Dequeue();
                HandlePoseResult(msg);
            }
        }
        
        // 接続状態の更新（3秒以上データがない場合は接続なしと判定）
        if (Time.time - lastDataTime > 3.0f)
        {
            hasRecentData = false;
        }
        
        UpdateUI();
    }
    
    void HandlePoseResult(PoseMessage msg)
    {
        isPoseSuccess = msg.success;
        angleError = msg.angle_error;
        lastUpdateTime = DateTime.Now.ToString("HH:mm:ss.fff");
        
        Debug.Log($"ポーズ判定: {(msg.success ? "成功" : "失敗")} (誤差: {msg.angle_error:F1}度)");
        
        if (msg.success)
        {
            OnPoseSuccess?.Invoke();
            PlaySuccessEffect();
        }
        else
        {
            OnPoseFail?.Invoke();
        }
    }
    
    void PlaySuccessEffect()
    {
        // 成功エフェクトの例
        ParticleSystem particles = GetComponent<ParticleSystem>();
        if (particles != null)
        {
            particles.Play();
        }
        
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
        
        // 簡単な振動効果（モバイルの場合）
        #if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
        #endif
    }
    
    void UpdateUI()
    {
        if (statusText != null)
        {
            statusText.text = isPoseSuccess ? "成功!" : "再挑戦";
            statusText.color = isPoseSuccess ? Color.green : Color.red;
        }
        
        if (errorText != null)
        {
            errorText.text = $"角度誤差: {angleError:F1}度";
        }
        
        if (connectionText != null)
        {
            connectionText.text = hasRecentData ? "Python接続中" : "Python待機中";
            connectionText.color = hasRecentData ? Color.green : Color.yellow;
        }
    }
    
    void OnDestroy()
    {
        StopUDPListener();
    }
    
    void OnApplicationQuit()
    {
        StopUDPListener();
    }
    
    void StopUDPListener()
    {
        isReceiving = false;
        
        if (udpThread != null)
        {
            udpThread.Abort();
        }
        
        if (udpClient != null)
        {
            udpClient.Close();
        }
        
        Debug.Log("UDP受信を停止しました");
    }
    
    // デバッグ用GUI
    void OnGUI()
    {
        if (!showDebugGUI) return;
        
        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        
        GUILayout.Label("=== ポーズ判定受信状況 ===");
        GUILayout.Label($"接続状態: {(hasRecentData ? "受信中" : "待機中")}");
        GUILayout.Label($"ポート: {udpPort}");
        GUILayout.Label($"ポーズ判定: {(isPoseSuccess ? "成功" : "失敗")}");
        GUILayout.Label($"角度誤差: {angleError:F1}度");
        GUILayout.Label($"最終更新: {lastUpdateTime}");
        GUILayout.Label($"キュー内メッセージ: {messageQueue.Count}");
        
        if (GUILayout.Button("受信再開"))
        {
            StopUDPListener();
            System.Threading.Thread.Sleep(500);
            StartUDPListener();
        }
        
        GUILayout.EndArea();
    }
    
    // 公開メソッド - 他のスクリプトから状態を取得
    public bool IsConnected()
    {
        return hasRecentData;
    }
    
    public bool GetLastResult()
    {
        return isPoseSuccess;
    }
    
    public float GetLastError()
    {
        return angleError;
    }
}
