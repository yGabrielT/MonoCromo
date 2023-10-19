using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Cinemachine;
using Menu;
public class DialogueManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI TextName;
    [SerializeField] private TextMeshProUGUI TextDesc;
    [SerializeField] private float diagMaxCooldown = 2f;
    [SerializeField] private GameObject objPainelNPC;                       // Painel do Dialogo
    [SerializeField] private GameObject setaCooldown;                       // Seta ativada quando o jogador puder interagir
    [SerializeField] private CinemachineVirtualCamera _npcCam;

    [SerializeField] private CinemachineTargetGroup _targetGroup;
    private List<AudioClip> _npcAudios = new List<AudioClip>();
    private List<AudioClip> _primaryAudios = new List<AudioClip>();

    [SerializeField] private AudioSource _audioSource;

    private List<NpcDesc> _npc = new List<NpcDesc>();                        // Retirado do ScriptableObject
    private int _ActualIndex = 0;                                            // Index da conversa

    private bool _isPrimary;
    private string _nameText;
    private string _descText;
    private bool isInCooldown = false;
    private StarterAssetsInputs _input;
    public bool _alreadyChatitng;

    private Personagem _pers;

	public float letterDelay = 0.2f;

    private int audioIndex1;
    private int audioIndex2;

    private void Start()
    {
        
        _pers = GetComponent<Personagem>();
        _input = GetComponent<StarterAssetsInputs>();    
    }

    void Update()
    {
        if (isInCooldown)
        {
            _input.interact = false;
            _input.jump = false;
        }
        if (_input.jump && _alreadyChatitng && !isInCooldown)
        {
            _input.jump = false;
            Debug.Log("Next Conv");
            ContinueConversation();
        }
        if(_alreadyChatitng){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if(_alreadyChatitng && !PauseController.estaPausado){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
    }

    // Invocado quando jogador interage com NPC
    public void Dialogue(SODialogue diag, Transform npcPos)
    { 
        
        if (diag != null && !isInCooldown)
        {
            _targetGroup.m_Targets[1].target = npcPos;
            _npcCam.gameObject.SetActive(true);
            _pers.ativarControles = false;
            _alreadyChatitng = true;
            Debug.Log(_npc);
            objPainelNPC.SetActive(true);
            _npcAudios.AddRange(diag.AudioNpc);
            _primaryAudios.AddRange(diag.AudioPrimary);


            // Pega valores do npc caso esteja vazio
            if (_npc.Count == 0)
            {
                _npc.AddRange(diag.Dialogue);
            }
            

            ContinueConversation();



        }
    }

    public void ContinueConversation()
    {
        setaCooldown.SetActive(false);
        isInCooldown = true;
        // Verificar se a quantidade de dialogos foi ou n�o foi excedida 
        if (_ActualIndex < _npc.Count)
        {
            AdvanceConversation();
            _ActualIndex += 1;
        }
        else
        {
            EndConversation();
        }
    }

    void AdvanceConversation()
    {
        
        NpcDesc npcvar = _npc[_ActualIndex];
        _nameText = npcvar.NpcName;
        _descText = npcvar.NpcDescription;
        _isPrimary = npcvar.isPrimary;
        DisplayDiag();
    }

    // Esvaziar valores e esconder dialogo
    void EndConversation()
    {
        _npcAudios.Clear();
        _primaryAudios.Clear();
        objPainelNPC.SetActive(false);
        _ActualIndex = 0;
        _npc.Clear();
        _alreadyChatitng = false;
        _npcCam.gameObject.SetActive(false);
        _pers.ativarControles = true;

        Invoke(nameof(ReturnCooldown),.5f);
    }

    void ReturnCooldown()
    {
        setaCooldown.SetActive(true);
        isInCooldown = false;
    }

    void DisplayDiag()
    {
        TextName.text = _nameText;
        //TextDesc.text = _descText;
        StartCoroutine(nameof(EnterFullDesc));
    }

    IEnumerator EnterFullDesc () {
		char[] fullTxtArray = _descText.ToCharArray();
		string nextTxt = "";
        int i = 0;
		for (i = 0; i < fullTxtArray.Length; i++){


            PlayDialogueAudio();
            nextTxt += fullTxtArray[i];

			TextDesc.text = nextTxt;
            
            


            yield return new WaitForSeconds(letterDelay);
		}
        if (i <= fullTxtArray.Length)
        {
            Debug.Log("Acabou o texto");
            Invoke(nameof(ReturnCooldown), 1.5f);
        }

	}

    void PlayDialogueAudio()
    {
        
        if (_npcAudios.Count != 0 && _primaryAudios.Count != 0)
        {
            audioIndex1 = Random.Range(0, _npcAudios.Count);
            audioIndex2 = Random.Range(0, _primaryAudios.Count);
            if (!_isPrimary)
            {
                Debug.Log(_npcAudios[audioIndex1]);
                _audioSource.PlayOneShot(_npcAudios[audioIndex1]);
            }
            else
            {
                Debug.Log(_primaryAudios[audioIndex2]);
                _audioSource.PlayOneShot(_primaryAudios[audioIndex2]);
            }
        }
        
    }
}
