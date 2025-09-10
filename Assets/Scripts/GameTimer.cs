using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float timeLimit = 60f; 
    private float currentTime;

    public TextMeshProUGUI timerText;

    void Start()
    {
        currentTime = timeLimit;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime < 0)
        {
            currentTime = 0;
        }

        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(currentTime).ToString();
        }
    }
}
