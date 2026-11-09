using System;
using System.Data;
using UnityEngine;

/// <summary>
/// Destroys the game object when called
/// </summary>
[System.Serializable]
public class RemoveObject : Interaction
{
    [SerializeField] private GameObject _removedObject;
    private bool _removed = false;

    private void Awake()
    {
        if (!_removedObject)
            _removedObject = gameObject;
    }

    public override void Behaviour()
    {
        if (!_removed)
        {
            Destroy(_removedObject);
            _removed = true;
        }
            
    }
}
