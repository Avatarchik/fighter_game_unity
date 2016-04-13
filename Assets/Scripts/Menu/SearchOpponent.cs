using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SearchOpponent : MonoBehaviour {

    private GameObject searchOpponentPanel;
    private GameObject informationPanel;
    private MenuController menuController;
    private Canvas canvas;
    private string session;

    public void OnPlayClick() {
        searchOpponentPanel.SetActive(true);
    }

    public void On1v1Click() {
        menuController.SendData(DefaultMessages.SEARCH + ";" + session + ";" + DefaultMessages.SEARCH_1v1);
        
    }

    public void OnCancelClick() {
        menuController.SendData(DefaultMessages.SEARCH + ";" + session + ";" + DefaultMessages.SEARCH_CANCEL);
        informationPanel.SetActive(false);
    }

    public void OnBackClick() {
        searchOpponentPanel.SetActive(true);
    }

    

	// Use this for initialization
	void Start () {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        informationPanel = canvas.transform.FindChild("InformationPanel").gameObject;
        searchOpponentPanel = canvas.transform.FindChild("SearchOpponentPanel").gameObject;
        menuController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MenuController>();
        session = PlayerPrefs.GetString(Login.PREF_LOGIN_SESSION);
	}

    public void OnRecive(string[] data) {
        if(int.Parse(data[0]) == DefaultMessages.SEARCH) {
            if(int.Parse(data[1]) == DefaultMessages.SEARCH_COMMAND_ACCEPTED) {
                informationPanel.SetActive(true);
                informationPanel.transform.FindChild("FrontPanel/MainText").GetComponent<Text>().text = "Search...";
                informationPanel.transform.FindChild("FrontPanel/ButtonOk").GetComponent<Button>().onClick.AddListener(() => OnCancelClick());
                informationPanel.transform.FindChild("FrontPanel/ButtonOk/Text").GetComponent<Text>().text = "Cancel";
            } else if(int.Parse(data[1]) == DefaultMessages.GAME_READY) {
                Application.LoadLevel(data[2]);
            }
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
