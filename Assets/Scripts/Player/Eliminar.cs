using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Eliminar : MonoBehaviour
{
    [SerializeField] private NavMeshAgent NavMeshAgente;
    [SerializeField] private Transform Player;
    public int vida;
    public int vidaMax = 30;
    private Renderer renderer;
    private Material defaultMaterial;
    public Material atordoadoMaterial;
    public float cooldownTime = 5f;
    private float temp;
    

    private void Start()
    {
        var gObj = GameObject.FindWithTag("Model");
        Player = gObj.transform;
        defaultMaterial = this.GetComponent<Renderer>().material;
        renderer = this.GetComponent<Renderer>();
        vida = vidaMax;
    }
    void Update()
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

    public void atordoar(float cooldown)
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
