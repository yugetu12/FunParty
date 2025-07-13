using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float rotY = 4f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(0, rotY * Time.deltaTime, 0);
    }
}
