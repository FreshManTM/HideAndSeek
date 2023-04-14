using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;

    [SerializeField] int timer = 0;
    bool count;

    Hashtable setTime = new Hashtable();
    GameManager gameManager;

    private void Start()
    {
        count = true;
        gameManager = GetComponent<GameManager>();
    }
    private void Update()
    {
        timer = (int)PhotonNetwork.CurrentRoom.CustomProperties["Time"];
        float minutes = Mathf.FloorToInt((int)PhotonNetwork.CurrentRoom.CustomProperties["Time"] / 60);
        float seconds = Mathf.FloorToInt((int)PhotonNetwork.CurrentRoom.CustomProperties["Time"] % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes,seconds);
        if(timer == 0)
        {
            timerText.text = "∞";
        }
        else
        {
            if (timer > 0)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    if (count)
                    {
                        count = false;
                        StartCoroutine(_Timer());
                    }
                }
            }
            else
            {
                gameManager.GameOver(true);
            }
        }
        


    }

    IEnumerator _Timer()
    {
        yield return new WaitForSeconds(1);
        int nextTime = timer -= 1;
        setTime["Time"] = nextTime;
        PhotonNetwork.CurrentRoom.SetCustomProperties(setTime);
        count = true;
    }
}
