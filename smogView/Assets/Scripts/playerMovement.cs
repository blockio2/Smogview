using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class playerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float moveSpeed = 5f;
    private Vector2 input;
    private bool moving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(moving)
        {
            rb.MovePosition(rb.position + input * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            moving = false;
        }
        
    }


    public void Move(InputAction.CallbackContext context)
    {
        input = context.action.ReadValue<Vector2>();
        moving = true;
    }

}

