using System.Collections;
using System.Collections.Generic;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoSingleton<AudioManager>
{
    public AudioMixer audioMixer;
    
    public void MuteAudioGroup(bool value)
    {
        audioMixer.SetFloat("BaseVolume", !value ? 0f : -80f);
    }
}
