using System;
using UnityEngine;

[System.Serializable]
public class ToggleObject : Interaction
{
    [SerializeField] private GameObject _linkedObject;
    public override void Behaviour()
    {
        _linkedObject.SetActive(!_linkedObject.activeSelf);
    }

}
