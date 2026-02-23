using TMPro;
using UnityEngine;

public class QuestionAnswerUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI problemCountTextUi;
    [SerializeField] private TextMeshProUGUI problemTextUi;
    [SerializeField] private TextMeshProUGUI 
        answer_A_textUi,
        answer_B_textUi,
        answer_C_textUi,
        answer_D_textUi;


    private void Awake() {
        //SetProblemCountText(0,20);
        //SetProblem("1+2");
    }

    public void SetProblem(string problem) {
        problemTextUi.text = $"{problem} = ?";
    }

    public void SetProblemCountText(int currentProblem,int problemMaxCount) {
        problemCountTextUi.text = $"Problem {currentProblem} / {problemMaxCount}";
    }

    public void SetAnswers(string a, string b, string c, string d) {
        answer_A_textUi.text = a;
        answer_B_textUi.text = b;
        answer_C_textUi.text = c;
        answer_D_textUi.text = d;
    }
}
