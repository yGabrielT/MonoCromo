using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using StarterAssets;
using UnityEngine.Animations.Rigging;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using UnityEngine.Events;
using other;

public class Personagem : MonoBehaviour
{
    [Header("Atributos do Player")]
    public int playerVida;
    public static int vidaAtual;
    public HealthBar barraDeVida;
    [Range(4f, 7f)]
    public float velocidade = 4f;
    [Range(10f, 30f)]
    public float sprintVelocidade = 10f;
    public float gravidade = -15f;

    public float AddSpeed = 4f;

    



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




    [Header("Interação")]
    [SerializeField] private float maxInteractDistance = 10;
    [SerializeField] private UnityEvent onInteract;


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
    public StarterAssetsInputs _input;
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

    private bool isInteracting;

    [Header("Elementos da Morte")]
    [SerializeField] private GameObject telaCaptura;
    [SerializeField] private Animator animacaoCaptura;

    [SerializeField] private bool isDead = false;

    private float sprintVelocidadebase;

    private Material[] _temporarysMaterial;
    [SerializeField]private Material _hitMaterial;

    [SerializeField] float _hitColorRefreshRate;
    private SkinnedMeshRenderer[] _skinnedMeshes;

    [SerializeField] int _timesToChangeColor;
    [SerializeField] float _invencibleFrames;

    private bool _isInvencible;
    


    // Start is called before the first frame update
    private void Awake()
    {
        _diag = GetComponent<DialogueManager>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        _anim = GetComponentInChildren<Animator>();
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
        sprintVelocidadebase = sprintVelocidade;
        _skinnedMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
        _temporarysMaterial = new Material[_skinnedMeshes.Length];
    }

    // Update is called once per frame
    void Update()
    {           
        if (isDead)
        {
            ativarControles = false;
            animacaoCaptura.Play("telaVermelha");
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        Controlar();
        
        
    }


    public void DesativarControles(){
        ativarControles = false;
    }
    public void RetivarControles(){
        ativarControles = true;
    }
    public void Controlar()
    {
        if (ativarControles)
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
            _anim.SetFloat("Speed", 0);
        }

    }

    private void PerderVida()
    {
        
        if (vidaAtual > 0 && !_isInvencible)
        {
            //levar dano
            vidaAtual -= inimigo.dano;
            impulseSource.GenerateImpulse(ShakeForce);
            // atualiza a barra de vida
            barraDeVida.atualizarVida(vidaAtual);
            StartCoroutine(nameof(OnHit));
        }
        if(vidaAtual <= 0)
        {
            //morte
            audioMorte.Play();
            telaCaptura.SetActive(true);
            vidaAtual = 0;
            isDead = true;
            ativarControles = false;
            _anim.SetTrigger("isDead");
        }
    }

    private IEnumerator OnHit(){
        _isInvencible = true;
        for(int f = 0; f < _timesToChangeColor; f++)
        {
            yield return new WaitForSecondsRealtime(_hitColorRefreshRate);
            for (int i = 0; i < _skinnedMeshes.Length; i++)
            {
                Debug.Log(_skinnedMeshes[i]);
                _temporarysMaterial[i] = _skinnedMeshes[i].material;
                _skinnedMeshes[i].material = _hitMaterial; 
                
                
            }
            yield return new WaitForSecondsRealtime(_hitColorRefreshRate);
            for (int i = 0; i < _skinnedMeshes.Length; i++)
            {
                _skinnedMeshes[i].material = _temporarysMaterial[i];
            }
        }
        yield return new WaitForSecondsRealtime(_invencibleFrames);
        _isInvencible = false;
    }
 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "InProj" && !isDead)
        {
            //Destroy(other.gameObject);
            PerderVida();
        }

        
        if (other.gameObject.tag == "Wind")
        {
            Debug.Log("Perdeu Vida com o vento");
            //Destroy(other.gameObject);
            PerderVida();
        }

    }

    

    private IEnumerator onInteractEnum(RaycastHit hit)
    {
        onInteract.Invoke();
        BoxCollider box = hit.transform.GetComponent<BoxCollider>();
        InterectableTrigger interact = hit.transform.GetComponent<InterectableTrigger>();
        Debug.Log(hit.transform.name + "Detectado no Enumerator");
        interact.onAlavancaTrigger();
        box.enabled = false;
        yield return new WaitForSeconds(4.5f);
        box.enabled = true;
    }

    private void InteractInput()
    {
        if (_input.interact)
        {

            _input.interact = false;
            isInteracting = true;


        }

        RaycastHit hit;







        if (Physics.Raycast(headRay.transform.position, Camera.main.transform.forward, out hit, maxInteractDistance))
        {
            Debug.DrawRay(headRay.transform.position, Camera.main.transform.forward * maxInteractDistance, Color.green);
            var obj = hit.transform.gameObject;
            if ((hit.transform.gameObject.tag == "Interactable") || (hit.transform.gameObject.tag == "NPC"))
            {

                if (hit.transform.gameObject.tag == "Interactable")
                {
                    textInteract.SetActive(true);

                    if (!change && isInteracting)
                    {
                        isInteracting = false;
                        textInteract.SetActive(false);
                        change = true;

                        StartCoroutine(onInteractEnum(hit));
                        Debug.Log("Interagido");
                    }
                    Invoke("ColldownInteract", 1f);
                }


                if (hit.transform.gameObject.tag == "NPC")
                {
                    textInteract.SetActive(true);
                    if (isInteracting)
                    {
                        isInteracting = false;
                        //Interagir com NPC
                        textInteract.SetActive(false);
                        SODialogue diagData = obj.GetComponent<DialogueHolder>().NpcDialogue;
                        Debug.Log(diagData);
                        Debug.Log(obj.transform);
                        _diag.Dialogue(diagData, obj.transform);

                    }
                }


            }
            else
            {
                isInteracting = false;
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

    private void CollInter()
    {
        isInteracting = false;
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
        if (ativarControles)
        {
            if (_input.aim && (_throwScript.isPrincipal || _throwScript._isSecundario))
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
                if(!_controller._isClimbing){
                    _controller.SetRotateOnMove(true);
                }
                
            }
        }
        if(_throwScript.isPrincipal){
            weaponObj.SetActive(true);
        }
        if (!_throwScript.isPrincipal && !_throwScript._isSecundario)
        {
            weaponObj.SetActive(false);
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
            velocidade *= AddSpeed;
            sprintVelocidade *= AddSpeed;
        }
        else if (!_input.slow && !isCrouch)
        {
            isDouble = false;
            velocidade = baseSpeed;
            sprintVelocidade = sprintVelocidadebase;
        
        }
    }


}
