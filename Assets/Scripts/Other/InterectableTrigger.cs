using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace other
{
    public class InterectableTrigger : MonoBehaviour
    {
        public String Trigger;
        public bool isReseted; 
        [SerializeField] Animator animator;
        public void onAlavancaTrigger()
        {
            Debug.Log("Trigger Ativado");
            StartCoroutine(nameof(cooldownAlavanca));
        }

        private IEnumerator cooldownAlavanca(){
            animator.SetTrigger(Trigger);
            isReseted = true;
            Debug.Log("Trigger timer terminado");
            yield return new WaitForSeconds(4.5f);
            isReseted = false;
            Debug.Log(gameObject.name + " foi resetado");

        }
    }

}

