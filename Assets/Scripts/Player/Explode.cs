using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    private float raio = 10f;
    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
