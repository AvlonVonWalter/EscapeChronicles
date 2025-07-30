using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        if (visualCue != null)
        {
            visualCue.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Visual Cue GameObject is not assigned in the inspector.");
        }
    }

    private void Update()
    {
        if (playerInRange &&!DialogueManager.GetInstance().dialogueIsPlaying)
        {
            if (visualCue != null && !visualCue.activeSelf)
            {
                visualCue.SetActive(true);
            }
            if (InputManager.GetInstance().GetInteractPressed())
            {
                if (inkJSON != null)
                {
                    DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                }
                else
                {
                    Debug.LogWarning("Ink JSON is not assigned in the inspector.");
                }
            }
        }
        else
        {
            if (visualCue != null && visualCue.activeSelf)
            {
                visualCue.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}