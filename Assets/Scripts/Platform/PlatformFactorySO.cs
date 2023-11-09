using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Factory/PlatformFactory")]
public class PlatformFactorySO : FactorySO<Platform>
{
    [SerializeField] private Platform prefab;
    public override Platform Create()
    {
        return Instantiate(prefab);
    }
}
