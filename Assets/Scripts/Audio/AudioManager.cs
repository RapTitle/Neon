using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : Singleton<AudioManager>
{


    private SoundEmitterVault soundEmitterVault;
    private SoundEmitterPool soundEmitterPool;
    
    [SerializeField] private int initSize;

    protected override void Awake()
    {
        base.Awake();
        soundEmitterPool = GetComponent<SoundEmitterPool>();
        soundEmitterPool.SetParent(transform);
        soundEmitterPool.Prewarm(initSize);

        soundEmitterVault = new SoundEmitterVault();
    }

    public AudioCueKey PlayAudioCue(AudioCueSO audioCue,AudioConfigurationSO settings,Vector3 position=default)
    {
        AudioClip[] clipToPlay=audioCue.GetClips();

        SoundEmitter[] soundEmitters=new SoundEmitter[clipToPlay.Length];
      
        AudioCueKey audioCueKey = soundEmitterVault.Add(audioCue, soundEmitters);


        int nOfClips=clipToPlay.Length;
        int maxClipIndex = 0;
        int clipMaxLength = 0;

        for(int i=0; i<clipToPlay.Length; i++)
        {
            soundEmitters[i] = soundEmitterPool.Request();

            if (soundEmitters[i] != null)
            {
                soundEmitters[i].PlayAudioClip(clipToPlay[i], settings, audioCue.looping, position);
               
                if(!audioCue.looping)
                {
                    
                    soundEmitters[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
                }
            }

            if (clipToPlay[i].length>clipMaxLength)
            {
                maxClipIndex = i;
            }

            soundEmitters[i].OnSoundFinishedPlaying += (s) =>
            {
                OnAudioCueFinishedPlaying(audioCueKey);
            };
        }

        //
        //在这一组播放完后才进行把Vault中的KeyRemove

        return audioCueKey;
    }


    public AudioCueKey PlayAudioCue(AudioCueSO audioCue, AudioConfigurationSO settings, Transform parent)
    {
        AudioClip[] clipToPlay = audioCue.GetClips();

        SoundEmitter[] soundEmitters = new SoundEmitter[clipToPlay.Length];

        AudioCueKey audioCueKey = soundEmitterVault.Add(audioCue, soundEmitters);


        int nOfClips = clipToPlay.Length;
        int maxClipIndex = 0;
        int clipMaxLength = 0;

        for (int i = 0; i < clipToPlay.Length; i++)
        {
            soundEmitters[i] = soundEmitterPool.Request();

            if (soundEmitters[i] != null)
            {
                soundEmitters[i].PlayAudioClip(clipToPlay[i], settings, audioCue.looping, parent);

                if (!audioCue.looping)
                {

                    soundEmitters[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
                }
            }

            if (clipToPlay[i].length > clipMaxLength)
            {
                maxClipIndex = i;
            }

            soundEmitters[i].OnSoundFinishedPlaying += (s) =>
            {
                OnAudioCueFinishedPlaying(audioCueKey);
            };
        }
        return audioCueKey;
    }

    //Finish，在播放完毕后关闭声音
    public bool FinishAudioCue(AudioCueKey audioCueKey)
    {
        bool isFound = soundEmitterVault.Get(audioCueKey, out SoundEmitter[] soundEmitters);

        if(isFound)
        {
            for (int i=0;i<soundEmitters.Length;i++)
            {
                float clipMaxLength = 0;
                
                soundEmitters[i].Finish();
                soundEmitters[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;

                if (soundEmitters[i].AudioClipLength > clipMaxLength)
                {
                    soundEmitters[i].OnSoundFinishedPlaying += (s) =>
                    {
                        OnAudioCueFinishedPlaying(audioCueKey);
                    };
                }
            }
        }
        return isFound;
    }

    public bool StopAudioCue(AudioCueKey audioCueKey)
    {
        bool isFound=soundEmitterVault.Get(audioCueKey,out SoundEmitter[] soundEmitters);

        if(isFound)
        {
            for(int i=0; i<soundEmitters.Length;i++)
            {
                OnSoundEmitterFinishedPlaying(soundEmitters[i]);
            }

            soundEmitterVault.Remove(audioCueKey);
        }
        return isFound;
    }


    private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
    {
        if(!soundEmitter.IsLooping())
        {
            soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;
        }
        soundEmitter.Stop();
        soundEmitterPool.Return(soundEmitter);
       
    }

    private void OnAudioCueFinishedPlaying(AudioCueKey key)
    {
        soundEmitterVault.Remove(key);
    }


}


