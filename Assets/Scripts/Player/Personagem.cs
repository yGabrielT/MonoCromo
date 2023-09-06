using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using StarterAssets;
using UnityEngine.Animations.Rigging;
using DG.Tweening;

public class Personagem : MonoBehaviour
{   
    [Header("Atributos do Player")]
    public int playerVida;
    public int vidaAtual;
    public HealthBar barraDeVida;
    [Range(4f,7f)]
    public float velocidade = 4f;
    [Range(10f,30f)]
    public float sprintVelocidade = 10f;
    public float gravidade = -15f;
    
    private float AddSpeed = 4f;
    private bool isDouble = false;
    private float baseSpeed;

    

    public bool ativarControles = true;

    [SerializeField] private CinemachineVirtualCamera _cam;
    [SerializeField] private float normalSensivity = 1f;
    [SerializeField] private float aimSensivity = .5f;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private float ShakeForce = 0.1f;
    [SerializeField] private GameObject _crosshair;
    public RaycastHit raycastHit;
    public bool isLooking;
    public AudioSource audioMorte;
    [SerializeField] private float maxDistance = 10;
    
    private bool change = false;
    public Renderer materialObj;
    private float contador;
    private CinemachineImpulseSource impulseSource;
    private Animator _anim;
    private Equipamento _throwScript;
    private ThirdPersonController _controller;
    private StarterAssetsInputs _input;
    public Inimigo inimigo;

    [Header("VFX")]
    [SerializeField] private ParticleSystem SmokeWalkTrail;
    [SerializeField] private ParticleSystem SmokeSprintTrail;
    [SerializeField] private GameObject SmokeJump;
    [SerializeField] private GameObject SmokeFall;
    [Range(0f, 2f)]
    [SerializeField] private float tempoFormacaoPoeiraWalk;
    [Range(0f, 2f)]
    [SerializeField] private float tempoFormacaoPoeiraSprint;
    [SerializeField] private Transform vfxPos;
    private bool isInAir = false;
    private float contadorWalk;
    private float contadorSprint;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject weaponObj;

    private CharacterController _char;

    [Header("Crouch")]
    private bool isCrouch;
    [SerializeField] private float crouchSpeed;
    private float baseHeight;
    private Vector3 baseCenter;
    private float baseJump;
    public float crouchHeight = 0.85f;
    [SerializeField] private float smoothTime = 2f;

    [Header("Rig")]
    [SerializeField] Rig rigger;


    // Start is called before the first frame update
    private void Awake()
    {
        
        impulseSource = GetComponent<CinemachineImpulseSource>();
        _anim = GetComponent<Animator>();
        _controller = GetComponent<ThirdPersonController>();
        _input = GetComponent<StarterAssetsInputs>();
        _throwScript = GetComponent<Equipamento>();
        _char = GetComponent<CharacterController>(); 
    }

    void Start()
    {
        baseHeight = _char.height;
        baseCenter = _char.center;
        baseJump = _controller.JumpHeight;
        // afirmando que a vida atual eh 100%
        vidaAtual = playerVida;
        // Chamando script para que a barra de vida fique no total
        barraDeVida.vidaMaxima(playerVida);
        baseSpeed = _controller.MoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
        InteractInput();
        Controlar();
    }

    public void Controlar()
    {
        if(ativarControles)
        {
            DoubleSpeed();
            Crouch();
            _controller.enabled = true;
            SmokeUpdate();
            ControlarCamera();
            _controller.MoveSpeed = velocidade;
            _controller.SprintSpeed = sprintVelocidade;
            _controller.Gravity = gravidade;
        }
        else
        {
            _controller.enabled = false;
        }
        
    }

    private void PerderVida()
    {

        if(vidaAtual >= 0)
        {
            //levar dano
            vidaAtual -= inimigo.dano;
            impulseSource.GenerateImpulse(ShakeForce);
            // atualiza a barra de vida
            barraDeVida.atualizarVida(vidaAtual);
        }
        else
        {
            //morte
            audioMorte.Play();
            vidaAtual = 0;
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "InProj")
        {
            Destroy(other.gameObject);
            PerderVida();
        }
        
    }

