using TMPro;
using UnityEngine;
namespace Ui.Game {
    public class PlayerProfileSingleUi : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI usernameText;
        [SerializeField] private TextMeshProUGUI rankText;
        [SerializeField] private Transform onlineStatusTransform;

        private string _username;
        private ushort _rank;
        private bool _isOnline;

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
}

