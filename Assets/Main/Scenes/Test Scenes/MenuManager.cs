using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField createInput;
    [SerializeField] TMP_InputField joinInput;

    [SerializeField] int lengthOfTimer = 480;
    public void CreateRoom()
    {
        Debug.Log("Connected to Master");
        RoomOptions roomOptions = new RoomOptions();
        Hashtable options = new Hashtable();
        options.Add("Time", lengthOfTimer);
        roomOptions.CustomRoomProperties = options;

        PhotonNetwork.CreateRoom(createInput.text, roomOptions);
    }
    public void JoinRoom()
    {
        Debug.Log("Joined");
        PhotonNetwork.JoinRoom(joinInput.text);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("TownLevel_Test");
    }
}

