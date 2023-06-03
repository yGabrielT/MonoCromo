using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroObj : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != ("Player") && other.gameObject.tag != ("Inimigo") && other.gameObject.tag != ("Bullet") && other.gameObject.tag != ("Bombs"))
        {
            Destroy(gameObject);
        }

        if (other.gameObject.tag == ("Inimigo"))
        {
            other.GetComponent<EliminarInimigo>().TomarDano(10);
            Debug.Log("Atingido");
        }
        
    }
}
