using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class VolumeSet : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetLevel(float Value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Value) * 20);
    }
}
