using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI TextName;
    [SerializeField] private TextMeshProUGUI TextDesc;
    [SerializeField] private float diagMaxCooldown = 2f;
    [SerializeField] private GameObject objPainelNPC;                       // Painel do Dialogo
    
    private List<NpcDesc> _npc = new List<NpcDesc>();                        // Retirado do ScriptableObject
    private int _ActualIndex = 0;                                            // Index da conversa

    private float _diagCooldown;
    private string _nameText;
    private string _descText;
    private bool isInCooldown = false;
    private StarterAssetsInputs _input;
    private bool _alreadyChatitng;

    private void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();    
    }

    void Update()
    {
        if (isInCooldown)
        {
            _diagCooldown += Time.deltaTime;
            if(_diagCooldown > diagMaxCooldown )
            {
                isInCooldown = false;
                _diagCooldown = 0;
            }
        }

        if (_input.interact && _alreadyChatitng && !isInCooldown)
        {
            ContinueConversation();
        }
        
    }

    // Invocado quando jogador interage com NPC
    public void Dialogue(SODialogue diag)
    { 
        
        if (diag != null && !isInCooldown)
        {
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
        // Verificar se a quantidade de dialogos foi ou não foi excedida 
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
    }

    void DisplayDiag()
    {
        TextName.text = _nameText;
        TextDesc.text = _descText;

    }
}
