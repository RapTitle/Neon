using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Factory/SoundEmitterFactory")]
public class SoundEmitterFactorySO : FactorySO<SoundEmitter> 
{
    public SoundEmitter prefab = default;
    
    public override SoundEmitter Create()
    {
        return Instantiate(prefab);
    }
}

