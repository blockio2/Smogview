using UnityEngine;
using TMPro;

public class Button : Interactable
{
    

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnInteract()
    {
        ActivateInteractions();
    }
}
