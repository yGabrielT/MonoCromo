using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInvocations : MonoBehaviour
{
    private DialogueManager _diag;


    void Start(){
        _diag = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<DialogueManager>();
        
    }

    public void IniciarConversaAoColidir(SODialogue diag){
        
       
        _diag.Dialogue(diag,transform);
        Debug.Log("Colidiu");
        
    }
}
