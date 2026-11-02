using UnityEngine;
using UnityEngine.UI;
using System;

public class DelayedInteraction : Interactable
{
    [SerializeField] private float _timeToInteract;
    private float _interactTimer;
    private bool _canActivate = false;
    private GameObject _interactionIndicatorObj;
    private Image _interactionIndicator;

    private PlayerMovement _playerMovement;


    public override void Awake()
    {
        base.Awake();
        _interactionIndicatorObj = GameObject.Find("InteractionIndicator");
        _interactionIndicator = _interactionIndicatorObj.GetComponent<Image>();
        _playerMovement = FindAnyObjectByType<PlayerMovement>();
    }

    public void Update()
    {
        if (BeingHeld)
        {

            //Move indicator wheel above object position and change the fill amount based on completion percent
            _interactionIndicatorObj.transform.position = new Vector2(transform.position.x, transform.position.y + 1.2f);
            _interactionIndicator.fillAmount = _interactTimer / _timeToInteract;

            _interactTimer += Time.deltaTime;

            //Activate interaction when time is reached
            if (_interactTimer > _timeToInteract)
            {
                _canActivate = true;
                OnInteract();
            }
                
        }
        else
        {
            _interactTimer = 0;
            _interactionIndicator.fillAmount = 0;
        }
    }

    public override void OnInteract()
    {
        if (_canActivate)
        {
            _canActivate = false;
            _interactTimer = 0;
            _interactionIndicator.fillAmount = 0;
            ActivateInteractions();
        }
    }
}
