using System;
using System.Security.Cryptography;
using UnityEngine;

public class DisableOnLens : MonoBehaviour
{
    private LensManager _lensSwitching;
    [SerializeField] private LensManager.lens _lens;

    private Vector2 inactivePosition = new Vector2(1000, 1000);
    private Vector2 startingPosition;

    private void Awake()
    {
        _lensSwitching = FindAnyObjectByType<LensManager>();
        startingPosition = transform.position;
    }

    public void Update()
    {
        if (_lensSwitching.CurrentLens == _lens)
        {
            transform.position = inactivePosition;
        }
        else
        {
            transform.position = startingPosition;
        }
    }
}
