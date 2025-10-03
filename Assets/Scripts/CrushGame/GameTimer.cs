using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public float timeLimit = 60f; 
    [HideInInspector] public float currentTime;
    public float[] timeSet;

    void Start()
    {
        currentTime = timeLimit;
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0)
            {
                currentTime = 0;
                GetComponent<ChangeScene>().LoadScene("GameClearScene");
            }
        }
    }
}
