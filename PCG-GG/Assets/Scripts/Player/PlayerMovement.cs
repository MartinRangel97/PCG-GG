using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D PlayerRB;
    private PlayerInput playerInput;
    private MyPlayerActions inputActions;
    private PlayerStats playerStats;

    private float inputX;
    private float moveSpeed = 2f;

    private bool isGrounded = true;
    private int numberOfJumps;

    private void Awake()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        inputActions = new MyPlayerActions();
        inputActions.Player.Enable();
        inputActions.Player.Jump.performed += Jump;
        playerStats = GetComponent<PlayerStats>();
    }

    private void FixedUpdate()
    {
        PlayerRB.velocity = new Vector2(inputX * moveSpeed, PlayerRB.velocity.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            PlayerRB.velocity = new Vector2(PlayerRB.velocity.y, 5f);
            numberOfJumps++;
        } else if (numberOfJumps < playerStats.Jumps)
        {
            PlayerRB.velocity = Vector2.zero;
            PlayerRB.velocity = new Vector2(PlayerRB.velocity.y, 5f);
        }
           
    }

    public void Move(InputAction.CallbackContext context)
    {
        inputX = context.ReadValue<Vector2>().x;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Floor"))
        {
            isGrounded = true;
            numberOfJumps = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Floor"))
        {
            isGrounded = false;
        }
    }
}
