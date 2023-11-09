using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCueKey
{
    public static AudioCueKey Invalid= new AudioCueKey(-1,null);

    internal int value;

    internal AudioCueSO audioCue;



    internal AudioCueKey(int value, AudioCueSO audioCue)
    {
        this.value = value;
        this.audioCue = audioCue;

    }

    public override bool Equals(object obj)
    {
        return obj is AudioCueKey x && value==x.value&&audioCue==x.audioCue;
    }

    public override int GetHashCode()
    {
        return value.GetHashCode() ^ audioCue.GetHashCode();
    }
    public static bool operator ==(AudioCueKey x, AudioCueKey y)
    {
        return x.value == y.value && x.audioCue == y.audioCue;
    }

    public static bool operator !=(AudioCueKey x, AudioCueKey y) { return !(x == y); }
}

