using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Cinemachine;

public class DialogueManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI TextName;
    [SerializeField] private TextMeshProUGUI TextDesc;
    [SerializeField] private float diagMaxCooldown = 2f;
    [SerializeField] private GameObject objPainelNPC;                       // Painel do Dialogo
    
    [SerializeField] private CinemachineVirtualCamera _npcCam;

    [SerializeField] private CinemachineTargetGroup _targetGroup;



    private List<NpcDesc> _npc = new List<NpcDesc>();                        // Retirado do ScriptableObject
    private int _ActualIndex = 0;                                            // Index da conversa


    private string _nameText;
    private string _descText;
    private bool isInCooldown = false;
    private StarterAssetsInputs _input;
    private bool _alreadyChatitng;

    private Personagem _pers;

	public float letterDelay = 0.2f;

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
        }
        if (_input.interact && _alreadyChatitng && !isInCooldown)
        {
            _input.interact = false;
            Debug.Log("Next Conv");
            ContinueConversation();
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
        DisplayDiag();
    }

    // Esvaziar valores e esconder dialogo
    void EndConversation()
    {
        objPainelNPC.SetActive(false);
        _ActualIndex = 0;
        _npc.Clear();
        _alreadyChatitng = false;
        _npcCam.gameObject.SetActive(false);
        _pers.ativarControles = true;
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
			nextTxt += fullTxtArray[i];

			TextDesc.text = nextTxt;

			yield return new WaitForSeconds(letterDelay);
		}
        if (i <= fullTxtArray.Length)
        {
            Debug.Log("Acabou o texto");
            isInCooldown = false;
        }

	}
}
