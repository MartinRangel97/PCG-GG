using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private PlayerInput playerInput;
    private MyPlayerActions inputActions;
    private PlayerStats playerStats;

    private float inputX;
    private float moveSpeed = 2f;

    private bool isGrounded = true;
    public int numberOfJumps;

    public float dashForce;
    public float startDashTimer;
    float currentDashTimer;
    float dashDirection;
    private bool isDashing;
    public int numberOfDashes;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        inputActions = new MyPlayerActions();
        inputActions.Player.Enable();
        playerStats = GetComponent<PlayerStats>();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            playerRB.velocity = transform.right * dashDirection * dashForce;
            currentDashTimer -= Time.deltaTime;

            if(currentDashTimer <= 0)
            {
                isDashing = false;
            }
        } 
        else
        {
            playerRB.velocity = new Vector2(inputX * moveSpeed, playerRB.velocity.y);
        }
    }

    public void OnJump()
    {
        if (isGrounded)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.y, 5f);
            numberOfJumps++;
        }
        else if (numberOfJumps < playerStats.Jumps)
        {
            playerRB.velocity = Vector2.zero;
            playerRB.velocity = new Vector2(playerRB.velocity.y, 5f);
            numberOfJumps++;
        }
    }

    public void OnMovement(InputValue inputValue)
    {
        inputX = inputValue.Get<Vector2>().x;
    }

    public void OnDashLeft()
    {
        if(numberOfDashes < playerStats.Dashes)
        {
            numberOfDashes++;
            isDashing = true;
            currentDashTimer = startDashTimer;
            playerRB.velocity = Vector2.zero;
            dashDirection = -1f;
        }
    }

    public void OnDashRight()
    {
        if (numberOfDashes < playerStats.Dashes)
        {
            numberOfDashes++;
            isDashing = true;
            currentDashTimer = startDashTimer;
            playerRB.velocity = Vector2.zero;
            dashDirection = 1f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Floor"))
        {
            isGrounded = true;
            numberOfJumps = 0;
            numberOfDashes = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Floor"))
        {
            numberOfDashes = 0;
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
