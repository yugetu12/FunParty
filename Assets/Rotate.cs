using UnityEngine;

public class Rotate : MonoBehaviour
{
<<<<<<< HEAD
    [SerializeField] private float rotY = 4f;

=======
     [SerializeField] private float rotY = 4f;  //ジャンプ力

    // Start is called once before the first execution of Update after the MonoBehaviour is created
>>>>>>> c8d726d (回るポールとジャンプするプレイヤーを追加しました。)
    void Start()
    {
        
    }

<<<<<<< HEAD
=======
    // Update is called once per frame
>>>>>>> c8d726d (回るポールとジャンプするプレイヤーを追加しました。)
    void Update()
    {
        transform.Rotate(0, rotY * Time.deltaTime, 0);
    }
}
