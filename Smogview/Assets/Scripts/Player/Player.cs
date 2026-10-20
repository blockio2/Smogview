using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerMovement _movementController;
    private CircleCollider2D _interactionCollider;

    public void Awake()
    {
        _movementController = GetComponent<PlayerMovement>();
        _interactionCollider = GameObject.Find("PlayerInteractionHitbox").GetComponent<CircleCollider2D>();
    }


    public void Update()
    {

    }

}
