using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestructionScript : MonoBehaviour {

    public GameObject explosionReference;
    public GameObject player;
    public LayerMask allergicMask;

    // Use this for initialization
    void Explode()
    {
        Quaternion rotation = Quaternion.Euler(-90, 0, 0);
        var explode = Instantiate(explosionReference, player.transform.position, rotation);
        Destroy(explode, 10);

    }
    // Update is called once per frame
    void Update () {
        if (Physics.CheckSphere(player.transform.position, player.transform.lossyScale.x, allergicMask))
        {
           
            StartCoroutine(DestroyPlayer());

        }
    }
    //runs parallel to the main program
    IEnumerator DestroyPlayer()
    {

        Explode();
        player.SetActive(false);
        yield return new WaitForSeconds(1f);
        player.transform.position = new Vector3(3, 0.5f, 3);
        player.SetActive(true);


    }
}
