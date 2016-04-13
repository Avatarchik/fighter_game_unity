using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System;
using UnityEngine.UI;

public class GameController : Reciver {
    
    public GameObject character;
    public GameObject enemyCharacter;
    private GameObject enemy;
    private GameObject player;
    private float lastTime;
    private CameraFollow cameraFollow;

    // Use this for initialization
    void Start () {
        lastTime = Time.time;
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        session = PlayerPrefs.GetString(Login.PREF_LOGIN_SESSION);
        informationPanel = canvas.transform.FindChild("InformationPanel").gameObject;
        StartCoroutine(Connect());
        cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
    }

    public void SendData(string message) {
        Send(message);
    }

    public override void ConnectionResult(int result) {
        base.ConnectionResult(result);
        int connectionStatus = networkInterface.connectionStatus;
        if (connectionStatus == NetworkInterface.CONNECTION_SUCESSFULL) {
            SendData(DefaultMessages.GAME_MESSAGE + ";" + session + ";" + DefaultMessages.GAME_READY);
            informationPanel.SetActive(true);
            informationPanel.transform.FindChild("FrontPanel/MainText").GetComponent<Text>().text = "Waiting for players..";
            informationPanel.transform.FindChild("FrontPanel/ButtonOk").gameObject.SetActive(false);
        }
    }

    public override void OnRecive(string[] data) {
        if(int.Parse(data[0]) == DefaultMessages.GAME_MESSAGE) {
            if(int.Parse(data[1]) == DefaultMessages.GAME_SPAWN_ENEMY) {
                enemy = Instantiate(enemyCharacter).gameObject;
                enemy.transform.position = new Vector2(float.Parse(data[2]), float.Parse(data[3]));
            } else if(int.Parse(data[1]) == DefaultMessages.GAME_SPAWN_CHARACTER) {
                player = Instantiate(character).gameObject;
                player.transform.position = new Vector2(float.Parse(data[2]), float.Parse(data[3]));
                informationPanel.SetActive(false);
                cameraFollow.Initialize();
            } else if(int.Parse(data[1]) == DefaultMessages.GAME_POSITION) {
                enemy.GetComponent<EnemyController>().SetPosition(new Vector2(float.Parse(data[2]),float.Parse(data[3])));
            }
        }
    }
}
