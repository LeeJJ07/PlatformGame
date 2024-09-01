using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class SoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public string ClipName
    {
        get
        { 
           return audioSource.clip.name; 
        }
    }

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioMixerGroup audioMixerGroup, bool isLoop)
    {
        audioSource.outputAudioMixerGroup = audioMixerGroup;    
        audioSource.loop = isLoop;
        audioSource.Play();

        if (!isLoop) { StartCoroutine(DestroyWhenFinish(audioSource.clip.length)); }
    }

    public void SetClip(AudioClip clip)
    {
        audioSource.clip = clip;
    }

    private IEnumerator DestroyWhenFinish(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        Destroy(gameObject);
    }

}
