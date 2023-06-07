using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroObj : MonoBehaviour
{
    public GameObject ExploHitFX;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != ("Inimigo") && other.gameObject.tag != ("Player") )
        {
            if (this.gameObject.tag == ("Bombs"))
            {
                Instantiate(ExploHitFX, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
            
        }

        if (other.gameObject.tag == ("Inimigo"))
        {
            if(this.gameObject.tag == ("Bombs"))
            {
                Instantiate(ExploHitFX, transform.position, Quaternion.identity);
            }
            
            other.GetComponent<Eliminar>().TomarDano(10);
            Destroy(gameObject);
            Debug.Log("Atingido");
            
        }
        
    }
}
