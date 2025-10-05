// 見えてますかー
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
    public int currentPoseNumber = 0;
    public float successDuration = 0f;
    public float requiredDuration = 2.0f;
    
    [Header("UI表示")]
    public UnityEngine.UI.Text statusText;
    public UnityEngine.UI.Text errorText;
    public UnityEngine.UI.Text connectionText;
    public UnityEngine.UI.Text poseNumberText;
    public UnityEngine.UI.Text progressText;
    
    [Header("イベント")]
    public UnityEngine.Events.UnityEvent OnPoseSuccess;
    public UnityEngine.Events.UnityEvent OnPoseFail;
    public UnityEngine.Events.UnityEvent OnPoseComplete;  // 2秒間成功してクリア
    public UnityEngine.Events.UnityEvent OnPoseChange;  // ポーズ番号が変わった
    
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
        public int pose_number;
        public float success_duration;
        public float required_duration;
    }
    
    [System.Serializable]
    public class PoseChangeMessage
    {
        public string type;
        public int pose_number;
        public int remaining_poses;
        public double timestamp;
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
            // まずtypeを確認
            var msgType = JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, object>>(jsonMessage);
            
            if (msgType.ContainsKey("type"))
            {
                string type = msgType["type"].ToString();
                
                if (type == "pose_result")
                {
                    PoseMessage poseMsg = JsonConvert.DeserializeObject<PoseMessage>(jsonMessage);
                    // メインスレッドで実行するためキューに追加
                    lock (messageQueue)
                    {
                        messageQueue.Enqueue(poseMsg);
                    }
                }
                else if (type == "pose_change")
                {
                    PoseChangeMessage changeMsg = JsonConvert.DeserializeObject<PoseChangeMessage>(jsonMessage);
                    HandlePoseChange(changeMsg);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"メッセージ処理エラー: {e.Message}");
            Debug.LogError($"受信メッセージ: {jsonMessage}");
        }
    }
    
    void HandlePoseChange(PoseChangeMessage msg)
    {
        currentPoseNumber = msg.pose_number;
        Debug.Log($"新しいポーズに変更: ポーズ #{msg.pose_number} (残り: {msg.remaining_poses})");
        OnPoseChange?.Invoke();
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
    
    private bool wasCompleted = false;
    
    void HandlePoseResult(PoseMessage msg)
    {
        isPoseSuccess = msg.success;
        angleError = msg.angle_error;
        lastUpdateTime = DateTime.Now.ToString("HH:mm:ss.fff");
        
        // ポーズ番号と成功持続時間を更新
        if (msg.pose_number > 0)
        {
            currentPoseNumber = msg.pose_number;
        }
        
        if (msg.success)
        {
            successDuration = msg.success_duration;
            requiredDuration = msg.required_duration;
            
            // 2秒間成功を維持したかチェック
            bool isCompleted = successDuration >= requiredDuration;
            
            if (isCompleted && !wasCompleted)
            {
                Debug.Log($"ポーズ #{currentPoseNumber} 完全クリア！ (2秒維持)");
                OnPoseComplete?.Invoke();
                PlaySuccessEffect();
                wasCompleted = true;
            }
            else if (!isCompleted)
            {
                Debug.Log($"ポーズ #{currentPoseNumber} 維持中: {successDuration:F1}s / {requiredDuration:F1}s (誤差: {msg.angle_error:F1}度)");
                wasCompleted = false;
            }
            
            OnPoseSuccess?.Invoke();
        }
        else
        {
            successDuration = 0f;
            wasCompleted = false;
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
            if (isPoseSuccess && successDuration >= requiredDuration)
            {
                statusText.text = "クリア!";
                statusText.color = Color.cyan;
            }
            else if (isPoseSuccess)
            {
                statusText.text = "維持中...";
                statusText.color = Color.yellow;
            }
            else
            {
                statusText.text = "再挑戦";
                statusText.color = Color.red;
            }
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
        
        if (poseNumberText != null)
        {
            poseNumberText.text = $"ポーズ #{currentPoseNumber}";
        }
        
        if (progressText != null && isPoseSuccess)
        {
            float progress = Mathf.Clamp01(successDuration / requiredDuration);
            progressText.text = $"進捗: {progress * 100:F0}% ({successDuration:F1}s / {requiredDuration:F1}s)";
        }
        else if (progressText != null)
        {
            progressText.text = "ポーズを取ってください";
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
        
        GUILayout.BeginArea(new Rect(10, 10, 350, 250));
        
        GUILayout.Label("=== ポーズ判定受信状況 ===");
        GUILayout.Label($"接続状態: {(hasRecentData ? "受信中" : "待機中")}");
        GUILayout.Label($"ポート: {udpPort}");
        GUILayout.Label($"現在のポーズ: #{currentPoseNumber}");
        GUILayout.Label($"ポーズ判定: {(isPoseSuccess ? "成功" : "失敗")}");
        GUILayout.Label($"角度誤差: {angleError:F1}度");
        GUILayout.Label($"維持時間: {successDuration:F1}s / {requiredDuration:F1}s");
        GUILayout.Label($"進捗: {(successDuration / requiredDuration * 100):F0}%");
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
    
    public int GetCurrentPoseNumber()
    {
        return currentPoseNumber;
    }
    
    public float GetSuccessDuration()
    {
        return successDuration;
    }
    
    public float GetSuccessProgress()
    {
        return Mathf.Clamp01(successDuration / requiredDuration);
    }
    
    public bool IsPoseCompleted()
    {
        return successDuration >= requiredDuration;
    }
}
