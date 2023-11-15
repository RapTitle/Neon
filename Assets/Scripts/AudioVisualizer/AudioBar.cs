using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AudioBar : MonoBehaviour
{
    [SerializeField] private Light2D _light;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private float _startScaleY;
    private Vector3 _updateVector;

    public void Initialize()
    {
        _startScaleY=transform.localScale.y;
        _updateVector = transform.lossyScale;
    }


    public void SetColor(Color color)
    {
        _light.color = color;
        _spriteRenderer.color = color;
    }

    public void SetScaleY(float scaleY,float size)
    {
        
        _updateVector.y = scaleY*size;
        transform.localScale = _updateVector;
    }

    [ContextMenu("≤‚ ‘")]
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _light = GetComponent<Light2D>();
    }

}
