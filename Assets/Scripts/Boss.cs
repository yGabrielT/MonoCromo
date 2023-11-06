using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    // nomeie de qqlr jeito pq nn sei o q fala s� queria separar
    [Header("Coisas Importantes")]
    public Transform player;
    public int VidaAtual;
    [SerializeField] private Animator anim;
    [SerializeField] private Collider colisaoMaoR;
    [SerializeField] private Collider colisaoMaoL;

    [Header("Coisas Secund�rias")]
    public int dano = 1;

    [SerializeField] private bool atacando;
    [SerializeField] private bool podeAtacar;
    [SerializeField] private float tempoAtaque;
    [SerializeField] private float distancia;
    [SerializeField] private float smoothTime;
    [SerializeField] private float cooldownAtaque = 3;
    private AudioSource _bossAudio;

    [SerializeField] private float distanciaParaAndar;

    [SerializeField] NavMeshAgent _nav;

    private bool canRun;

    private int numbersOfHits;
    [SerializeField] int hitsToStunItself;
    private int damage = 20;
    public bool isStunned;

    private Transform target;

    [SerializeField] float _cooldownStun;

    [Header("Audio")]
    [SerializeField] AudioClip[] _passosAudios; 
    private float _delayPassoAudio;
    [SerializeField] float _delayAteOProximoPasso = .5f;
    [SerializeField] AudioClip _ataque1;
    [SerializeField] AudioClip _ataque2;


    void Start() {
        _bossAudio = GetComponent<AudioSource>();
        target = player;
        numbersOfHits = 0;    
    }
    // Update is called once per frame
    void Update()
    {
        if(!isStunned){
            Atacar();
            SeguirJogador();
        }else{
            canRun = false;
        }
        if(VidaAtual <= 0){
            Destroy(gameObject);
        }
        if(canRun){
            target = player;
            anim.SetBool("CanRun",true);
            
            
            
        }
        else{
          
            target = gameObject.transform;
            anim.SetBool("CanRun",false);
        }
    }

    
    public void TomarDano(int dano)
    {
        this.VidaAtual -= dano;
        Debug.Log("Boss Danificado");
    }


    public void SeguirJogador(){
       if(distanciaParaAndar < distancia && !podeAtacar)
       {
            _nav.SetDestination(target.position);
            canRun = true;
       } 
       else
       {
            canRun = false;
       }

        

    }
    public void Atacar()
    {
        if (!atacando)
        {
            // Acompanha o player com a rota��o do boss. "olhando" para ele
            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(lookDir),smoothTime * Time.deltaTime);
        }

        distancia = Vector3.Distance(transform.position, player.position);

        if (!podeAtacar)
        {
            tempoAtaque += Time.deltaTime; // falar cm biel para usar o input dele
            if(tempoAtaque > cooldownAtaque)
            {
              
                tempoAtaque = 0;
                canRun = false;
                podeAtacar = true;
                
            }
            else{
                atacando = false;
                canRun = true;
                if(_delayPassoAudio >= _delayAteOProximoPasso){
                EscolherPassoAleatorio();
                _delayPassoAudio= 0f;
            }else{
                _delayPassoAudio += Time.deltaTime;
            }
            }
        }

        // AO ENTRAR EM UM RANGE MELEE ELE ATACA DE PERTO
        if(distancia < 7 && podeAtacar)
        {
            _bossAudio.clip = _ataque1;
            _bossAudio.Play();
            numbersOfHits++;
            
            canRun = false;
            atacando = true;
            int numeroAtaque = Random.Range(1, 3);
            anim.SetTrigger(numeroAtaque.ToString());
            podeAtacar = false;
            
        }

        // caso fique longe la�a um ataque em �rea no ch�o
        if((distancia >= 7 && distancia <= 10) &&  podeAtacar)
        {
            _bossAudio.clip = _ataque2;
            _bossAudio.Play();
            canRun = false;
            numbersOfHits++;
            atacando = true;
            
            int numeroAtaque = 3;
            anim.SetTrigger(numeroAtaque.ToString());
            podeAtacar = false;
            // criar dano em �rea
            // buscar uma anima��o disso
        }
        if(distancia > 10){
            atacando = false;
            podeAtacar = false;
            canRun = true;
        }
        //Boss se atordoa após certa quantidade de ataques
        if(numbersOfHits >= hitsToStunItself){
            numbersOfHits = 0;
            StartCoroutine(nameof(Atordoarse));
        }

        // tava pensando em adicionar uma parte onde o personagem ficando distante por dois ataques ao chao o robo se aproxima e lan�a um surpresa
    }

    public void AtivarCollider(){
        colisaoMaoR.enabled = true;
        colisaoMaoL.enabled = true;
    }

    public void DesativarCollider(){
        colisaoMaoL.enabled = false;
        colisaoMaoR.enabled = false;
    }   

    //Fica atordoado e enquanto isso ativar os pontos fracos do Boss
    public IEnumerator Atordoarse()
    {
        isStunned = true;
        canRun = false;
        Debug.Log("Atordoado");
        yield return new WaitForSeconds(_cooldownStun);
        isStunned = false;
        canRun = true;
        podeAtacar = false;
    }

    private void EscolherPassoAleatorio(){
        int Randnumb = Random.Range(0,1);
        _bossAudio.volume = Random.Range(0.2f,.7f);
        _bossAudio.clip= _passosAudios[Randnumb];
        _bossAudio.Play();

    }
}
