using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pool<T> : MonoBehaviour,IPool<T>
{

    protected readonly Stack<T> _available = new Stack<T>();

    public abstract IFactory<T> Factory { get; set; }

    protected bool hasBeenPrewarmed {  get; set; }

    protected virtual T Create()
    {
        return Factory.Create();
    }    public virtual void Prewarm(int num)
    {
        if(hasBeenPrewarmed)
        {
            Debug.Log($"Pool has already been prewarmed");
            return;
        }

        for(int i = 0; i < num; i++)
        {
            _available.Push(Create());
        }
        hasBeenPrewarmed = true;
    }

    public virtual T Request()
    {
        return _available.Count > 0 ? _available.Pop():Create();
    }

    public virtual IEnumerable<T> Request(int num=1)
    {
        List<T> member = new List<T>();
        for(int i = 0;i < num;i++)
        {
            member.Add(Request());
        }
        return member;
    }

    public virtual void Return(T member)
    {
        _available.Push(member);
    }

    public virtual void Return(IEnumerable members) 
    {
        foreach(T member in members)
        {
            Return(member);
        }
    }

    public virtual void OnDisable()
    {
        _available.Clear();
        hasBeenPrewarmed=false;
    }
}
