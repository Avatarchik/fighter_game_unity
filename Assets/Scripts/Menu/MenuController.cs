using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuController : Reciver {

    private GameObject searchOpponentPanel;

    public void OnLogoutClick() {
        Application.LoadLevel("Login");
    }


    // Use this for initialization
    void Start () {
        searchOpponentPanel = canvas.transform.FindChild("SearchOpponentPanel").gameObject;
        StartCoroutine(Connect());
    }

    

    public void SendData(string message) {
        Send(message);
    }
    

    // Update is called once per frame
    void Update () {
	
	}

    public void OnPlayClick() {
        searchOpponentPanel.SetActive(true);
    }

    public override void OnRecive(string[] data) {
        base.OnRecive(data);
        if (int.Parse(data[0]) == DefaultMessages.SEARCH) {
            searchOpponentPanel.GetComponent<SearchOpponent>().OnRecive(data);
        }
    }
}
