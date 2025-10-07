using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using System.Linq;

/// <summary>
/// Pythonから送信されるポーズマッチ情報を受信し、
/// ゲーム内で表示されているポーズ画像の番号と照合して検証するクラス
/// </summary>
public class PoseMatchValidator : MonoBehaviour
{
    [Header("UDP設定")]
    [SerializeField] private int receivePort = 5006;
    
    [Header("ゲーム連携")]
    [SerializeField] private DanceGameManager danceGameManager;
    [SerializeField] private PlayerManager playerManager;
    
    [Header("デバッグ設定")]
    [SerializeField] private bool enableDebugGUI = true;
    
    private UdpClient udpClient;
    private Thread receiveThread;
    private bool isRunning = false;
    
    private int currentGamePoseIndex = -1;
    private List<int> matchedPoseNumbers = new List<int>();
    private bool isPoseMatched = false;
    
    private string lastReceivedData = "";
    private float lastReceiveTime = 0f;
    private int totalMatchCount = 0;
    private int totalMismatchCount = 0;
    
    // スレッド間通信用のキュー
    private Queue<string> messageQueue = new Queue<string>();
    private object queueLock = new object();
    
    [System.Serializable]
    public class PoseMatchMessage
    {
        public string type;
        public List<MatchedPose> matched_poses;
        public float timestamp;
        public Dictionary<string, float> angles;
    }
    
    [System.Serializable]
    public class MatchedPose
    {
        public int pose_number;
        public float angle_error;
    }
    
    void Start()
    {
        if (danceGameManager == null)
        {
            danceGameManager = FindFirstObjectByType<DanceGameManager>();
            if (danceGameManager == null)
            {
                Debug.LogWarning("DanceGameManagerが見つかりません。");
            }
        }
        
        if (playerManager == null)
        {
            playerManager = FindFirstObjectByType<PlayerManager>();
            if (playerManager == null)
            {
                Debug.LogWarning("PlayerManagerが見つかりません。");
            }
            else
            {
                // Pythonからの制御を有効化
                playerManager.usePythonControl = true;
                Debug.Log("PlayerManager: Pythonからの制御を有効化");
            }
        }
        else
        {
            // 手動で設定されている場合も有効化
            playerManager.usePythonControl = true;
            Debug.Log("PlayerManager: Pythonからの制御を有効化");
        }
        
        StartUDPReceiver();
        Debug.Log("PoseMatchValidator 開始");
    }
    
    void OnDestroy()
    {
        StopUDPReceiver();
        
        // Python制御を無効化してキーボード制御に戻す
        if (playerManager != null)
        {
            playerManager.usePythonControl = false;
            Debug.Log("PlayerManager: キーボード制御に戻しました");
        }
    }
    
