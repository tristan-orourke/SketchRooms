using System;
using UnityEngine;

public class VolumeRolloff : MonoBehaviour
{
    public float maxVolume;
    public float minVolume;
    private float minDistance;
    private float maxDistance;
    AudioSource audioSource;
    AudioListener listener;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        listener = FindObjectOfType<AudioListener>();
        minDistance = audioSource.minDistance;
        maxDistance = audioSource.maxDistance;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, listener.transform.position);
        audioSource.volume = CalculateVolumeRolloff(distance);
    }

    private float CalculateVolumeRolloff(float distance)
    {
        if (distance < minDistance)
        {
            return maxVolume;
        } else if (distance > maxDistance)
        {
            return minVolume;
        } else
        {
            float dVolume = maxVolume - minVolume;
            float dDistance = maxDistance - minDistance;
            return maxVolume - (dVolume) * (distance - minDistance) / dDistance;
        }
    }
}
