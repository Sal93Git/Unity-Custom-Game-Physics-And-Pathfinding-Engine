using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class PerformanceTracker : MonoBehaviour
{
    // private long lastGCMemory;
    // private float lastGCTime;

    // void Start()
    // {
    //     lastGCMemory = Profiler.GetTotalReservedMemoryLong(); // Initial memory usage in bytes
    //     lastGCTime = Time.realtimeSinceStartup; // Initial time since startup

    //     // Log initial memory usage
    //     LogMemoryUsage();
    // }

    // void Update()
    // {
    //     // Check if it's time to record garbage collection
    //     if (Time.realtimeSinceStartup - lastGCTime >= 0.5f) // Record every half a second
    //     {
    //         // Calculate the current memory usage after garbage collection in bytes
    //         long currentMemory = Profiler.GetTotalReservedMemoryLong();

    //         // Calculate the change in memory usage since the last garbage collection
    //         long memoryDelta = currentMemory - lastGCMemory;

    //         // Log the memory usage if there's a change
    //         if (memoryDelta != 0)
    //         {
    //             // Convert memory values to bits
    //             long currentMemoryBits = currentMemory * 8;
    //             long memoryDeltaBits = memoryDelta * 8;

    //             // Log the memory usage in bits
    //             Debug.Log("Memory usage: " + currentMemoryBits.ToString("N0") + " bits (" + memoryDeltaBits.ToString("N0") + " bits since last update)");
    //         }

    //         // Update lastGCMemory and lastGCTime for the next recording
    //         lastGCMemory = currentMemory;
    //         lastGCTime = Time.realtimeSinceStartup;
    //     }
    // }

    // private void LogMemoryUsage()
    // {
    //     // Calculate the current memory usage in bytes
    //     long currentMemory = Profiler.GetTotalReservedMemoryLong();

    //     // Convert memory value to bits
    //     long currentMemoryBits = currentMemory * 8;

    //     // Log the initial memory usage in bits
    //     Debug.Log("Memory usage: " + currentMemoryBits.ToString("N0") + " bits (Initial)");
    // }

    //*****************************************************
    
    // float updateInterval = 0.5f; // Update interval in seconds
    // float lastInterval;
    // int frames;
    // float fps;
    // long memoryUsage; // Using long for memory usage to match Profiler.GetMonoUsedSizeLong()

    // void Start()
    // {
    //     lastInterval = Time.realtimeSinceStartup;
    //     frames = 0;
    // }

    // void Update()
    // {
    //     frames++;
    //     float timeNow = Time.realtimeSinceStartup;
    //     if (timeNow > lastInterval + updateInterval)
    //     {
    //         fps = frames / (timeNow - lastInterval);
    //         frames = 0;
    //         lastInterval = timeNow;

    //         // Track memory usage using Unity's Profiler
    //         memoryUsage = (long)Profiler.GetMonoUsedSizeLong() / (1024L * 1024L); // Convert to MB

    //         Debug.Log("FPS: " + fps.ToString("F2") + " | Memory Usage: " + memoryUsage.ToString() + " MB");
    //     }
    // }
    
    //***************************************
    float updateInterval = 0.5f; // Update interval in seconds
    float lastInterval;
    int frames;
    float fps;
    long highestMemoryUsage; // Track highest memory usage
    long memoryUsage;

    void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
        highestMemoryUsage = 0; // Initialize highest memory usage to 0
    }

    void Update()
    {
        frames++;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval)
        {
            fps = frames / (timeNow - lastInterval);
            frames = 0;
            lastInterval = timeNow;

            // Track memory usage using Unity's Profiler
            memoryUsage = (long)Profiler.GetMonoUsedSizeLong() / (1024L * 1024L); // Convert to MB

            // Check if current memory usage is higher than the previous highest
            if (memoryUsage > highestMemoryUsage)
            {
                highestMemoryUsage = memoryUsage;
                Debug.Log("New highest memory peak reached: " + highestMemoryUsage.ToString() + " MB");
            }
        }
    }
}
