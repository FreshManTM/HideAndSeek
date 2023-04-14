using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Egg : MonoBehaviour
{
    [SerializeField] float timeToRespawn = 45f;
    GameObject model;

    int coroutineCount = 0;

    private void Start()
    {
        model = GetComponentInChildren<MeshRenderer>().gameObject;
        StartCoroutine(_EggsRemove());
    }

    private void OnCollisionEnter(Collision other)
    {
        //Stops falling down when hits the ground/object
        if(other.gameObject.tag == "Untagged")
        {
            this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<MeshCollider>().isTrigger = true;
        }
    }

    //Starting Coroutine that respawns egg
    public void EggPicked()
    {
         StartCoroutine(_EggRespawn());
    }

    //Respawning egg after being picked
    IEnumerator _EggRespawn()
    {
        model.SetActive(false);
        GetComponent<MeshCollider>().enabled = false;

        yield return new WaitForSeconds(timeToRespawn);

        model.SetActive(true);
        GetComponent<MeshCollider>().enabled = true;
    }

    //Checking if egg is under the map and destroying it
    IEnumerator _EggsRemove()
    {
        yield return new WaitForSeconds(.5f);
        coroutineCount++;
        if(coroutineCount <= 7)
        { 
            if (transform.position.y <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(_EggsRemove());
            }
        }
        else
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.SceneLoaded();
            yield return null;
        }
    }
}
