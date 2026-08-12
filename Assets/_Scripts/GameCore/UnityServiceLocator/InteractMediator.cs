using System;
using UnityEngine;
using UnityServiceLocator;

public class InteractMediator : Mediator<Interaction>
{
    private void Awake() =>ServiceLocator.Global.Register(this as Mediator<Interaction>);
    

    protected override bool MediatorConditionMet(Interaction target) => target.HasItem();

    protected override void OnRegistered(Interaction entity)
    {
        Debug.Log($"{entity.name} registered");
        BroadCast(entity, new MessagePayload{Content = "Registered"});
    }

    protected override void OnDeregistered(Interaction entity)
    {
        Debug.Log($"{entity.name} deregistered");
        BroadCast(entity, new MessagePayload{Content = "Deregistered"});
    }
}