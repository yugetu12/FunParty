using UnityEngine;

public class ShowTexture2D : MonoBehaviour
{
    [SerializeField] private Renderer renderer;

    public void ChangeTexture(Texture2D texture)
    {
        //テクスチャを張り替える
        renderer.material.mainTexture = texture;
    }
}
