using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testaction : MonoBehaviour
{
    private InputManager inputManager;
    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.PlayerJumped())
        {
            Debug.Log("Player pulou");
        }
        

    }
}
