using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Platform : MonoBehaviour
{
    [SerializeField] private Light2D _light;

    [SerializeField] private float _speed;

    public void Initiazlie(Color color, float speed)
    {
        _light.color = color;
        _speed = speed;
    }

    private void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * _speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlatformCreator.PlatformPool.Return(this);
    }
}
