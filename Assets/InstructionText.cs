using UnityEngine;
using UnityEngine.UI;
public class InstructionText : MonoBehaviour
{
    public Raise raiseScript;      // Raise スクリプトの参照
    public Text instructionText;   // 指示を表示するUI Text
    public Text check; // 正誤
    public string currentInstruction;
    public float changeInterval = 2f; // 指示が変わる間隔（秒）

    private string[] instructions = { //指示文を格納した配列
        "Right up",
        "Left up",
        "Right down",
        "Left down"
    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("ShowRandomInstruction", 0f, changeInterval);
        InvokeRepeating("checkflag", changeInterval, changeInterval); //0fからはじめ、changeIntaervalの間隔でShowRandomInstructionを繰り返す
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ShowRandomInstruction()
    {
        int randIndex = Random.Range(0, instructions.Length);
        currentInstruction = instructions[randIndex];
        instructionText.text = currentInstruction;
    }

    void checkflag()
    {
        if (currentInstruction == "Right up")
        {
            if (raiseScript.isRaisedR)
                check.text = "true";
            else
                check.text = "false";
        }
        if (currentInstruction == "Right down")
        {
            if (!raiseScript.isRaisedR)
                check.text = "true";
            else
                check.text = "false";
        }
        if (currentInstruction == "Left up")
        {
            if (raiseScript.isRaisedL)
                check.text = "true";
            else
                check.text = "false";
        }
        if (currentInstruction == "Left down")
        {
            if (!raiseScript.isRaisedL)
                check.text = "true";
            else
                check.text = "false";
        }
    }
}



