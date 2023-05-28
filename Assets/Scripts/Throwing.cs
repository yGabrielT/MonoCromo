using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;
using UnityEngine.Rendering.Universal.Internal;

public class Throwing : MonoBehaviour
{
    [Header("References")]
    private Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    private StarterAssetsInputs _input;
    private ThirdPersonManager _personManager;
    private RaycastHit hit;

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public float throwForce;
    public float throwUpwardForce;

    bool readyToThrow;

    private void Start()
    {
        cam = Camera.main.transform;
        _personManager = GetComponent<ThirdPersonManager>();
        _input = GetComponent<StarterAssetsInputs>();
        readyToThrow = true;
    }

    private void Update()
    {
        hit = _personManager.raycastHit;
        if (_input.shoot)
        {
            _input.shoot = false;
            if (readyToThrow && totalThrows >= 1)
            {

                Throw();
            }
        }
    }

    private void Throw()
    {
        
        readyToThrow = false;

        // instantiate object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calculate direction
        Vector3 forceDirection = cam.transform.forward;
        if (_personManager.isLooking)
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        // implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }


    

}