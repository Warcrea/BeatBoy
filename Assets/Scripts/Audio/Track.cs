using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Track : ScriptableObject {
    public string eventId;
    public AudioSource audioSource;
    public string audioClipName;
}
