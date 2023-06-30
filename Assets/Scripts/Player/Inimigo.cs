using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Inimigo : MonoBehaviour
{
    [Header("Spawner(Apenas para spawners)")]
    public GameObject Tipo;

    [Header("Inimigo(Apenas para inimigos)")]
    public int vidaMax = 30;
    [Range(0,20f)]
    public float cooldownTime = 5f;
    public Material atordoadoMaterial;

    private int vida;
    private Transform Player;
    private Renderer renderer;
    private Material defaultMaterial;
    private NavMeshAgent NavMeshAgente;
    private float temp;
    private bool Spawnado = false;
    

    private void Start()
    {
        var gObj = GameObject.FindWithTag("Model");
        Player = gObj.transform;
        vida = vidaMax;
        if(this.gameObject.tag == "Inimigo")
        {
            NavMeshAgente = this.GetComponent<NavMeshAgent>();
            defaultMaterial = this.GetComponent<Renderer>().material;
            renderer = this.GetComponent<Renderer>();
        }
        GerarInimigo();
    }

    private void Update()
    {
        
        if(this.gameObject.tag == "Inimigo")
        {
            Movimentar();
            
            if(this.vida == 0)
            {
                atordoar(cooldownTime);
            }
        }
        
        
    }

    private void GerarInimigo()
    {
        if(this.gameObject.tag != "Inimigo")
        {
            for(int i = 0; i < 5; i++)
            {
                Vector3 InimigoPosition = new Vector3(Random.Range(-5, 5) + this.transform.position.x, this.transform.position.y, this.transform.position.z + Random.Range(-5, 5));
                Instantiate(Tipo, InimigoPosition, Quaternion.identity);
                
            }
        }
    }

    private void Movimentar()
    {
        if (vida > 0)
        {
            NavMeshAgente.SetDestination(Player.position);
        }
        else
        {
            atordoar(cooldownTime);
        }
    }

    private void atordoar(float cooldown)
    {
        renderer.material = atordoadoMaterial;
        NavMeshAgente.SetDestination(transform.position);
        if (cooldown > temp)
        {
            temp += Time.unscaledDeltaTime;
        }
        else
        {
            this.vida = vidaMax;
            renderer.material = defaultMaterial;
            temp = 0;
        }
    }

    public void TomarDano(int dano)
    {
        this.vida -= dano;
    }
}
