using System;
using System.Linq;
using Gameplay.CoreSystem;
using Gameplay.Inventory;
using R3;
using UnityEngine;
using UnityServiceLocator;

namespace HSM
{
    public class PlayerStateDriver : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private PlayerData data;
        [SerializeField] private bool cursorLocked = true;

        public PlayerInventory Inventory { get; private set; }
        public Core Core { get; private set; }

        private StateMachine Machine;
        private State root;
        private string lastPath;

        public InputReader Reader => inputReader;
        public PlayerData Data => data;
        private bool isBusy = false;
        public bool IsBusy => isBusy;
        public bool HasInteractable { get; set; }

        private void Awake()
        {
            SetupComponents();
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

        private void FixedUpdate()
        {
            Machine.FixedTick(Time.fixedDeltaTime);
        }

        private void SetupComponents()
        {
            ServiceLocator.Global.Register<PlayerInventory>(Inventory =
                new PlayerInventory.Builder().WithEntryItems(Data.StartItem).Build()
            );
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

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

        internal void SetBusy(bool busy) => isBusy = busy;

        // private void OnGUI()
        // {
        //     
        // }
    }
}