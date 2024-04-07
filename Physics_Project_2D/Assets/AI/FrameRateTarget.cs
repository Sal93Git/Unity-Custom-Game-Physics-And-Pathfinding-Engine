using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateTarget : MonoBehaviour
{
    public int targetFrameRate = 60; // Set your desired target frame rate here

    void Start()
    {
        // Set the target frame rate
        Application.targetFrameRate = targetFrameRate;
    }
}
