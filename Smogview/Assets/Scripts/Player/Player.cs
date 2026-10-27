using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private CircleCollider2D _interactionCollider;
    private ContactFilter2D _contactFilter;
    private readonly List<Collider2D> _results = new List<Collider2D>();

    // The prompt and the E action use the same eligibility check.
    public bool CanInteract => FindInteractionTarget() != null;

    public void Awake()
    {
        _interactionCollider = transform.Find("InteractionHitbox").GetComponent<CircleCollider2D>();
        _contactFilter.useTriggers = true;
    }

    private Interactable FindInteractionTarget()
    {
        if (!isActiveAndEnabled || _interactionCollider == null || !_interactionCollider.isActiveAndEnabled)
            return null;
        _results.Clear();
        _interactionCollider.Overlap(_contactFilter, _results);
        Interactable nearest = null;
        float nearestDistance = float.PositiveInfinity;
        foreach (var hit in _results)
        {
            if (hit == null || hit.name != "InteractionHitbox" || hit.transform.IsChildOf(transform)) continue;
            var target = hit.GetComponentInParent<Interactable>();
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

    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        var target = FindInteractionTarget();
        if (target != null) target.OnInteract();
    }
}
