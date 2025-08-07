using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float rotY = 4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    // Update is called once per frame

    void Update()
    {
        transform.Rotate(0, rotY * Time.deltaTime, 0);
    }
}
