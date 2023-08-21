using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;
using Cinemachine;


public class Equipamento : MonoBehaviour
{

    [Header("PrincipalEquip")]
    public GameObject tipoPrincipal;
    public bool isPrincipal;
    public float forcaPrincipal;
    public float forcaCimaPrincipal;
    public float tempoDeRecargaPrincipal;
    public int municaoPrincipal;
    [SerializeField] private float forceShake = 0.1f;
    public AudioSource audioPrincipal;


    [Header("SecundaryEquip")]
    public GameObject _tipoSecundario;
    public bool _isSecundario;
    public float _forcaSecundaria;
    public float _forcaCimaSecundaria;
    public float _tempoDeRecargaSecundaria;
    public int _municaoSecundaria;
    public AudioSource audioSecundario;

    [Header("Config")]
    [SerializeField] private int municao1Max;
    [SerializeField] private int municao2Max;
    public int municaoAtual;
    public float cooldownAtual;
    public bool prontoPraJogar;

    [Header("References")]
    private Transform cam;
    public Transform pontoAtaque;
    private GameObject objectToThrow;
    private StarterAssetsInputs _input;
    private Personagem _Personagem;
    private RaycastHit hit;
    public TMP_Text MunicaoText;
    public TMP_Text GranadaText;
    public Image armaPrincipal;
    public Image Granada1;
    public Image Granada2;
    public Image Granada3;

    private float forcaAtual;
    private float forcaCimaAtual;
    private CinemachineImpulseSource impulseSource;
    

    

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        cam = Camera.main.transform;
        _Personagem = GetComponent<Personagem>();
        _input = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        LimitarMunicao();
        AtualizarCanvasMunicao();
        CheckEquips();
        trocarArma();
        ValidarEntrada();
        hit = _Personagem.raycastHit;
    }

    private void ValidarEntrada()
    {
        if (_input.shoot)
        {
            _input.shoot = false;
            if (prontoPraJogar && municaoAtual >= 1 && (isPrincipal || _isSecundario) && _input.aim)
            {
                UsarEquipamento();
                if(isPrincipal)
                {
                    audioPrincipal.Play();
                    impulseSource.GenerateImpulse(forceShake);
                }
                else{
                    audioSecundario.Play();
                }
                
            }
        }
    }

    private void UsarEquipamento()
    {
        
        prontoPraJogar = false;

        GameObject projetil = Instantiate(objectToThrow, pontoAtaque.position, cam.rotation);

        Rigidbody projetilRb = projetil.GetComponent<Rigidbody>();

        Vector3 forcaDir = cam.transform.forward;
        if (_Personagem.isLooking)
        {
            forcaDir = (hit.point - pontoAtaque.position).normalized;
        }

        Vector3 forcaAdicional = forcaDir * forcaAtual + transform.up * forcaCimaAtual;

        projetilRb.AddForce(forcaAdicional, ForceMode.Impulse);
        if(isPrincipal && !_isSecundario)
        {
            municaoPrincipal--;
        }
        if(!isPrincipal && _isSecundario)
        {
            _municaoSecundaria--;
        }


        StartCoroutine(ResetThrow(cooldownAtual));
    }

    private IEnumerator ResetThrow(float esperaTempo)
    {
        yield return new WaitForSecondsRealtime(esperaTempo);
        prontoPraJogar = true;
    }
    private void LimitarMunicao()
    {
        if(municaoPrincipal > municao1Max)
        {
            municaoPrincipal = municao1Max;
        }
        if(_municaoSecundaria > municao2Max)
        {
            _municaoSecundaria = municao2Max;
        }
    }

    private void CheckEquips()
    {
        //Usando arma de tinta
        if (isPrincipal && !_isSecundario)
        {
            objectToThrow = tipoPrincipal;
            cooldownAtual = tempoDeRecargaPrincipal; 
            forcaAtual = forcaPrincipal;
            forcaCimaAtual = forcaCimaPrincipal;
            municaoAtual = municaoPrincipal;
        }
        //Usando Granada de tinta
        else if (!isPrincipal && _isSecundario)
        {
            cooldownAtual = _tempoDeRecargaSecundaria;
            objectToThrow = _tipoSecundario;
            forcaAtual = _forcaSecundaria;
            forcaCimaAtual = _forcaCimaSecundaria;
            municaoAtual = _municaoSecundaria;
        }
        //Sem equipamentos
        if (!isPrincipal && !_isSecundario)
        {
            prontoPraJogar = false;
            municaoAtual = 0;
        }
    }
    
    private void AtualizarCanvasMunicao()
    {
        if (isPrincipal && !_isSecundario)
        {
            MunicaoText.SetText(municaoPrincipal.ToString() + "/20");
            armaPrincipal.enabled = true;
            GranadaText.SetText("");
            Granada1.enabled = false;
            Granada2.enabled = false;
            Granada3.enabled = false;
        }
        else if (isPrincipal && municaoPrincipal == 0)
        {
            GranadaText.SetText("Não há munição!");
        }
        else if (_isSecundario && _municaoSecundaria == 1)
        {
            armaPrincipal.enabled = false;
            MunicaoText.SetText("");
            Granada1.enabled = true;
            Granada2.enabled = false;
            Granada3.enabled = false;
            GranadaText.SetText("");
        }
        else if (_isSecundario && _municaoSecundaria == 2)
        {
            armaPrincipal.enabled = false;
            MunicaoText.SetText("");
            Granada1.enabled = true;
            Granada2.enabled = true;
            Granada3.enabled = false;
            GranadaText.SetText("");
        }
        else if (_isSecundario && _municaoSecundaria == 3)
        {
            armaPrincipal.enabled = false;
            MunicaoText.SetText("");
            Granada1.enabled = true; 
            Granada2.enabled = true;
            Granada3.enabled = true;
            GranadaText.SetText("");
        }
        else if (_isSecundario && _municaoSecundaria == 0)
        {
            armaPrincipal.enabled = false;
            MunicaoText.SetText("");
            Granada1.enabled = false;
            Granada2.enabled = false;
            Granada3.enabled = false;
            GranadaText.SetText("N�o h� granadas!");
        }
        
        if (!isPrincipal && !_isSecundario)
        {
            MunicaoText.SetText("");
            GranadaText.SetText("");
            armaPrincipal.enabled = false;
            Granada1.enabled = false; 
            Granada2.enabled = false;
            Granada3.enabled = false;
        }

    }

    private void trocarArma()
    {
        if(_input.scroll > 0)
        {
            Debug.Log("Mouse Scroll cima");
            isPrincipal = true;
            _isSecundario = false;

        }
        else if (_input.scroll < 0)
        {
           Debug.Log("Mouse Scroll baixo");
            isPrincipal = false;
            _isSecundario = true;
        }
        

    }
}