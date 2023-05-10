using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacterModel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int distance = 5;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + Camera.main.transform.forward * distance * Time.deltaTime;
    }
}
