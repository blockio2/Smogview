using UnityEngine;

public class SlowPlayer : MonoBehaviour
{
    [SerializeField] private float _speedMultiplier;
    private PlayerMovement _playerMovement;

    public float SpeedMultiplier { get; }

    public void Awake()
    {
        _playerMovement = FindAnyObjectByType<PlayerMovement>();
    }

    /// <summary>
    /// Adds this obstacles speed modifier to the player
    /// </summary>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "PlayerCollisionHitbox")
        {
            _playerMovement.AddSpeedMultiplier(_speedMultiplier);
        }
    }

    /// <summary>
    /// Removes this obstacles speed modifier from the player
    /// </summary>
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "PlayerCollisionHitbox")
        {
            _playerMovement.RemoveSpeedMultiplier(_speedMultiplier);
        }
    }
}
