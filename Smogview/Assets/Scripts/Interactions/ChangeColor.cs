using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class ChangeColor : Interaction
{
    //Color field perhaps 
    //Field for duration as well

    private SpriteRenderer _spriteRenderer;

    /// <summary>
    /// Changes color and resets it after a second
    /// </summary>
    public override void Behaviour()
    {
        _spriteRenderer = GetComponentInParent<SpriteRenderer>();
        _spriteRenderer.color = Color.red;

        StartCoroutine("ColorChange", 1.0f);
    }

    IEnumerator ColorChange(float resetTime)
    {
        yield return new WaitForSeconds(resetTime);
        _spriteRenderer.color = Color.white;
    }

}
