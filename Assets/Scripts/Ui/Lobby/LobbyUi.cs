using NetworkShared.Packets.ClientServer;
using NetworkShared.Packets.ServerClient;
using PacketHandlers;
using System;
using TMPro;
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

    private void Awake() {
        cancelFindOpponent.gameObject.SetActive(false);
        loadingAnimTransform.gameObject.SetActive(false);
        playerProfileSingleUi.gameObject.SetActive(false);

        findOpponentButton.onClick.AddListener(() => {
            findingOpponentText.gameObject.SetActive(false);
            loadingAnimTransform.gameObject.SetActive(true);
            cancelFindOpponent.gameObject.SetActive(true);
        });

        cancelFindOpponent.onClick.AddListener(() => {
            findingOpponentText.gameObject.SetActive(true);
            loadingAnimTransform.gameObject.SetActive(false);
            cancelFindOpponent.gameObject.SetActive(false);
        });

        logoutButton.onClick.AddListener(() => {
            // For simplicity, just reload the scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
        });
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
