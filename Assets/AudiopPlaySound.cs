using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudiopPlaySound : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        audioSource.Play();
    }

}
