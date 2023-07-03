using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaGroundDetect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(this.gameObject);
        }
    }
}
