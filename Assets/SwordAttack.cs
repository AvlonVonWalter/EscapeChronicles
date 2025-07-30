using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider2D swordCollider;
    public float damage = 3;

    // Serialized fields for editable offsets
    [SerializeField] private Vector2 rightAttackOffset = new Vector2(0.5f, 0f);
    [SerializeField] private Vector2 upAttackOffset = new Vector2(0.3f, 0.3f);
    [SerializeField] private Vector2 downAttackOffset = new Vector2(0.3f, -0.3f);

    private Vector2 originalPosition;

    private void Start()
    {
        // Save initial position as original position
        originalPosition = transform.localPosition;

        // Log initial offsets for verification
        
    }

    public void AttackRight()
    {
        swordCollider.enabled = true;
        transform.localPosition = originalPosition + rightAttackOffset;
        swordCollider.offset = rightAttackOffset;
    }

    public void AttackLeft()
    {
        swordCollider.enabled = true;
        transform.localPosition = originalPosition; // Reset to original position
        swordCollider.offset = Vector2.zero; // Reset offset
    }

    public void AttackUp()
    {

        swordCollider.enabled = true;
        transform.localPosition = originalPosition + upAttackOffset;
        swordCollider.offset = upAttackOffset;
    }

    public void AttackDown()
    {
        swordCollider.enabled = true;
        transform.localPosition = originalPosition + downAttackOffset;
        swordCollider.offset = downAttackOffset;
    }

    public void StopAttack()
    {
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.Health -= damage;
            }
        }
    }

    // Visualize the collider's position in the Scene view
    private void OnDrawGizmos()
    {
        if (swordCollider != null)
        {
            Gizmos.color = swordCollider.enabled ? Color.red : Color.green;
            Gizmos.DrawWireCube(transform.position, swordCollider.bounds.size);
        }
    }
}