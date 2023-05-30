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
    private GameObject objectToThrow;
    private StarterAssetsInputs _input;
    private ThirdPersonManager _personManager;
    private RaycastHit hit;

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;
    public bool readyToThrow;

    [Header("PrincipalEquip")]
    public GameObject PrincipalThrowObj;
    public bool isEquip1;
    public float throwForceEquip1;
    public float throwUpwardForceEquip1;
    public float throwCooldown1;
    public int totalThrows1;

    [Header("SecundaryEquip")]
    public GameObject SecundaryThrowObj;
    public bool isEquip2;
    public float throwForceEquip2;
    public float throwUpwardForceEquip2;
    public float throwCooldown2;
    public int totalThrows2;

    private float throwForce;
    private float throwUpwardForce;

    

    private void Start()
    {
        cam = Camera.main.transform;
        _personManager = GetComponent<ThirdPersonManager>();
        _input = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {

        CheckEquips();

        hit = _personManager.raycastHit;
        if (_input.shoot)
        {
            _input.shoot = false;
            if (readyToThrow && totalThrows >= 1 && (isEquip1 || isEquip2))
            {

                Throw();
            }
        }
    }

    private void Throw()
    {
        
        readyToThrow = false;

        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, Quaternion.Euler(90f, 0f, 0f));

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = cam.transform.forward;
        if (_personManager.isLooking)
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    private void CheckEquips()
    {
        if (isEquip1 && !isEquip2)
        {
            objectToThrow = PrincipalThrowObj;
            throwCooldown = throwCooldown1; 
            throwForce = throwForceEquip1;
            throwUpwardForce = throwUpwardForceEquip1;
            totalThrows = totalThrows1;
        }
        else if (!isEquip1 && isEquip2)
        {
            throwCooldown = throwCooldown2;
            objectToThrow = SecundaryThrowObj;
            throwForce = throwForceEquip2;
            throwUpwardForce = throwUpwardForceEquip2;
            totalThrows = totalThrows2;
        }
        if (!isEquip1 && !isEquip2)
        {
            readyToThrow = false;
            totalThrows = 0;
        }
    }
    

}