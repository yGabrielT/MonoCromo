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
    [SerializeField] private LayerMask _mask;
    [SerializeField] private GameObject _crosshair;
    private Animator _anim;
    public RaycastHit raycastHit;
    public bool isLooking;

    private Throwing _throwScript;
    private ThirdPersonController _controller;
    private StarterAssetsInputs _input;
    // Start is called before the first frame update
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _controller = GetComponent<ThirdPersonController>();
        _input = GetComponent<StarterAssetsInputs>();
        _throwScript = GetComponent<Throwing>();
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
            _crosshair.gameObject.SetActive(true);
            _controller.SetSensivity(aimSensivity);
            _controller.SetRotateOnMove(false);
            _anim.SetLayerWeight(1, Mathf.Lerp(_anim.GetLayerWeight(1), 1, Time.deltaTime * 15f));

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 15f);
        }
        else
        {
            _anim.SetLayerWeight(1, Mathf.Lerp(_anim.GetLayerWeight(1), 0, Time.deltaTime * 15f));
            _crosshair.gameObject.SetActive(false);
            _cam.gameObject.SetActive(false);
            _controller.SetSensivity(normalSensivity);
            _controller.SetRotateOnMove(true);
        }
    }

    public void BoolThrow(bool isThrownable)
    {
        if(isThrownable)
        {
            _throwScript.enabled = true;
        }
        else
        {
            _throwScript.enabled = false;
        }
    }

}
