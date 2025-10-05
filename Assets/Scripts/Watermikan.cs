using UnityEngine;

public class Watermikan : MonoBehaviour
{
    public float totalTime = 60f; // 落ちるまでの時間
    private float elapsedTime = 0f;
    private Vector3 initialPos;
    public float dropDistance = 2.5f; // 落下距離

    void Start()
    {
        initialPos = transform.position;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        float t = Mathf.Clamp01(elapsedTime / totalTime);
        float newY = initialPos.y - dropDistance * t;

        transform.position = new Vector3(initialPos.x, newY, initialPos.z);
    }
}