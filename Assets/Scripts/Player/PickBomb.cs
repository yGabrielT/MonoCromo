using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PickBomb : MonoBehaviour
{
    private Equipamento _throw;
    private void Start()
    {
        _throw = GetComponent<Equipamento>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bombs"))
        {
            
            Destroy(other.gameObject);
            _throw._municaoSecundaria++;
            _throw.prontoPraJogar = true;
            _throw._isSecundario = true;
            _throw.isPrincipal = false;
        }

        if (other.CompareTag("Bullet"))
        {
            
            Destroy(other.gameObject);
            _throw.municaoPrincipal += 5;
            _throw.prontoPraJogar = true;
            _throw._isSecundario = false;
            _throw.isPrincipal = true;
        }
    }
}
