using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using System.Collections;

public class UDPImageReceiver : MonoBehaviour
{
    [Header("UDP Settings")]
    public int localPort = 9999;
    
    [Header("Display Settings")]
    public Renderer imageRenderer; // 画像を表示するRenderer
    public bool showInUI = true;   // UIで表示するかどうか
    
    private UdpClient udpClient;
    private Thread receiveThread;
    private bool isReceiving = false;
    
    // 受信した画像データを格納
    private byte[] latestImageData;
    private bool hasNewImage = false;
    
    // Texture2D for displaying
    public Texture2D imageTexture;
    
    void Start()
    {
        InitializeUDPReceiver();
    }
    
    void InitializeUDPReceiver()
    {
        try
        {
            udpClient = new UdpClient(localPort);
            isReceiving = true;
            
            receiveThread = new Thread(ReceiveData);
            receiveThread.IsBackground = true;
            receiveThread.Start();
            
            Debug.Log($"UDP Receiver started on port {localPort}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to initialize UDP receiver: {e.Message}");
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
                
                if (data.Length > 4) // サイズ情報（4バイト）+ 画像データ
                {
                    // サイズ情報を読み取り
                    int imageSize = BitConverter.ToInt32(data, 0);
                    
                    if (data.Length >= 4 + imageSize)
                    {
                        // 画像データを抽出
                        byte[] imageData = new byte[imageSize];
                        Array.Copy(data, 4, imageData, 0, imageSize);
                        
                        // メインスレッドで処理するためにキューに追加
                        lock (this)
                        {
                            latestImageData = imageData;
                            hasNewImage = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (isReceiving)
                {
                    Debug.LogError($"UDP receive error: {e.Message}");
                }
            }
        }
    }
    
    void Update()
    {
        // 新しい画像データがある場合、メインスレッドで処理
        if (hasNewImage)
        {
            lock (this)
            {
                if (latestImageData != null)
                {
                    ProcessReceivedImage(latestImageData);
                    hasNewImage = false;
                }
            }
        }
    }
    
    void ProcessReceivedImage(byte[] imageData)
    {
        try
        {
            // 画像データを直接Texture2Dとして読み込み
            if (imageTexture == null)
            {
                imageTexture = new Texture2D(2, 2);
            }
            
            // JPEGデータをTexture2Dに読み込み
            bool loaded = imageTexture.LoadImage(imageData);
            
            if (loaded)
            {
                // Rendererに画像を設定
                if (imageRenderer != null)
                {
                    imageRenderer.material.mainTexture = imageTexture;
                }
                
                Debug.Log($"Image received and displayed. Size: {imageTexture.width}x{imageTexture.height}");
            }
            else
            {
                Debug.LogWarning("Failed to load image data");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error processing image: {e.Message}");
        }
    }
    
    void OnDestroy()
    {
        StopReceiving();
    }
    
    void OnApplicationQuit()
    {
        StopReceiving();
    }
    
    void StopReceiving()
    {
        isReceiving = false;
        
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Join(1000); // 1秒待機
        }
        
        if (udpClient != null)
        {
            udpClient.Close();
        }
        
        Debug.Log("UDP Receiver stopped");
    }
    
    // GUI for debugging
    void OnGUI()
    {
        if (showInUI && imageTexture != null)
        {
            GUI.DrawTexture(new Rect(10, 10, 200, 200), imageTexture, ScaleMode.ScaleToFit);
        }
        
        GUI.Label(new Rect(10, 220, 200, 20), $"Port: {localPort}");
        GUI.Label(new Rect(10, 240, 200, 20), $"Receiving: {isReceiving}");
        if (imageTexture != null)
        {
            GUI.Label(new Rect(10, 260, 200, 20), $"Size: {imageTexture.width}x{imageTexture.height}");
        }
    }
}
