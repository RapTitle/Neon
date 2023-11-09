using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ComponentPool<T> : Pool<T> where T : Component
{
    private Transform _parent;

    private Transform _poolRoot;

    private Transform PoolRoot
    {
        get 
        {
            if(_poolRoot==null)
            {
                _poolRoot = new GameObject("Pool").transform;
                _poolRoot.SetParent(_parent);
                _poolRoot.localPosition = Vector3.zero;
            }
            return _poolRoot;
        }
    }

    public void SetParent(Transform t)
    {
        _parent = t;
        PoolRoot.SetParent(t);
        PoolRoot.localPosition=Vector3.zero;
    }

    public override T Request()
    {
        T member=base.Request();
        member.gameObject.SetActive(true);
        return member;
    }

    public override void Return(T member)
    {
        member.transform.SetParent(PoolRoot);
        member.gameObject.SetActive(false);
        base.Return(member);
    }

    protected override T Create()
    {
        T member = base.Create();
        member.transform.SetParent(PoolRoot);
        member.gameObject.SetActive(false);
        return member;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (PoolRoot != null)
        {
#if UNITY_EDITOR
            DestroyImmediate(_poolRoot.gameObject);
#else
            Destroy(_poolRoot.gameObject);
#endif
        }
    }
}
