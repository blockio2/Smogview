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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(MoveDirection);
        if(_moving)
        {
            _rb.MovePosition(_rb.position + _input * _moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            _moving = false;
        }
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.action.ReadValue<Vector2>();
        _moving = true;
    }

}

