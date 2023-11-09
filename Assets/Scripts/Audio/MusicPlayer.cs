using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioConfigurationSO audioConfig = default;
    [SerializeField] private AudioCueSO audioCue = default;

    private AudioCueKey audioCueKey = default;

    public void AudioPlay()
    {
        audioCueKey=AudioManager.Instance.PlayAudioCue(audioCue, audioConfig,transform);
    }

    public void StopAudio()
    {
        AudioManager.Instance.StopAudioCue(audioCueKey);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J)) 
        {
            StopAudio();
        }
    }

}
