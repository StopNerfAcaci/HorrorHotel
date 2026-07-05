using System;
using System.Linq;
using Gameplay.Combat;
using Gameplay.CoreSystem;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HSM
{
    public class PlayerStateDriver : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        // [SerializeField] private Animator animator;
        [SerializeField] private PlayerData data;
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;
        private const float crossFadeDuration = 0.15f;

        public Core Core { get; private set; }

        private StateMachine Machine;
        private State root;

        private Transform mainCam;
        private string lastPath;

        private IInteractable interactable;
        private WeaponView weaponView;
        public WeaponView WeaponView => weaponView;
        public ReactiveCommand<bool> AttackCommand = new();

        public InputReader Reader => inputReader;
        // public Animator Animator => animator;
        public PlayerData Data => data;

        public Transform MainCam
        {
            get
            {
                if (mainCam == null || !mainCam.gameObject.activeInHierarchy)
                    RefreshMainCamera();

                return mainCam;
            }
        }

        private void Awake()
        {
            SetupComponents();

            SetupCore();
            SetupMachine();
        }


        private void Start()
        {
            RefreshMainCamera();

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
            inputReader.Roll += OnDash;
            inputReader.Attack += OnAttack;
        }


        private void OnDisable()
        {
            if (inputReader == null) return;
            inputReader.Roll -= OnDash;
            inputReader.Attack -= OnAttack;
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
                Debug.Log(path);
                lastPath = path;
            }
        }

        private void FixedUpdate()
        {
            Machine.FixedTick(Time.fixedDeltaTime);
        }

        private void SetupComponents()
        {
            // animator = GetComponentInChildren<Animator>();
            weaponView = GetComponentInChildren<WeaponView>(true);

            RefreshMainCamera();
        }

        private void RefreshMainCamera()
        {
            mainCam = FindMainCameraInOwnScene();
        }

        private Transform FindMainCameraInOwnScene()
        {
            Scene scene = gameObject.scene;
            if (!scene.IsValid() || !scene.isLoaded)
                return null;

            Camera fallbackCamera = null;
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                Camera[] cameras = root.GetComponentsInChildren<Camera>(true);
                foreach (Camera camera in cameras)
                {
                    if (!camera.enabled || !camera.gameObject.activeInHierarchy)
                        continue;

                    if (camera.CompareTag("MainCamera"))
                        return camera.transform;

                    fallbackCamera ??= camera;
                }
            }

            return fallbackCamera != null ? fallbackCamera.transform : null;
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

        private void OnDash(bool pressed)
        {
            // dashPressed = pressed;
        }

        private void OnAttack(bool request)
        {
            AttackCommand.Execute(request);
        }

        // public void CrossFadeIfReady(int stateHash)
        // {
        //     if (!animator || !animator.runtimeAnimatorController)
        //         return;
        //
        //     if (!animator.HasState(0, stateHash))
        //         return;
        //
        //     animator.CrossFade(stateHash, crossFadeDuration);
        // }

        //This to help knowing the current state. Only call when debug for optimization
        static string StatePath(State s)
            => string.Join(" > ", s.PathToRoot().AsEnumerable().Reverse().Select(path => path.GetType().Name));

        enum AbilityType
        {
            Attack,
            Roll
        }
    }
}
