using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPool : ComponentPool<Platform>
{
    [SerializeField] private PlatformFactorySO platformFactory;
    public override IFactory<Platform> Factory { get => platformFactory; set => platformFactory = value as PlatformFactorySO; }
}
