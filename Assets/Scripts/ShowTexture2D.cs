using UnityEngine;

public class ShowTexture2D : MonoBehaviour
{
    [SerializeField] private UDPImageReceiver receiver; // UDP画像受信スクリプト
    [SerializeField] private GameObject quad;
    private Renderer quadRenderer;

    void Start()
    {
        quadRenderer = quad.GetComponent<Renderer>();
    }

    void Update()
    {
        if (receiver.imageTexture != null)
        {
            quadRenderer.material.mainTexture = receiver.imageTexture;
        }
    }
}
