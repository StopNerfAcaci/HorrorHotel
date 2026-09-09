using System;
using System.Linq;
using Gameplay.CoreSystem;
using Horror.Events;
using Sirenix.Utilities;
using UnityEngine;

namespace HSM
{
    public class PlayerStateDriver : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private PlayerData data;
        [SerializeField] private bool cursorLocked = true;
        [SerializeField] private bool inDebugMode = false;
        public Core Core { get; private set; }

        private StateMachine Machine;
        private State root;
        private string lastPath;

        public InputReader Reader => inputReader;
        public PlayerData Data => data;
        private bool isBusy = false;
        public bool IsBusy => isBusy;

        private IInteractable _interactable;

        private void Awake()
        {
            SetupCore();
            SetupMachine();
        }

        private void Start()
        {
            Machine.Start();
        }

        private void OnDestroy()
        {
            if (root != null) root.Dispose();
        }

        private void OnEnable()
        {
            if (inputReader == null) return;
            inputReader.EnablePlayerActions();
        }


        private void OnDisable()
        {
            if (inputReader == null) return;
            inputReader.DisablePlayerActions();
        }

        private void Update()
        {
            if (!inDebugMode) return;
            OnUpdate();
        }

        private void FixedUpdate()
        {
            if(!inDebugMode) return;
            OnFixedUpdate();
        }

        internal void OnUpdate()
        {
            if (inputReader == null || Machine == null) return;

            Machine.Tick(Time.deltaTime);
            Core.LogicUpdate();

            var path = StatePath(Machine.Root.Leaf());
            if (path != lastPath)
            {
                // Debug.Log(path);
                lastPath = path;
            }
        }
        internal void OnFixedUpdate()
        {
            Machine?.FixedTick(Time.fixedDeltaTime);
        }

        public float GetSpeed() => Reader.Sprint ? data.SprintSpeed : data.MoveSpeed;

        private void SetupCore()
        {
            Core = GetComponentInChildren<Core>();
        }

        private void SetupMachine()
        {
            root = new PlayerRoot(null, this);
            var builder = new StateMachineBuilder(root);
            Machine = builder.Build();
        }
        
        //This to help knowing the current state. Only call when debug for optimization
        static string StatePath(State s)
            => string.Join(" > ", s.PathToRoot().AsEnumerable().Reverse().Select(path => path.GetType().Name));


        internal void SetBusy(bool busy) => isBusy = busy;
        
        public bool CanPerformAnim() => _interactable != null && !_interactable.PlayerAnimName.IsNullOrWhitespace();

        public T GetInteractable<T>() where T : IInteractable
        {
            if (_interactable is T typed)
                return typed;

            throw new InvalidCastException(
                $"Interactable of type {_interactable?.GetType().Name} does not implement {typeof(T).Name}");
        }
        public bool TryGetInteractable<T>(out T result) where T : class, IInteractable
        {
            result = _interactable as T;
            return result != null;
        }
        public void SetInteractable(IInteractable interactable)
        {
            this._interactable = interactable;
        }
        
    }
}