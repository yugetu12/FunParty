using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float rotZ = 4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    // Update is called once per frame

    void Update()
    {
        if (Input.GetKey(KeyCode.J))
        {
            transform.Rotate(0, 0, rotZ * Time.deltaTime);
        }
    }
}
