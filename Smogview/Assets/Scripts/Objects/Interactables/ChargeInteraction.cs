using UnityEngine;
using UnityEngine.UI;
using System;

public class ChargeInteraction : Interactable
{

    /// <summary>
    /// How many clicks before all interactions are activated
    /// </summary>
    [SerializeField] private int _clicksToActivate;
    public int _currentClicks = 0;

    InteractionIndicatorUI _interactionIndicator;


    public override void Awake()
    {
        base.Awake();
        _interactionIndicator = FindAnyObjectByType<InteractionIndicatorUI>();
    }

    public void Update()
    {
       
    }

    /// <summary>
    /// Increments clicks until fully charged, then activates interactions and resets
    /// </summary>
    public override void OnInteract()
    {
        _currentClicks++;

        _interactionIndicator.Active = true;
        //Move indicator wheel above object position and change the fill amount based on completion percent
        _interactionIndicator.Move(new Vector2(transform.position.x, transform.position.y + 1.2f));
        _interactionIndicator.SetFill((float)_currentClicks / (float)_clicksToActivate);
        if(_currentClicks >= _clicksToActivate)
        {
            ActivateInteractions();
            ResetCharge();
        }
    }

    public override void PlayerLeaves()
    {
        ResetCharge();
    }

    /// <summary>
    /// Clears the charge and resets the interaction indicators
    /// </summary>
    private void ResetCharge()
    {
        _currentClicks = 0;
        _interactionIndicator.SetFill(0);
        _interactionIndicator.Active = false;
    }
}