    void StartUDPReceiver()
    {
        try
        {
            udpClient = new UdpClient(receivePort);
            isRunning = true;
            
            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
            
            Debug.Log($"UDP受信開始 (Port: {receivePort})");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"UDP受信失敗: {e.Message}");
        }
    }
    
    void StopUDPReceiver()
    {
        isRunning = false;
        
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Abort();
        }
        
        if (udpClient != null)
        {
            udpClient.Close();
        }
        
        Debug.Log("UDP受信停止");
    }
    
    void ReceiveData()
    {
        while (isRunning)
        {
            try
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, receivePort);
                byte[] data = udpClient.Receive(ref remoteEP);
                string jsonMessage = Encoding.UTF8.GetString(data);
                
                // メインスレッドで処理するためキューに追加
                lock (queueLock)
                {
                    messageQueue.Enqueue(jsonMessage);
                }
            }
            catch (System.Exception e)
            {
                if (isRunning)
                {
                    // エラーメッセージもキューに追加（特殊なマーカー付き）
                    lock (queueLock)
                    {
                        messageQueue.Enqueue($"ERROR:{e.Message}");
                    }
                }
            }
        }
    }
    
    void ProcessReceivedMessage(string jsonMessage)
    {
        try
        {
            PoseMatchMessage message = JsonConvert.DeserializeObject<PoseMatchMessage>(jsonMessage);
            
            if (message.type == "pose_matches")
            {
                lastReceivedData = jsonMessage;
                lastReceiveTime = Time.time;
                
                matchedPoseNumbers.Clear();
                if (message.matched_poses != null)
                {
                    foreach (var match in message.matched_poses)
                    {
                        matchedPoseNumbers.Add(match.pose_number);
                    }
                }
                
                CheckPoseMatch();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"メッセージ処理エラー: {e.Message}");
        }
    }
    
    void CheckPoseMatch()
    {
        if (playerManager == null || danceGameManager == null)
        {
            return;
        }
        
        bool wasMatched = isPoseMatched;
        isPoseMatched = false;
        
        // ゲーム内で現在表示されているポーズ番号を取得(1始まり)
        int currentGamePoseNumber = danceGameManager.CurrentPoseNumber;
        
        // Pythonから送られてきたポーズ番号リストと照合
        if (matchedPoseNumbers.Contains(currentGamePoseNumber))
        {
            // ゲーム内のポーズとPythonの検出が一致している
            isPoseMatched = true;
            playerManager.posetrue = true;
            
            if (!wasMatched)
            {
                totalMatchCount++;
                string poses = string.Join(", ", matchedPoseNumbers.Select(p => $"#{p}"));
                Debug.Log($"<color=green>✓ ポーズ一致！ゲーム: #{currentGamePoseNumber}, 検出: {poses}</color>");
            }
        }
        else
        {
            // ポーズが一致していない
            playerManager.posetrue = false;
            
            if (wasMatched)
            {
                totalMismatchCount++;
                if (matchedPoseNumbers.Count > 0)
                {
                    string poses = string.Join(", ", matchedPoseNumbers.Select(p => $"#{p}"));
                    Debug.Log($"<color=yellow>✗ ポーズ不一致 ゲーム: #{currentGamePoseNumber}, 検出: {poses}</color>");
                }
                else
                {
                    Debug.Log($"<color=red>✗ ポーズ未検出 ゲーム: #{currentGamePoseNumber}</color>");
                }
            }
        }
    }
    
    void Update()
    {
        // キューからメッセージを取得して処理
        lock (queueLock)
        {
            while (messageQueue.Count > 0)
            {
                string message = messageQueue.Dequeue();
                
                if (message.StartsWith("ERROR:"))
                {
                    Debug.LogError($"UDP受信エラー: {message.Substring(6)}");
                }
                else
                {
                    ProcessReceivedMessage(message);
                }
            }
        }
        
        // キーボード操作 - デバッグ情報表示
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("=== PoseMatchValidator ===");
            Debug.Log($"UDP: {isRunning}, Port: {receivePort}");
            if (danceGameManager != null)
            {
                Debug.Log($"ゲーム内ポーズ: #{danceGameManager.CurrentPoseNumber}");
            }
            Debug.Log($"検出ポーズ: {string.Join(", ", matchedPoseNumbers.Select(p => $"#{p}"))}");
            Debug.Log($"マッチ: {totalMatchCount}, ミスマッチ: {totalMismatchCount}");
            if (playerManager != null)
            {
                Debug.Log($"PlayerManager.posetrue: {playerManager.posetrue}");
            }
        }
    }
    
    void OnGUI()
    {
        if (!enableDebugGUI) return;
        
        GUILayout.BeginArea(new Rect(10, 10, 500, 500));
        
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label) { fontSize = 16, fontStyle = FontStyle.Bold, richText = true };
        GUIStyle normalStyle = new GUIStyle(GUI.skin.label) { fontSize = 14, richText = true };
        
        GUILayout.Label("<b><color=cyan>Pose Match Validator</color></b>", titleStyle);
        GUILayout.Space(10);
        
        string udpStatus = isRunning ? "<color=green>受信中</color>" : "<color=red>停止</color>";
        GUILayout.Label($"UDP状態: {udpStatus} (Port: {receivePort})", normalStyle);
        
        if (Time.time - lastReceiveTime < 5f)
        {
            GUILayout.Label($"<color=green>最終受信: {Time.time - lastReceiveTime:F2}秒前</color>", normalStyle);
        }
        else if (lastReceiveTime > 0)
        {
            GUILayout.Label($"<color=yellow>最終受信: {Time.time - lastReceiveTime:F1}秒前</color>", normalStyle);
        }
        else
        {
            GUILayout.Label("<color=red>データ未受信</color>", normalStyle);
        }
        
        GUILayout.Space(10);
        
        if (matchedPoseNumbers.Count > 0)
        {
            string poseList = string.Join(", ", matchedPoseNumbers.Select(p => $"#{p}"));
            GUILayout.Label($"<color=lime>検出ポーズ: {poseList}</color>", normalStyle);
        }
        else
        {
            GUILayout.Label("<color=red>検出ポーズ: なし</color>", normalStyle);
        }
        
        GUILayout.Space(10);
        
        GUILayout.Label($"マッチ回数: <color=green>{totalMatchCount}</color>", normalStyle);
        GUILayout.Label($"ミスマッチ回数: <color=red>{totalMismatchCount}</color>", normalStyle);
        
        GUILayout.Space(10);
        
        if (danceGameManager != null)
        {
            GUILayout.Label("<color=green> DanceGameManager</color>", normalStyle);
        }
        else
        {
            GUILayout.Label("<color=red> DanceGameManager</color>", normalStyle);
        }
        
        if (playerManager != null)
        {
            string playerStatus = playerManager.posetrue ? "<color=green>TRUE</color>" : "<color=red>FALSE</color>";
            GUILayout.Label($"<color=green> PlayerManager</color> (posetrue: {playerStatus})", normalStyle);
        }
        else
        {
            GUILayout.Label("<color=red> PlayerManager</color>", normalStyle);
        }
        
        GUILayout.Space(10);
        
        // 現在のゲーム内ポーズ番号を表示
        if (danceGameManager != null)
        {
            GUILayout.Label($"<color=cyan>ゲーム内ポーズ: #{danceGameManager.CurrentPoseNumber}</color>", normalStyle);
        }
        
        GUILayout.Space(10);
        
        GUILayout.Label("<color=lime>自動判定反映: 常時ON</color>", normalStyle);
        
        GUILayout.Space(10);
        
        GUILayout.Label("<color=white>[I] 詳細情報</color>", normalStyle);
        
        GUILayout.EndArea();
    }
}
