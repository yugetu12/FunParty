using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float gameTime;
    public Text infoText;

    void Start()
    {
        Instraction(infoText);
        gameTime = 0.0f;
    }

    void Update()
    {
        gameFlagJudge(infoText);
    }

    private void gameFlagJudge(Text infoText)
    {
        //旗上げゲームにはTextが必要
        if (infoText == null) return;

        //判定
        if (gameTime > 10.5f)
        {
            gameTime = 0.0f;

            //判定を伝える
            Debug.Log(Judge(infoText) ? "Correct!" : "Wrong!");

            //指示を変える
            Instraction(infoText);
        }
        //計測
        gameTime += Time.deltaTime;
    }
    private void Instraction(Text infoText)
    {
        //指示をランダムにする
        string infoLR, infoTB;
        if (Percent(50))
        {
            infoLR = "Left";
        }
        else
        {
            infoLR = "Right";
        }
        if (Percent(50))
        {
            infoTB = "Top";
        }
        else
        {
            infoTB = "Bottom";
        }
        //代入
        infoText.text = infoLR + " " + infoTB;
    }
    private bool Judge(Text infoText)
    {
        //右腕と左腕を取得
        FlagController rightArm = GameObject.Find("RightArm").GetComponent<FlagController>();
        FlagController leftArm = GameObject.Find("LeftArm").GetComponent<FlagController>();

        //指示を取得
        string[] instructions = infoText.text.Split(' ');
        if (instructions.Length != 2) return false;

        string infoLR = instructions[0];
        string infoTB = instructions[1];

        //判定
        bool isCorrect = true;

        if (infoLR == "Left")
        {
            if (leftArm.rotZ > 0 && infoTB == "Top") isCorrect = true;
            else if (leftArm.rotZ < 0 && infoTB == "Bottom") isCorrect = true;
            else isCorrect = false;
        }
        else if (infoLR == "Right")
        {
            if (rightArm.rotZ > 0 && infoTB == "Top") isCorrect = true;
            else if (rightArm.rotZ < 0 && infoTB == "Bottom") isCorrect = true;
            else isCorrect = false;
        }

        return isCorrect;
    }

    private bool Percent(int percent)
    {
        return Random.Range(0, 100) < percent;
    }
}
