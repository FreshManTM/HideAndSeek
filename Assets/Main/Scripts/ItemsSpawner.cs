using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemsSpawner : MonoBehaviour
{
    [SerializeField] GameObject Egg;
    [SerializeField] int eggsAmount;
    [SerializeField] float spawnHeight;
    [SerializeField] float rangeX;
    [SerializeField] float rangeY;
    private void Awake()
    {
        for (int i = 0; i < eggsAmount; i++)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(rangeX, rangeY), spawnHeight, Random.Range(rangeX, rangeY));
            PhotonNetwork.Instantiate(Egg.name, randomSpawnPosition, Quaternion.identity);
        }

    }
    
}
