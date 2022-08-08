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
    public float gravityScale;

    public bool isGrounded = true;
    public int numberOfJumps;
    public float jumpForce;

    public float dashForce;
    public float startDashTimer;
    float currentDashTimer;
    float dashDirection;
    private bool isDashing;
    public int numberOfDashes;

    public bool isFalling;
    public bool isGliding = false;
    public float glideTimer;

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

        if(playerRB.velocity.y < 0)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false; 
        }

        if (isFalling)
        {
            if (isGliding && glideTimer <= playerStats.Glide )
            {
                playerRB.gravityScale = .1f;
                glideTimer += Time.deltaTime;
            }
            else
            {
                playerRB.gravityScale = gravityScale;
            }
        }
        
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        if (context.performed)
        {
            if (isGrounded)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.y, jumpForce);
                isGliding = false;
                numberOfJumps++;
            }
            else if (numberOfJumps < playerStats.Jumps)
            {
                playerRB.gravityScale = gravityScale;
                playerRB.velocity = Vector2.zero;
                isGliding = false;
                playerRB.velocity = new Vector2(playerRB.velocity.y, jumpForce);
                numberOfJumps++;
            }
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        inputX = context.ReadValue<Vector2>().x * 3;
    }

    public void OnDashLeft(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        if (numberOfDashes < playerStats.Dashes && !isDashing)
        {
            numberOfDashes++;
            isDashing = true;
            currentDashTimer = startDashTimer;
            playerRB.velocity = Vector2.zero;
            dashDirection = -1f;
        }
    }

    public void OnDashRight(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        if (numberOfDashes < playerStats.Dashes && !isDashing)
        {
            numberOfDashes++;
            isDashing = true;
            currentDashTimer = startDashTimer;
            playerRB.velocity = Vector2.zero;
            dashDirection = 1f;
        }
    }

    public void OnGlide(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isGliding = true;
        }

        if (context.canceled)
        {
            isGliding = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Floor"))
        {
            isGrounded = true;
            numberOfJumps = 0;
            numberOfDashes = 0;
            isGliding = false;
            playerRB.gravityScale = gravityScale;
            glideTimer = 0;
        }

        if (collision.gameObject.name.Equals("Wall"))
        {
            inputX = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Floor"))
        {
            numberOfDashes = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Floor"))
        {
            isGrounded = false;
        }
    }
}
