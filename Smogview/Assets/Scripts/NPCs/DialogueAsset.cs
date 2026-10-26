using UnityEngine;

[CreateAssetMenu(fileName = "DialogueAsset", menuName = "DialogueAsset")]
public class DialogueAsset : ScriptableObject
{
    [TextArea]
    public string[] dialogue;

    public int DialogueCount
    {
        get { return dialogue.Length; }
    }
}
