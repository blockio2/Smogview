using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;

    private CircleCollider2D _interactionCollider;

    [SerializeField] private float _moveSpeed = 5f;
    private Vector2 _input;
    private bool _moving = false;

    
    /// <summary>
    /// Returns the direction the player is moving in based on the input vector
    /// </summary>
    public Global.Direction MoveDirection
    {
        get
        {
            if(_input.x == -1)
            {
                return Global.Direction.Left;
            }
            else if (_input.x == 1)
            {
                return Global.Direction.Right;
            }
            else if (_input.y == 1)
            {
                return Global.Direction.Up;
            }
            else if (_input.y == -1)
            {
                return Global.Direction.Down;
            }
            else
            {
                return Global.Direction.None;
            }
        }
    }
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _interactionCollider = transform.Find("InteractionHitbox").GetComponent<CircleCollider2D>();
    }

    /// <summary>
    /// Moves the rigidbody and rotates the object based on move direction
    /// </summary>
    void FixedUpdate()
    {
        //Moving
        if(_moving)
        {
            _rb.MovePosition(_rb.position + _input * _moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            _moving = false;
        }


        //Rotate player based on move direction
        switch (MoveDirection)
        {
            case Global.Direction.Right:
                _interactionCollider.transform.localPosition = new Vector3(0.2f, 0f, 2);
                break;
            case Global.Direction.Up:
                _interactionCollider.transform.localPosition = new Vector3(0.0f, 0.2f, 2);
                break;
            case Global.Direction.Left:
                _interactionCollider.transform.localPosition = new Vector3(-0.2f, 0f, 2);
                break;
            case Global.Direction.Down:
                _interactionCollider.transform.localPosition = new Vector3(0.0f, -0.2f, 2);
                break;
        }

    }

    /// <summary>
    /// Recieves the movement vector from the input map
    /// </summary>
    public void Move(InputAction.CallbackContext context)
    {
        _input = context.action.ReadValue<Vector2>();
        _moving = true;
    }

}

