using System;
using UnityEngine;

//需要MainAudio的添加有什么功能呢，简单一点，可以添加，可以全部删除，可以单个删除

[CreateAssetMenu(menuName ="Audio/AudioCue")]
public class AudioCueSO : ScriptableObject
{
    public bool looping=false;

    [SerializeField] protected AudioClipsGroup[] audioClipsGroups = default;

    public AudioClip[] GetClips()
    {
        int numberOfClips=audioClipsGroups.Length;
        AudioClip[] resultingClips=new AudioClip[numberOfClips];

        for(int i = 0; i < numberOfClips; i++)
        {
            resultingClips[i] = audioClipsGroups[i].GetNextClip();
        }

        return resultingClips;
    }
}


[Serializable]
public class AudioClipsGroup
{

    public SequenceMode sequenceMode = SequenceMode.RandomNoImmediateRepeat;
    public AudioClip[] audioClips;

    private int _nextClipToPlay = -1;
    private int _lastClipPlayed = -1;

    public AudioClip GetNextClip()
    {
        if(audioClips.Length==1)
            return audioClips[0];
        if(_nextClipToPlay==-1)
        {
            _nextClipToPlay=(sequenceMode==SequenceMode.Sequential)?0:UnityEngine.Random.Range(0, audioClips.Length);
        }
        else
        {
            switch(sequenceMode)
            {
                case SequenceMode.Random:
                    _nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                    break;
                case SequenceMode.Sequential:
                    _nextClipToPlay = (int)Mathf.Repeat(++_nextClipToPlay, audioClips.Length);
                    break;
                case SequenceMode.RandomNoImmediateRepeat:
                    do
                    {
                        _nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                    } while (_nextClipToPlay == _lastClipPlayed);
                    break;
            }
        }
        _lastClipPlayed=_nextClipToPlay;
        return audioClips[_nextClipToPlay];
    }


    public enum SequenceMode
    {
        Random,
        RandomNoImmediateRepeat,
        Sequential,
    }

}
