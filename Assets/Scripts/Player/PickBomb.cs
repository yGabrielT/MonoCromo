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
            _throw.totalThrows2++;
            _throw.readyToThrow = true;
            _throw.isEquip2 = true;
            _throw.isEquip1 = false;
        }

        if (other.CompareTag("Bullet"))
        {
            
            Destroy(other.gameObject);
            _throw.totalThrows1 += 5;
            _throw.readyToThrow = true;
            _throw.isEquip2 = false;
            _throw.isEquip1 = true;
        }
    }
}
