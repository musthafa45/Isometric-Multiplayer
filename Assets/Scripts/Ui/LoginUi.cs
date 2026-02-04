using PacketHandlers;
using NetworkShared.Packets.ClientServer;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUi : MonoBehaviour {

    public static LoginUi Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private Button loginBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;

    [SerializeField] private GameObject userNameErrorText;
    [SerializeField] private GameObject passwordErrorText;
    [SerializeField] private GameObject userNameOrPasswordErrorText;

    [SerializeField] private GameObject loginLoadingAnim;

    [Header("Validation Settings")]
    [SerializeField] private int minLength = 3;
    [SerializeField] private int maxLength = 15;

    private string _username = string.Empty;
    private string _password = string.Empty;
    private bool isConnected;

    // Event for network / backend
    public event Action<string, string> OnLoginAttempt;

    private void Awake() {
        Instance = this;

        loginBtn.onClick.AddListener(OnLoginClicked);
        exitBtn.onClick.AddListener(OnExitClicked);
    }

    private void Start() {
        InitializeUI();

        usernameInput.onValueChanged.AddListener(OnUsernameChanged);
        passwordInput.onValueChanged.AddListener(OnPasswordChanged);

        OnAuthFailure.OnAuthFailureEvent += (msg) => {
            ShowLoginError();
        };

        NetworkClient.Instance.OnConnected += () => {
            isConnected = true;
        };
    }

    private void InitializeUI() {
        loginBtn.interactable = false;

        userNameErrorText.SetActive(false);
        passwordErrorText.SetActive(false);
        userNameOrPasswordErrorText.SetActive(false);

        loginLoadingAnim.SetActive(false);

        usernameInput.text = string.Empty;
        passwordInput.text = string.Empty;
    }

    // -------------------------
    // INPUT HANDLERS
    // -------------------------

    private void OnUsernameChanged(string value) {
        _username = value;
        userNameErrorText.SetActive(!IsValidUsername(_username));
        UpdateLoginButtonState();
    }

    private void OnPasswordChanged(string value) {
        _password = value;
        passwordErrorText.SetActive(!IsValidPassword(_password));
        UpdateLoginButtonState();
    }

    // -------------------------
    // VALIDATION
    // -------------------------

    private bool IsValidUsername(string input) {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        if (input.Length < minLength || input.Length > maxLength)
            return false;

        // Must contain at least ONE letter, numbers allowed
        if (!Regex.IsMatch(input, @"^(?=.*[a-zA-Z])[a-zA-Z0-9]+$"))
            return false;

        return true;
    }

    private bool IsValidPassword(string input) {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        if (input.Length < minLength || input.Length > maxLength)
            return false;

        // Password: alphanumeric only (can be numeric-only if you want)
        if (!Regex.IsMatch(input, @"^[a-zA-Z0-9]+$"))
            return false;

        return true;
    }

    private void UpdateLoginButtonState() {
        loginBtn.interactable =
            IsValidUsername(_username) &&
            IsValidPassword(_password);
    }

    private void SetInputsInteractable(bool value) {
        usernameInput.interactable = value;
        passwordInput.interactable = value;
    }


    // -------------------------
    // BUTTON ACTIONS
    // -------------------------

    private void OnLoginClicked() {
        userNameOrPasswordErrorText.SetActive(false);

        if (!IsValidUsername(_username) || !IsValidPassword(_password))
            return;

        loginBtn.interactable = false;

        Debug.Log($"Login Attempt: {_username}");

        OnLoginAttempt?.Invoke(_username, _password);

        loginLoadingAnim.SetActive(true);

        // 🔒 LOCK INPUTS
        SetInputsInteractable(false);
        loginBtn.interactable = false;

        StopCoroutine(LoginRoutine());
        StartCoroutine(LoginRoutine());

    }

    private IEnumerator LoginRoutine() {
       NetworkClient.Instance.Connect();

       while (!isConnected)
       {
           Debug.Log("Connecting to server...");
           yield return null;
       }

       NetworkClient.Instance.SendDataToServer(new Net_AuthRequest {
           Username = _username,
           Password = _password,
       });

        Debug.Log("Connected to server.");
    }

    private void OnExitClicked() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // -------------------------
    // NETWORK CALLBACKS
    // -------------------------

    public void ShowLoginError(string message = "Invalid username or password") {
        userNameOrPasswordErrorText.SetActive(true);
        loginBtn.interactable = true;
        SetInputsInteractable(true);
        loginLoadingAnim.SetActive(false);

        TMP_Text txt = userNameOrPasswordErrorText.GetComponent<TMP_Text>();
        if (txt != null) {
            txt.text = message;
        } 
    }

    public void OnLoginSuccess() {
        userNameOrPasswordErrorText.SetActive(false);

        _password = string.Empty;
        passwordInput.text = string.Empty;

        gameObject.SetActive(false);
    }

    // -------------------------
    // CLEANUP
    // -------------------------

    private void OnDestroy() {
        loginBtn.onClick.RemoveListener(OnLoginClicked);
        exitBtn.onClick.RemoveListener(OnExitClicked);
        usernameInput.onValueChanged.RemoveListener(OnUsernameChanged);
        passwordInput.onValueChanged.RemoveListener(OnPasswordChanged);
    }
}
