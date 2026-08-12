using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Mediator<T> : MonoBehaviour where T : Component, IVisitable
{
    private readonly List<T> entities = new();

    public void Register(T entity)
    {
        if (!entities.Contains(entity))
        {
            entities.Add(entity);
            OnRegistered(entity);
        }
    }


    public void Unregister(T entity)
    {
        if (entities.Contains(entity))
        {
            entities.Remove(entity);
            OnDeregistered(entity);
        }
    }

    protected virtual void OnRegistered(T entity) {
        // noop
    }
    protected virtual void OnDeregistered(T entity) {
        // noop
    }


    public void Message(T source, T target, IVisitor message)
    {
        entities.FirstOrDefault(entity => entity.Equals(target))?.Accept(message);
    }

    public void BroadCast(T source, IVisitor message, Func<T, bool> predicate = null)
    {
        entities.Where(target => source != target && SenderConditionMet(target, predicate) && MediatorConditionMet(target))
            .ForEach(target => target.Accept(message));
    }
    bool SenderConditionMet(T target, Func<T, bool> predicate) => predicate == null || predicate(target);
    protected abstract bool MediatorConditionMet(T target);
}


public static class EnumerableExtensions {
    /// <summary>
    /// Performs an action on each element in the sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence to iterate over.</param>
    /// <param name="action">The action to perform on each element.</param>    
    public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action) {
        foreach (var item in sequence) {
            action(item);
        }
    }
}