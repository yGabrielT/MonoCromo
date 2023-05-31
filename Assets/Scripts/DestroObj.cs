using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroObj : MonoBehaviour
{
    private bool Destroy;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != ("Player") && other.gameObject.tag != ("Bullet") && other.gameObject.tag != ("Bombs") && !Destroy)
        {
            Destroy = true;
            Destroy(gameObject);
        }
        
    }
}
