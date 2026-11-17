using UnityEngine;

public class Clutter : ChargeInteraction
{
    private Player _player;

    public override void Awake()
    {
        base.Awake();

        _player = FindAnyObjectByType<Player>();
    }

    private void OnDestroy()
    {
        _player.ClutterCleaned++;
    }
}
