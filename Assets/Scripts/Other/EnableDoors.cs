using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDoors : MonoBehaviour
{
    public bool isReseted  = false;
    public bool isChanged = false;
    public Light TopLight;
    public Color LightAfterColor;
    public DoorCloseAndOpen Door1;
    public DoorCloseAndOpen Door2;
    public Animator _doorAnim;
    private int AlavancaCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isReseted && !isChanged)
        {
            isReseted = false;
            isChanged = true;
            _doorAnim.SetTrigger("CanOpen");
            

        }
        if(AlavancaCount == 2 && !isReseted){
            isReseted = true;
            Door1.hasBeenReseted = true;
            Door2.hasBeenReseted = true;
            TopLight.color = LightAfterColor;
        }
    }

    public void IncreaseCounter(){
        AlavancaCount++;
    }

    


}
