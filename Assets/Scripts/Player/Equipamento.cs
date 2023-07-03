using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;
using Cinemachine;


public class Equipamento : MonoBehaviour
{
    [Header("References")]
    private Transform cam;
    public Transform attackPoint;
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



    [Header("Config")]
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
    public AudioSource audioPistol;
    [SerializeField] private float ShakeForce = 0.1f;


    [Header("SecundaryEquip")]
    public GameObject SecundaryThrowObj;
    public bool isEquip2;
    public float throwForceEquip2;
    public float throwUpwardForceEquip2;
    public float throwCooldown2;
    public int totalThrows2;
   // public AudioSource audioGranada;

    private float throwForce;
    private float throwUpwardForce;
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

        hit = _Personagem.raycastHit;
        if (_input.shoot)
        {
            _input.shoot = false;
            if (readyToThrow && totalThrows >= 1 && (isEquip1 || isEquip2) && _input.aim)
            {
                Throw();
                if(isEquip1)
                
                AudioPistol();
                impulseSource.GenerateImpulse(ShakeForce);
              //  else
              //  AudioGranada();
            }
        }
    }


    private void AudioPistol()
    {
        audioPistol.Play();
    }

    /*
    private void AudioGranada()
    {
        audioGranada.Play();
    }
    */

    private void Throw()
    {
        
        readyToThrow = false;

        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = cam.transform.forward;
        if (_Personagem.isLooking)
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
        if(isEquip1 && !isEquip2)
        {
            totalThrows1--;
        }
        if(!isEquip1 && isEquip2)
        {
            totalThrows2--;
        }


        StartCoroutine(ResetThrow(throwCooldown));
    }

    private IEnumerator ResetThrow(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        readyToThrow = true;
    }
    private void LimitarMunicao()
    {
        if(totalThrows1 > 20)
        {
            totalThrows1 = 20;
        }
        if(totalThrows2 > 3)
        {
            totalThrows2 = 3;
        }
    }

    private void CheckEquips()
    {
        //Usando arma de tinta
        if (isEquip1 && !isEquip2)
        {
            objectToThrow = PrincipalThrowObj;
            throwCooldown = throwCooldown1; 
            throwForce = throwForceEquip1;
            throwUpwardForce = throwUpwardForceEquip1;
            totalThrows = totalThrows1;
        }
        //Usando Granada de tinta
        else if (!isEquip1 && isEquip2)
        {
            throwCooldown = throwCooldown2;
            objectToThrow = SecundaryThrowObj;
            throwForce = throwForceEquip2;
            throwUpwardForce = throwUpwardForceEquip2;
            totalThrows = totalThrows2;
        }
        //Sem equipamentos
        if (!isEquip1 && !isEquip2)
        {
            readyToThrow = false;
            totalThrows = 0;
        }
    }
    
    private void AtualizarCanvasMunicao()
    {
        if (isEquip1 && !isEquip2)
        {
            MunicaoText.SetText(totalThrows1.ToString() + "/20");
            armaPrincipal.enabled = true;
            GranadaText.SetText("");
            Granada1.enabled = false;
            Granada2.enabled = false;
            Granada3.enabled = false;
        }
        else if (isEquip1 && totalThrows1 == 0)
        {
            GranadaText.SetText("Não há munição!");
        }
        else if (isEquip2 && totalThrows2 == 1)
        {
            armaPrincipal.enabled = false;
            MunicaoText.SetText("");
            Granada1.enabled = true;
            Granada2.enabled = false;
            Granada3.enabled = false;
            GranadaText.SetText("");
        }
        else if (isEquip2 && totalThrows2 == 2)
        {
            armaPrincipal.enabled = false;
            MunicaoText.SetText("");
            Granada1.enabled = true;
            Granada2.enabled = true;
            Granada3.enabled = false;
            GranadaText.SetText("");
        }
        else if (isEquip2 && totalThrows2 == 3)
        {
            armaPrincipal.enabled = false;
            MunicaoText.SetText("");
            Granada1.enabled = true; 
            Granada2.enabled = true;
            Granada3.enabled = true;
            GranadaText.SetText("");
        }
        else if (isEquip2 && totalThrows2 == 0)
        {
            armaPrincipal.enabled = false;
            MunicaoText.SetText("");
            Granada1.enabled = false;
            Granada2.enabled = false;
            Granada3.enabled = false;
            GranadaText.SetText("N�o h� granadas!");
        }
        
        if (!isEquip1 && !isEquip2)
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
            isEquip1 = true;
            isEquip2 = false;

        }
        else if (_input.scroll < 0)
        {
           Debug.Log("Mouse Scroll baixo");
            isEquip1 = false;
            isEquip2 = true;
        }
        

    }
}