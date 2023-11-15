using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AudioVisualization : MonoBehaviour
{

    [SerializeField] private MainAudioCueSO _audioCueSO;
    [SerializeField] private AudioConfigurationSO _configurationSO;

    [SerializeField] private AudioBarGroup _audioBarGroup;

    private AudioCueKey _audioCueKey;
    private AudioSource[] _audioSources;

    private float[] sampleData = new float[512];   

    private float[] _freqBand= new float[8];

    private float[] _bandBuffer=new float[8];


    [SerializeField] private Button _button;
    private bool _useBuffer = true;
    

    private Coroutine _dataAcquisitionCoroutine;

    private void Start()
    {
        Application.targetFrameRate =60;
        Initialize(_audioCueSO,_configurationSO);
        StartCoroutine(DataAcquisitionCoroutine());
       
        StartCoroutine(StartShowBar());

        _button.onClick.AddListener(() =>
        {
            _useBuffer = !_useBuffer;
            if(_useBuffer)
            {
                _button.GetComponentInChildren<Text>().text = "True";
            }
            else
            {
                _button.GetComponentInChildren<Text>().text = "False";
            }
        });

    }

    public void Initialize(AudioCueSO audioCue,AudioConfigurationSO settings)
    {
        _audioCueKey = AudioManager.Instance.PlayAudioCue(audioCue, settings, transform);
        _audioSources=GetComponentsInChildren<AudioSource>();

        _audioBarGroup.Initialize();
    }

    public void ToggleOn(bool on)
    {
        _audioBarGroup.ToggleOnGroup(on);
        if(!on)
        {
            if (_dataAcquisitionCoroutine != null)
            {
                StopCoroutine(_dataAcquisitionCoroutine);
                _dataAcquisitionCoroutine = null;
            }
        }  
        else
        {
            _dataAcquisitionCoroutine = StartCoroutine(DataAcquisitionCoroutine());
            
        }
            
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
        List<float> tmpData = new List<float>(512);
        float[] tmp = new float[512];
        while (_audioSources != null)
        {
            for(int i=0;i<_audioSources.Length;i++)
            {
                _audioSources[i].GetSpectrumData(tmp, 0, FFTWindow.Blackman);
                tmpData.AddRange(tmp);
            }
            sampleData = tmpData.ToArray();
            tmpData.Clear();

            MakeFrequencyBands();
            BandBuffer();

            yield return null;
        }
    }

    private IEnumerator StartShowBar()
    { 
        while(true)
        {
            if(_useBuffer)
            {
                _audioBarGroup.SetScaleY(_bandBuffer);
            }
            else
                _audioBarGroup.SetScaleY(_freqBand);
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

    private void MakeFrequencyBands()
    {
        int count = 0;

        for(int i=0;i<8;i++)
        {
            float average = 0;
            int sampleCount=(int)Mathf.Pow(2,i)*2;
            if(i==7)
            {
                sampleCount += 2;
            }
            for(int j=0;j<sampleCount;j++)
            {
                average += sampleData[count]*(count+1) ;
                count++;
            }

            average /= count;
            _freqBand[i] = average * 300;
        }

    }

    private void BandBuffer()
    {
        for(int i=0;i<8; i++)
        {
            _bandBuffer[i] = Mathf.Lerp(_bandBuffer[i], _freqBand[i], 0.1f);
        }
    }
}
