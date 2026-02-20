using Ui.Game;
using UnityEngine;

public class GameUi : MonoBehaviour {
    [SerializeField] private Transform containerTop;
    [SerializeField] private Transform containerBottom;
    [SerializeField] private PlayerProfileSingleUi playerProfileTemplatePrefab;

    private void Start() {
        CleanUpUi();
        SetUpUi(GameManager.Instance.CurrentActiveGame.Players);
    }

    private void SetUpUi(string[] players) {
        int topCount = Mathf.CeilToInt(players.Length / 2f);

        for (int i = 0; i < players.Length; i++) {
            Transform parent = i < topCount ? containerTop : containerBottom;
            var ui = Instantiate(playerProfileTemplatePrefab, parent);
            ui.gameObject.SetActive(true);
            ui.SetPlayerData(players[i], 0, true);
        }
    }

    private void CleanUpUi() {
        foreach (Transform child in containerTop)
            Destroy(child.gameObject);

        foreach (Transform child in containerBottom)
            Destroy(child.gameObject);
    }
}
