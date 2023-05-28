using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class ThirdPersonManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cam;
    [SerializeField] private GameObject _crosshair;
    [SerializeField] private float normalSensivity = 1f;
    [SerializeField] private float aimSensivity = .5f;
    [SerializeField] private LayerMask _mask;
    public RaycastHit raycastHit;
    public bool isLooking;


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
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out raycastHit, 999f, _mask))
        {
            isLooking = true;
            mouseWorldPosition = raycastHit.point;
        }
        else
        {
            isLooking = false;
        }

        if (_input.aim)
        {
            _cam.gameObject.SetActive(true);
            _controller.SetSensivity(aimSensivity);
            _crosshair.SetActive(true);
            _controller.SetRotateOnMove(false);

            Vector3 worldAimTarget = mouseWorldPosition;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            _cam.gameObject.SetActive(false);
            _controller.SetSensivity(normalSensivity);
            _crosshair.SetActive(false);
            _controller.SetRotateOnMove(true);
        }
    }

  


}
