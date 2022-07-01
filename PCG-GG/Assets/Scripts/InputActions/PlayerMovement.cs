using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D PlayerRB;
    private PlayerInput playerInput;
    private MyPlayerActions inputActions;

    private float inputX;
    private float moveSpeed = 2f;

    private void Awake()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        inputActions = new MyPlayerActions();
        inputActions.Player.Enable();
        inputActions.Player.Jump.performed += Jump;
    }

    private void FixedUpdate()
    {
        PlayerRB.velocity = new Vector2(inputX * moveSpeed, PlayerRB.velocity.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        PlayerRB.velocity = new Vector2(PlayerRB.velocity.y, 5f);
    }

    public void Move(InputAction.CallbackContext context)
    {
        inputX = context.ReadValue<Vector2>().x;
    }
}
