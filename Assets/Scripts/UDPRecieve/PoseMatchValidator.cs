using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using System.Linq;

/// <summary>
/// Pythonから送信される全ポーズマッチ情報を受信し、
/// Unity側でランダムに選んだポーズと合致しているか検証するクラス
/// </summary>
public class PoseMatchValidator : MonoBehaviour
{
    [Header("UDP設定")]
    [SerializeField] private int receivePort = 5006;
    
    [Header("ポーズ設定")]
    [SerializeField] private int totalPoseCount = 20;  // sample_pose.jsonに保存されているポーズの総数
    [SerializeField] private float matchThreshold = 15.0f;  // 角度誤差の閾値
    
    [Header("ゲームロジック")]
    [SerializeField] private float poseDuration = 3.0f;  // 各ポーズの表示時間(秒)
    [SerializeField] private bool autoChangepose = true;  // 自動でポーズを切り替えるか
    
    [Header("UI表示(オプション)")]
    [SerializeField] private UnityEngine.UI.Text statusText;
    [SerializeField] private UnityEngine.UI.Text currentPoseText;
    [SerializeField] private UnityEngine.UI.Text matchedPosesText;
    [SerializeField] private UnityEngine.UI.Text scoreText;
    [SerializeField] private UnityEngine.UI.Image poseReferenceImage;  // ポーズ参考画像
    [SerializeField] private Sprite[] poseReferenceSprites;  // ポーズ参考画像の配列
    
    // UDP受信用
    private UdpClient udpClient;
    private Thread receiveThread;
    private bool isRunning = false;
    
    // 現在のゲーム状態
    private int currentTargetPoseNumber = -1;  // Unity側が現在提示しているポーズ番号(1始まり)
    private List<int> matchedPoseNumbers = new List<int>();  // Pythonから送られてきた合致ポーズリスト
    private float lastPoseChangeTime = 0f;
    private int score = 0;
    private bool isPoseMatched = false;
    
