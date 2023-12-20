using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private SceneLoader sceneLoader = null;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
    }

    private void Update()
    {
        if(PhotonNetwork.IsConnectedAndReady)
        {
            GameObject.Find("Play Button").GetComponent<Button>().interactable = true;
            GameObject.Find("ConnectingTxt").GetComponent<Text>().enabled = false;
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to" + PhotonNetwork.CloudRegion);
        PhotonNetwork.AutomaticallySyncScene = true;   
    }

    public void SearchMatch()
    {
        Debug.Log("Searching for match");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Could not find room - Creating a room");
        MakeRoom();
    }

    void MakeRoom()
    {
        int randomRoomName = Random.Range(0, 5000);
        RoomOptions roomOptions= 
            new RoomOptions() { 
                IsVisible= true,
                IsOpen= true,
                MaxPlayers= 2,
            };

        //Room Properties
        Hashtable RoomCustomProps = new Hashtable();
        RoomCustomProps.Add("P1READY", false);
        RoomCustomProps.Add("P1SCORE", 0);
        RoomCustomProps.Add("P1VIEWID", 0);

        RoomCustomProps.Add("P2READY", false);
        RoomCustomProps.Add("P2SCORE", 0);
        RoomCustomProps.Add("P2VIEWID", 0);
        roomOptions.CustomRoomProperties = RoomCustomProps;


        PhotonNetwork.CreateRoom("RoomName_" + randomRoomName, roomOptions);
        Debug.Log("Room Created, Waiting for Another Player");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            
            GameObject.Find("StopSearchBtn").GetComponent<Button>().interactable = false;
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount + "/2 Starting Game");
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            //Start Game
            sceneLoader.LoadScene(1);
        }
    }

    public void StopSearch()
    {
        PhotonNetwork.LeaveRoom();
        Debug.Log("Stopped, Back to Menu");
    }
}
