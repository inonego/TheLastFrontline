using System.Collections;
using System.Collections.Generic;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoSingleton<AudioManager>
{
    public AudioMixer AudioMixer;
    
    public void MuteAudioGroup(bool value)
    {
        if (AudioMixer == null) return;

        AudioMixer.SetFloat("BaseVolume", !value ? 0f : -80f);
    }
}
