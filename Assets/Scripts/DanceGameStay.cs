using UnityEngine;
using UnityEngine.SceneManagement;

public class DanceGameStay : MonoBehaviour
{
    public float holdTime = 0f;//秒数カウント
    public float requiredTime = 10f;//目標秒数
    public bool setupTrue = false;//Ｔポーズがあっているかどうか判別、こうちゃんからくる正誤判定いれる？
    public bool danceTime = false;//目標の秒数がたったか判別

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Tpose();//tが押されたsetupTrueで判別、本番は消す
        SetupPose();//setupTrueの時秒数加算、話されたら引く

        if (holdTime >= requiredTime)
        {
            danceTime = true;
        }

        if (danceTime)
        {
            SceneManager.LoadScene("FlagJudge");
        }


    }
        

    public void SetupPose()
    {
        if (setupTrue)
        {
            holdTime += Time.deltaTime;
        }
        else
        {
            holdTime -= Time.deltaTime;
            holdTime = Mathf.Max(holdTime, 0f);//二つのうち大きい方を代入
        }
    }

    public void Tpose()
    {
        if (Input.GetKey(KeyCode.T))
        {
            setupTrue = true;
        }
        else
        {
            setupTrue = false;
        }
    }
}
