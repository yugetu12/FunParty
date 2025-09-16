using UnityEngine;

public class PoleController : MonoBehaviour
{
    public float rotSpeed = 90f;
    public bool isRotating = true;

    void Start()
    {
        
    }

    void Update()
    {
        if (!isRotating) return;

        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
    }
}
