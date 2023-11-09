using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitterVault
{
    private int _nextUniqueKey = 0;
    private List<AudioCueKey> _emittersKey;
    private List<SoundEmitter[]> _emittersList;

    public SoundEmitterVault()
    {
        _emittersKey = new List<AudioCueKey>();
        _emittersList = new List<SoundEmitter[]>();
    }

    public AudioCueKey GetKey(AudioCueSO cue)
    {
        return new AudioCueKey(_nextUniqueKey, cue);
    }

    public void Add(AudioCueKey key, SoundEmitter[] emitters)
    {
        _emittersKey.Add(key);
        _emittersList.Add(emitters);
    }

    public AudioCueKey Add(AudioCueSO cue, SoundEmitter[] emitters)
    {
        AudioCueKey emitterKey=GetKey(cue);
        _emittersKey.Add(emitterKey);
        _emittersList.Add(emitters);
        return emitterKey;
    }

    public bool Get(AudioCueKey key,out SoundEmitter[] emitters)
    {
        int index = _emittersKey.FindIndex(x => x == key);

        if (index<0)
        {
            emitters = null;
            return false;
        }

        emitters = _emittersList[index];
        return true;
    }


    public bool Remove(AudioCueKey key)
    {
        int index=_emittersKey.FindIndex(x => x == key);
        return RemoveAt(index);
    }

    public bool RemoveAt(int index)
    {
        if(index<0)
        {
            return false;
        }

        _emittersKey.RemoveAt(index);
        _emittersList.RemoveAt(index); 
        return true;
    }

}
