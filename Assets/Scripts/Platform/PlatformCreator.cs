using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCreator : MonoBehaviour
{
    //���ڵ������� ��������ֻ�����������
    [SerializeField] private int _initSize;

    [SerializeField] private float _createDuration;
    private WaitForSeconds _createWait;

    //����д����ȫ
    public static PlatformPool PlatformPool
    {
        get { return _platformPool; }
    }
    private static PlatformPool _platformPool;

    private Coroutine _createCoroutine;

    private void Start()
    {
        Initialize();
        CreatePlatform();
    }

    public void Initialize()
    {
        _platformPool=GetComponent<PlatformPool>();
        _platformPool.SetParent(transform);
        _platformPool.Prewarm(_initSize);

        _createWait = new WaitForSeconds(_createDuration);
    }

    public void CreatePlatform()
    {
        _createCoroutine = StartCoroutine(PlatformCreateCoroutine());
    }

    public void StopCreatePlatform()
    {
        if(_createCoroutine != null )
        {
            StopCoroutine(_createCoroutine);
            _createCoroutine=null;
        }
    }

    private IEnumerator PlatformCreateCoroutine()
    {
        while(true)
        {
            Platform platform = _platformPool.Request();
            //
            // Todo (λ����Ҫ���д���Ӧ�ÿ��Ը����������ӻ������ݶ�λ�ý��й���)

            platform.transform.localPosition=Vector3.zero;
            platform.Initiazlie(Color.blue, 5);
            yield return _createWait;
        }
    }
}
