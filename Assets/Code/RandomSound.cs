using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomSound : MonoBehaviour
{

    public AudioClip[] clips;

    public AudioClip GetRandomClip()
    {
        int n = Random.Range(0, clips.Length);
        return clips[n];

    }
}
