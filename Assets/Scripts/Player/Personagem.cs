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
    

    

    public bool ativarControles = true;

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera _cam;
    
    [SerializeField] private Transform headRay;
    [SerializeField] private float normalSensivity = 1f;
    [SerializeField] private float aimSensivity = .5f;
    [SerializeField] private float ShakeForce = 0.1f;
    [SerializeField] private GameObject _crosshair;
    [SerializeField] private LayerMask _mask;
    public RaycastHit raycastHit;
    public bool isLooking;


    public AudioSource audioMorte;
    [SerializeField] private float maxInteractDistance = 10;
    
    
    [HideInInspector] public Renderer materialObj;
    
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
    
    
    [SerializeField] private GameObject textInteract;
    [SerializeField] private GameObject weaponObj;

    [Header("Crouch")]
    [SerializeField] private float crouchSpeed;
    public float crouchHeight = 0.85f;
    [SerializeField] private float smoothTime = 2f;

    [Header("Rig")]
    [SerializeField] TwoBoneIKConstraint WeaponRigger;


    private bool isCrouch;
    private float contador;
    private CinemachineImpulseSource impulseSource;
    private Animator _anim;
    private Equipamento _throwScript;
    private ThirdPersonController _controller;
    private StarterAssetsInputs _input;
    private float baseHeight;
    private Vector3 baseCenter;
    private float baseJump;
    private CharacterController _char;
    private float contadorWalk;
    private float contadorSprint;
    private bool isInAir = false;
    private bool change = false;
    private bool isDouble = false;
    private float baseSpeed;
    private DialogueManager _diag;


    // Start is called before the first frame update
    private void Awake()
    {
        _diag = GetComponent<DialogueManager>();
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
        
        
        Controlar();
    }

    public void Controlar()
    {
        if(ativarControles)
        {
            InteractInput();
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
            WeaponRigger.weight = 0;
            _controller.enabled = false;
            _anim.SetFloat("Speed",0);
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
        if (Physics.Raycast(headRay.transform.position, Camera.main.transform.forward, out hit, maxInteractDistance))
        {
            Debug.DrawRay(headRay.transform.position, Camera.main.transform.forward * maxInteractDistance, Color.green);
            if(hit.transform.gameObject.tag == "Interactable")
            {
                textInteract.SetActive(true);
                var obj = hit.transform.gameObject;
                if(_input.interact)
                {
                    _input.interact = false;
                    if (!change)
                    {
                        textInteract.SetActive(false);
                        change = true;
                        materialObj = obj.GetComponent<Renderer>();
                        materialObj.material.color = Color.red;
                        Debug.Log("Interagido");
                    }
                    else{
                        textInteract.SetActive(false);
                    }
                    Invoke("ColldownInteract", 0.1f);
                }
            }
            else{
                textInteract.SetActive(false);
            }

            if (hit.transform.gameObject.tag =="NPC"){
                textInteract.SetActive(true);
                var obj = hit.transform.gameObject;
                if (_input.interact){
                    _input.interact = false;
                    //Interagir com NPC
                    textInteract.SetActive(false);
                    SODialogue diagData = obj.GetComponent<DialogueHolder>().NpcDialogue;
                    Debug.Log(diagData);
                    Debug.Log(obj.transform);
                    _diag.Dialogue(diagData, obj.transform);
                }
                
                

            }
            else{
                textInteract.SetActive(false);
            }
            
        }
        else
        {
            textInteract.SetActive(false);
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
        if(ativarControles){
            if (_input.aim)
            {
                _cam.gameObject.SetActive(true);
                _crosshair.gameObject.SetActive(true);
                _controller.SetSensivity(aimSensivity);
                _controller.SetRotateOnMove(false);
                _anim.SetLayerWeight(1, Mathf.Lerp(_anim.GetLayerWeight(1), 1, Time.deltaTime * 15f));

                //Riggar o bra�o do jogador
                WeaponRigger.weight = Mathf.Lerp(WeaponRigger.weight, 1f, Time.deltaTime * 20);
                


                Vector3 worldAimTarget = mouseWorldPosition;
                worldAimTarget.y = transform.position.y;
                Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
                
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 15f);
                
            }
            else
            {
                _anim.SetLayerWeight(1, Mathf.Lerp(_anim.GetLayerWeight(1), 0, Time.deltaTime * 15f));

                //voltar o rig ao normal
                WeaponRigger.weight = Mathf.Lerp(WeaponRigger.weight, 0f, Time.deltaTime * 20);
                
                _crosshair.gameObject.SetActive(false);
                _cam.gameObject.SetActive(false);
                _controller.SetSensivity(normalSensivity);
                _controller.SetRotateOnMove(true);
            }
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
