using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] PhotonView[] hiderPrefabs;
    [SerializeField] PhotonView seekerPrefab;

    [SerializeField] Transform hSpawnPoint;
    [SerializeField] Transform sSpawnPoint;

    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] Text gameOverText;

    Score scoreScript;
    [SerializeField] GameObject loadingCanvas;

    int seekerNumber = 1;

    [SerializeField]GameObject loadingCamera;

    public AudioClip turnToSeekerSound;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (PhotonNetwork.IsMasterClient)
        {
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                if(player.ActorNumber == seekerNumber)
                {
                    Hashtable hash = new Hashtable();
                    hash.Add("skin", 0);
                    player.SetCustomProperties(hash);
                }
            }

        }

    }

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();

        scoreScript = FindObjectOfType<Score>();

        Hashtable hash = new Hashtable();
        hash.Add("seekersAmount", 1);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);

        SetPlayersSkin();
        StartCoroutine(_SpawnPlayers(PhotonNetwork.LocalPlayer));
        foreach(Player player in PhotonNetwork.PlayerList)
            StartCoroutine(_CheckPlayerSkin(player));
    }

    IEnumerator _CheckPlayerSkin(Player newPlayer)
    {
        if (newPlayer.CustomProperties.TryGetValue("skin", out object skin))
        {
            if ((int)skin != 0)
            {
                print("Skin is " + skin);
                scoreScript.AddScoreboardItem(newPlayer);
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(.1f);
            StartCoroutine(_CheckPlayerSkin(newPlayer));
        }
    }
    //Setting player skin number
    void SetPlayersSkin()
    {
        Hashtable hash = new Hashtable();
        int playerSkinNumber = 0;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.IsLocal)
            {
                if (seekerNumber != player.ActorNumber)
                {
                    int random = Random.Range(1, 8);
                    playerSkinNumber = random;
                }
                else
                {
                    playerSkinNumber = 0;
                }

                hash.Add("skin", playerSkinNumber);
                player.SetCustomProperties(hash);
            }

        }
    }

    //Removing ScoreBoardItem when player leaves room
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(otherPlayer.CustomProperties.TryGetValue("skin", out object skin))
        {
            if ((int)skin != 0)
            {
                scoreScript.RemoveScoreBoardItem(otherPlayer);
            }              
        }     
    }

    //Spawning players prefab
    IEnumerator _SpawnPlayers(Player player)
    {
        if (player.CustomProperties.TryGetValue("skin", out object skin))
        {
            if ((int)skin != 0)
            {
                print("skin is " + (int)skin);
                Vector3 randomPosition = new Vector3(hSpawnPoint.position.x + Random.Range(-2.5f, 2.5f), hSpawnPoint.position.y, hSpawnPoint.position.z + Random.Range(-2.5f, 2.5f));
                PhotonNetwork.Instantiate(hiderPrefabs[(int)skin].name, randomPosition, Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate(seekerPrefab.name, sSpawnPoint.position, Quaternion.identity);
            }
        }
        else
        {
            yield return new WaitForSeconds(.1f);
            StartCoroutine(_SpawnPlayers(player));
        }
    }

    //Ending the game when game is over
    public void GameOver(bool hidersWon)
    {
        Time.timeScale = 0;
        if (hidersWon)
        {
            int maxValue = 0;
            string maxScorePlayerName = PhotonNetwork.PlayerList[0].NickName;
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.TryGetValue("score", out object score))
                {
                    if ((int)score >= maxValue)
                    {
                        maxValue = (int)score;
                        maxScorePlayerName = player.NickName;
                    }
                }
            }
            gameOverCanvas.SetActive(true);
            gameOverText.text = "Player " + maxScorePlayerName + " won the game with " + maxValue + " points.";
        }
        else
        {
            gameOverCanvas.SetActive(true);
            gameOverText.text = "Seekers Victory!!!";
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SceneLoaded()
    {
        GetComponent<GameManager>().enabled = true;
        GetComponent<Timer>().enabled = true;
        if(loadingCamera != null)
        {
            Destroy(loadingCamera);
        }
        loadingCanvas.SetActive(false);

    }

    public void BackToLobbyButton()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }

}
