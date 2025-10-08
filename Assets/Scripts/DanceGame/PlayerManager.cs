using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public bool posetrue;
    [HideInInspector] public bool usePythonControl = false;  // Pythonからの制御を使用するか

    void Start()
    {
        
    }

    void Update()
    {
        // Pythonからの制御を使用しない場合のみ、キーボード入力で制御
        if (!usePythonControl)
        {
            posetrue = Input.GetKey(KeyCode.F);
        }
    }
}
