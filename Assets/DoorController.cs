using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    private Collider2D doorCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
        doorCollider = GetComponent<Collider2D>();
    }

    public void OpenDoor()
    {
        animator.SetBool("isOpen", true);
        doorCollider.enabled = false; // Disable collider to allow passage
    }

    public void CloseDoor()
    {
        animator.SetBool("isOpen", false);
        doorCollider.enabled = true; // Enable collider to block passage
    }

    public void CheckAndOpenDoor()
    {
        Debug.Log("CheckAndOpenDoor called. questionOne: " + GlobalManager.Instance.questionOne);
        if (GlobalManager.Instance.questionOne)
        {
            Debug.Log("Opening door");
            OpenDoor();
        }
    }
}