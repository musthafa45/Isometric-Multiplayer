using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Ui.Game {
    public class PlayerProfileSingleUi : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI usernameText;
        [SerializeField] private TextMeshProUGUI rankText;
        [SerializeField] private Transform onlineStatusTransform;
        [SerializeField] private Image timerImage;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private Color32 localPlayerTextColor;
        [SerializeField] private Shadow bgShadow;

        private string _username;
        private ushort _score;
        private bool _isOnline;

        private Coroutine timerCoroutine;
        private float timerDuration = 30f;

        public void SetPlayerData(string userName, ushort score, bool isOnline, Color32 shadowColor) {
            _username = userName;
            _score = score;
            _isOnline = isOnline;
            bgShadow.effectColor = shadowColor;

            UpdateUi();
        }

        public void SetSelected(bool isSelected) {
            if (isSelected) {
                StartTimer(30);
            }
            else {
                StopTimer();
            }
        }

        private void UpdateUi() {
            usernameText.text = _username;
            rankText.text = $"Score: {_score}";
            onlineStatusTransform.gameObject.SetActive(_isOnline);
        }

        public void StartTimer(float duration) {
            timerDuration = duration;

            if (timerCoroutine != null)
                StopCoroutine(timerCoroutine);

            timerCoroutine = StartCoroutine(TimerRoutine());
        }

        public void StopTimer() {
            if (timerCoroutine != null) {
                StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }

            timerImage.fillAmount = 0f;
            timerText.text = "";
        }


        private IEnumerator TimerRoutine() {
            float time = timerDuration;

            while (time > 0f) {
                time -= Time.deltaTime;

                float remaining = Mathf.Max(0f, time);

                timerImage.fillAmount = remaining / timerDuration;

                int seconds = Mathf.CeilToInt(remaining);
                timerText.text = seconds.ToString();

                // Change color when less than 10 seconds
                timerText.color = seconds < 10 ? Color.red : Color.white;

                yield return null;
            }

            timerImage.fillAmount = 0f;
            timerText.text = "0";
            timerText.color = Color.red;
        }
    }
}
