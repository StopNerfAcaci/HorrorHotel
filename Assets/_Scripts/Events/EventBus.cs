using System;
using System.Collections.Generic;
using UnityEngine;

namespace Horror.Events
{
    public static class EventBus
    {
        private static Dictionary<Type, Delegate> assignedActions = new();

        public static void Raise(EventData data)
        {
            Type t = data.GetType();
            if (assignedActions.TryGetValue(t, out Delegate action))
            {
                action?.DynamicInvoke(data);
            }
        }

        public static void Register<T>(Action<T> callback) where T : EventData
        {
            Type t = typeof(T);

            if (assignedActions.ContainsKey(t))
            {
                assignedActions[t] = Delegate.Combine(assignedActions[t], callback);
            }
            else
            {
                assignedActions[t] = callback;
            }
        }

        public static void InRegister<T>(Action<T> callback) where T : EventData
        {
            Type t = typeof(T);

            if (assignedActions.ContainsKey(t))
            {
                assignedActions[t] = Delegate.Remove(assignedActions[t], callback);
            }
        }
    }
}