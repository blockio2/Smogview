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
    private bool _nearInteractable = false;

    private string _interactionHitboxName = "InteractionHitbox";


    public void Awake()
    {
        _movementController = GetComponent<PlayerMovement>();
        _interactionCollider = transform.Find("InteractionHitbox").GetComponent<CircleCollider2D>();
        
        //Setup layer mask for detecting interactions
        _contactFilter.useTriggers = true;
    }


    public void Update()
    {
        if (_nearInteractable)
        {
            //ui popup?
        }
    }

    /// <summary>
    /// Detects when the player is staying near an interactable hitbox
    /// </summary>
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == _interactionHitboxName)
        {
            _nearInteractable = true;
        }
    }

    /// <summary>
    /// When the player walks away from an interactable, update _nearInteractable
    /// </summary>
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == _interactionHitboxName)
        {
            _nearInteractable = false;
        }
    }

    /// <summary>
    /// When interacting, gets a reference to all nearby interactable objects and calls the interaction method for the first one
    /// </summary>
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _interactionCollider.Overlap(_contactFilter, _results);

            if (_results[0] && _results[0].name == _interactionHitboxName)
            {
                _results[0].GetComponentInParent<Interactable>().OnInteract();
            }

            _results = new Collider2D[3];
        }
    }



}
