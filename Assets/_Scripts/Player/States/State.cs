using System;
using System.Collections.Generic;
using Gameplay.CoreSystem;
using UnityEngine;

namespace HSM
{
    public abstract class State : IDisposable
    {
        protected const float ZeroF = 0f;
         
        public readonly StateMachine Machine;
        public readonly State Parent;
        public State ActiveChild;

        readonly List<IActivity> activities = new List<IActivity>();
        public IReadOnlyList<IActivity> Activities;
        
        protected Core core;
        private bool disposed;
        
        public State(StateMachine machine, State parent)
        {
            Machine = machine;
            Parent = parent;
            Activities = activities;
        }

        public void Add(IActivity activity)
        {
            if(activity != null)
                activities.Add(activity);
        }
        protected virtual State GetInitialState() => null; //Initial child enter when this state start
        protected virtual State GetTransition() => null; //Target state to change to this frame

        #region LifeCycle

        protected virtual void OnEnter(){}
        protected virtual void OnExit(){}
        protected virtual void OnUpdate(float deltaTime) { }
        protected virtual void PhysicsUpdate(float deltaTime) { }
        internal void Enter()
        {
            if (Parent != null) Parent.ActiveChild = this;
            OnEnter();
            State init = GetInitialState();
            if(init != null) init.Enter();
        }

        internal void Exit()
        {
            if(ActiveChild != null) ActiveChild.Exit();
            ActiveChild = null;
            OnExit();
        }

        internal void Update(float deltaTime)
        {
            State t = GetTransition();
            if (t != null)
            {
                Machine.Sequencer.RequestTransition(this, t);
                return;
            }
            
            if(ActiveChild != null) ActiveChild.Update(deltaTime);
            OnUpdate(deltaTime);
        }

        internal void FixedUpdate(float deltaTime)
        {
            if(ActiveChild != null) ActiveChild.FixedUpdate(deltaTime);
            PhysicsUpdate(deltaTime);
        }
        #endregion

        public State Leaf()
        {
            State s = this;
            while(s.ActiveChild != null) s = s.ActiveChild;
            return s;
        }

        public IEnumerable<State> PathToRoot()
        {
            for (State s = this; s != null; s = s.Parent) yield return s;
        }

        public virtual void Dispose()
        {
            if (disposed)
                return;

            disposed = true;
            ActiveChild?.Dispose();
            ActiveChild = null;
        }
    }
}
