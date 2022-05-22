using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    private Vector2 movementInput;
    private SpriteRenderer playerRenderer;

    private Rigidbody2D rb;

    private Animator playerAnimator;

    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        // If movement input is not 0, try to move
        if(movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);

            // If movement was not successful, try to move in only x direction.
            if(!success){
                success = TryMove(new Vector2(movementInput.x, 0));

                // If x movement was not successful, try to move in only y direction.
                if(!success){
                    success = TryMove(new Vector2(0, movementInput.y));
                }
            }

            playerAnimator.SetBool("IsMoving", success);
        }else{
            playerAnimator.SetBool("IsMoving", false);
        }

        // Set direction of sprite to movement direction
        if(movementInput.x < 0){
            playerRenderer.flipX = true;
        }else if(movementInput.x > 0){
            playerRenderer.flipX = false;
        }
    }

    private void OnMove(InputValue movementValue)
    {
        // Grab the input that was pressed and store for later processing.
        movementInput = movementValue.Get<Vector2>();
    }

    private bool TryMove(Vector2 direction)
    {
        // Can't move if there is no direction to move in.
        if(direction == Vector2.zero){
            return false;
        }

        // Check for potential collisions.
        int count = rb.Cast(
            direction, // X and U vlaues between -1 and 1 that represent the direction from the body to look for collisions.
            movementFilter, // The settings that determine where a collision can occur on such as layers to collide with.
            castCollisions, // List of collisions to store and found collisions into after the Cast is finished.
            moveSpeed * Time.fixedDeltaTime + collisionOffset // The amount to cast equal to the movement plus an offset.
        );

        if(count == 0){
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }

        return false;
    }
}
