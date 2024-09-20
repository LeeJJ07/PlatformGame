using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
public enum SoundType
{
    BGM,
    EFFECT,
}

public class SoundManager : MonoBehaviour
{

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float currentBGMVolume, currentEffectVolume;
    private Dictionary<string, AudioClip> clipsDictionary;
    private AudioClip[] audioClips;
    private List<SoundPlayer> loopSounds;
    public static SoundManager Instance;
    public float CurrentBGMVolume => currentBGMVolume;
    public float CurrentSFXVolume => currentEffectVolume;

    private void Start()
    {
        Instance = this;

        audioClips = Resources.LoadAll<AudioClip>("Sound");
        clipsDictionary = new Dictionary<string, AudioClip>();
        
        foreach (AudioClip clip in audioClips)
        {
            clipsDictionary[clip.name] = clip;
        }

        loopSounds = new List<SoundPlayer>();

        PlaySound("BlizzardCastle", true, SoundType.BGM);
        SetVolumes(currentBGMVolume, currentEffectVolume);
    }


    private AudioClip GetClip(string clipName)
    {
        AudioClip clip = clipsDictionary[clipName];

        if (clip == null) { Debug.LogWarning(clipName + "이 존재하지 않습니다."); }

        return clip;
    }

    public void StopLoopSound(string clipName)
    {
        foreach (SoundPlayer audioPlayer in loopSounds)
        {
            if (audioPlayer == null)
                continue;
            
            if (audioPlayer.ClipName == clipName)
            {
                loopSounds.Remove(audioPlayer);
                Destroy(audioPlayer.gameObject);
                return;
            }
        }
    }

    public void ClearLoop()
    {
        foreach (SoundPlayer audioPlayer in loopSounds)
        {
            Destroy(audioPlayer.gameObject);
        }
        loopSounds.Clear();
    }
    public void PlaySound(string clipName, bool isLoop = false, SoundType type = SoundType.EFFECT)
    {
        GameObject obj = new GameObject(clipName + "Sound");
        obj.transform.SetParent(transform);
        obj.AddComponent<AudioSource>();
        SoundPlayer soundPlayer = obj.AddComponent<SoundPlayer>();
        
        if (isLoop) { loopSounds.Add(soundPlayer); }

        soundPlayer.SetClip(GetClip(clipName));
        soundPlayer.Play(audioMixer.FindMatchingGroups(type.ToString())[0], isLoop);
    }

    public void SetVolumes(float bgm, float effect)
    {
        SetVolume(SoundType.BGM, bgm);
        SetVolume(SoundType.EFFECT, effect);
    }

    public void SetVolume(SoundType type, float value)
    {
        audioMixer.SetFloat(type.ToString(), value);
    }

}
