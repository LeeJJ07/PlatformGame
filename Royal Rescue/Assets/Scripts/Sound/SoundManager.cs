using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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
    [SerializeField] private AudioClip[] audioClips;
    private List<SoundPlayer> loopSounds;
    public static SoundManager Instance;

    private void Start()
    {
        Instance = this;

        clipsDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in audioClips)
        {
            clipsDictionary.Add(clip.name, clip);
        }

        loopSounds = new List<SoundPlayer>();

        PlaySound("BlizzardCastle", true, SoundType.BGM);
        SetVolumes(currentBGMVolume, currentEffectVolume);
    }


    private AudioClip GetClip(string clipName)
    {
        AudioClip clip = clipsDictionary[clipName];

        if (clip == null) { Debug.LogError(clipName + "�� �������� �ʽ��ϴ�."); }

        return clip;
    }

    public void StopLoopSound(string clipName)
    {
        foreach (SoundPlayer audioPlayer in loopSounds)
        {
            if (audioPlayer.ClipName == clipName)
            {
                loopSounds.Remove(audioPlayer);
                Destroy(audioPlayer.gameObject);
                return;
            }
        }

        Debug.LogError(clipName + "�� ã�� �� �����ϴ�.");
    }

    public void PlaySound(string clipName, bool isLoop = false, SoundType type = SoundType.EFFECT)
    {
        GameObject obj = new GameObject(clipName + "Sound");
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
