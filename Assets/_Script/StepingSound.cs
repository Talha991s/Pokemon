using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepingSound : MonoBehaviour
{
    [SerializeField]
    AudioClip[] footstepclips;

    [SerializeField]
    AudioSource stepSource;


    [SerializeField]
    float frequencyvariance = 0.1f;

    public void Footstep()
    {
        stepSource.clip = footstepclips[0];
        stepSource.pitch = Random.Range(1.0f - frequencyvariance, 1.0f + frequencyvariance);
        stepSource.Play();
    }

 
}
