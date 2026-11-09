using UnityEngine;
using UnityEngine.UI;
using System;

public class DelayedInteraction : Interactable
{
    [SerializeField] private float _timeToInteract;
    private float _interactTimer;
    private bool _canActivate = false;

    private PlayerMovement _playerMovement;
    InteractionIndicatorUI _interactionIndicator;


    public override void Awake()
    {
        base.Awake();
        _playerMovement = FindAnyObjectByType<PlayerMovement>();
        _interactionIndicator = FindAnyObjectByType<InteractionIndicatorUI>();
    }

    public void Update()
    {
        if (BeingHeld)
        {
            _interactionIndicator.Active = true;

            //Move indicator wheel above object position and change the fill amount based on completion percent
            _interactionIndicator.Move(new Vector2(transform.position.x, transform.position.y + 1.2f));
            _interactionIndicator.SetFill(_interactTimer / _timeToInteract);

            _interactTimer += Time.deltaTime;

            //Activate interaction when time is reached
            if (_interactTimer > _timeToInteract)
            {
                _canActivate = true;
                OnInteract();
            }       
        }
    }

    public override void OnInteract()
    {
        if (_canActivate)
        {
            _canActivate = false;
            _interactTimer = 0;
            _interactionIndicator.SetFill(0);
            ActivateInteractions();
        }
    }

    public override void Release()
    {
        _interactTimer = 0;
        _interactionIndicator.SetFill(0);
        _interactionIndicator.Active = false;
    }
}
