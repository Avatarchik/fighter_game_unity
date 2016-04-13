using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Reciver : MonoBehaviour {

    protected NetworkInterface networkInterface;
    protected Canvas canvas;
    protected GameObject informationPanel;
    protected string session;
    
    protected GameObject frontPanel;

    public virtual void OnRecive(string[] data) {

    }

    void Awake() {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        informationPanel = canvas.transform.FindChild("InformationPanel").gameObject;
        session = PlayerPrefs.GetString(Login.PREF_LOGIN_SESSION);
        
        informationPanel = canvas.transform.FindChild("InformationPanel").gameObject;
        frontPanel = informationPanel.transform.FindChild("FrontPanel").gameObject;
        networkInterface = GameObject.FindGameObjectWithTag("NetworkInterface").GetComponent<NetworkInterface>();
    }

	// Use this for initialization
	protected void Init () {
        networkInterface.Init();
    }

    public virtual void ConnectionResult(int res) {
        int connectionStatus = networkInterface.connectionStatus;
        if (connectionStatus == NetworkInterface.CONNECTION_SUCESSFULL) {
            informationPanel.SetActive(false);
            frontPanel.transform.FindChild("ButtonOk").gameObject.SetActive(true);
        } else if (connectionStatus == NetworkInterface.UNABLE_TO_CONNECT) {
            informationPanel.SetActive(true);
            frontPanel.transform.FindChild("MainText")
                .GetComponent<Text>().text = "Unable to connect to server!";
            frontPanel.transform.FindChild("ButtonOk")
                .FindChild("Text").GetComponent<Text>().text = "Retry";
            frontPanel.transform.FindChild("ButtonOk").gameObject.SetActive(true);
            frontPanel.transform.FindChild("ButtonOk")
                .GetComponent<Button>().onClick.RemoveAllListeners();
            frontPanel.transform.FindChild("ButtonOk")
                .GetComponent<Button>().onClick.AddListener(() => StartCoroutine(Connect()));
        }
    }

    protected IEnumerator Connect() {
        informationPanel.SetActive(true);
        informationPanel.transform.FindChild("FrontPanel/MainText").GetComponent<Text>().text = "Connecting..";
        informationPanel.transform.FindChild("FrontPanel/ButtonOk").gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Init();
    }

    protected void Send(string message) {
        networkInterface.Send(message);
    }
    

    // Update is called once per frame
    void Update () {
	
	}
}
