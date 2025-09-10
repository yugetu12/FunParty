using UnityEngine;

public class BarMikan : MonoBehaviour
{
    [SerializeField] private float rotY = 4f;
    public bool canRotate = false;
    void Start()
    {
        canRotate = false;
    }
    void Update()
    {
        if (!canRotate) return;
        transform.Rotate(0,rotY * Time.deltaTime,0); // ← X軸回転

    }
}
