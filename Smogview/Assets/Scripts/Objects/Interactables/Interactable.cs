using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Abstact interactable class
/// Each interactable should have a collider attached as a child
/// </summary>
public abstract class Interactable : MonoBehaviour
{
    [SerializeField] public List<Interaction> interactions;
    private List<Interaction> _activeInteractions = new List<Interaction>();


    [SerializeField] private Collider2D _interactionArea;

    public virtual void Awake()
    {
        foreach (Interaction interaction in interactions)
        {
            Interaction tempInt = Instantiate(interaction, transform);
            tempInt.transform.SetParent(transform);
            _activeInteractions.Add(tempInt);
        }
    }


    public abstract void OnInteract();

    public virtual void ActivateInteractions()
    {
        foreach (Interaction interaction in _activeInteractions)
        {
            interaction.Behaviour();
        }
    }
}
