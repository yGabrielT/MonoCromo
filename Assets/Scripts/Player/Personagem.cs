using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using StarterAssets;

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

    public bool ativarControles = true;

    [SerializeField] private CinemachineVirtualCamera _cam;
    [SerializeField] private float normalSensivity = 1f;
    [SerializeField] private float aimSensivity = .5f;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private GameObject _crosshair;
    [SerializeField] private float ShakeForce = 0.1f;
    public RaycastHit raycastHit;
    public bool isLooking;
    public AudioSource audioMorte;
<<<<<<< Updated upstream

    private bool change = false;
    public Renderer materialObj;
    private float contador;
=======
    
>>>>>>> Stashed changes
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
    [SerializeField] private float tempoFormacaoPoeiraWalk;
    [SerializeField] private float tempoFormacaoPoeiraSprint;
    [SerializeField] private Transform vfxPos;
    private bool isInAir = false;
    private float contadorWalk;
    private float contadorSprint;
    // Start is called before the first frame update
    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        _anim = GetComponent<Animator>();
        _controller = GetComponent<ThirdPersonController>();
        _input = GetComponent<StarterAssetsInputs>();
        _throwScript = GetComponent<Equipamento>();
    }

    void Start()
    {
        // afirmando que a vida atual eh 100%
        vidaAtual = playerVida;
        // Chamando script para que a barra de vida fique no total
        barraDeVida.vidaMaxima(playerVida);
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            
            InteractInput(other.gameObject);
            
        }
    }

    private void InteractInput(GameObject obj)
    {
        if (_input.interact && !change)
        {
            
            change = true;
            _input.interact = false;

            materialObj = obj.GetComponent<Renderer>();
            materialObj.material.color = Color.red;
            Debug.Log("Interagido");
        }
        Invoke("ColldownInteract", 0.1f);
    }

    private void ColldownInteract()
    {
        change = false;
    }


    private void SmokeUpdate()
    {
        //Rastro para quando estiver andando
        contadorWalk += Time.deltaTime;
        if(SmokeWalkTrail != null && !_input.sprint && _controller._controller.isGrounded && _controller._controller.velocity != Vector3.zero)
        {
            if(contadorWalk > tempoFormacaoPoeiraWalk)
            {
                SmokeWalkTrail.Play();
                contadorWalk = 0;
            }
        }

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
        if(SmokeFall != null && _controller._controller.isGrounded && isInAir && !_input.jump)
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

    
    
}
