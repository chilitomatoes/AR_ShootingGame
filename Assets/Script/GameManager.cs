using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.InputSystem;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Game World")]
    public GameObject gameWorld;

    [Header("Players Setting")]
    public GameObject[] spawnPoints;


    [Header("UI Objects")]
    public GameObject joystickUI;
    public GameObject[] UIToDisable;
    public GameObject[] UIToEnable;
    public Text PlayerTxt;
    public Text P1ScoreTxt;
    public Text P2ScoreTxt;
    public GameObject EndGameMenu;
    public GameObject WaitingMenu;
    public GameObject Countdown;
    public Text vicPly;

    [Header("Animator")]
    public Animator countdown;

    private string p1Txt = "Plyaer 1 : ";
    private string p2Txt = "Plyaer 2 : ";
    private bool gameStarted = false;
    private bool gameEnded = false;
    private GameObject ply = null;
    private bool P1Ready = false;
    private bool P2Ready = false;
    private bool startflag = true;
    private PhotonView PV= null;
    // Start is called before the first frame update
    void Start()
    {      
        PV = GetComponent<PhotonView>();
        EndGameMenu.SetActive(false);

        if(PhotonNetwork.IsMasterClient)
        {
            PlayerTxt.text = "You are Player 1";
        }
        else
        {
            PlayerTxt.text = "You are Player 2";
            gameWorld.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        P1Ready = bool.Parse(PhotonNetwork.CurrentRoom.CustomProperties["P1READY"].ToString());
        P2Ready = bool.Parse(PhotonNetwork.CurrentRoom.CustomProperties["P2READY"].ToString());

        if(P1Ready && P2Ready && startflag)
        {
            SetupGame();
            StartCoroutine(StartGame());
            startflag= false;
        }

        if (!gameStarted)
        {
            return;
        }

        int P1Score = int.Parse(PhotonNetwork.CurrentRoom.CustomProperties["P1SCORE"].ToString());
        int P2Score = int.Parse(PhotonNetwork.CurrentRoom.CustomProperties["P2SCORE"].ToString());

        P1ScoreTxt.text = p1Txt + P1Score.ToString();
        P2ScoreTxt.text = p2Txt + P2Score.ToString();

        if(P1Score >= 10)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                ply.GetComponentInChildren<Player>().victory = true;
            }
            else
            {
                ply.GetComponentInChildren<Player>().lose = true;
            }         
            vicPly.text = "Player 1";
            gameEnded = true;
        }

        if (P2Score >= 10)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                ply.GetComponentInChildren<Player>().lose = true;
            }
            else
            {
                ply.GetComponentInChildren<Player>().victory = true;
            }                    
            vicPly.text = "Player 2";
            gameEnded = true;
        }

        if (gameEnded)
        {
            EndGameMenu.SetActive(true);
            UIToEnable[0].SetActive(false);
            UIToEnable[1].SetActive(false);
            ply.GetComponentInChildren<ThirdPersonController>().enabled = false;
            ply.GetComponentInChildren<Player>().isShooting = false;
        }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(6);
        gameStarted = true;
        EnableGameplayUI();
        Countdown.SetActive(false);
    }

    public void SetupGame()
    {
        WaitingMenu.SetActive(false);
        Countdown.SetActive(true);
        SpawnPlayer();
    }


    public void SpawnPlayer()
    {
        int index = 0;
        if(!PhotonNetwork.IsMasterClient)
        {
            index = 1;
        }
        string playerPrefab = "Player" + (index + 1);
        ply = PhotonNetwork.Instantiate(playerPrefab, spawnPoints[index].transform.position, spawnPoints[index].transform.rotation);

        joystickUI.GetComponent<UICanvasControllerInput>().starterAssetsInputs = ply.GetComponentInChildren<StarterAssetsInputs>();
        joystickUI.GetComponent<MobileDisableAutoSwitchControls>().playerInput = ply.GetComponentInChildren<PlayerInput>();

        StartCoroutine(SyncSpawn());

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "P1VIEWID", ply.GetComponentInChildren<PhotonView>().ViewID } });
        //    int P2ID = int.Parse(PhotonNetwork.CurrentRoom.CustomProperties["P2VIEWID"].ToString());
        //    GameObject tmp = PhotonNetwork.GetPhotonView(P2ID).gameObject;
        //    tmp.transform.position = spawnPoints[1].transform.position;
        //    tmp.transform.rotation = spawnPoints[1].transform.rotation;
        //}
        //else
        //{
        //    PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "P2VIEWID", ply.GetComponentInChildren<PhotonView>().ViewID } });
        //    int P1ID = int.Parse(PhotonNetwork.CurrentRoom.CustomProperties["P1VIEWID"].ToString());
        //    GameObject tmp = PhotonNetwork.GetPhotonView(P1ID).gameObject;
        //    tmp.transform.position = spawnPoints[0].transform.position;
        //    tmp.transform.rotation = spawnPoints[0].transform.rotation;
        //}
    }

    IEnumerator SyncSpawn()
    {
        yield return new WaitForSeconds(3);
        GameObject tmp = null;

        if (PhotonNetwork.IsMasterClient)
        {
            tmp = GameObject.Find("Player2(Clone)");
            tmp.transform.position = spawnPoints[1].transform.position;
            tmp.transform.rotation = spawnPoints[1].transform.rotation;
        }
        else
        {
            tmp = GameObject.Find("Player1(Clone)");
            tmp.transform.position = spawnPoints[0].transform.position;
            tmp.transform.rotation = spawnPoints[0].transform.rotation; 
        }
    }


    public void ReadyGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "P1READY", true } });
            Debug.Log("Player 1 Ready");
        }
        else
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "P2READY", true } });
            Debug.Log("Player 2 Ready");
        }
    }

    public void DisablePlaceUI()
    {
        foreach (GameObject go in UIToDisable)
        {
            go.SetActive(false);
        }
    }

    public void EnableGameplayUI()
    {
        foreach (GameObject go in UIToEnable)
        {
            go.SetActive(true);
        }
    }

    public void ShowWaitingMenu()
    {
        WaitingMenu.SetActive(true);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
