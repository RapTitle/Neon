using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCreator : MonoBehaviour
{
    //现在的问题是 这个对象池只能在这里访问
    [SerializeField] private int _initSize;

    [SerializeField] private float _createDuration;
    private WaitForSeconds _createWait;

    //这样写不安全
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
            // Todo (位置需要进行处理，应该可以根据声音可视化的数据对位置进行规整)

            platform.transform.localPosition=Vector3.zero;
            platform.Initiazlie(Color.blue, 5);
            yield return _createWait;
        }
    }
}
