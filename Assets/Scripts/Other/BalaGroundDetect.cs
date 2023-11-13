using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaGroundDetect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.tag == "Player" || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}
