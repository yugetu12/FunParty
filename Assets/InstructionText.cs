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
        "右上げて",
        "左上げて",
        "右下げて",
        "左下げて",
        "右上げないで",
        "左上げないで",
        "右下げないで",
        "左下げないで",
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
        if (currentInstruction == "右上げて")
        {
            if (raiseScript.isRaisedR)
                check.text = "true";
            else
                check.text = "false";
        }
        if (currentInstruction == "右下げて")
        {
            if (!raiseScript.isRaisedR)
                check.text = "true";
            else
                check.text = "false";
        }
        if (currentInstruction == "左上げて")
        {
            if (raiseScript.isRaisedL)
                check.text = "true";
            else
                check.text = "false";
        }
        if (currentInstruction == "左下げて")
        {
            if (!raiseScript.isRaisedL)
                check.text = "true";
            else
                check.text = "false";
        }
    }
}



