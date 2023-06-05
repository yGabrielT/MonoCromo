using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public float cool;
    [SerializeField]
    private Image image;
    private bool isCooldon = false;




    void Start()
    {
        image.fillAmount = 0;
    }



    // Update is called once per frame
    void Update()
    {
        imagemTempo();
    }
    private void imagemTempo()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isCooldon)
        {
            Debug.Log("FUNCIONA");
            isCooldon = true;
            image.fillAmount = 1;
        }



        if (isCooldon)
        {
            image.fillAmount -= 1 / cool * Time.deltaTime;



            if (image.fillAmount <= 0)
            {
                image.fillAmount = 0;
                isCooldon = false;
            }
        }
    }
}
