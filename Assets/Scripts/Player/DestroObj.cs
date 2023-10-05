using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroObj : MonoBehaviour
{
    public GameObject ExploHitFX;
    public GameObject HitImpactFX;
    [SerializeField]private float raio = 10f;
    void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.tag == ("Bombs"))
        {
            Explode();
        }

        if (this.gameObject.tag != ("Bombs"))
        {
            if(other.gameObject.tag == ("Inimigo"))
            {
                other.GetComponent<Inimigo>().TomarDano(10);
            }
            
            
            Instantiate(HitImpactFX, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            Debug.Log("Atingido");
        }
    }

    private void Explode()
    {
        
        Instantiate(ExploHitFX, this.transform.position, Quaternion.identity);
        Collider[] colisores = Physics.OverlapSphere(this.transform.position, raio);
        Destroy(this.gameObject);
        foreach (Collider nearbyObjs in colisores)
        {
            if (nearbyObjs != null && nearbyObjs.gameObject.tag == ("Inimigo") && nearbyObjs.gameObject.tag != ("Ground"))
            {
                nearbyObjs.GetComponent<Inimigo>().TomarDano(30);
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
