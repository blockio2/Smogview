using System;
using System.Data;
using UnityEngine;

/// <summary>
/// Destroys the game object when called
/// </summary>
[System.Serializable]
public class RemoveBehavior : Interaction
{
    [SerializeField] private Interaction _removedInteraction;
    private bool _removed = false;

    public override void Behaviour()
    {
        if (!_removed)
        {
            Destroy(_removedInteraction);
            _removed = true;
        }
            
    }
}
