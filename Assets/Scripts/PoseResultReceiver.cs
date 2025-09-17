using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Newtonsoft.Json;

public class PoseResultReceiver : MonoBehaviour
{
    [Header("通信設定")]
    public int port = 12345;
    
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
    
    private TcpListener tcpListener;
    private Thread tcpListenerThread;
    private bool isListening = false;
    private bool isConnected = false;
    
    [System.Serializable]
    public class PoseMessage
    {
        public string type;
        public bool success;
        public float angle_error;
        public double timestamp;
        public System.Collections.Generic.Dictionary<string, float> angles;
    }
    
    void Start()
    {
        StartServer();
    }
    
    void StartServer()
    {
        try
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListenerThread = new Thread(new ThreadStart(ListenForClients));
            tcpListenerThread.IsBackground = true;
            tcpListenerThread.Start();
            isListening = true;
            Debug.Log($"ポーズ結果受信サーバーをポート {port} で開始しました");
            UpdateConnectionUI();
        }
        catch (Exception e)
        {
            Debug.LogError($"サーバー開始エラー: {e.Message}");
        }
    }
    
    void ListenForClients()
    {
        tcpListener.Start();
        
        while (isListening)
        {
            try
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                isConnected = true;
                Debug.Log("Pythonクライアントが接続しました");
                
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.IsBackground = true;
                clientThread.Start(client);
            }
            catch (Exception e)
            {
                if (isListening)
                    Debug.LogError($"クライアント接続エラー: {e.Message}");
            }
        }
    }
    
    void HandleClientComm(object client)
    {
        TcpClient tcpClient = (TcpClient)client;
        NetworkStream clientStream = tcpClient.GetStream();
        
        byte[] message = new byte[4096];
        int bytesRead;
        
        while (tcpClient.Connected && isListening)
        {
            bytesRead = 0;
            
            try
            {
                bytesRead = clientStream.Read(message, 0, 4096);
            }
            catch
            {
                break;
            }
            
            if (bytesRead == 0)
            {
                break;
            }
            
            string jsonMessage = Encoding.UTF8.GetString(message, 0, bytesRead);
            ProcessMessage(jsonMessage);
        }
        
        tcpClient.Close();
        isConnected = false;
        Debug.Log("Pythonクライアントが切断しました");
    }
    
    void ProcessMessage(string jsonMessage)
    {
        try
        {
            // 複数のメッセージが結合されている可能性があるため分割
            string[] messages = jsonMessage.Split('\n');
            
            foreach (string msg in messages)
            {
                if (string.IsNullOrEmpty(msg.Trim())) continue;
                
                PoseMessage poseMsg = JsonConvert.DeserializeObject<PoseMessage>(msg);
                
                if (poseMsg.type == "pose_result")
                {
                    // メインスレッドで実行するためキューに追加
                    lock (messageQueue)
                    {
                        messageQueue.Enqueue(poseMsg);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"メッセージ処理エラー: {e.Message}");
            Debug.LogError($"受信メッセージ: {jsonMessage}");
        }
    }
    
    private readonly System.Collections.Generic.Queue<PoseMessage> messageQueue = 
        new System.Collections.Generic.Queue<PoseMessage>();
    
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
        
        UpdateUI();
    }
    
    void HandlePoseResult(PoseMessage msg)
    {
        isPoseSuccess = msg.success;
        angleError = msg.angle_error;
        lastUpdateTime = DateTime.Now.ToString("HH:mm:ss");
        
        Debug.Log($"ポーズ判定: {(msg.success ? "成功" : "失敗")} (誤差: {msg.angle_error:F1}度)");
        
        if (msg.success)
        {
            OnPoseSuccess?.Invoke();
            // ここで成功時の処理を追加
            // 例: エフェクト再生、スコア加算、次のポーズへ進行など
            PlaySuccessEffect();
        }
        else
        {
            OnPoseFail?.Invoke();
            // ここで失敗時の処理を追加
        }
    }
    
    void PlaySuccessEffect()
    {
        // 成功エフェクトの例
        // パーティクルシステムがあれば再生
        ParticleSystem particles = GetComponent<ParticleSystem>();
        if (particles != null)
        {
            particles.Play();
        }
        
        // オーディオソースがあれば再生
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
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
        
        UpdateConnectionUI();
    }
    
    void UpdateConnectionUI()
    {
        if (connectionText != null)
        {
            connectionText.text = isConnected ? "Python接続済み" : "Python待機中";
            connectionText.color = isConnected ? Color.green : Color.yellow;
        }
    }
    
    void OnDestroy()
    {
        StopServer();
    }
    
    void OnApplicationQuit()
    {
        StopServer();
    }
    
    void StopServer()
    {
        isListening = false;
        
        if (tcpListener != null)
        {
            tcpListener.Stop();
        }
        
        if (tcpListenerThread != null)
        {
            tcpListenerThread.Abort();
        }
        
        Debug.Log("ポーズ結果受信サーバーを停止しました");
    }
    
    // デバッグ用GUI
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 150));
        
        GUILayout.Label($"接続状態: {(isConnected ? "接続済み" : "待機中")}");
        GUILayout.Label($"ポーズ判定: {(isPoseSuccess ? "成功" : "失敗")}");
        GUILayout.Label($"角度誤差: {angleError:F1}度");
        GUILayout.Label($"最終更新: {lastUpdateTime}");
        
        if (GUILayout.Button("サーバー再起動"))
        {
            StopServer();
            System.Threading.Thread.Sleep(1000);
            StartServer();
        }
        
        GUILayout.EndArea();
    }
}