    // ポーズ管理
    private List<int> availablePoses = new List<int>();
    
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
        InitializePoseList();
        SelectNewPose();
        StartUDPReceiver();
        lastPoseChangeTime = Time.time;
    }
    
    void OnDestroy()
    {
        StopUDPReceiver();
    }
    
    /// <summary>
    /// 利用可能なポーズリストを初期化
    /// </summary>
    void InitializePoseList()
    {
        availablePoses.Clear();
        for (int i = 1; i <= totalPoseCount; i++)
        {
            availablePoses.Add(i);
        }
        Debug.Log($"ポーズリストを初期化しました。総数: {totalPoseCount}");
    }
    
    /// <summary>
    /// 新しいポーズをランダムに選択
    /// </summary>
    void SelectNewPose()
    {
        if (availablePoses.Count == 0)
        {
            Debug.Log("全ポーズをクリア!リセットします。");
            InitializePoseList();
        }
        
        int randomIndex = Random.Range(0, availablePoses.Count);
        currentTargetPoseNumber = availablePoses[randomIndex];
        availablePoses.RemoveAt(randomIndex);
        
        isPoseMatched = false;
        lastPoseChangeTime = Time.time;
        
        Debug.Log($"新しいポーズを選択: #{currentTargetPoseNumber} (残り: {availablePoses.Count})");
        
        // UI更新
        UpdateUI();
        
        // 参考画像を表示
        if (poseReferenceImage != null && poseReferenceSprites != null && poseReferenceSprites.Length >= currentTargetPoseNumber)
        {
            poseReferenceImage.sprite = poseReferenceSprites[currentTargetPoseNumber - 1];
        }
    }
    
    /// <summary>
    /// UDP受信を開始
    /// </summary>
    void StartUDPReceiver()
    {
        try
        {
            udpClient = new UdpClient(receivePort);
            isRunning = true;
            
            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
            
            Debug.Log($"UDP受信を開始しました (Port: {receivePort})");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"UDP受信の開始に失敗: {e.Message}");
        }
    }
    
    /// <summary>
    /// UDP受信を停止
    /// </summary>
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
        
        Debug.Log("UDP受信を停止しました");
    }
    
    /// <summary>
    /// UDP受信スレッド
    /// </summary>
    void ReceiveData()
    {
        while (isRunning)
        {
            try
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, receivePort);
                byte[] data = udpClient.Receive(ref remoteEP);
                string jsonMessage = Encoding.UTF8.GetString(data);
                
                // メインスレッドで処理
                UnityMainThreadDispatcher.Instance().Enqueue(() => ProcessReceivedMessage(jsonMessage));
            }
            catch (System.Exception e)
            {
                if (isRunning)
                {
                    Debug.LogError($"UDP受信エラー: {e.Message}");
                }
            }
        }
    }
    
    /// <summary>
    /// 受信したメッセージを処理
    /// </summary>
    void ProcessReceivedMessage(string jsonMessage)
    {
        try
        {
            PoseMatchMessage message = JsonConvert.DeserializeObject<PoseMatchMessage>(jsonMessage);
            
            if (message.type == "pose_matches")
            {
                // 合致しているポーズ番号のリストを更新
                matchedPoseNumbers.Clear();
                if (message.matched_poses != null)
                {
                    foreach (var match in message.matched_poses)
                    {
                        matchedPoseNumbers.Add(match.pose_number);
                    }
                }
                
                // 現在のターゲットポーズと合致しているか検証
                bool isCurrentPoseMatched = matchedPoseNumbers.Contains(currentTargetPoseNumber);
                
                if (isCurrentPoseMatched && !isPoseMatched)
                {
                    // ポーズが合致した!
                    OnPoseMatched();
                }
                else if (!isCurrentPoseMatched && isPoseMatched)
                {
                    // ポーズが崩れた
                    isPoseMatched = false;
                    Debug.Log("ポーズが崩れました");
                }
                
                // UI更新
                UpdateUI();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"メッセージ処理エラー: {e.Message}");
        }
    }
    
    /// <summary>
    /// ポーズが合致したときの処理
    /// </summary>
    void OnPoseMatched()
    {
        isPoseMatched = true;
        score += 100;
        
        Debug.Log($"ポーズ #{currentTargetPoseNumber} 成功! スコア: {score}");
        
        // 効果音やエフェクトを再生する処理をここに追加
        
        // 次のポーズへ
        if (autoChangepose)
        {
            Invoke("SelectNewPose", 1.0f);  // 1秒後に次のポーズ
        }
    }
    
    /// <summary>
    /// UI更新
    /// </summary>
    void UpdateUI()
    {
        if (currentPoseText != null)
        {
            currentPoseText.text = $"Target Pose: #{currentTargetPoseNumber}";
        }
        
        if (matchedPosesText != null)
        {
            if (matchedPoseNumbers.Count > 0)
            {
                string matchedList = string.Join(", ", matchedPoseNumbers.Select(p => $"#{p}"));
                matchedPosesText.text = $"Matched: {matchedList}";
                matchedPosesText.color = matchedPoseNumbers.Contains(currentTargetPoseNumber) ? Color.green : Color.yellow;
            }
            else
            {
                matchedPosesText.text = "Matched: None";
                matchedPosesText.color = Color.red;
            }
        }
        
        if (statusText != null)
        {
            if (isPoseMatched)
            {
                statusText.text = "SUCCESS!";
                statusText.color = Color.green;
            }
            else if (matchedPoseNumbers.Count > 0)
            {
                statusText.text = "DIFFERENT POSE";
                statusText.color = Color.yellow;
            }
            else
            {
                statusText.text = "NO MATCH";
                statusText.color = Color.red;
            }
        }
        
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }
    
    void Update()
    {
        // タイムアウトで自動的に次のポーズへ(オプション)
        if (autoChangepose && !isPoseMatched && Time.time - lastPoseChangeTime > poseDuration)
        {
            Debug.Log("タイムアウト。次のポーズへ");
            SelectNewPose();
        }
        
        // デバッグ用: Spaceキーで手動で次のポーズへ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectNewPose();
        }
        
        // デバッグ用: Rキーでリセット
        if (Input.GetKeyDown(KeyCode.R))
        {
            score = 0;
            InitializePoseList();
            SelectNewPose();
        }
    }
    
    void OnGUI()
    {
        // デバッグ情報表示
        GUILayout.BeginArea(new Rect(10, 10, 400, 300));
        GUILayout.Label($"<b>Pose Match Validator</b>", new GUIStyle(GUI.skin.label) { fontSize = 16, richText = true });
        GUILayout.Label($"Target Pose: #{currentTargetPoseNumber}");
        GUILayout.Label($"Matched Poses: {string.Join(", ", matchedPoseNumbers.Select(p => $"#{p}"))}");
        GUILayout.Label($"Status: {(isPoseMatched ? "<color=green>MATCHED!</color>" : "<color=red>NOT MATCHED</color>")}", new GUIStyle(GUI.skin.label) { richText = true });
        GUILayout.Label($"Score: {score}");
        GUILayout.Label($"Remaining Poses: {availablePoses.Count}");
        GUILayout.Space(10);
        GUILayout.Label("[Space] Next Pose  [R] Reset");
        GUILayout.EndArea();
    }
}

/// <summary>
/// メインスレッドでアクションを実行するためのディスパッチャー
/// </summary>
public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static UnityMainThreadDispatcher instance;
    private Queue<System.Action> actions = new Queue<System.Action>();
    
    public static UnityMainThreadDispatcher Instance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("UnityMainThreadDispatcher");
            instance = go.AddComponent<UnityMainThreadDispatcher>();
            DontDestroyOnLoad(go);
        }
        return instance;
    }
    
    public void Enqueue(System.Action action)
    {
        lock (actions)
        {
            actions.Enqueue(action);
        }
    }
    
    void Update()
    {
        lock (actions)
        {
            while (actions.Count > 0)
            {
                actions.Dequeue()?.Invoke();
            }
        }
    }
}
