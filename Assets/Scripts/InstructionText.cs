using UnityEngine;
using UnityEngine.UI;

public class InstructionText : MonoBehaviour
{
    public Raise raiseScript;      // Raise スクリプトの参照
    public Text instructionText;   // 指示を表示するUI Text
    public Text check;             // 正誤表示用
    public Text resultText;        // GameClear表示用（新しく追加する）

    public string currentInstruction;
    public float changeInterval = 2f; // 指示が変わる間隔（秒）

    private int successCount = 0;      // true の回数
    public int clearThreshold = 5;     // クリア条件（例: 5回trueになったらクリア）

    private string[] instructions = {
        "右上げて",
        "左上げて",
        "右下げて",
        "左下げて",
    };

    void Start()
    {
        InvokeRepeating("ShowRandomInstruction", 0f, changeInterval);
        InvokeRepeating("checkflag", changeInterval, changeInterval);
    }

    void ShowRandomInstruction()
    {
        int randIndex = Random.Range(0, instructions.Length);
        currentInstruction = instructions[randIndex];
        instructionText.text = currentInstruction;
    }

    void checkflag()
    {
        if (resultText == null) Debug.LogError("resultText が割り当てられていません！"); 
        bool isCorrect = false;

        if (currentInstruction == "右上げて")
            isCorrect = raiseScript.isRaisedR;

        if (currentInstruction == "右下げて")
            isCorrect = !raiseScript.isRaisedR;

        if (currentInstruction == "左上げて")
            isCorrect = raiseScript.isRaisedL;

        if (currentInstruction == "左下げて")
            isCorrect = !raiseScript.isRaisedL;

        if (isCorrect)
        {
            check.text = "true";
            successCount++;

            if (successCount >= clearThreshold)
            {
                resultText.text = "GameClear!";
                CancelInvoke(); // これでランダム指示とチェックを止める
            }
        }
        else
        {
            check.text = "false";
        }
    }
}
