using UnityEngine;
using System.Collections;

public class Button : Interactable
{
    private SpriteRenderer _spriteRenderer;

    public override void Awake()
    {
        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();   
    }

    public override void OnInteract()
    {
        base.OnInteract();

        _spriteRenderer.color = Color.red;

        StartCoroutine("ColorChange", 1.0f);
    }

    public void Update()
    {
    }

    IEnumerator ColorChange(float resetTime)
    {
        yield return new WaitForSeconds(resetTime);
        _spriteRenderer.color = Color.white;
    }
}
