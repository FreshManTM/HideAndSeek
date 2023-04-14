using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreBoardItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI usernameText;
    [SerializeField] TextMeshProUGUI scoreText;

    Player player;

    //Initializing the player, his text, and score
    public void Initialize(Player player)
    {
        usernameText.text = player.NickName;
        this.player = player;
        UpdateStats();
    }
    
    //Updating player's score
    void UpdateStats()
    {
        if(player.CustomProperties.TryGetValue("score", out object score))
        {
            scoreText.text = score.ToString();
        }
    }

    //Updating player's score when player's Custom Properies has been changed
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(targetPlayer == player)
        {
            if (changedProps.ContainsKey("score"))
            {
                UpdateStats();
            }
        }
    }

}
