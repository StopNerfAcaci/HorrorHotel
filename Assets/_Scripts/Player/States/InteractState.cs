using System;
using Cysharp.Threading.Tasks;
using Gameplay.CoreSystem;
using Horror.Events;
using UnityEngine;
using R3;

namespace HSM
{
    public class InteractState : State
    {
        private readonly PlayerStateDriver player;

        private bool isAbilityDone;
        private float _yaw;
        private float _pitch;

        private IInteractable _itemCached;
        private Vector2 prevPos;
        private DisposableBag _bag;
        private AnimationHandler _animationHandler;
        public AnimationHandler AnimationHandler => _animationHandler ?? core.GetCoreComponent<AnimationHandler>();

        private Interaction _interaction;
        public Interaction Interaction => _interaction ?? core.GetCoreComponent<Interaction>();
        public InteractState(StateMachine machine, State parent, PlayerStateDriver player) : base(machine, parent)
        {
            this.player = player;
            core = player.Core;
        }


        protected override void OnEnter()
        {
            isAbilityDone = false;
            player.Reader.Interact += OnConfirm;
            player.Reader.Pointed += GetLastPoint;
            player.SetBusy(true);
            _itemCached = player.GetInteractable<IInteractable>();
            if (player.CanPerformAnim())
            {
                AnimationHandler.SmoothChangeAnim(_itemCached.PlayerAnimName);
            }

            _yaw = 0f;
            _pitch = 0f;
        }

        protected override void OnExit()
        {
            player.Reader.Interact -= OnConfirm;
            player.Reader.Pointed -= GetLastPoint;
            player.SetBusy(false);
            AnimationHandler.StopAnim();
        }

        private void GetLastPoint(Vector2 pos)
        {
            prevPos = pos;
        }

        private void OnConfirm()
        {
            Interaction.TryConfirm(_itemCached, () =>
            {
                EventBus.Raise(new HideMenuEventData());
                _itemCached = null;
                isAbilityDone = true;
            });

        }

        protected override void OnUpdate(float deltaTime)
        {
            HandleHold();
        }

        private void HandleHold()
        {
            if (!player.Reader.IsHolding || _itemCached is not IItem) return;
            _yaw += prevPos.x * player.Data.RotationSpeed * Time.deltaTime;
            _pitch += -prevPos.y * player.Data.RotationSpeed * Time.deltaTime;
            _itemCached.Transform.localRotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }
        
        protected override State GetTransition() => isAbilityDone ? ((PlayerRoot)Parent.Parent).Locomotion : null;

        public override void Dispose()
        {
            base.Dispose();
            _bag.Dispose();
        }
    }
}