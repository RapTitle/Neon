using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioVisualization : MonoBehaviour
{

    [SerializeField] private MainAudioCueSO _audioCueSO;
    [SerializeField] private AudioConfigurationSO _configurationSO;

    private AudioCueKey _audioCueKey;
    private AudioSource[] _audioSources;

    private float[] sampleData = new float[64];   
    public float[] normalizedData = new float[64];

    private Coroutine _dataAcquisitionCoroutine;

    private void Start()
    {
        Initialize(_audioCueSO,_configurationSO);
        //StartCoroutine(DataAcquisitionCoroutine());
    }

    public void Initialize(AudioCueSO audioCue,AudioConfigurationSO settings)
    {
        _audioCueKey = AudioManager.Instance.PlayAudioCue(audioCue, settings, transform);

        _audioSources=GetComponentsInChildren<AudioSource>();
    }

    public void ToggleOn(bool on)
    {
        if(!on)
        {
            if (_dataAcquisitionCoroutine != null)
            {
                StopCoroutine(_dataAcquisitionCoroutine);
                _dataAcquisitionCoroutine = null;
            }
        }  
        else
            _dataAcquisitionCoroutine = StartCoroutine(DataAcquisitionCoroutine());
    }

    public void StopMusic()
    {
        //音乐渐入
        AudioManager.Instance.StopAudioCue(_audioCueKey);
        for(int i=0;i<_audioSources.Length;i++)
        {
            _audioSources[i]=null;
        }
    }

    public void MusicFadeOut(AudioClip clip)
    {
        _audioCueSO.SetMainAudio(clip);

        AudioManager.Instance.PlayAudioCue(_audioCueSO, _configurationSO, transform);
        _audioSources = GetComponentsInChildren<AudioSource>();

        for(int i=0;i<_audioSources.Length; i++)
        {
            _audioSources[i].DOFade(1, 3).OnComplete(() =>
            {
                Debug.Log("Debug FadeOut");
            });
        }
    }

    //数据获取协程
    private IEnumerator DataAcquisitionCoroutine()
    {
        List<float> tmpData = new List<float>(64);
        float[] tmp=new float[64];
        while (_audioSources != null)
        {
            for(int i=0;i<_audioSources.Length;i++)
            {
                _audioSources[i].GetSpectrumData(tmp, 0, FFTWindow.Blackman);
                tmpData.AddRange(tmp);
            }
            sampleData = tmpData.ToArray();
            tmpData.Clear();
            normalizedData = NormalizeData(sampleData);
            yield return null;
        }
    }

    //归一化音频
    private float[] NormalizeData(float[] input)
    {
        float[] output = new float[input.Length];
        float max = 0;
        float min = 0;
        for(int i=0 ; i<input.Length ; i++) 
        {
            max=Mathf.Max(max, input[i]);
            min=Mathf.Min(min, input[i]);
        }

        float len=max-min;

        for(int i=0;i<input.Length ; i++)
        {
            if(len<=0) 
            {
                output[i] = 0;
            }
            else
            {
                output[i] = (input[i]-min)/len;
            }
        }

        return output;
    }
}
