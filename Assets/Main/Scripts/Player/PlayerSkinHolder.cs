using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerSkinHolder : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject currentSkin;
    [SerializeField] GameObject seekerSkin;
    [SerializeField] GameObject[] allSkins;

    GameManager gameManager;
    Network network;

    Score scoreScript;

    //Delete Find() and remove if not needed
    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        gameManager = FindObjectOfType<GameManager>();
        network = FindObjectOfType<Network>();
        scoreScript = FindObjectOfType<Score>();

        network.AllPlayers.Add(gameObject);
    }

    public void SetSeekerSkin()
    {
        gameObject.AddComponent<Seeker>();
        GetComponent<ThirdPersonControllerScript>().speed += GetComponent<ThirdPersonControllerScript>().speed * .1f;

        currentSkin.SetActive(false);
        seekerSkin.SetActive(true);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        for (int i = 0; i < network.AllPlayers.ToArray().Length; i++)
        {
            if (targetPlayer.ActorNumber == i+1)
            {
                if (changedProps.ContainsKey("isSeeker") && network.AllPlayers[i].GetComponent<Seeker>() == null)
                {

                    network.AllPlayers[i].GetComponent<PlayerSkinHolder>().SetSeekerSkin();

                    scoreScript.RemoveScoreBoardItem(targetPlayer);

                    if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("seekersAmount", out object seekersAmount))
                    {
                        if ((int)seekersAmount >= PhotonNetwork.PlayerList.Length)
                        {
                            Debug.Log("Seekers won!");
                            gameManager.GameOver(false);
                        }
                    }
                    else
                    {
                        print("Not able to get seekersAmount value!");
                    }
                }
            }
        }                  
    }
}

