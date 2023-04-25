using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudarFov : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float targetFOV = 60f;
    public float transitionTime = 1f;
    public float dampTime = 0.15f;
    public float returnTime = 0.5f;
    private InputManager inputManager;
    private float currentFOV;
    private float originalFOV;
    private float fovVelocity = 0.0f;
    private bool isTransitioning = false;

    private void Start()
    {
        currentFOV = virtualCamera.m_Lens.FieldOfView;
        originalFOV = currentFOV;
        inputManager = InputManager.Instance;
    }

    private void Update()
    {
        if (inputManager.isSprinting && !isTransitioning)
        {
            currentFOV = targetFOV;
            isTransitioning = true;
        }

        if (isTransitioning)
        {
            virtualCamera.m_Lens.FieldOfView = Mathf.SmoothDamp(virtualCamera.m_Lens.FieldOfView, currentFOV, ref fovVelocity, dampTime, transitionTime);

            if (Mathf.Abs(virtualCamera.m_Lens.FieldOfView - currentFOV) < 0.01f)
            {
                isTransitioning = false;
            }
        }
        else if (!inputManager.isSprinting)
        {
            virtualCamera.m_Lens.FieldOfView = Mathf.SmoothDamp(virtualCamera.m_Lens.FieldOfView, originalFOV, ref fovVelocity, returnTime);
        }
        else
        {
            virtualCamera.m_Lens.FieldOfView = originalFOV;
        }
    }
}
