using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    public bool questionOne = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetQuestionOne(bool value)
    {
        questionOne = value;
        Debug.Log("questionOne set to: " + questionOne);
    }
}