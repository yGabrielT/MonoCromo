using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class Inimigo : MonoBehaviour
{
    [Header("Spawner(Apenas para spawners)")]
    public GameObject Tipo;
    public int quantidade = 3;

    //É possivel usar Scriptable Objects para essas variaveis
    [Header("Inimigo(Apenas para as prefabs dos inimigos)")]
    public int vidaMax = 30;
    [Range(0, 10)]
    public int velocidade = 4;
    public int dano = 1;
    [Range(0, 20f)]
    public float AtordTimer = 5f;
    public Material atordoadoMaterial;
    public float TempoPraAtirar = .5f;
    public GameObject Projetil;
    public float ProjForc = 3f;
    public AudioSource audioAcertado;
    public AudioSource audioAtirar;
    public AudioSource audioAtordoado;
    [HideInInspector] public bool AtacarBool = true;
    [SerializeField] private GameObject _stunGO;
    public InimigoFov _fov;
    [SerializeField] Animator _anim;
    [Header("Torreta(Apenas para as prefabs de torretas)")]
    public bool isTurret = false;
    [SerializeField] private Transform PivotTurret;


    private float projTemp;
    private bool somTocando;
    private bool atordoado;
    private int vidaAtual;
    private Transform playerPos;


    private NavMeshAgent NavMeshAgente;
    private float temp;
    private bool Spawnado = false;
    private bool isPatrulhando = false;
    private float newPosTemp;
    private bool _startStunAnim = false;

    private Vector3 previousPosition;
    private float curSpeed;



    [Header("Stealth")]
    [SerializeField] private bool isStealth = false;
    [SerializeField] private float newPosCooldown;
    [SerializeField] private float AggroRange;
    [SerializeField] private float offSetWalk;


    private void Start()
    {

        //Encontrar o jogador
        var gObj = GameObject.FindWithTag("Track");
        playerPos = gObj.transform;


        //Aplicar vida máxima a atual
        vidaAtual = vidaMax;

        //Pegar os componentes do IA para depois executar o PathFinding
        if (this.gameObject.tag == "Inimigo" && !isTurret)
        {
            NavMeshAgente = this.GetComponent<NavMeshAgent>();
            NavMeshAgente.speed = velocidade;

        }
        GerarInimigo();
    }

    private void Update()
    {
        if (playerPos == null)
        {
            AtacarBool = false;
        }
        if (!isTurret)
        {
            Stealth();
            Movimentar();
            Atordoar(AtordTimer);
            AtordoarSom();
            AplicarAnimacoes();
        }


        AtacarPersonagem();


    }

    private void Stealth()
    {
        if (this.gameObject.tag == "Inimigo" && isStealth)
        {
            newPosTemp += Time.deltaTime;
            if (!_fov.canSeePlayer && vidaAtual > 0 && playerPos != null)
            {

                AtacarBool = false;
                if (newPosCooldown <= newPosTemp)
                {
                    //var rand = new Vector3(Random.Range(-5,5), 0, Random.Range(-5,5)) + transform.position;
                    
                    NavMeshAgente.SetDestination(RandomPos());
                    newPosTemp = 0;

                }



            }
            else
            {

                AtacarBool = true;
                isStealth = false;
            }
        }
    }

    //Instanciando o inimigo em lugares aleatorios proximos ao GameObject
    private void GerarInimigo()
    {
        if (this.gameObject.tag != "Inimigo")
        {
            for (int i = 0; i < quantidade; i++)
            {
                Vector3 InimigoPosition = new Vector3(Random.Range(-5, 5) + this.transform.position.x, this.transform.position.y + 5, this.transform.position.z + Random.Range(-5, 5));
                Instantiate(Tipo, InimigoPosition, Quaternion.identity);

            }
        }
    }

    //Movimentar até o jogador caso esteja com vida senão função Atordoar é chamada
    private void Movimentar()
    {
        if (this.gameObject.tag == "Inimigo")
        {
            if (vidaAtual > 0 && playerPos != null && AtacarBool)
            {

                NavMeshAgente.SetDestination(playerPos.position);
            }
            else
            {

                Atordoar(AtordTimer);
            }


        }


    }

    private void AplicarAnimacoes()
    {
        if (this.gameObject.tag == "Inimigo")
        {
            //Checar Posição para saber a atual velocidade do inimigo
            Vector3 curMove = transform.position - previousPosition;
            curSpeed = curMove.magnitude / Time.deltaTime;
            previousPosition = transform.position;


            if (AtacarBool)
            {
                _anim.SetBool("AtacarBool", true);
            }
            else
            {
                _anim.SetBool("AtacarBool", false);
            }
            if (curSpeed > 0.1f)
            {
                _anim.SetBool("IsWalking", true);
            }
            else
            {
                _anim.SetBool("IsWalking", false);

            }
        }

    }

    private void AtacarPersonagem()
    {
        if (this.gameObject.tag == "Inimigo" && Projetil != null && this.vidaAtual > 0 && playerPos != null && AtacarBool)
        {
            projTemp += Time.deltaTime;

            if (!isTurret)
            {
                this.gameObject.transform.LookAt(playerPos.transform);
                var angle = gameObject.transform.eulerAngles;
                angle.x = 0;

                gameObject.transform.rotation = Quaternion.Euler(angle);
            }
            else
            {
                this.transform.LookAt(playerPos.transform);
                var angle = transform.rotation.eulerAngles;
                angle.z = 0;
                angle.x = 0;
                transform.rotation = Quaternion.Euler(angle);
            }


            if (projTemp > TempoPraAtirar)
            {
                audioAtirar.Play();
                GameObject InimigoProjetil = Instantiate(Projetil, transform.position + new Vector3(0, .5f, 0), this.gameObject.transform.rotation);
                Rigidbody Rb = InimigoProjetil.GetComponent<Rigidbody>();
                Rb.AddForce(transform.forward * ProjForc, ForceMode.VelocityChange);
                projTemp = 0;
                Destroy(InimigoProjetil, 2f);
            }

        }

    }

    //Mudar o material e a vida atual baseado em quanto tempo o inimigo está atordoado
    private void Atordoar(float cooldown)
    {
        if (this.gameObject.tag == "Inimigo" && this.vidaAtual <= 0)
        {
            if (!_startStunAnim)
            {
                _startStunAnim = true;
                _anim.SetTrigger("Stun");
            }
            atordoado = true;
            _stunGO.SetActive(true);
            NavMeshAgente.SetDestination(transform.position);
            if (cooldown > temp)
            {
                temp += Time.deltaTime;
            }
            else
            {

                AtacarBool = false;
                isStealth = true;
                atordoado = false;
                _stunGO.SetActive(false);
                this.vidaAtual = vidaMax;
                _startStunAnim = false;
                temp = 0;

            }

        }

    }

    // Aplicar dano a vida atual quando chamado em qualquer script
    public void TomarDano(int dano)
    {
        this.vidaAtual -= dano;
        audioAcertado.Play();
    }

    private void AtordoarSom()
    {

        if (atordoado && !somTocando)
        {
            somTocando = true;
            audioAtordoado.Play();
        }
        if (!atordoado)
        {
            somTocando = false;
        }
    }

    //Retorna true caso o inimigo esteja longe do jogador
    private bool EstaLonge(float limite)
    {
        float distance = Vector3.Distance(transform.position, playerPos.position);
        if (distance >= limite)
        {
            return true;

        }
        else
        {
            return false;
        }
    }

    //Posição aleatória
    private Vector3 RandomPos()
    {
        return new Vector3(Random.Range(-offSetWalk, offSetWalk), 0, Random.Range(-offSetWalk, offSetWalk)) + transform.position;
    }

}
