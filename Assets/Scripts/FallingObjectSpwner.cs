using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class FallingObjectSpawner : MonoBehaviour
{
    public List<FallingObjectSetting> fallingObjects;

    public float initialSpawnInterval = 2f;  // ゲーム開始直後の間隔（秒）
    public float minSpawnInterval = 0.3f;    // 最終的な最小間隔（秒）
    public float gameDuration = 60f;         // ゲームの総時間（秒）

    public Vector3 spawnAreaMin = new Vector3(-8, 10, -8);
    public Vector3 spawnAreaMax = new Vector3(8, 10, 8);

    private float timer = 0f;
    private float gameTimer = 0f;
    private bool gameOver = false;

    void Update()
    {
        if (gameOver) return;

        timer += Time.deltaTime;
        gameTimer += Time.deltaTime;

        float currentSpawnInterval = Mathf.Lerp(initialSpawnInterval, minSpawnInterval, gameTimer / gameDuration);

        if (timer >= currentSpawnInterval)
        {
            SpawnRandomObject();
            timer = 0f;
        }

        if (gameTimer >= gameDuration)
        {
            WinGame();
            gameOver = true;
        }
    }

    void SpawnRandomObject()
    {
        var candidates = new List<FallingObjectSetting>();

        foreach (var obj in fallingObjects)
        {
            if (obj.isActive && Random.value < obj.spawnChance)
            {
                candidates.Add(obj);
            }
        }

        if (candidates.Count > 0)
        {
            var selected = candidates[Random.Range(0, candidates.Count)];

            Vector3 pos = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                spawnAreaMin.y,
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            Instantiate(selected.prefab, pos, Quaternion.identity);
        }
    }

    void WinGame()
    {
        SceneManager.LoadScene("GameClearScene");
    }
}
