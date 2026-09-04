using System;
using UnityEngine;
using UnityServiceLocator;
using VitalRouter;


public class Bed : MonoBehaviour, IEnvironment
{
    [SerializeField] private float delay = 2f;
    
    private Collider _col;
    public float Delay => delay;
    
    private GameplayManager _gameplayManager;
    private Router _publisher;
    private void Awake()
    {
        _col = GetComponent<Collider>();
        _col.enabled = true;
    }

    private void Start()
    {
        ServiceLocator.For(this).Get<GameplayManager>(out _gameplayManager);
        ServiceLocator.For(this).Get<Router>(out _publisher);
    }

    public bool CanPerform() => _gameplayManager.CanMoveNextPhase();

    public void Interact(InteractContext ctx)
    {
        if (!CanPerform())
        {
            Debug.Log("Require all current progress done");
            // _publisher.PublishAsync(new PopupCommand(PopupType.NotDoneProgress));
            return;
        }
        _gameplayManager.HandleNextPhase();
        _col.enabled = false;
    }
}
