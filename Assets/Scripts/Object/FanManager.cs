using UnityEngine;

public class FanManager : MonoBehaviour
{
    [SerializeField] private GameObject fan;
    [SerializeField] private float rotSpeed = 120f;

    void Update()
    {
        fan.transform.Rotate(0, 0, rotSpeed * Time.deltaTime);
    }
}
