using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TerceiroAndarAlavs : MonoBehaviour
{
    private int numberOfInter = 0;
    private bool canInteract = false;
    public int maxNumsTillUnlock = 3;
    public BoxCollider _collidToUnlock;
    public GameObject DoorOpen;

    public UnityEvent OnUnlock;
    
    public void Start(){

    }

    public void IncreaseInter(){
        numberOfInter++;
    }

    public void Update(){
        if(numberOfInter == maxNumsTillUnlock && !canInteract){
            
            canInteract = true;
            _collidToUnlock.enabled = true;
            
        }
        if(numberOfInter == maxNumsTillUnlock + 1 && canInteract){
            DoorOpen.SetActive(false);
            OnUnlock.Invoke();
        }
    }

    
}
