using UnityEngine;
using System.Collections;
public class PistonManager : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(PistonMove());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator PistonMove()
    {
        while (true)
        {
            rb.AddForce(Vector3.up * 18f, ForceMode.Impulse);
            yield return new WaitForSeconds(5f);
        }
    }
}
