using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public bool posetrue;

    void Start()
    {
        
    }

    void Update()
    {
        posetrue = Input.GetKey(KeyCode.F);
    }
}
