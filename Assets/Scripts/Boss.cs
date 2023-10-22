using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // nomeie de qqlr jeito pq nn sei o q fala só queria separar
    [Header("Coisas Importantes")]
    public Transform player;
    [SerializeField] private Animator anim;
    [SerializeField] private Collider colisaoMao;

    [Header("Coisas Secundárias")]
    public int dano = 1;

    [SerializeField] private bool atacando;
    [SerializeField] private bool podeAtacar;
    [SerializeField] private float tempoAtaque;
    [SerializeField] private float distancia;
    private int damage = 20;

    // Update is called once per frame
    void Update()
    {
        Atacar();
    }

    public void Atacar()
    {
        if (!atacando)
        {
            // Acompanha o player com a rotação do boss. "olhando" para ele
            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        distancia = Vector3.Distance(transform.position, player.position);

        if (!podeAtacar)
        {
            tempoAtaque += Time.deltaTime; // falar cm biel para usar o input dele
            if(tempoAtaque > 3)
            {
                tempoAtaque = 0;
                podeAtacar = true;
            }
        }

        // AO ENTRAR EM UM RANGE MELEE ELE ATACA DE PERTO
        if(distancia < 7 && podeAtacar)
        {
            podeAtacar = false;
            int numeroAtaque = Random.Range(1, 2);
            anim.SetTrigger(numeroAtaque.ToString());
            // acionar dano por meio de um colider seguindo a mao do personagem
            // penso em colocar isso por meio do rigging, o touro que peguei nn tem aniaçao por rigging no mixamo, mas sim por mudança de posição ent o collider nn acompanha
            // mudo esse personagem dps
        }

        // caso fique longe laça um ataque em área no chão
        if(distancia > 8 && podeAtacar)
        {
            podeAtacar = false;
            int numeroAtaque = 3;
            anim.SetTrigger(numeroAtaque.ToString());

            // criar dano em área
            // buscar uma animação disso
        }

        // tava pensando em adicionar uma parte onde o personagem ficando distante por dois ataques ao chao o robo se aproxima e lança um surpresa
    }

}
