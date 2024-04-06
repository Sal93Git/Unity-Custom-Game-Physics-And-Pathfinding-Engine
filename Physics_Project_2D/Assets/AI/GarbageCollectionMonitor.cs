using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class GarbageCollectionMonitor : MonoBehaviour
{
    private long lastGCMemory;
    private float lastGCTime;

    void Start()
    {
        lastGCMemory = Profiler.GetTotalReservedMemoryLong(); // Initial memory usage in bytes
        lastGCTime = Time.realtimeSinceStartup; // Initial time since startup

        // Log initial memory usage
        LogMemoryUsage();
    }

    void Update()
    {
        // Check if it's time to record garbage collection
        if (Time.realtimeSinceStartup - lastGCTime >= 0.5f) // Record every half a second
        {
            // Calculate the current memory usage after garbage collection in bytes
            long currentMemory = Profiler.GetTotalReservedMemoryLong();

            // Calculate the change in memory usage since the last garbage collection
            long memoryDelta = currentMemory - lastGCMemory;

            // Log the memory usage if there's a change
            if (memoryDelta != 0)
            {
                // Convert memory values to bits
                long currentMemoryBits = currentMemory * 8;
                long memoryDeltaBits = memoryDelta * 8;

                // Log the memory usage in bits
                Debug.Log("Memory usage: " + currentMemoryBits.ToString("N0") + " bits (" + memoryDeltaBits.ToString("N0") + " bits since last update)");
            }

            // Update lastGCMemory and lastGCTime for the next recording
            lastGCMemory = currentMemory;
            lastGCTime = Time.realtimeSinceStartup;
        }
    }

    private void LogMemoryUsage()
    {
        // Calculate the current memory usage in bytes
        long currentMemory = Profiler.GetTotalReservedMemoryLong();

        // Convert memory value to bits
        long currentMemoryBits = currentMemory * 8;

        // Log the initial memory usage in bits
        Debug.Log("Memory usage: " + currentMemoryBits.ToString("N0") + " bits (Initial)");
    }
}
