using System;
using UnityEngine;
using UnityServiceLocator;
using VitalRouter;

[DefaultExecutionOrder(-2000)]
public class ServiceHolder : MonoBehaviour, IDisposable
{
    private Router _publisher;
    private void Awake()
    {
        _publisher = new Router();
        var loc = ServiceLocator.For(this);
        ServiceLocator.For(this).Register<Router>(_publisher);
    }

    private void OnDestroy()
    {
        Dispose();
    }

    public void Dispose()
    {
        
    }
}
