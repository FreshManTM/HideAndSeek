using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class Seeker : MonoBehaviour
{
    [SerializeField] float hitDistance = 1.5f;


    RaycastHit hit;
    Vector3 rayStartPos;
    Ray ray;

    GameManager gameManager;
    PhotonView view;
    Network network;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        view = GetComponent<PhotonView>();
        network = FindObjectOfType<Network>();

    }
    void Update()
    {
        HitPlayer();      
    }

    //Making the Raycast that checks if the hit object it Player
    void HitPlayer()
    {
        //Counts Ray distance
        rayStartPos = new Vector3(transform.position.x, transform.position.y + 1.3f, transform.position.z);
        ray = new Ray(rayStartPos, transform.forward);
        Physics.Raycast(ray, out hit, hitDistance);

        //Hitting
        if (view.IsMine && Input.GetKeyDown(KeyCode.Mouse0) && hit.collider.tag == "Player" && hit.collider.gameObject.GetComponent<Seeker>() == null)
        {
            Debug.Log("Hit");
            TurnToSeeker(hit);
        }
        Debug.DrawRay(rayStartPos, transform.forward, Color.red);
    }

    void TurnToSeeker(RaycastHit hit)
    {
        AudioSource.PlayClipAtPoint(gameManager.turnToSeekerSound, transform.position, .5f);

        Hashtable hash = new Hashtable();
        int seekerAmount = 0;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("seekersAmount", out object seekerAm))
        {
            seekerAmount = (int)seekerAm + 1;
            hash.Add("seekersAmount", seekerAmount);
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }
        SetCustomPropForSeeker(hit);

        hit.collider.gameObject.GetComponent<PlayerSkinHolder>().SetSeekerSkin();

        if (seekerAmount >= PhotonNetwork.PlayerList.Length)
        {
            Debug.Log("Seekers won!");
            gameManager.GameOver(false);
        }
    }

    //Sets Custom Properties if the player has been turned to seeker
    void SetCustomPropForSeeker(RaycastHit hit)
    {
        for (int i = 0; i < network.AllPlayers.ToArray().Length; i++)
        {
            if (hit.collider.gameObject == network.AllPlayers[i])
            {
                foreach(Player player in PhotonNetwork.PlayerList)
                {
                    if(player.ActorNumber == i+1)
                    {
                        if (view.IsMine)
                        {
                            Hashtable hash = new Hashtable();

                            hash.Add("isSeeker", true);
                            player.SetCustomProperties(hash);

                            Score scoreScript = FindObjectOfType<Score>();
                            scoreScript.RemoveScoreBoardItem(player);
                        }
                    }
                }
            }
        } 
    }
}
