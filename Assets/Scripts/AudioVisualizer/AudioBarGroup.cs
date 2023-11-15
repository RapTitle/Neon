using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBarGroup : MonoBehaviour
{
    [SerializeField] private AudioBar[] _audioBars;

    private float[] scaleSizes=new float[8];
    public void Initialize()
    {
        //在这里初始化
        //生成AudioBar并排列顺序
        for(int i=0;i<_audioBars.Length;i++)
        {
            _audioBars[i].Initialize();
        }

        for (int i = 0; i < scaleSizes.Length; i++)
            scaleSizes[i] = 1;
    }

    public void SetScale(float[] scaleSize)
    {
        scaleSizes=scaleSize;
    }

    public void ToggleOnGroup(bool on)
    {
        gameObject.SetActive(on);
    }

    public void SetColor()
    {
        //要根据数据对其设置颜色
    }

    public void SetScaleY(float[] scaleYs)
    {
        for (int i=0;i<_audioBars.Length;i++)
        {
            _audioBars[i].SetScaleY(scaleYs[i], scaleSizes[i]);
        }
    }
}