    private void InteractInput()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * maxDistance, Color.green);
            if(hit.transform.gameObject.tag == "Interactable")
            {
                text.SetActive(true);
                var obj = hit.transform.gameObject;
                if(_input.interact)
                {
                    _input.interact = false;
                    if (!change)
                    {
                        text.SetActive(false);
                        change = true;
                        materialObj = obj.GetComponent<Renderer>();
                        materialObj.material.color = Color.red;
                        Debug.Log("Interagido");
                    }
                    else{
                        text.SetActive(false);
                    }
                    Invoke("ColldownInteract", 0.1f);
                }
            }
            else{
                text.SetActive(false);
            }
            
        }
        
    }

    private void ColldownInteract()
    {
        change = false;
    }


    private void SmokeUpdate()
    {
        //Rastro para quando estiver andando
        contadorWalk += Time.deltaTime;
        if (SmokeWalkTrail != null && !_input.sprint && _controller._controller.isGrounded && _controller._controller.velocity != Vector3.zero) 
        {
            if (contadorWalk > tempoFormacaoPoeiraWalk)
            {
                SmokeWalkTrail.Play();
                contadorWalk = 0;
            }
                
        }
        //Rastro pra correr
        contadorSprint += Time.deltaTime;
        if (SmokeSprintTrail != null && _input.sprint && _controller._controller.isGrounded && _controller._controller.velocity != Vector3.zero)
        {
            if (contadorSprint > tempoFormacaoPoeiraSprint)
            {
                SmokeSprintTrail.Play();
                contadorSprint = 0;
            }
        }
        //Rastro pra pular
        if (SmokeJump != null && _input.jump && !_controller._controller.isGrounded && !isInAir)
        {
            isInAir = true;
            Instantiate(SmokeJump, vfxPos.position, Quaternion.identity);
        }
        if (SmokeFall != null && _controller._controller.isGrounded && isInAir && !_input.jump)
        {
            isInAir = false;
            Instantiate(SmokeFall, vfxPos.position, Quaternion.identity);
        }
    }

    private void ControlarCamera()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, _mask))
        {
            Debug.DrawRay(transform.position, Vector3.forward, Color.blue);
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

            //Riggar o bra�o do jogador
            DOVirtual.Float(1f, 0f, 1.5f, v => rigger.weight = v).SetEase(Ease.InElastic);


            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 15f);
            
        }
        else
        {
            _anim.SetLayerWeight(1, Mathf.Lerp(_anim.GetLayerWeight(1), 0, Time.deltaTime * 15f));

            //voltar o rig ao normal
            DOVirtual.Float(0f, 1f, 1.5f, v => rigger.weight = v).SetEase(Ease.InElastic);
            
            _crosshair.gameObject.SetActive(false);
            _cam.gameObject.SetActive(false);
            _controller.SetSensivity(normalSensivity);
            _controller.SetRotateOnMove(true);
        }

        if (_throwScript._isSecundario && _throwScript._municaoSecundaria != 0)
        {
            weaponObj.SetActive(false);
            if (_input.aim)
            {
                _anim.SetBool("isAiming", true);
                if (_input.shoot && _throwScript.prontoPraJogar)
                {
                    _anim.SetTrigger("Throw");
                }
                
            }
        }
        else
        {
            weaponObj.SetActive(true);
            _anim.SetBool("isAiming", false);
        }
    }

    private void Crouch()
    {
        if (_input.crouch && !isCrouch && !_input.sprint)
        {
            
            isCrouch = true;
            
            _char.height = Mathf.Lerp(_char.height, crouchHeight, smoothTime * Time.deltaTime);
            _char.center = new Vector3(0, Mathf.Lerp(_char.center.y, .45f, smoothTime * Time.deltaTime), 0f);
            velocidade = crouchSpeed;
            _anim.SetBool("Crouched", true);
            _controller.JumpHeight = 0f;
        }
        if (!_input.crouch && isCrouch && !_input.sprint)
        {
            isCrouch = false;
            _char.height = Mathf.Lerp(_char.height, baseHeight, smoothTime * Time.deltaTime);
            _char.center = new Vector3(0, Mathf.Lerp(_char.center.y, baseCenter.y, smoothTime * Time.deltaTime), 0f);
            velocidade = baseSpeed;
            _anim.SetBool("Crouched", false);
            _controller.JumpHeight = baseJump;
        }
        if (isCrouch)
        {
            _input.sprint = false;
        }
    }

    private void DoubleSpeed()
    {
        if (_input.slow && !isDouble && !isCrouch)
        {
            isDouble = true;
            velocidade = velocidade + AddSpeed;
        }
        else if (!_input.slow && !isCrouch)
        {
            isDouble = false;
            velocidade = baseSpeed;
        }
    }


}
