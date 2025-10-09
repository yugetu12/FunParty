using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadyAreaChecker : MonoBehaviour
{
    [Header("プレイヤー")]
    public Transform player;

    [Header("判定距離")]
    public float readyThreshold = 1.0f; // どれだけ近づいたら準備完了とするか


    [Header("次に進むシーン名")]
    public string nextSceneName = "PoleAvoid";

    private bool isReady = false;
    private float stayTimer = 0f;//測る
    public float stayTime = 3f; // プレイヤーがどんくらいいるか

    void Update()
    {
        if (isReady) return;

        // プレイヤーとの距離を測る
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < readyThreshold)
        {
            stayTimer += Time.deltaTime;

            if (stayTimer >= stayTime)
            {
                Debug.Log("準備完了");
                isReady = true;
                SceneManager.LoadScene(nextSceneName);
            }
        }
        else
        {
            stayTimer = 0f; // リセット
        }
    }
}