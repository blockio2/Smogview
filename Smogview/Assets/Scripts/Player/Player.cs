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
        switch (_movementController.MoveDirection)
        {
            case Global.Direction.Right:
                transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            case Global.Direction.Up:
                transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case Global.Direction.Left:
                transform.eulerAngles = new Vector3(0, 0, 180);
                break;
            case Global.Direction.Down:
                transform.eulerAngles = new Vector3(0, 0, 270);
                break;
        }
    }

}
