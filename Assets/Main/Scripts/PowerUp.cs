using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    GameObject powerUpModel;
    private void Start()
    {
        powerUpModel = GetComponentInChildren<MeshRenderer>().gameObject;
    }


    public IEnumerator _PowerUpRespawn()
    {
        powerUpModel.SetActive(false);
        powerUpModel.GetComponentInParent<SphereCollider>().enabled = false;

        yield return new WaitForSeconds(3f);

        powerUpModel.SetActive(true);
        powerUpModel.GetComponentInParent<SphereCollider>().enabled = true;
        yield return null;
    }

    public IEnumerator _PowerUp(GameObject PlayerObj)
    {
        ThirdPersonControllerScript thirdPerson = PlayerObj.GetComponent<ThirdPersonControllerScript>();

        thirdPerson.speed += 4;
        yield return new WaitForSeconds(2);
        thirdPerson.speed -= 4;
        yield return null;

    }
}
