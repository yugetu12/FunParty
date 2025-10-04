using UnityEngine;

public class ShowTexture2D : MonoBehaviour
{
    [SerializeField] private Renderer rend;

    public void ChangeTexture(Texture2D texture)
    {
        //テクスチャを張り替える
        rend.material.mainTexture = texture;
    }
}
