using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class ItemsPickUp : MonoBehaviour
{
    string eggTag = "Egg";
    string powerUpTag = "PowerUp";

    int score = 0;
    PhotonView view;
    GameObject powerUpModel;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(GetComponent<Seeker>() == null)
        {
            if (other.gameObject.tag == eggTag)
            {
                other.GetComponent<AudioSource>().Play();

                AddScore();
                other.GetComponent<Egg>().EggPicked();
            }
            if (other.gameObject.tag == powerUpTag)
            {
                StartCoroutine(other.GetComponent<PowerUp>()._PowerUpRespawn());

                other.GetComponent<AudioSource>().Play();

                StartCoroutine(other.GetComponent<PowerUp>()._PowerUp(gameObject));
            }
        }
    }

    //Below are the methods for adding score and syncing it in multiplayer
    public void AddScore()
    {
        if (view.IsMine)
        {
            view.RPC(nameof(RPC_AddScore), view.Owner);
            print("AddScore");
        }

    }
    [PunRPC]
    void RPC_AddScore()
    {
        score++;

        Hashtable hash = new Hashtable();
        hash.Add("score", score);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

}
