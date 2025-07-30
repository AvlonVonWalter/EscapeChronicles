using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;

    void Start()
    {
        // Play the audio clip immediately when the scene starts
        source.PlayOneShot(clip);
    }
}