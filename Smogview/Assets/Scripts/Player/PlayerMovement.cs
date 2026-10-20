using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;

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
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
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

    /// <summary>
    /// Recieves the movement vector from the input map
    /// </summary>
    public void Move(InputAction.CallbackContext context)
    {
        _input = context.action.ReadValue<Vector2>();
        _moving = true;
    }

}

