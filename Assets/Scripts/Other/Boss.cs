using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Audio;

public class Boss : MonoBehaviour
{
    // nomeie de qqlr jeito pq nn sei o q fala s� queria separar
    [Header("Coisas Importantes")]
    public Transform player;
    public int VidaAtual;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform posMaoR;
    [SerializeField] private Transform posMaoL;
    public UnityEvent onSpecial;

    [Header("Coisas Secund�rias")]

    private bool atacando;
    private bool podeAtacar;
    private float tempoAtaque;
    private float distancia;
    [SerializeField] private float smoothTime;
    [SerializeField] private float cooldownAtaque = 3;
    private AudioSource _bossAudio;
    [SerializeField] private float balaForca = 10;


    [SerializeField] private float distanciaParaAndar;

    [SerializeField] NavMeshAgent _nav;

    [SerializeField] GameObject _windPrefab;
    [SerializeField] float DestroyWindTime;
    [SerializeField] float distanciaParaAtacarLeve = 10;
    [SerializeField] Transform _transformWind;

    private bool canRun;

    private int numbersOfHits;
    [SerializeField] int hitsToStunItself;
    private int damage = 20;
    private bool isStunned;

    private Transform target;

    [SerializeField] float _cooldownStun;

    [SerializeField] GameObject _flameParticleOmbro;
    [SerializeField] GameObject _flameParticlePe;
    [SerializeField] GameObject _projectileToShoot;

    [Header("Audio")]
    [SerializeField] AudioClip _flameAudio;


    [SerializeField] AudioClip _ataqueleve;

    [SerializeField] AudioClip _ataqueEspecial;

    [SerializeField] private AudioSource _bossJetpackAudioSource;
    private int numeroAtaque;

    private bool canLightAttack;

    private Vector3 prevPos;
    private Vector3 curMov;
    private float curSpeed;
    
    private float timerExpireAttack;
    [SerializeField] private float timeTillAttackRenews = 7f;
    public AudioMixer volVfx;
    private float volumeSfx;

    void Start()
    {
        canLightAttack = true;
        _bossAudio = GetComponent<AudioSource>();
        target = player;
        numbersOfHits = 0;
    }
    // Update is called once per frame
    void Update()
    {

        

        if (!isStunned)
        {
            Atacar();
            SeguirJogador();
        }
        else
        {
            canRun = false;
        }
        if (VidaAtual <= 0)
        {
            Destroy(gameObject);
        }
        //Renova o timer de ataque caso aja um erro nas aninações
        if(!canLightAttack){
            timerExpireAttack += Time.deltaTime;
            if(timeTillAttackRenews < timerExpireAttack){
                timerExpireAttack = 0;
                canLightAttack = true;
            }
        }
    }

    public float GetVolumeSFX(){
        float value;
		volVfx.GetFloat("VolSFX", out value);
        
		if(value != 0){
			return Mathf.Pow(10f,value/20);
		}else{
			return 100f;
		}
        
    }


    public void TomarDano(int dano)
    {
        this.VidaAtual -= dano;
        BossHealthTween.ChangedLife = true;
        Debug.Log("Boss Danificado");
    }


