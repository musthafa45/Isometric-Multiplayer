using NetworkShared.Packets.ClientServer;
using NetworkShared.Packets.ServerClient;
using PacketHandlers;
using Packets.ClientServer;
using TMPro;
using Ui.Lobby;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUi : MonoBehaviour
{
    [SerializeField] private Transform topPlayerContainer;
    [SerializeField] private PlayerProfileSingleUi playerProfileSingleUi;
    [SerializeField] private Button logoutButton, findOpponentButton,cancelFindOpponent;
    [SerializeField] private TextMeshProUGUI totalPlayersCountOnlineText;
    [SerializeField] private Transform loadingAnimTransform;
    [SerializeField] private TextMeshProUGUI findingOpponentText;
    [SerializeField] private Transform selectPlayerUi;
    [SerializeField] private Button TwoPlayerButton, ThreePlayerButton, FourPlayerButton,selectPlayerCancelButton;
    [SerializeField] private Image TwoPlayerButtonBg, ThreePlayerButtonBg, FourPlayerButtonBg;

    private int selectedPlayerCount = 2;
    private void Awake() {
        cancelFindOpponent.gameObject.SetActive(false);
        loadingAnimTransform.gameObject.SetActive(false);
        playerProfileSingleUi.gameObject.SetActive(false);

        TwoPlayerButtonBg.gameObject.SetActive(true);
        ThreePlayerButtonBg.gameObject.SetActive(false);
        FourPlayerButtonBg.gameObject.SetActive(false);
        selectPlayerUi.gameObject.SetActive(false);

        findOpponentButton.onClick.AddListener(() => {
            findOpponentButton.interactable = false;
            logoutButton.interactable = false;

            selectPlayerUi.gameObject.SetActive(true);
            findingOpponentText.gameObject.SetActive(false);
        });

        selectPlayerCancelButton.onClick.AddListener(() => {
            selectPlayerUi.gameObject.SetActive(false);

            findOpponentButton.interactable = true;
            logoutButton.interactable = true;

            findingOpponentText.gameObject.SetActive(true);
            loadingAnimTransform.gameObject.SetActive(false);
            cancelFindOpponent.gameObject.SetActive(false);
        });

        cancelFindOpponent.onClick.AddListener(() => {
            findOpponentButton.interactable = true;
            logoutButton.interactable = true;

            findingOpponentText.gameObject.SetActive(true);
            loadingAnimTransform.gameObject.SetActive(false);
            cancelFindOpponent.gameObject.SetActive(false);

            CancelFindOpponent();
        });

        logoutButton.onClick.AddListener(() => {
            NetworkClient.Instance.Disconnect();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
        });

        TwoPlayerButton.onClick.AddListener(() => {
            TwoPlayerButtonBg.gameObject.SetActive(true);
            ThreePlayerButtonBg.gameObject.SetActive(false);
            FourPlayerButtonBg.gameObject.SetActive(false);
            selectedPlayerCount = 2;

            loadingAnimTransform.gameObject.SetActive(true);
            cancelFindOpponent.gameObject.SetActive(true);

            selectPlayerUi.gameObject.SetActive(false);

            FindOpponent();
        });

        ThreePlayerButton.onClick.AddListener(() => {
            TwoPlayerButtonBg.gameObject.SetActive(false);
            ThreePlayerButtonBg.gameObject.SetActive(true);
            FourPlayerButtonBg.gameObject.SetActive(false);
            selectedPlayerCount = 3;

            loadingAnimTransform.gameObject.SetActive(true);
            cancelFindOpponent.gameObject.SetActive(true);

            selectPlayerUi.gameObject.SetActive(false);

            FindOpponent();
        });

        FourPlayerButton.onClick.AddListener(() => {
            TwoPlayerButtonBg.gameObject.SetActive(false);
            ThreePlayerButtonBg.gameObject.SetActive(false);
            FourPlayerButtonBg.gameObject.SetActive(true);
            selectedPlayerCount = 4;

            loadingAnimTransform.gameObject.SetActive(true);
            cancelFindOpponent.gameObject.SetActive(true);

            selectPlayerUi.gameObject.SetActive(false);

            FindOpponent();
        });
    }

    private void FindOpponent() {
        Net_FindOpponentRequest msg = new Net_FindOpponentRequest() {
            PlayersCount = (ushort)selectedPlayerCount
        };
        if (NetworkClient.Instance != null)
            NetworkClient.Instance.SendDataToServer(msg);
    }

    private void CancelFindOpponent() {
        Net_CancelFindOpponentRequest msg = new Net_CancelFindOpponentRequest();
        if(NetworkClient.Instance != null)
            NetworkClient.Instance.SendDataToServer(msg);
    }


    private void Start() {
        if(NetworkClient.Instance != null) {
            Net_ServerRequestStatus msg = new Net_ServerRequestStatus();
            NetworkClient.Instance.SendDataToServer(msg);
        }

        OnServerStatusRequestHandler.OnServerStatusResponseEvent += OnServerStatusRequestReceived;

    }

    private void OnServerStatusRequestReceived(Net_OnServerStatus msg) {
        foreach (Transform child in topPlayerContainer) {
            if (child.gameObject == playerProfileSingleUi.gameObject) continue;
            Destroy(child.gameObject);
        }

        SetTotalOnlinePlayersCount((int)msg.OnlinePlayersCount);

        foreach (var player in msg.TopPlayersNetDTOs) {
            var entry = Instantiate(playerProfileSingleUi, topPlayerContainer);
            entry.gameObject.SetActive(true);

            entry.SetPlayerData(player.Username, player.Score, player.IsOnline);
        }
    }


    private void SetTotalOnlinePlayersCount(int totalPlayersCount) {
        totalPlayersCountOnlineText.text = $"{totalPlayersCount} Players Online";
    }

    private void OnDestroy() {
        OnServerStatusRequestHandler.OnServerStatusResponseEvent -= OnServerStatusRequestReceived;
    }
}
