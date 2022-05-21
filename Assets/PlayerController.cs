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

    private Rigidbody2D rb;

    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        }
    }

    private void OnMove(InputValue movementValue)
    {
        // Grab the input that was pressed and store for later processing.
        movementInput = movementValue.Get<Vector2>();
    }

    private bool TryMove(Vector2 direction){
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
