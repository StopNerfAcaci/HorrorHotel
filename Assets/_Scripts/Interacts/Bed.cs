using System;
using UnityEngine;
using UnityServiceLocator;

public class Bed : MonoBehaviour, IEnvironment
{
    [SerializeField] private float delay = 2f;
    
    private Collider _col;
    public float Delay => delay;
    
    private GameplayManager _gameplayManager;

    private void Awake()
    {
        _col = GetComponent<Collider>();
        _col.enabled = true;
    }

    private void Start()
    {
        ServiceLocator.For(this).Get<GameplayManager>(out _gameplayManager);
    }

    public bool CanPerform() => _gameplayManager.CanMoveNextPhase();

    public void Interact(InteractContext ctx)
    {
        _gameplayManager.HandleNextPhase();
        _col.enabled = false;
    }

}
