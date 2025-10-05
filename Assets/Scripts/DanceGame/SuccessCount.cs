using UnityEngine;

public class SuccessCount : MonoBehaviour
{
    [HideInInspector] public int count;
    public GameObject[] signs;
    [SerializeField] private GameObject lightYellow;

    void Start()
    {
        count = 0;
    }

    public void CountPlus()
    {
        signs[count].GetComponent<SignLightManager>().ChangeLight(lightYellow);
        count++;
    }
}
