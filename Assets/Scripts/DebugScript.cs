using UnityEngine;

[RequireComponent(typeof(ChangeScene))]
public class DebugScript : MonoBehaviour
{
    [System.Serializable]
    public class KeyScenePair
    {
        public KeyCode key;          // 押すキー
        public string sceneName;     // 飛ぶ先のシーン名
    }

    [SerializeField] private ChangeScene scene;     //シーン管理スクリプト
    public KeyScenePair[] keyScenePairs;            // エディタで複数設定できる配列

    void Update()
    {
        foreach (var pair in keyScenePairs)
        {
            if (Input.GetKeyDown(pair.key))
            {
                scene.LoadScene(pair.sceneName);
                break;
            }
        }
    }
}
