using System;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class OpenDoor : Interaction
{
    private Collider2D _doorCollider;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private bool _open = false;
    [SerializeField] private Sprite _closedSprite;
    [SerializeField] private Sprite _openSprite;

    private void Awake()
    {
        _doorCollider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_open)
        {
            
        }
    }

    public override void Behaviour()
    {
        _open = !_open;
        if (_open)
            Open();
        else
            Close();

    }

    public void Open()
    {
        _doorCollider.enabled = false;
        _spriteRenderer.sprite = _openSprite;
    }

    public void Close()
    {
        _doorCollider.enabled = true;
        _spriteRenderer.sprite = _closedSprite;
    }

}
