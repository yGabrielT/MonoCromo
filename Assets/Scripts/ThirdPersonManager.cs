using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class ThirdPersonManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cam;
    [SerializeField] private float normalSensivity = 1f;
    [SerializeField] private float aimSensivity = .5f;

    private ThirdPersonController _controller;
    private StarterAssetsInputs _input;
    // Start is called before the first frame update
    private void Awake()
    { 
        _controller = GetComponent<ThirdPersonController>();
        _input = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.aim)
        {
            _cam.gameObject.SetActive(true);
            _controller.SetSensivity(aimSensivity);
        }
        else
        {
            _cam.gameObject.SetActive(false);
            _controller.SetSensivity(normalSensivity);
        }
    }
}
