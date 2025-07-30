using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;

    Vector2 movementInput;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    bool canMove = true;

    bool isFacingUp = false;
    bool isFacingRight = true;
    bool isFacingLeft = false;

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if(DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }
        if (canMove)
        {
            bool success = false;

            if (movementInput != Vector2.zero)
            {
                success = TryMove(movementInput);

                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));
                }

                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }

                UpdateFacingDirection(movementInput);
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

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    private void UpdateAnimator(bool isMoving)
    {
        animator.SetBool("isMovingRight", movementInput.x > 0 && isMoving);
        animator.SetBool("isMovingLeft", movementInput.x < 0 && isMoving);
        animator.SetBool("isMovingUp", movementInput.y > 0 && isMoving);
        animator.SetBool("isMovingDown", movementInput.y < 0 && isMoving);

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
}