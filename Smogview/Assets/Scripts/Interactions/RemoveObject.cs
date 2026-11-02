using System.Data;
using UnityEngine;

/// <summary>
/// Destroys the game object when called
/// </summary>
[System.Serializable]
public class RemoveOnHold : Interaction
{
    public override void Behaviour()
    {
        Destroy(this.gameObject);
    }
}
