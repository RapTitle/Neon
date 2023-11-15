using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBarGroup : MonoBehaviour
{
    [SerializeField] private AudioBar[] _audioBars;

    private float[] scaleSizes=new float[8];
    public void Initialize()
    {
        //�������ʼ��
        //����AudioBar������˳��
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
        //Ҫ�������ݶ���������ɫ
    }

    public void SetScaleY(float[] scaleYs)
    {
        for (int i=0;i<_audioBars.Length;i++)
        {
            _audioBars[i].SetScaleY(scaleYs[i], scaleSizes[i]);
        }
    }
}