    public void SeguirJogador()
    {
        if (distanciaParaAndar < distancia)
        {

            canRun = true;
        }
        else
        {
            canRun = false;
        }
        if (canRun)
        {
            target = player;

        }
        else
        {

            target = gameObject.transform;

        }
        _nav.SetDestination(target.position);

        curMov = transform.position - prevPos;
        curSpeed = curMov.magnitude / Time.deltaTime;
        prevPos = transform.position;

        if(curSpeed > .1f && !isStunned){
            AtivarCombustores();
        }
        if(curSpeed < .1f){
            DesativarCombustores();
        }

    }
    public void Atacar()
    {
        
        // Acompanha o player com a rota��o do boss. "olhando" para ele
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir), smoothTime * Time.deltaTime);
        distancia = Vector3.Distance(transform.position, player.position);

        
            if (tempoAtaque > cooldownAtaque)
            {

                tempoAtaque = 0;
                
                podeAtacar = true;
                
                
            }
            else
            {
                tempoAtaque += Time.deltaTime;
                atacando = false;
                canRun = true;
                
            }
        
            
            
        

        // Atacar leve com projeteis se for perto e com onda de choque se estiver longe
        if (canLightAttack)
        {
            canLightAttack = false;
            numbersOfHits++;
            
            
            atacando = true;

            if (distancia <= distanciaParaAtacarLeve)
            {
                numeroAtaque = Random.Range(1, 3);
                
                
            }
            if (distancia > distanciaParaAtacarLeve)
            {
                numeroAtaque = 3;
                AudioSource.PlayClipAtPoint(_ataqueEspecial, transform.position, GetVolumeSFX());
                
            }
            anim.SetTrigger(numeroAtaque.ToString());


        }

        if (distancia > distanciaParaAtacarLeve)
        {
            atacando = false;
            //podeAtacar = false;
            canRun = true;
        }

        //Boss se atordoa após certa quantidade de ataques
        if (numbersOfHits >= hitsToStunItself)
        {
            numbersOfHits = 0;
            StartCoroutine(nameof(Atordoarse));
        }

        // tava pensando em adicionar uma parte onde o personagem ficando distante por dois ataques ao chao o robo se aproxima e lan�a um surpresa
    }

    public void AtirarMissil()
    {
        
        if (numeroAtaque == 1)
        {
            Vector3 forcaDir = posMaoL.transform.forward;
            forcaDir = (player.position - posMaoL.position).normalized;
            
            GameObject projetil = Instantiate(_projectileToShoot, posMaoL.position, Quaternion.LookRotation(forcaDir));
            Rigidbody projetilRb = projetil.GetComponent<Rigidbody>();
            forcaDir = forcaDir * balaForca;
            projetilRb.AddForce(forcaDir, ForceMode.Impulse);
            AudioSource.PlayClipAtPoint(_ataqueleve, transform.position, GetVolumeSFX());
        }
        if (numeroAtaque == 2)
        {
            Vector3 forcaDir = posMaoR.transform.forward;
            forcaDir = (player.position - posMaoR.position).normalized;
            
            GameObject projetil = Instantiate(_projectileToShoot, posMaoR.position, Quaternion.LookRotation(forcaDir));
            Rigidbody projetilRb = projetil.GetComponent<Rigidbody>();
            forcaDir = forcaDir * balaForca;
            projetilRb.AddForce(forcaDir, ForceMode.Impulse);
            AudioSource.PlayClipAtPoint(_ataqueleve, transform.position, GetVolumeSFX());
            
        }
        Invoke(nameof(ReativarCooldownMissil),.5f);
    }

    public void ReativarCooldownMissil()
    {

        canLightAttack = true;
    }

    //Fica atordoado e enquanto isso ativar os pontos fracos do Boss
    public IEnumerator Atordoarse()
    {
        isStunned = true;
        canRun = false;
        DesativarCombustores();
        Debug.Log("Atordoado");
        yield return new WaitForSeconds(_cooldownStun);
        isStunned = false;
        canRun = true;
        podeAtacar = false;
    }

    public void AtivarCombustores()
    {


        _bossJetpackAudioSource.volume = 1f;
        _flameParticleOmbro.SetActive(true);
    }

    public void DesativarCombustores()
    {

        _bossJetpackAudioSource.volume = 0f;
        _flameParticleOmbro.SetActive(false);
    }

    public void AtivarCombustoresPes()
    {


        _bossJetpackAudioSource.volume = 1f;
        _flameParticlePe.SetActive(true);
    }

    public void DesativarCombustoresPes()
    {

        _bossAudio.volume = 0f;
        _flameParticlePe.SetActive(false);
    }

    public void InstanciarAbilidadeEspecial()
    {
        GameObject wind = Instantiate(_windPrefab, _transformWind.position, Quaternion.identity);
        Destroy(wind, DestroyWindTime);
        
        Debug.Log(GetVolumeSFX());
        
        Invoke(nameof(SpawnarSupply), 2f);
        Invoke(nameof(ReativarCooldownMissil),2f);
    }

    public void SpawnarSupply()
    {
        onSpecial.Invoke();
    }
}
