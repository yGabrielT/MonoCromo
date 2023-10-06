using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct NpcDesc
{
    public string NpcName;

    [TextArea]
    public string NpcDescription;

    public bool isPrimary;
}

[CreateAssetMenu(menuName ="ScriptableObjects/Dialogues")]
public class SODialogue : ScriptableObject
{
    public List<NpcDesc> Dialogue = new List<NpcDesc>(2);

    public List<AudioClip> AudioNpc = new List<AudioClip>();

    public List<AudioClip> AudioPrimary = new List<AudioClip>();

    
}
