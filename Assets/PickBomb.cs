using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PickBomb : MonoBehaviour
{
    private Throwing _throw;

    private void Start()
    {
        _throw = GetComponent<Throwing>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Bombs"))
        {
            Destroy(other.gameObject);
            _throw.totalThrows++;
            
        }
    }
}
