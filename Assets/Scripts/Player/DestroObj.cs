using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroObj : MonoBehaviour
{
    public GameObject ExploHitFX;
    [SerializeField]private float raio = 10f;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != ("Inimigo") && other.gameObject.tag != ("Player") )
        {
            if (this.gameObject.tag == ("Bombs"))
            {
                Explode();
                Instantiate(ExploHitFX, transform.position, Quaternion.identity);

            }
            Destroy(this.gameObject);
            
        }

        if (other.gameObject.tag == ("Inimigo"))
        {
            if(this.gameObject.tag == ("Bombs"))
            {
                Explode();
                Instantiate(ExploHitFX, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
            else
            {
                other.GetComponent<Eliminar>().TomarDano(10);
                Destroy(gameObject);
                Debug.Log("Atingido");
            }
        }
        
    }

    private void Explode()
    {
        Collider[] colisores = Physics.OverlapSphere(transform.position, raio);
        Destroy(gameObject);
        foreach (Collider nearbyObjs in colisores)
        {
            if (nearbyObjs != null && nearbyObjs.gameObject.tag == ("Inimigo"))
            {
                nearbyObjs.GetComponent<Eliminar>().TomarDano(100);
                Debug.Log("Raio Atingido");
            }


        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, raio);
    }
}
