using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private CircleCollider2D _interactionCollider;
    private ContactFilter2D _contactFilter;
    private readonly List<Collider2D> _results = new List<Collider2D>();
    private Interactable _currentTarget;
    
    /// <summary>
    /// Whether or not the player is holding an object currently
    /// </summary>
    public bool TargetHeld
    {
        get {
            if (_currentTarget == null)
                return false;
            return _currentTarget.BeingHeld; 
        }
    }
    private PlayerMovement _playerMovement;
    // The prompt and the E action use the same eligibility check.
    public bool CanInteract => FindInteractionTarget() != null;

    public void Awake()
    {
        _interactionCollider = transform.Find("InteractionHitbox").GetComponent<CircleCollider2D>();
        _playerMovement = GetComponent<PlayerMovement>();
        _contactFilter.useTriggers = true;
    }

    public void Update()
    {
        //Stops player movement if holding an item
        if (TargetHeld)
            _playerMovement.CanMove = false;
        else
            _playerMovement.CanMove = true;
    }

    private Interactable FindInteractionTarget()
    {
        if (!isActiveAndEnabled || _interactionCollider == null || !_interactionCollider.isActiveAndEnabled)
        {
            return null;
        }
        _results.Clear();
        _interactionCollider.Overlap(_contactFilter, _results);
        Interactable nearest = null;
        float nearestDistance = float.PositiveInfinity;
        foreach (var hit in _results)
        {
            if (hit == null || hit.name != "InteractionHitbox" || hit.transform.IsChildOf(transform)) continue;
            Interactable target = hit.GetComponentInParent<Interactable>();
            if (target == null || !target.isActiveAndEnabled) continue;
            float distance = ((Vector2)hit.bounds.center - (Vector2)_interactionCollider.bounds.center).sqrMagnitude;
            if (distance < nearestDistance)
            {
                nearest = target;
                nearestDistance = distance;
            }
        }
        return nearest;
    }


    /// <summary>
    /// Actiavtes the interactable object or 
    /// </summary>
    /// <param name="context"></param>
    public void Interact(InputAction.CallbackContext context)
    {
        if(_currentTarget != null && FindInteractionTarget() == null)
        {
            _currentTarget.BeingHeld = false;
            _currentTarget = null;
            return;
        }

        _currentTarget = FindInteractionTarget();

        if (_currentTarget != null)
        {
            if (context.started)
            {
                _currentTarget.BeingHeld = true;
            }
            else if (context.canceled)
            {
                _currentTarget.BeingHeld = false;
            }

            if (context.performed)
            {
                _currentTarget.OnInteract();
            }
        }
    }
}
