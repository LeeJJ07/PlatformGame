using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSound : MonoBehaviour
{
    SoundManager soundManager;
    private void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }
    public void SoundEffect(string name)
    {
        soundManager.PlaySound(name);
    }
}
