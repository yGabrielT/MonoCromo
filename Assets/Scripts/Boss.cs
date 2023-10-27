using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    // nomeie de qqlr jeito pq nn sei o q fala s� queria separar
    [Header("Coisas Importantes")]
    public Transform player;
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

    [SerializeField] private float distanciaParaAndar;

    [SerializeField] NavMeshAgent _nav;

    private bool canRun;


    private int damage = 20;

    // Update is called once per frame
    void Update()
    {
        Atacar();
        SeguirJogador();
        
    }


    public void SeguirJogador(){
       if(distanciaParaAndar < distancia && !podeAtacar)
       {
            _nav.SetDestination(player.position);
            canRun = true;
            anim.SetTrigger("run"); 
       } 
       else
       {
            anim.SetTrigger("Back");
       }
       if(canRun)
       {
            canRun = false;
            anim.SetTrigger("run");
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
            if(tempoAtaque > 3)
            {
                tempoAtaque = 0;
                canRun = false;
                podeAtacar = true;
                
            }
            else{
                atacando = false;
                canRun = true;
            }
        }

        // AO ENTRAR EM UM RANGE MELEE ELE ATACA DE PERTO
        if(distancia < 7 && podeAtacar)
        {
            podeAtacar = false;
             anim.SetTrigger("run");
            canRun = false;
            atacando = true;
            int numeroAtaque = Random.Range(1, 3);
            anim.SetTrigger(numeroAtaque.ToString());
            
        }

        // caso fique longe la�a um ataque em �rea no ch�o
        if((distancia > 8 && distancia < 10) &&  podeAtacar)
        {
            atacando = true;
            podeAtacar = false;
            int numeroAtaque = 3;
            anim.SetTrigger(numeroAtaque.ToString());

            // criar dano em �rea
            // buscar uma anima��o disso
        }
        if(distancia > 10){
            atacando = false;
            podeAtacar = false;
            canRun = true;
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

    public void Atordoarse()
    {
        
    }
}
