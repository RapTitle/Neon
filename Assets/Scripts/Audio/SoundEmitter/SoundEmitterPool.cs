using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitterPool : ComponentPool<SoundEmitter> 
{

    [SerializeField] private SoundEmitterFactorySO soundEmitterFactory;
    public override IFactory<SoundEmitter> Factory { get => soundEmitterFactory; set => soundEmitterFactory = value as SoundEmitterFactorySO; }
 }
