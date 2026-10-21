using UnityEngine;

/// <summary>
/// Abstact interactable class
/// Each interactable should have a collider attached as a child
/// </summary>
public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private Collider2D _interactionArea;

    public virtual void Awake()
    {

    }


    public virtual void OnInteract()
    {

    }
}
