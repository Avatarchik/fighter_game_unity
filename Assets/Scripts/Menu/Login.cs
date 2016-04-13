using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Login : Reciver {
    
    public const string PREF_LOGIN_SESSION = "pref.login.session";
    private InputField usernameInput;
    private InputField passwordInput;
    private Button btnLogin;
    private Button btnEvent;
    private Text informationText;
    private Text btnEventText;


    // Use this for initialization
    void Start () {
        
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        passwordInput = canvas.transform.FindChild("PasswordInput").GetComponent<InputField>();
        usernameInput = canvas.transform.FindChild("UsernameInput").GetComponent<InputField>();
        btnLogin = canvas.transform.FindChild("LoginButton").GetComponent<Button>();
        btnLogin.onClick.AddListener(() => OnLoginClick());
        informationPanel = canvas.transform.FindChild("InformationPanel").gameObject;
        frontPanel = informationPanel.transform.FindChild("FrontPanel").gameObject;
        btnEvent = frontPanel.transform.FindChild("ButtonOk").GetComponent<Button>();
        btnEventText = btnEvent.transform.FindChild("Text").GetComponent<Text>();
        
        informationText = frontPanel.transform.FindChild("MainText").GetComponent<Text>();
        StartCoroutine(Connect());
    }

    private void OnLoginClick() {
        //Application.LoadLevel("Game");
        Send(DefaultMessages.LOGIN_UNAME_PASS + ";" + usernameInput.text + ";" + passwordInput.text);
    }

    public void OnFacebookLogin() {

    }

    public void OnGoogleLogin() {
        GooglePlayController.Init();
    }

    public override void OnRecive(string[] data) {
        if (int.Parse(data[0]).Equals(DefaultMessages.LOGIN)) {
            if (data.Length > 1) {
                if (data[1].Equals(DefaultMessages.LOGIN_FAILED + "")) {
                    informationPanel.SetActive(true);
                    informationText.text = "Bad username or password!";
                    btnEvent.onClick.RemoveAllListeners();
                    btnEvent.onClick.AddListener(() => HideInformationPanel());
                    btnEventText.text = "Ok";
                } else {
                    PlayerPrefs.SetString(PREF_LOGIN_SESSION, data[1]);
                    Application.LoadLevel("Menu");
                }
            } else {
                Debug.Log("Wrong Message!");
            }
        } else {
            Debug.Log("Wrong Message!");
        }
    }

    private void HideInformationPanel() {
        informationPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update () {

	}


}
