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
        //時間を計測
        if (currentTime > 0) currentTime -= Time.deltaTime;
    }
}
