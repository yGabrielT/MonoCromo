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
        private AudioSource _audioAlav; 
        [SerializeField] Animator animator;

        private void Start(){
            _audioAlav = GetComponentInChildren<AudioSource>(); 
        }
        public void onAlavancaTrigger()
        {
            Debug.Log("Trigger Ativado");
            _audioAlav.Play();
            StartCoroutine(nameof(cooldownAlavanca));
        }

        private IEnumerator cooldownAlavanca(){
            animator.SetTrigger(Trigger);
            isReseted = true;
            Debug.Log("Trigger timer iniciado");
            yield return new WaitForSeconds(4.5f);
            isReseted = false;
            Debug.Log(gameObject.name + " foi resetado");

        }
    }

}

