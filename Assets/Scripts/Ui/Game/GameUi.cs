using System.Collections.Generic;
using Ui.Game;
using UnityEngine;

public class GameUi : MonoBehaviour {
    [SerializeField] private Transform containerTop;
    [SerializeField] private Transform containerBottom;
    [SerializeField] private PlayerProfileSingleUi playerProfileTemplatePrefab;
    [SerializeField] private QuestionAnswerUi questionAnswerUi;
    private Dictionary<string, PlayerProfileSingleUi> playerUis = new();

    [SerializeField]
    private Color32[] playerColors = new Color32[]
    {
    new Color32(255, 80, 80, 255),   // Red
    new Color32(80, 160, 255, 255),  // Blue
    new Color32(80, 220, 120, 255),  // Green
    new Color32(255, 210, 80, 255)   // Yellow
    };


    private void Start() {
        if (GameManager.Instance != null) {
            CleanUpUi();
            SetUpUi(GameManager.Instance.CurrentActiveGame.Players);
        }    
    }

    private void SetUpUi(string[] players) {
        int topCount = Mathf.CeilToInt(players.Length / 2f);

        for (int i = 0; i < players.Length; i++) {

            Transform parent = i < topCount ? containerTop : containerBottom;

            var ui = Instantiate(playerProfileTemplatePrefab, parent);
            ui.gameObject.SetActive(true);

            Color32 color = playerColors[i % playerColors.Length];

            ui.SetPlayerData(players[i], 0, true,color);

            playerUis.Add(players[i], ui);
        }

        //UpdateTurnUi(GameManager.Instance.CurrentActiveGame.CurrentPlayerTurn);
        if (GameManager.Instance.CurrentProblem != null) {
            questionAnswerUi.SetProblemCountText(GameManager.Instance.CurrentProblem.Id + 1, 10);
            questionAnswerUi.SetProblem(GameManager.Instance.CurrentProblem.Question);
            string answerA = GameManager.Instance.CurrentProblem.AnswerA;
            string answerB = GameManager.Instance.CurrentProblem.AnswerB;
            string answerC = GameManager.Instance.CurrentProblem.AnswerC;
            string answerD = GameManager.Instance.CurrentProblem.AnswerD;
            questionAnswerUi.SetAnswers(answerA, answerB, answerC, answerD);
        }
        else {
            Debug.LogError("GameManager.Instance.CurrentProblem Not exist");
        }
        
    }

    private void UpdateTurnUi(string currentPlayerName) {
        foreach (var kvp in playerUis) {
            bool isCurrentPlayer = kvp.Key == currentPlayerName;
            kvp.Value.SetSelected(isCurrentPlayer);
        }
    }

    private void CleanUpUi() {
        foreach (Transform child in containerTop)
            Destroy(child.gameObject);

        foreach (Transform child in containerBottom)
            Destroy(child.gameObject);
    }
}
