using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void StartButton()
    {
        Debug.Log("start");
        SceneManager.LoadScene("flag game");
    }
}
