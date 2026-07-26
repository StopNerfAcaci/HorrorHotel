using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityServiceLocator;
using VitalRouter;
using R3;

namespace HSM
{
    public class InteractState : State
    {
        private readonly PlayerStateDriver player;
        
        private bool isAbilityDone;
        private float _yaw;
        private float _pitch;

        private IItem _heldItem;
        private Vector2 prevPos;
        private GameplayManager gm;
        private readonly Router router;
        private DisposableBag _bag;
        public InteractState(StateMachine machine, State parent, PlayerStateDriver player) : base(machine, parent)
        {
            this.player = player;
            ServiceLocator.For(player).Get<GameplayManager>(out gm);
            
            ServiceLocator.For(player).Get<Router>(out router);
        }


        protected override void OnEnter()
        {
            isAbilityDone = false;
            player.Reader.Interact += OnInteract;
            player.Reader.Pointed += GetLastPoint;
            player.SetBusy(true);
            var root = (PlayerRoot)Parent.Parent;
            _heldItem = root.PendingInteractable as IItem;
            root.PendingInteractable = null;
            router.PublishAsync(new ItemInteractionStartedCommand(_heldItem.Item));
            gm.State = GameplayManager.GameState.Interact;
            
            _yaw = 0f;
            _pitch = 0f;
            // ShowPreviewAsync().Forget();
        }

        protected override void OnExit()
        {
            player.Reader.Interact -= OnInteract;
            player.Reader.Pointed -= GetLastPoint;
            player.SetBusy(false);
            router.PublishAsync(new ItemInteractionEndedCommand());
            gm.State = GameplayManager.GameState.Movement;
        }

        private void GetLastPoint(Vector2 pos)
        {
            prevPos = pos;
        }

        private void OnInteract()
        {
            InteractAsync().Forget();
        }

        protected override void OnUpdate(float deltaTime)
        {
            HandleHold();
        }

        private void HandleHold()
        {
            if (!player.Reader.IsHolding) return;
            _yaw += prevPos.x * player.Data.RotationSpeed * Time.deltaTime;
            _pitch += -prevPos.y * player.Data.RotationSpeed * Time.deltaTime;
            _heldItem.Transform.localRotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }

        private async UniTask InteractAsync()
        {
            await _heldItem.Use();
            _heldItem = null;
            isAbilityDone = true;
        }

        protected override State GetTransition() => isAbilityDone ? ((PlayerRoot)Parent.Parent).Locomotion : null;

        public override void Dispose()
        {
            base.Dispose();
            _bag.Dispose();
        }
    }
}