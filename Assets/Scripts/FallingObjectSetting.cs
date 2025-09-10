using UnityEngine;

[System.Serializable]
public class FallingObjectSetting
{
    public GameObject prefab;
    [Range(0f, 1f)]
    public float spawnChance = 1.0f; // 出現確率（0〜1）
    public bool isActive = true;     // 出現させるか
}
