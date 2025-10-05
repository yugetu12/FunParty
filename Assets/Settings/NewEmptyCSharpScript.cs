using UnityEngine;

public class NewEmptyCSharpScript : MonoBehaviour
{
    [SerializeField] float rotY = 4f;

    void Start()
    {

    }
    void Update()
    {
        transform.Rotate(0, rotY * Time.deltaTime, 0);
    }
}
