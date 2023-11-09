using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTrigger : MonoBehaviour
{
    public UnityEvent onTrigger;
    
    void OnTriggerEnter(Collider other) {
        onTrigger.Invoke();
    }    


}
