using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    private AudioSource _audioSource;
    public float AudioClipLength
    {
        get { return _audioSource.clip.length;}
    }

    public event UnityAction<SoundEmitter> OnSoundFinishedPlaying;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }



    public void PlayAudioClip(AudioClip clip,AudioConfigurationSO settings,bool hasLoop,Vector3 position=default)
    {
        _audioSource.clip = clip;
        settings.ApplyTo(_audioSource);

        transform.position = position;

        _audioSource.loop = hasLoop;
        _audioSource.time = 0f;
        _audioSource.Play();

        if(!hasLoop)
        {
            //如果是非循环，则等待结束，结束时需要触发事件
            StartCoroutine(FinishedPlaying(clip.length));
        }
    }

    public void PlayAudioClip(AudioClip clip,AudioConfigurationSO settings,bool hasLoop,Transform parent)
    {
        _audioSource.clip = clip;
        settings.ApplyTo(_audioSource);

        transform.SetParent(parent);
        transform.position = Vector3.zero;

        _audioSource.loop = hasLoop;
        _audioSource.time = 0f;
        _audioSource.Play();

        if (!hasLoop)
        {
            //如果是非循环，则等待结束，结束时需要触发事件
            StartCoroutine(FinishedPlaying(clip.length));
        }
    }

    public void FadeMusicIn(AudioClip musicClip,AudioConfigurationSO settings,float duration,float startTime=0f )
    {
        PlayAudioClip(musicClip, settings, true);
        _audioSource.volume = 0f;

        if(startTime<=_audioSource.clip.length)
        {
            _audioSource.time = startTime;
        }
        _audioSource.DOFade(settings.Volume, duration);
    }

    public float FadeMusicOut(float duration)
    {
        _audioSource.DOFade(0f, duration).onComplete += OnFadeOutComplete;

        return _audioSource.time;
    }

    public void OnFadeOutComplete()
    {
        NotifyBeingDone();
    }

    public AudioClip GetClip()
    {
        return _audioSource.clip;
    }

    public void Resume()
    {
        _audioSource.Play();
    }

    public void Pause()
    {
        _audioSource.Pause();
    }

    public void Stop()
    {
        _audioSource.Stop();
    }

    public void Finish()
    {
        if(_audioSource.loop)
        {
            _audioSource.loop=false; 
            float timeRemaining=_audioSource.clip.length-_audioSource.time;
            StartCoroutine(FinishedPlaying(timeRemaining));
        }
    }

    public bool IsPlaying()
    {
        return _audioSource.isPlaying;
    }

    public bool IsLooping()
    {
        return _audioSource.loop;
    }

    private IEnumerator FinishedPlaying(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        NotifyBeingDone();
        
    }

    private void NotifyBeingDone()
    {
        OnSoundFinishedPlaying?.Invoke(this);
    }



}
