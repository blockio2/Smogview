using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    private PlayerMovement _movementController;
   
    
    //Fields for interactables
    private CircleCollider2D _interactionCollider;

    private ContactFilter2D _contactFilter;
    private Collider2D[] _results = new Collider2D[3];


    public void Awake()
    {
        _movementController = GetComponent<PlayerMovement>();
        _interactionCollider = transform.Find("InteractionHitbox").GetComponent<CircleCollider2D>();
        
        //Setup layer mask for detecting interactions
        _contactFilter.SetLayerMask(LayerMask.GetMask("Interactable"));
        _contactFilter.useTriggers = true;
    }


    public void Update()
    {

    }

    /// <summary>
    /// When interacting, gets a reference to all nearby interactable objects and calls the interaction method for the first one
    /// </summary>
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _interactionCollider.Overlap(_contactFilter, _results);

            if (_results[0])
            {
                _results[0].GetComponentInParent<Interactable>().OnInteract();
            }

            _results[0] = null;
        }
    }

}
