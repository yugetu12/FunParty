using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject startPanel; // スタート画面
    public GameObject gamePanel;  // ゲーム画面
    public GameObject player;     // プレイヤー
    public GameObject[] bars;     // 回転バー

    public void StartGame()
    {
        startPanel.SetActive(false);
        gamePanel.SetActive(true);

        // プレイヤーの移動ON
        player.GetComponent<MikanSit>().canMove = true;

        // バー回転ON
        foreach (GameObject bar in bars)
        {
            bar.GetComponent<BarMikan>().canRotate = true;
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
