using System;
using TMPro;
using UnityEngine;

public class PlayerProfileSingleUi : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI usernameText;
   [SerializeField] private TextMeshProUGUI rankText;
   [SerializeField] private Transform onlineStatusTransform;

    private string _username;
    private ushort _rank;
    private bool _isOnline;
    private void Start() {
        // Just for testing purposes
        // In real scenario, data should be set from outside
        //SetPlayerData("Player", 200, false);
    }

    public void SetPlayerData(string userName, ushort rank, bool isOnline) {
        _username = userName;
        _rank = rank;
        _isOnline = isOnline;

        UpdateUi();
    }

    private void UpdateUi() {
        usernameText.text = _username;
        rankText.text = $"{_rank}";
        onlineStatusTransform.gameObject.SetActive(_isOnline);
    }
}
