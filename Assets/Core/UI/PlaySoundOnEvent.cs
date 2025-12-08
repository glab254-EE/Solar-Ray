using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnEvent : MonoBehaviour
{
    [field:SerializeField]
    private List<AudioClip> sounds;
    private AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void PlaySound(int id)
    {
        if (sounds.Count > id && id >= 0 && sounds[id] != null)
        {
            source.clip = sounds[id];
            source.Play();
        }
        else if (sounds.Count == 1)
        {
            source.clip = sounds[0];
            source.Play();
        }
    }
}
