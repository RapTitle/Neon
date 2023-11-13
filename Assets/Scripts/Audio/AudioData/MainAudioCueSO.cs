using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Audio/MainAudio")]
public class MainAudioCueSO : AudioCueSO
{
    //¸üÐÂAudio
    public bool SetMainAudio(AudioClip clip)
    {
        if (audioClipsGroups.Length > 0)
        {
            audioClipsGroups[0].audioClips[0] = clip;
            return true;
        }
        Debug.LogError("main audio is null");
        return false;
    }
}
