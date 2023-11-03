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
                other.TryGetComponent<Inimigo>(out Inimigo _inim);
                if(_inim != null){
                    _inim.TomarDano(10);
                }
                else{
                    Boss _boss = other.GetComponentInParent<Boss>();
                    if(_boss !=null){
                        _boss.TomarDano(2);
                    }
                }
                
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
            if(nearbyObjs != null && nearbyObjs.gameObject.tag == ("Botao"))
            {
                nearbyObjs.GetComponent<EnableDoors>().isReseted = true;
                Debug.Log("Portas Desbloqueadas");
            }

        }
         

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, raio);
    }
}
