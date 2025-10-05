using UnityEngine;

[RequireComponent(typeof(ChangeScene))]
public class DebugScript : MonoBehaviour
{
    [SerializeField] private ChangeScene scene;   //Script

    void Start()
    {
        if (scene == null) scene = GetComponent<ChangeScene>();
    }

    void Update()
    {
        if (Input.GetKeyDown("r")) scene.ReloadScene();
    }
}
