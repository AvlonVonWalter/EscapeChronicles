using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // Input fields
    private Vector2 moveDirection = Vector2.zero;
    private bool jumpPressed = false;
    private bool interactPressed = false;
    private bool submitPressed = false;

    // Movement and physics parameters
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    bool canMove = true;

    bool isFacingUp = false;
    bool isFacingRight = true;
    bool isFacingLeft = false;

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    private static PlayerController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one PlayerController in the scene.");
        }
        instance = this;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }
        if (canMove)
        {
            bool success = false;

            if (moveDirection != Vector2.zero)
            {
                success = TryMove(moveDirection);

                if (!success)
                {
                    success = TryMove(new Vector2(moveDirection.x, 0));
                }

                if (!success)
                {
                    success = TryMove(new Vector2(0, moveDirection.y));
                }

                UpdateFacingDirection(moveDirection);
            }

            UpdateAnimator(success);
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = rb.Cast(
                direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset
            );

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void UpdateAnimator(bool isMoving)
    {
        animator.SetBool("isMovingRight", moveDirection.x > 0 && isMoving);
        animator.SetBool("isMovingLeft", moveDirection.x < 0 && isMoving);
        animator.SetBool("isMovingUp", moveDirection.y > 0 && isMoving);
        animator.SetBool("isMovingDown", moveDirection.y < 0 && isMoving);

        animator.SetBool("isFacingUp", isFacingUp);
        animator.SetBool("isFacingRight", isFacingRight);
        animator.SetBool("isFacingLeft", isFacingLeft);
    }

    private void UpdateFacingDirection(Vector2 direction)
    {
        if (direction.x > 0)
        {
            isFacingRight = true;
            isFacingLeft = false;
            isFacingUp = false;
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            isFacingRight = false;
            isFacingLeft = true;
            isFacingUp = false;
            spriteRenderer.flipX = true;
        }
        else if (direction.y > 0)
        {
            isFacingRight = false;
            isFacingLeft = false;
            isFacingUp = true;
        }
        else if (direction.y < 0)
        {
            isFacingRight = false;
            isFacingLeft = false;
            isFacingUp = false;
        }
    }

    void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }

    public void SwordAttack()
    {
        LockMovement();

        if (isFacingRight)
        {
            swordAttack.AttackRight();
        }
        else if (isFacingLeft)
        {
            swordAttack.AttackLeft();
        }
        else if (isFacingUp)
        {
            swordAttack.AttackUp();
        }
        else
        {
            swordAttack.AttackDown();
        }
    }

    public void EndSwordAttack()
    {
        swordAttack.StopAttack();
        UnlockMovement();
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    // Input action methods
    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            moveDirection = Vector2.zero;
        }
    }

    public void JumpPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
        }
        else if (context.canceled)
        {
            jumpPressed = false;
        }
    }

    public void InteractButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactPressed = true;
        }
        else if (context.canceled)
        {
            interactPressed = false;
        }
    }

    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
        }
        else if (context.canceled)
        {
            submitPressed = false;
        }
    }

    // Input getters
    public Vector2 GetMoveDirection() 
    {
        return moveDirection;
    }

    public bool GetJumpPressed() 
    {
        bool result = jumpPressed;
        jumpPressed = false;
        return result;
    }

    public bool GetInteractPressed() 
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
    }

    public bool GetSubmitPressed() 
    {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }
}
