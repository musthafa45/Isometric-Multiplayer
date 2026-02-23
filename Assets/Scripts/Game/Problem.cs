public class Problem {
    public int Id;
    public Complexity Complexity;

    public string Question;
    public string AnswerA;
    public string AnswerB;
    public string AnswerC;
    public string AnswerD;

    public int CorrectIndex; // 0=A,1=B,2=C,3=D
}

public enum Complexity {
    None,
    Low,
    Medium,
    Hard,
}
