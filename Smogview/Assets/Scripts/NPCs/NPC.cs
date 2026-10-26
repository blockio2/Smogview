using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC : Interactable
{
    [SerializeField] DialogueAsset mainText;
    [SerializeField] DialogueAsset finishedText;
    private GameObject _npcTextArea;
    private Image _moreTextIndicator;
    private TMP_Text _npcText;

    private int _dialogueCounter = 0;
    private bool _allDialogueSeen = false;
    private float _dialogueDisplayTime = 3f;

    public override void Awake()
    {
        base.Awake();

        _npcTextArea = GameObject.Find("NPCTextArea");
        _moreTextIndicator = _npcTextArea.transform.Find("Indicator").GetComponent<Image>();
        _npcText = _npcTextArea.transform.Find("Text").GetComponent<TMP_Text>();
    }

    //Hide dialogue when switching lenses
    public void OnEnable()
    {
        EndDialogue();
    }
    public void OnDisable()
    {
        EndDialogue();
    }
    public override void OnInteract()
    {

        _npcTextArea.SetActive(true);
        _npcTextArea.transform.position = new Vector2(transform.position.x, transform.position.y - 1.2f);

        ShowDialogue();
    }

    public void ShowDialogue()
    {
        StopAllCoroutines();

        //Check if all dialogue has been seen. Reset counter to play it again
        if(_dialogueCounter >= mainText.DialogueCount)
        {
            _allDialogueSeen = true;
            _dialogueCounter = 0;
        }
        //Disable more dialogue indactor when the last text has been loaded
        if(_dialogueCounter >= mainText.DialogueCount - 1)
        {
            _moreTextIndicator.enabled = false;
        }
        
        //If all dialogue has been seen and there is repeated dialogue at the end, play that
        if(_allDialogueSeen && finishedText)
        {
            _npcText.text = finishedText.dialogue[0];
        }
        //Otherwise display text in order
        else
        {
            _npcText.text = mainText.dialogue[_dialogueCounter];
        }

        _dialogueCounter++;
        StartCoroutine("DialogueClearTimer");
    }

    public void EndDialogue()
    {
        if(_npcTextArea)
            _npcTextArea.SetActive(false);
    }

    IEnumerator DialogueClearTimer()
    {
        yield return new WaitForSeconds(_dialogueDisplayTime);
        EndDialogue();
    }
}
